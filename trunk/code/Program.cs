using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Xml;

namespace ZJUerXTopCoder
{
    class Program
    {
        static bool IsOnsite(String s)
        {
            return (s.IndexOf("TCCC") != -1 || s.IndexOf("TCO") != -1) && (s.IndexOf("Semi") != -1 || s.IndexOf("Final") != -1 || s.IndexOf("Wildcard") != -1);
        }
        
        static Dictionary<String, String> ReadZJUers()
        {
            string ZJUersFileName = "ZJUer.txt";
            Dictionary<String, String> ZJUers = new Dictionary<String, String>();
            StreamReader reader = new StreamReader(ZJUersFileName);
            while (!reader.EndOfStream)
            {
                string handle = reader.ReadLine();
                string id = reader.ReadLine();
                ZJUers[handle] = id;
            }
            reader.Close();
            return ZJUers;
        }

        static List<Round> ReadAllRoundInfo()
        {
            List<Round> roundList = new List<Round>();
            string URL = "http://www.topcoder.com/tc?module=BasicData&c=dd_round_list";

            XmlDocument xml = XmlHelper.LoadXML(URL, "round.xml");
            XmlNode rootNode = xml.LastChild;
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                Round round = Round.ParseRoundWithXML(node);
                roundList.Add(round);
            }
            return roundList;
        }

        /*static List<Round> ReadRounds(Dictionary<String, Round> roundInfo)
        {
            string roundsFileName = "NewRounds.txt";
            List<Round> rounds = new List<Round>();
            StreamReader reader = new StreamReader(roundsFileName);
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                rounds.Add(roundInfo[s]);
            }
            reader.Close();
            return rounds;
        }*/


        static void UpdateRound(Round round, Dictionary<String, String> ZJUers, List<Round> tour, List<Round> srm,
            Dictionary<String, CoderStat> stats, List<Record> ratingRecordList, List<Record> challengeInfoList,
            List<Record> volRecordList, List<Record> divWinnerList, List<Record> onsiteList, Dictionary<CoderStat, List<Record>> coderInfo)
        {
            string URL = "http://www.topcoder.com/tc?module=BasicData&c=dd_round_results&rd=" + round.roundID;
            XmlDocument xml = XmlHelper.LoadXML(URL, round.roundID + ".xml");
            if (xml == null)
            {
                return;
            }
            XmlNode rootNode = xml.LastChild;
            List<Coder> div1 = new List<Coder>(), div2 = new List<Coder>();
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                Coder coder = Coder.ParseCoderWithXML(node);
                if (coder != null && ZJUers.Keys.Contains(coder.handle))
                {
                    coder.id = ZJUers[coder.handle];
                    CoderStat coderStat = null;
                    Record record = new Record();
                    record.coder = coder;
                    record.round = round;
                    if (stats.Keys.Contains(coder.id))
                    {
                        coderStat = stats[coder.id];
                        coderStat.UpdateStat(coder, round);
                        coderInfo[coderStat].Add(record);
                    }
                    else
                    {
                        coderStat = CoderStat.NewCoderStat(coder, round);
                        stats.Add(coder.id, coderStat);
                        coderInfo[coderStat] = new List<Record>();
                        coderInfo[coderStat].Add(record);
                    }
                    if (coder.division == 1)
                    {
                        foreach (Coder other in div1)
                        {
                            coderStat.DualWith(coder, other);
                            stats[other.id].DualWith(other, coder);
                        }
                        div1.Add(coder);
                    }
                    else
                    {
                        foreach (Coder other in div2)
                        {
                            coderStat.DualWith(coder, other);
                            stats[other.id].DualWith(other, coder);
                        }
                        div2.Add(coder);
                    }
                    if (Convert.ToDouble(coder.challengePoints) > 0)
                    {
                        challengeInfoList.Add(record);
                    }
                    if (ratingRecordList.Count == 0 || coder.newRating > ratingRecordList.Last().coder.newRating)
                    {
                        ratingRecordList.Add(record);
                    }
                    if (coder.vol >= 600)
                    {
                        volRecordList.Add(record);
                    }
                    if (coder.divRank == 1)
                    {
                        divWinnerList.Add(record);
                    }
                    if (IsOnsite(round.name))
                    {
                        onsiteList.Add(record);
                    }
                }
            }
            if (div1.Count != 0 || div2.Count != 0)
            {
                div1.Sort(new Comparison<Coder>(Coder.CompareByRank));
                div2.Sort(new Comparison<Coder>(Coder.CompareByRank));
                HTMLGenerator.GenerateHTMLForRound(round, div1, div2);
                if (round.type.Equals("Single Round Match"))
                {
                    srm.Add(round);
                }
                else
                {
                    tour.Add(round);
                }
            }
        }

        static void CrawZJUer()
        {
            HashSet<String> handleList = new HashSet<String>();
            StreamReader rd = new StreamReader("ZJUer.txt");
            string handle;
            while (!rd.EndOfStream)
            {
                handle = rd.ReadLine();
                handleList.Add(handle);
                handle = rd.ReadLine();
            }
            rd.Close();
            StreamWriter wt = new StreamWriter("ZJUer.txt", true);
            for (int i = 1; ; i += 15)
            {
                string URL = "http://www.topcoder.com/tc?module=AdvancedSearch&sr=" + i + "&er=" + (i + 14) + "&sn=Zhejiang+University";
                WebClient wc = new WebClient();
                Console.WriteLine("Downloading " + URL);
                int count = 0;
                string text = null;
                while (true)
                {
                    ++count;
                    Console.WriteLine("Try" + count);
                    bool isok = true;
                    try
                    {
                        text = wc.DownloadString(URL);
                    }
                    catch (Exception)
                    {
                        isok = false;
                    }
                    if (isok)
                    {
                        break;
                    }
                }
                if (text.IndexOf("Your search returned 0 matches. Please try different search criteria.") != -1)
                {
                    break;
                }
                else
                {
                    int ind = text.IndexOf("Sort by");
                    while (true)
                    {
                        ind = text.IndexOf("MemberProfile", ind);
                        if (ind == -1)
                        {
                            break;
                        }
                        ind = text.IndexOf("=", ind);
                        ++ind;
                        int ind2 = text.IndexOf('"', ind);
                        string id = text.Substring(ind, ind2 - ind);
                        ind = text.IndexOf(">", ind);
                        ++ind;
                        ind2 = text.IndexOf("<", ind);
                        handle = text.Substring(ind, ind2 - ind);
                        ind = ind2;
                        if (!handleList.Contains(handle))
                        {
                            wt.WriteLine(handle);
                            wt.WriteLine(id);
                        }
                    }
                }
                wt.Flush();
            }
            wt.Close();
        }

        static void GenerateHistory(Dictionary<CoderStat, List<Record>> coderInfo)
        {
            string historyDire = Directory.GetCurrentDirectory() + "\\history\\";
            foreach (CoderStat coderStat in coderInfo.Keys)
            {
                List<Record> recordList = coderInfo[coderStat];
                string file = historyDire + coderStat.handle.ToLower() + ".xtx";
                FileStream stream = new FileStream(file, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(coderStat.handle);
                writer.WriteLine(coderStat.id);
                writer.WriteLine(HTMLGenerator.RatingToString(coderStat.rating, false));
                foreach (Record record in recordList)
                {
                    writer.WriteLine(record.round.roundIndex);
                    writer.WriteLine(record.round.roundID);
                    writer.WriteLine(record.round.name);
                    writer.WriteLine(record.coder.division);
                    writer.WriteLine(record.coder.divRank);
                    writer.WriteLine(record.coder.finalPoints);
                    writer.WriteLine(HTMLGenerator.RatingToString(record.coder.newRating, false));
                }
                writer.Close();
                stream.Close();
            }
        }

        static void Main(string[] args)
        {
            /*CrawZJUer();
            return;*/
            string dire = Directory.GetCurrentDirectory();
            string xmlDire = dire + "\\XML";
            if (!Directory.Exists(xmlDire))
            {
                Directory.CreateDirectory(xmlDire);
            }
            string rankDire = dire + "\\rank";
            if (!Directory.Exists(rankDire))
            {
                Directory.CreateDirectory(rankDire);
            }
            string statDire = dire + "\\stat";
            if (!Directory.Exists(statDire))
            {
                Directory.CreateDirectory(statDire);
            }
            string memberDire = dire + "\\member";
            if (!Directory.Exists(statDire))
            {
                Directory.CreateDirectory(statDire);
            }
            string historyDire = dire + "\\history";
            if (!Directory.Exists(historyDire))
            {
                Directory.CreateDirectory(historyDire);
            }
            Dictionary<String, CoderStat> stats = new Dictionary<string, CoderStat>();

            Dictionary<String, String> ZJUers = ReadZJUers();
            List<Round> roundList = ReadAllRoundInfo();
            int ind = 0;
            List<Round> tour = new List<Round>(), srm = new List<Round>();
            string lastDate = null;
            List<Record> ratingRecordList = new List<Record>();
            List<Record> challengeInfoList = new List<Record>();
            List<Record> volRecordList = new List<Record>();
            List<Record> divWinnerList = new List<Record>();
            List<Record> onsiteList = new List<Record>();
            Dictionary<CoderStat, List<Record>> coderInfo = new Dictionary<CoderStat, List<Record>>();
            //roundList.Reverse();
            foreach (Round round in roundList)
            {
                Console.WriteLine(++ind + " Processing " + round.name);
                UpdateRound(round, ZJUers, tour, srm, stats, ratingRecordList, challengeInfoList, volRecordList, divWinnerList, onsiteList, coderInfo);
                lastDate = round.date;
                //break;
            }
            HTMLGenerator.GenerateHTMLForIndex(srm, tour);
            HTMLGenerator.GenerateHTMLForAllCoder(coderInfo, stats);
            HTMLGenerator.GenerateStatPages(stats, lastDate, ratingRecordList, challengeInfoList, volRecordList, divWinnerList, onsiteList);
            GenerateHistory(coderInfo);
        }
    }
}
