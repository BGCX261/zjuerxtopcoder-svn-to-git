using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZJUerXTopCoder
{
    class HTMLGenerator
    {
        static string ColorToString(Color c)
        {
            switch (c)
            {
                case Color.White: return "coderTextWhite";
                case Color.Gray: return "coderTextGray";
                case Color.Green: return "coderTextGreen";
                case Color.Blue: return "coderTextBlue";
                case Color.Yellow: return "coderTextYellow";
                case Color.Red: return "coderTextRed";
                case Color.Target: return "coderTextTarget";
                default: return "coderTextDefault";
            }
        }

        static string StatusToString(string status)
        {
            if (status.Equals("Passed System Test"))
            {
                return "statusTextGolden";
            }
            else if (status.Equals("Challenge Succeeded"))
            {
                return "statusTextGreen";
            }
            else if (status.Equals("Failed System Test"))
            {
                return "statusTextRed";
            }
            else
            {
                return "statusTextWhite";
            }
        }

        static string ChallengeScoreToString(string score)
        {
            if (score.Equals("0.0"))
            {
                return "scoreTextWhite";
            }
            else if (score[0] == '-')
            {
                return "scoreTextRed";
            }
            else
            {
                return "scoreTextGreen";
            }
        }

        public static string RatingToString(int rating, bool newCoder)
        {
            if (newCoder)
            {
                return "ratingTextWhite";
            }
            else if (rating < 900)
            {
                return "ratingTextGray";
            }
            else if (rating < 1200)
            {
                return "ratingTextGreen";
            }
            else if (rating < 1500)
            {
                return "ratingTextBlue";
            }
            else if (rating < 2200)
            {
                return "ratingTextYellow";
            }
            else if (rating < 3000)
            {
                return "ratingTextRed";
            }
            else
            {
                return "ratingTextTarget";
            }
        }

        public static string RatingChangeToString(int ratingChange)
        {
            if (ratingChange == 0)
            {
                return "ratingChangeNo";
            }
            else if (ratingChange > 0)
            {
                return "ratingChangeInc";
            }
            else
            {
                return "ratingChangeDec";
            }
        }

        public static string RatingChangeToImage(int ratingChange)
        {
            if (ratingChange == 0)
            {
                return "";
            }
            else if (ratingChange > 0)
            {
                return "<img src=\"arrow_green_up.gif\"/>";
            }
            else
            {
                return "<img src=\"arrow_red_down.gif\"/>";
            }
        }

        public static void GenerateHTMLForRound(Round round, List<Coder> div1, List<Coder> div2)
        {
            string dire = Directory.GetCurrentDirectory();
            string rankFile = dire + "\\rank\\" + round.roundID + ".html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("<html><body>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + round.name + "</title><link rel=\"stylesheet\" href=\"rank.css\" type=\"text/css\" /></head>");
            writer.WriteLine("<h1><a href=\"http://www.topcoder.com/stat?c=round_stats&rd=" + round.roundID + "\" class=\"eventText\">" + round.name + "</a></h1>");
            if (div1.Count != 0)
            {
                writer.WriteLine("<div>");
                writer.WriteLine("<h2>Div 1</h2>");
                writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
                writer.WriteLine("\t<tr class=\"titleLine\">");
                writer.WriteLine("\t\t<td><span>Rank</span></td>");
                writer.WriteLine("\t\t<td><span>Handle</span></td>");
                writer.WriteLine("\t\t<td><span>Points</span></td>");
                if (!round.type.Equals("Single Round Match")) writer.WriteLine("\t\t<td><span>Advanced</span></td>");
                writer.WriteLine("\t\t<td><span>Level 1</span></td>");
                writer.WriteLine("\t\t<td><span>Level 2</span></td>");
                writer.WriteLine("\t\t<td><span>Level 3</span></td>");
                writer.WriteLine("\t\t<td><span>Challenge</span></td>");
                writer.WriteLine("\t\t<td><span>Challenge Points</span></td>");
                writer.WriteLine("\t\t<td><span>Old Rating</span></td>");
                writer.WriteLine("\t\t<td><span>New Rating</span></td>");
                writer.WriteLine("\t\t<td><span>Rating Change</span></td>");
                writer.WriteLine("\t\t<td><span>Volatility</span></td>");
                writer.WriteLine("\t</tr>");
                foreach (Coder coder in div1)
                {
                    writer.WriteLine("\t<tr>");
                    writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + round.roundID + "&cr=" + coder.id + "\">" + coder.divRank + "</a></td>");
                    writer.WriteLine("\t\t<td><a href=\"../member/" + coder.id + ".html\" class=\"" + ColorToString(coder.colortype) + "\">" + coder.handle + "</a></td>");
                    writer.WriteLine("\t\t<td><span>" + coder.finalPoints + "</span></td>");
                    if (!round.type.Equals("Single Round Match")) writer.WriteLine("\t\t<td><span>" + coder.advanced + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + StatusToString(coder.problemStatus[0]) + "\">" + (coder.problemStatus[0].Equals("Passed System Test") ? coder.problemPoints[0] : coder.problemStatus[0]) + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + StatusToString(coder.problemStatus[1]) + "\">" + (coder.problemStatus[1].Equals("Passed System Test") ? coder.problemPoints[1] : coder.problemStatus[1]) + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + StatusToString(coder.problemStatus[2]) + "\">" + (coder.problemStatus[2].Equals("Passed System Test") ? coder.problemPoints[2] : coder.problemStatus[2]) + "</span></td>");
                    writer.WriteLine("\t\t<td><span>" + (coder.succ_challenges > 0 ? "+" : "") + coder.succ_challenges + "/" + (coder.fail_challenges > 0 ? "-" : "") + coder.fail_challenges + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + ChallengeScoreToString(coder.challengePoints) + "\">" + coder.challengePoints + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coder.oldRating, coder.newCoder) + "\">" + coder.oldRating + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coder.newRating, false) + "\">" + coder.newRating + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + RatingChangeToString(coder.newRating - coder.oldRating) + "\">" + RatingChangeToImage(coder.newRating - coder.oldRating) + Math.Abs(coder.newRating - coder.oldRating) + "</span></td>");
                    writer.WriteLine("\t\t<td><span>" + coder.vol + "</span></td>");
                    writer.WriteLine("\t</tr>");
                }
                writer.WriteLine("</tbody></table></div>");
            }

            if (div2.Count != 0)
            {
                writer.WriteLine("<div>");
                writer.WriteLine("<h2>Div 2</h2>");
                writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
                writer.WriteLine("\t<tr class=\"titleLine\">");
                writer.WriteLine("\t\t<td><span>Rank</span></td>");
                writer.WriteLine("\t\t<td><span>Handle</span></td>");
                writer.WriteLine("\t\t<td><span>Points</span></td>");
                if (!round.type.Equals("Single Round Match")) writer.WriteLine("\t\t<td><span>Advanced</span></td>");
                writer.WriteLine("\t\t<td><span>Level 1</span></td>");
                writer.WriteLine("\t\t<td><span>Level 2</span></td>");
                writer.WriteLine("\t\t<td><span>Level 3</span></td>");
                writer.WriteLine("\t\t<td><span>Challenge</span></td>");
                writer.WriteLine("\t\t<td><span>Challenge Points</span></td>");
                writer.WriteLine("\t\t<td><span>Old Rating</span></td>");
                writer.WriteLine("\t\t<td><span>New Rating</span></td>");
                writer.WriteLine("\t\t<td><span>Rating Change</span></td>");
                writer.WriteLine("\t\t<td><span>Volatility</span></td>");
                writer.WriteLine("\t</tr>");
                foreach (Coder coder in div2)
                {
                    writer.WriteLine("\t<tr>");
                    writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + round.roundID + "&cr=" + coder.id + "\">" + coder.divRank + "</a></td>");
                    writer.WriteLine("\t\t<td><a href=\"../member/" + coder.id + ".html\" class=\"" + ColorToString(coder.colortype) + "\">" + coder.handle + "</a></td>");
                    writer.WriteLine("\t\t<td><span>" + coder.finalPoints + "</span></td>");
                    if (!round.type.Equals("Single Round Match")) writer.WriteLine("\t\t<td><span>" + coder.advanced + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + StatusToString(coder.problemStatus[0]) + "\">" + (coder.problemStatus[0].Equals("Passed System Test") ? coder.problemPoints[0] : coder.problemStatus[0]) + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + StatusToString(coder.problemStatus[1]) + "\">" + (coder.problemStatus[1].Equals("Passed System Test") ? coder.problemPoints[1] : coder.problemStatus[1]) + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + StatusToString(coder.problemStatus[2]) + "\">" + (coder.problemStatus[2].Equals("Passed System Test") ? coder.problemPoints[2] : coder.problemStatus[2]) + "</span></td>");
                    writer.WriteLine("\t\t<td><span>" + (coder.succ_challenges > 0 ? "+" : "") + coder.succ_challenges + "/" + (coder.fail_challenges > 0 ? "-" : "") + coder.fail_challenges + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + ChallengeScoreToString(coder.challengePoints) + "\">" + coder.challengePoints + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coder.oldRating, coder.newCoder) + "\">" + coder.oldRating + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coder.newRating, false) + "\">" + coder.newRating + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"" + RatingChangeToString(coder.newRating - coder.oldRating) + "\">" + RatingChangeToImage(coder.newRating - coder.oldRating) + Math.Abs(coder.newRating - coder.oldRating) + "</span></td>");
                    writer.WriteLine("\t\t<td><span>" + coder.vol + "</span></td>");
                    writer.WriteLine("\t</tr>");
                }
                writer.WriteLine("</tbody></table></div>");
            }
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        public static void GenerateHTMLForIndex(List<Round> srm, List<Round> tour)
        {
            srm.Reverse();
            tour.Reverse();
            string dire = Directory.GetCurrentDirectory();
            string rankFile = dire + "/ZJUerXTCer.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "ZJUerXTopCoder" + "</title><link rel=\"stylesheet\" href=\"index.css\" type=\"text/css\" /></head>");
            writer.WriteLine("<h1 align=\"center\">ZJUer X TopCoder</h1>");

            writer.WriteLine("<div class=\"divAllRank\">");
            writer.WriteLine("<div class=\"divSRM\">");
            writer.WriteLine("<h2>Single Round Match</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Date</span></td>");
            writer.WriteLine("\t</tr>");
            foreach (Round round in srm)
            {
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><a href=\"rank/" + round.roundID + ".html\" class=\"eventText\">" + round.name + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"dateText\">" + round.date + "</span></td>");
                writer.WriteLine("\t</tr>");
            }
            writer.WriteLine("</tbody></table></div>");

            writer.WriteLine("<div class=\"divTour\">");
            writer.WriteLine("<h2>Tournament</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Date</span></td>");
            writer.WriteLine("\t</tr>");
            foreach (Round round in tour)
            {
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><a href=\"rank/" + round.roundID + ".html\" class=\"eventText\">" + round.name + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"dateText\">" + round.date + "</span></td>");
                writer.WriteLine("\t</tr>");
            }
            writer.WriteLine("</tbody></table></div>");

            writer.WriteLine("<div class = \"divToolStat\">");
            writer.WriteLine("<h2>Tools</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Dual</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr class=\"dualLine\">");
            writer.WriteLine("\t\t<td><a href=\"dual.php\" class=\"dual\">Dual between ZJUers</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("</tbody></table>");

            writer.WriteLine("<h2>Statistics</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/toprating.html\" class=\"statText\">Top 20 with highest rating</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/topratingactive.html\" class=\"statText\">Top 20 with highest rating (active)</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/topmaxrating.html\" class=\"statText\">Top 20 with highest max rating</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/topmaxratingactive.html\" class=\"statText\">Top 20 with highest max rating (active)</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/ratingrecord.html\" class=\"statText\">First to achieve the highest ratings</a></td>");
            writer.WriteLine("\t</tr>");

            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Challenge</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/chasuccrate.html\" class=\"statText\">Top 20 challenge success rate</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/chapointstotal.html\" class=\"statText\">Top 20 challenge points in total</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/chapointsavg.html\" class=\"statText\">Top 20 challenge points in average</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/chapointsin1event.html\" class=\"statText\">Top 20 challenge points in one event</a></td>");
            writer.WriteLine("\t</tr>");

            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Misc</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/succrate.html\" class=\"statText\">Top 20 submission success rate</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/mostevents.html\" class=\"statText\">Top 20 with most events</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/mosteventslastyear.html\" class=\"statText\">Top 20 with most events in last year</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/chameleons.html\" class=\"statText\">Top 20 chameleons</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/volatile.html\" class=\"statText\">Top 20 most volatile</a></td>");
            writer.WriteLine("\t</tr>");

            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>List</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/divwinners.html\" class=\"statText\">All division winners</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/onsite.html\" class=\"statText\">All onsite ZJUers</a></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"stat/zjuers.html\" class=\"statText\">All ZJUers (with at least one event)</a></td>");
            writer.WriteLine("\t</tr>");

            writer.WriteLine("</tbody></table></div>");

            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"anounce\"><tbody>");
            writer.WriteLine("\t<tr class=\"anounceLine\">");
            writer.WriteLine("\t\t<td><span>Developed by <a href=\"http://www.topcoder.com/tc?module=MemberProfile&cr=22645364\" class=\"hhanger\">hhanger</a></span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("</tbody></table>");


            writer.WriteLine("</div>");

            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopRatingPage(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/toprating.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 with highest rating" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 with highest rating</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t\t<td><span>Last event</span></td>");
            writer.WriteLine("\t</tr>");
            int ct = 0;
            int lastrank = 0;
            int lastRating = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                if (coderStat.rating != lastRating)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.rating + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"eventText\">" + coderStat.myLastDate + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastRating = coderStat.rating;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopRatingActivePage(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/topratingactive.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 with highest rating (active)" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 with highest rating with at least one event in recent 6 months</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t\t<td><span>Last event</span></td>");
            writer.WriteLine("\t</tr>");
            int ct = 0;
            int lastrank = 0;
            int lastRating = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                if (coderStat.rating != lastRating)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.rating + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"eventText\">" + coderStat.myLastDate + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastRating = coderStat.rating;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopMaxRatingPage(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/topmaxrating.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 with highest max rating" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 with highest max rating</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Max rating</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t\t<td><span>Last event</span></td>");
            writer.WriteLine("\t</tr>");
            int ct = 0;
            int lastrank = 0;
            int lastRating = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                if (coderStat.maxrating != lastRating)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coderStat.maxrating, false) + "\">" + coderStat.maxrating + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"eventText\">" + coderStat.myLastDate + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastRating = coderStat.rating;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopMaxRatingActivePage(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/topmaxratingactive.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 with highest max rating (active)" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 with highest max rating (active)</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Max rating</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t\t<td><span>Last event</span></td>");
            writer.WriteLine("\t</tr>");
            int ct = 0;
            int lastrank = 0;
            int lastRating = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                if (coderStat.maxrating != lastRating)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coderStat.maxrating, false) + "\">" + coderStat.maxrating + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"eventText\">" + coderStat.myLastDate + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastRating = coderStat.rating;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopRatingRecordPage(List<Record> ratingRecordList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/ratingrecord.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            ratingRecordList.Reverse();

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "First to achieve the highest ratings" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>First to achieve the highest ratings</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Date</span></td>");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t</tr>");
            foreach (Record ratingRecord in ratingRecordList)
            {
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + ratingRecord.coder.id + ".html\" class=\"" + RatingToString(ratingRecord.coder.newRating, false) + "\">" + ratingRecord.coder.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"dateText\">" + ratingRecord.round.date + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + ratingRecord.round.roundID + "&cr=" + ratingRecord.coder.id + "\" class=\"eventText\">" + ratingRecord.round.name + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(ratingRecord.coder.newRating, false) + "\">" + ratingRecord.coder.newRating + "</span></td>");
                writer.WriteLine("\t</tr>");
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopChallengeSuccRate(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/chasuccrate.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 challenge success rate" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 challenge success rate (with at least 20 challenges)</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span># succ. challenges</span></td>");
            writer.WriteLine("\t\t<td><span># total challenges</span></td>");
            writer.WriteLine("\t\t<td><span>Succ. rate</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            double lastChaSuccRate = -1.00;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                double chaSuccRate = (Convert.ToDouble(coderStat.succ_challenges) / coderStat.total_challenges * 100);
                if (chaSuccRate != lastChaSuccRate)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.succ_challenges + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.total_challenges + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + chaSuccRate.ToString("N2") + "%</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastChaSuccRate = chaSuccRate;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopSubmissionSuccRate(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/succrate.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 submission success rate" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 submission success rate (with at least 20 submissions)</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span># solves</span></td>");
            writer.WriteLine("\t\t<td><span># submissions</span></td>");
            writer.WriteLine("\t\t<td><span>Succ. rate</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            double lastSubSuccRate = -1.00;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                double subSuccRate = (Convert.ToDouble(coderStat.solves) / coderStat.submits * 100);
                if (subSuccRate != lastSubSuccRate)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.solves + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.submits + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + subSuccRate.ToString("N2") + "%</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastSubSuccRate = subSuccRate;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopChallengePoints(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/chapointstotal.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 challenge points in total" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 challenge points in total (with at least 20 challenges)</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Total Challenge points</span></td>");
            writer.WriteLine("\t\t<td><span># succ. challenges</span></td>");
            writer.WriteLine("\t\t<td><span># failed challenges</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            double lastChallengePoints = -1.00;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                if (coderStat.chaPoints != lastChallengePoints)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.chaPoints.ToString("N2") + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.succ_challenges + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + (coderStat.total_challenges - coderStat.succ_challenges) + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastChallengePoints = coderStat.chaPoints;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopAverageChallengePoints(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/chapointsavg.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 challenge points in average" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 challenge points in average (with at least 20 challenges)</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Average challenge points</span></td>");
            writer.WriteLine("\t\t<td><span>Total challenge points</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            double lastAverageChallengePoints = -1.00;
            foreach (CoderStat coderStat in coderStatList)
            {
                ++ct;
                double averageChallengePoints = coderStat.chaPoints / coderStat.events;
                if (averageChallengePoints != lastAverageChallengePoints)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + averageChallengePoints.ToString("N3") + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.chaPoints.ToString("N2") + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastAverageChallengePoints = averageChallengePoints;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopChallengePointsInOneEvent(List<Record> challengeInfoList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/chapointsin1event.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 challenge points in one event" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 challenge points in one event</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Division</span></td>");
            writer.WriteLine("\t\t<td><span>Challenge points</span></td>");
            writer.WriteLine("\t\t<td><span># succ. challenges</span></td>");
            writer.WriteLine("\t\t<td><span># failed challenges</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            string lastChallengePoints = null;
            foreach (Record challengeRecord in challengeInfoList)
            {
                writer.WriteLine("\t<tr>");
                ++ct;
                if (!challengeRecord.coder.challengePoints.Equals(lastChallengePoints))
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + challengeRecord.coder.id + ".html\" class=\"" + RatingToString(challengeRecord.coder.newRating, false) + "\">" + challengeRecord.coder.handle + "</a></td>");
                writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + challengeRecord.round.roundID + "&cr=" + challengeRecord.coder.id + "\" class=\"eventText\">" + challengeRecord.round.name + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + challengeRecord.coder.division + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + challengeRecord.coder.challengePoints + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + challengeRecord.coder.succ_challenges + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + challengeRecord.coder.fail_challenges + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastChallengePoints = challengeRecord.coder.challengePoints;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateTopChameleons(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/chameleons.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 chameleons" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 chameleons</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span># changes</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            int lastChange = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                writer.WriteLine("\t<tr>");
                ++ct;
                if (coderStat.colorChangeTimes != lastChange)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + (coderStat.colorChangeTimes - 1) + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastChange = coderStat.colorChangeTimes;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateMostVolatile(List<Record> volRecordList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/volatile.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 most volatile" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 most volatile</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Division</span></td>");
            writer.WriteLine("\t\t<td><span>Volatility</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            int lastVolatility = -1;
            foreach (Record volRecord in volRecordList)
            {
                writer.WriteLine("\t<tr>");
                ++ct;
                if (volRecord.coder.vol != lastVolatility)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + volRecord.coder.id + ".html\" class=\"" + RatingToString(volRecord.coder.newRating, false) + "\">" + volRecord.coder.handle + "</a></td>");
                writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + volRecord.round.roundID + "&cr=" + volRecord.coder.id + "\" class=\"eventText\">" + volRecord.round.name + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + volRecord.coder.division + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + volRecord.coder.vol + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastVolatility = volRecord.coder.vol;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateMostEvents(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/mostevents.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 with most events" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 with most events</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            int lastEvents = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                writer.WriteLine("\t<tr>");
                ++ct;
                if (coderStat.events != lastEvents)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastEvents = coderStat.events;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateMostEventsLastYear(List<CoderStat> coderStatList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/mosteventslastyear.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "Top 20 with most events in last year" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>Top 20 with most events in last year</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t</tr>");

            int ct = 0;
            int lastrank = 0;
            int lastEvents = -1;
            foreach (CoderStat coderStat in coderStatList)
            {
                writer.WriteLine("\t<tr>");
                ++ct;
                if (coderStat.eventsLastYear != lastEvents)
                {
                    lastrank = ct;
                }
                if (lastrank > 20)
                {
                    break;
                }
                writer.WriteLine("\t\t<td><span>" + lastrank + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + coderStat.id + ".html\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.eventsLastYear + "</span></td>");
                writer.WriteLine("\t</tr>");
                lastEvents = coderStat.eventsLastYear;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateDivWinners(List<Record> divWinnerList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/divwinners.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "All division winners" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"firstTable\">");
            writer.WriteLine("<h2>All division 1 winners</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Date</span></td>");
            writer.WriteLine("\t</tr>");

            foreach (Record divWinnerRecord in divWinnerList)
            {
                if (divWinnerRecord.coder.division == 1)
                {
                    writer.WriteLine("\t<tr>");
                    writer.WriteLine("\t\t<td><a href=\"../member/" + divWinnerRecord.coder.id + ".html\" class=\"" + RatingToString(divWinnerRecord.coder.newRating, false) + "\">" + divWinnerRecord.coder.handle + "</a></td>");
                    writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + divWinnerRecord.round.roundID + "&cr=" + divWinnerRecord.coder.id + "\" class=\"eventText\">" + divWinnerRecord.round.name + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"dateText\">" + divWinnerRecord.round.date + "</span></td>");
                    writer.WriteLine("\t</tr>");
                }
            }
            writer.WriteLine("</tbody></table></div>");

            writer.WriteLine("<div class=\"secondTable\">");
            writer.WriteLine("<h2>All division 2 winners</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Date</span></td>");
            writer.WriteLine("\t</tr>");

            foreach (Record divWinnerRecord in divWinnerList)
            {
                if (divWinnerRecord.coder.division == 2)
                {
                    writer.WriteLine("\t<tr>");
                    writer.WriteLine("\t\t<td><a href=\"../member/" + divWinnerRecord.coder.id + ".html\" class=\"" + RatingToString(divWinnerRecord.coder.newRating, false) + "\">" + divWinnerRecord.coder.handle + "</a></td>");
                    writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + divWinnerRecord.round.roundID + "&cr=" + divWinnerRecord.coder.id + "\" class=\"eventText\">" + divWinnerRecord.round.name + "</span></td>");
                    writer.WriteLine("\t\t<td><span class=\"dateText\">" + divWinnerRecord.round.date + "</span></td>");
                    writer.WriteLine("\t</tr>");
                }
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        static void GenerateOnsite(List<Record> onsiteList)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/onsite.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "All Onsite ZJUers" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>All Onsite ZJUers</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Date</span></td>");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t</tr>");

            foreach (Record onsiteRecord in onsiteList)
            {
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><a href=\"../member/" + onsiteRecord.coder.id + ".html\" class=\"" + RatingToString(onsiteRecord.coder.newRating, false) + "\">" + onsiteRecord.coder.handle + "</a></td>");
                writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + onsiteRecord.round.roundID + "&cr=" + onsiteRecord.coder.id + "\" class=\"eventText\">" + onsiteRecord.round.name + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"dateText\">" + onsiteRecord.round.date + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + onsiteRecord.coder.divRank + "</span></td>");
                writer.WriteLine("\t</tr>");
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        public static void GenerateZJUerList(Dictionary<String, CoderStat> stats)
        {
            string dire = Directory.GetCurrentDirectory() + "/stat";
            string rankFile = dire + "/zjuers.html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine("<html><body>");
            writer.WriteLine("</body></html>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + "All ZJUers (with at least one event)" + "</title><link rel=\"stylesheet\" href=\"stat.css\" type=\"text/css\" /></head>");

            writer.WriteLine("<div class=\"rankTable\">");
            writer.WriteLine("<h2>All ZJUers (with at least one event)</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            int ind = 0;
            foreach (String id in stats.Keys)
            {
                if (ind % 8 == 0)
                {
                    writer.WriteLine("\t<tr>");
                }
                writer.WriteLine("\t\t<td><a href=\"../member/" + id + ".html\" class=\"" + RatingToString(stats[id].rating, false) + "\">" + stats[id].handle + "</a></td>");
                if (ind % 8 == 7)
                {
                    writer.WriteLine("\t</tr>");
                }
                ++ind;
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");

            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();
        }

        public static void GenerateStatPages(Dictionary<String, CoderStat> stats, string lastEvent,
            List<Record> ratingRecordList, List<Record> challengeInfoList, List<Record> volRecordList,
            List<Record> divWinnerList, List<Record> onsiteList)
        {
            List<CoderStat> statList = stats.Values.ToList();
            foreach (CoderStat coderStat in statList)
            {
                coderStat.CheckActive(lastEvent);
            }
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByRating));
            GenerateTopRatingPage(statList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByRatingActive));
            GenerateTopRatingActivePage(statList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByMaxRating));
            GenerateTopMaxRatingPage(statList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByMaxRatingActive));
            GenerateTopMaxRatingActivePage(statList);
            GenerateTopRatingRecordPage(ratingRecordList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByChallengeSuccRate));
            GenerateTopChallengeSuccRate(statList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByChallengePoints));
            GenerateTopChallengePoints(statList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByAverageChallengePoints));
            GenerateTopAverageChallengePoints(statList);
            challengeInfoList.Sort(new Comparison<Record>(Record.CompareByChaPoints));
            GenerateTopChallengePointsInOneEvent(challengeInfoList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByColorChangeTimes));
            GenerateTopChameleons(statList);
            volRecordList.Sort(new Comparison<Record>(Record.CompareByVolatility));
            GenerateMostVolatile(volRecordList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByEvents));
            GenerateMostEvents(statList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareByEventsLastYear));
            GenerateMostEventsLastYear(statList);
            divWinnerList.Reverse();
            GenerateDivWinners(divWinnerList);
            onsiteList.Reverse();
            GenerateOnsite(onsiteList);
            statList.Sort(new Comparison<CoderStat>(CoderStat.CompareBySubmissionSuccRate));
            GenerateTopSubmissionSuccRate(statList);
            GenerateZJUerList(stats);
        }

        public static void GenerateHTMLForCoder(CoderStat coderStat, List<Record> coderHistory, Dictionary<String, CoderStat> stats)
        {
            string dire = Directory.GetCurrentDirectory();
            string rankFile = dire + "\\member\\" + coderStat.id + ".html";
            FileStream stream = new FileStream(rankFile, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("<html><body>");
            writer.WriteLine("<head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
            writer.WriteLine("<title>" + coderStat.handle + "</title><link rel=\"stylesheet\" href=\"member.css\" type=\"text/css\" /></head>");
            writer.WriteLine("<h1><a href=\"http://www.topcoder.com/tc?module=MemberProfile&cr=" + coderStat.id + "\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></h1>");

            writer.WriteLine("<div class=\"statDualDiv\">");
            writer.WriteLine("<div class=\"statDiv\">");
            writer.WriteLine("<h2>Personal statistics</h2>");

            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span># events</span></td>");
            writer.WriteLine("\t\t<td><span># events last year</td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/tc?module=MemberProfile&cr=" + coderStat.id + "\" class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</a></td>");
            writer.WriteLine("\t\t<td><span>" + coderStat.events + "</span></td>");
            writer.WriteLine("\t\t<td><span>" + coderStat.eventsLastYear + "</span></td>");
            writer.WriteLine("\t</tr>");
            
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t\t<td><span>Max rating</span></td>");
            writer.WriteLine("\t\t<td><span>Volatility</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.rating + "</span></td>");
            writer.WriteLine("\t\t<td><span class=\"" + RatingToString(coderStat.maxrating, false) + "\">" + coderStat.maxrating + "</span></td>");
            writer.WriteLine("\t\t<td><span>" + coderStat.vol + "</span></td>");
            writer.WriteLine("\t</tr>");

            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span># submits</span></td>");
            writer.WriteLine("\t\t<td><span># solves</span></td>");
            writer.WriteLine("\t\t<td><span>Submission accuracy</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><span>" + coderStat.submits + "</span></td>");
            writer.WriteLine("\t\t<td><span>" + coderStat.solves + "</span></td>");
            writer.WriteLine("\t\t<td><span>" + (coderStat.submits == 0 ? "NaN" : (Convert.ToDouble(coderStat.solves) / Convert.ToDouble(coderStat.submits) * 100.0).ToString("N2") + "%") + "</span></td>");
            writer.WriteLine("\t</tr>");

            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span># challenges</span></td>");
            writer.WriteLine("\t\t<td><span># successful challenges</span></td>");
            writer.WriteLine("\t\t<td><span>Challenge accuracy</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("\t<tr>");
            writer.WriteLine("\t\t<td><span>" + coderStat.total_challenges + "</span></td>");
            writer.WriteLine("\t\t<td><span>" + coderStat.succ_challenges + "</span></td>");
            writer.WriteLine("\t\t<td><span>" + (coderStat.total_challenges == 0 ? "NaN" : (Convert.ToDouble(coderStat.succ_challenges) / Convert.ToDouble(coderStat.total_challenges) * 100.0).ToString("N2") + "%") + "</span></td>");
            writer.WriteLine("\t</tr>");
            writer.WriteLine("</tbody></table></div>");

            writer.WriteLine("<div class=\"dualDiv\">");
            writer.WriteLine("<h2>Cooccurrence</h2>");

            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            List<String> coderList = new List<string>();
            foreach (string coderID in coderStat.win.Keys)
            {
                coderList.Add(coderID);
            }
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Handle</span></td>");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t\t<td><span># cooccurrence</span></td>");
            writer.WriteLine("\t\t<td><span>Win / Lose / Deuce</span></td>");
            writer.WriteLine("\t\t<td><span></span></td>");
            writer.WriteLine("\t</tr>");
            for (int i = 0; i < 10 && coderList.Count != 0; ++i)
            {
                int maxcooc = 0;
                string bestcoderID = null;
                foreach (string coderID in coderList)
                {
                    if (coderStat.win[coderID] + coderStat.lose[coderID] + coderStat.deuce[coderID] > maxcooc)
                    {
                        maxcooc = coderStat.win[coderID] + coderStat.lose[coderID] + coderStat.deuce[coderID];
                        bestcoderID = coderID;
                    }
                }
                writer.WriteLine("\t<tr>");
                writer.WriteLine("\t\t<td><a href=\"" + bestcoderID + ".html\" class=\"" + RatingToString(stats[bestcoderID].rating, false) + "\">" + stats[bestcoderID].handle + "</a></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(stats[bestcoderID].rating, false) + "\">" + stats[bestcoderID].rating + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + maxcooc + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + coderStat.win[bestcoderID] + " / " + coderStat.lose[bestcoderID] + " / " + coderStat.deuce[bestcoderID] + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../dual.php?handle1=" + coderStat.handle + "&handle2=" + stats[bestcoderID].handle + "\">See dual between <span class=\"" + RatingToString(coderStat.rating, false) + "\">" + coderStat.handle + "</span> and <span class=\"" + RatingToString(stats[bestcoderID].rating, false) + "\">" + stats[bestcoderID].handle + "</span></a></td>");
                writer.WriteLine("\t</tr>");
                coderList.Remove(bestcoderID);
            }
            writer.WriteLine("</tbody></table></div>");

            coderHistory.Reverse();
            writer.WriteLine("<div class=\"historyDiv\">");
            writer.WriteLine("<h2>Competition history</h2>");
            writer.WriteLine("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody>");
            writer.WriteLine("\t<tr class=\"titleLine\">");
            writer.WriteLine("\t\t<td><span>Event</span></td>");
            writer.WriteLine("\t\t<td><span>Div</span></td>");
            writer.WriteLine("\t\t<td><span>Rank</span></td>");
            writer.WriteLine("\t\t<td><span>Points</span></td>");
            writer.WriteLine("\t\t<td><span>Level 1</span></td>");
            writer.WriteLine("\t\t<td><span>Level 2</span></td>");
            writer.WriteLine("\t\t<td><span>Level 3</span></td>");
            writer.WriteLine("\t\t<td><span>Challenge</span></td>");
            writer.WriteLine("\t\t<td><span>Challenge Points</span></td>");
            writer.WriteLine("\t\t<td><span>Rating</span></td>");
            writer.WriteLine("\t\t<td><span>Rating Change</span></td>");
            writer.WriteLine("\t\t<td><span>Volatility</span></td>");
            writer.WriteLine("\t</tr>");
            foreach (Record record in coderHistory)
            {
                writer.WriteLine("\t<tr>");
                //writer.WriteLine("\t\t<td><a href=\"http://www.topcoder.com/stat?c=coder_room_stats&rd=" + record.round.roundID + "&cr=" + record.coder.id + "\" class=\"eventText\">" + record.round.name + "</span></td>");
                writer.WriteLine("\t\t<td><a href=\"../rank/" + record.round.roundID + ".html\" class=\"eventText\">" + record.round.name + "</a></td>");
                writer.WriteLine("\t\t<td><span>" + record.coder.division + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + record.coder.divRank + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + record.coder.finalPoints + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + StatusToString(record.coder.problemStatus[0]) + "\">" + (record.coder.problemStatus[0].Equals("Passed System Test") ? record.coder.problemPoints[0] : record.coder.problemStatus[0]) + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + StatusToString(record.coder.problemStatus[1]) + "\">" + (record.coder.problemStatus[1].Equals("Passed System Test") ? record.coder.problemPoints[1] : record.coder.problemStatus[1]) + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + StatusToString(record.coder.problemStatus[2]) + "\">" + (record.coder.problemStatus[2].Equals("Passed System Test") ? record.coder.problemPoints[2] : record.coder.problemStatus[2]) + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + (record.coder.succ_challenges > 0 ? "+" : "") + record.coder.succ_challenges + "/" + (record.coder.fail_challenges > 0 ? "-" : "") + record.coder.fail_challenges + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + ChallengeScoreToString(record.coder.challengePoints) + "\">" + record.coder.challengePoints + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingToString(record.coder.newRating, false) + "\">" + record.coder.newRating + "</span></td>");
                writer.WriteLine("\t\t<td><span class=\"" + RatingChangeToString(record.coder.newRating - record.coder.oldRating) + "\">" + RatingChangeToImage(record.coder.newRating - record.coder.oldRating) + Math.Abs(record.coder.newRating - record.coder.oldRating) + "</span></td>");
                writer.WriteLine("\t\t<td><span>" + record.coder.vol + "</span></td>");
                writer.WriteLine("\t</tr>");
            }
            writer.WriteLine("</tbody></table></div>");
            writer.WriteLine("<div class=\"backDiv\">");
            writer.WriteLine("<h3><a href=\"../ZJUerXTCer.html\" class=\"backLink\">Back to homepage</h3>");
            writer.WriteLine("</div>");
        
            writer.WriteLine("</body></html>");
            writer.Close();
            stream.Close();            
        }

        public static void GenerateHTMLForAllCoder(Dictionary<CoderStat, List<Record>> coderInfo, Dictionary<String, CoderStat> stats)
        {
            foreach (CoderStat coderStat in coderInfo.Keys)
            {
                List<Record> coderHistory = coderInfo[coderStat];
                GenerateHTMLForCoder(coderStat, coderHistory, stats);
            }
        }
    }
}
