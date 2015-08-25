using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace ZJUerXTopCoder
{
    class Coder
    {
        public string handle;
        public int room;
        public string advanced;
        public int succ_challenges;
        public int fail_challenges;
        public int division;
        public int divRank;
        public int oldRating;
        public int newRating;
        public int vol;
        public string challengePoints;
        public string finalPoints;
        public bool newCoder;
        public Color colortype;
        public string[] problemStatus;
        public string[] problemPoints;
        public string id;

        public static int CompareByRank(Coder coder1, Coder coder2)
        {
            return coder1.divRank - coder2.divRank;
        }

        public static int RoomStringToInt(String room)
        {
            while (!(room[0] >= '0' && room[0] <= '9'))
            {
                room = room.Substring(1);
            }
            return Convert.ToInt32(room);
        }

        public static Coder ParseCoderWithXML(XmlNode node)
        {
            try
            {
                Coder coder = new Coder();
                Dictionary<string, string> dict = XmlHelper.ParseXml(node);
                coder.handle = dict["handle"];
                coder.room = RoomStringToInt(dict["room_name"]);
                coder.advanced = dict["advanced"];
                coder.succ_challenges = Convert.ToInt32(dict["challenges_made_successful"]);
                coder.fail_challenges = Convert.ToInt32(dict["challenges_made_failed"]);
                coder.division = Convert.ToInt32(dict["division"]);
                coder.divRank = Convert.ToInt32(dict["division_placed"]);
                coder.oldRating = Convert.ToInt32(dict["old_rating"]);
                coder.newRating = Convert.ToInt32(dict["new_rating"]);
                coder.vol = Convert.ToInt32(dict["new_vol"]);
                coder.challengePoints = dict["challenge_points"];
                coder.finalPoints = dict["final_points"];
                coder.newCoder = dict["num_ratings"].Equals("1");
                if (coder.newCoder)
                {
                    coder.colortype = Color.White;
                    coder.oldRating = 1200;
                }
                else
                {
                    if (coder.oldRating < 900)
                    {
                        coder.colortype = Color.Gray;
                    }
                    else if (coder.oldRating < 1200)
                    {
                        coder.colortype = Color.Green;
                    }
                    else if (coder.oldRating < 1500)
                    {
                        coder.colortype = Color.Blue;
                    }
                    else if (coder.oldRating < 2200)
                    {
                        coder.colortype = Color.Yellow;
                    }
                    else if (coder.oldRating < 3000)
                    {
                        coder.colortype = Color.Red;
                    }
                    else
                    {
                        coder.colortype = Color.Target;
                    }
                }
                coder.problemStatus = new String[3];
                coder.problemPoints = new String[3];
                coder.problemStatus[0] = dict["level_one_status"];
                coder.problemStatus[1] = dict["level_two_status"];
                coder.problemStatus[2] = dict["level_three_status"];
                coder.problemPoints[0] = dict["level_one_final_points"];
                coder.problemPoints[1] = dict["level_two_final_points"];
                coder.problemPoints[2] = dict["level_three_final_points"];
                for (int i = 0; i < 3; ++i)
                {
                    if (coder.problemStatus[i].Equals(String.Empty))
                    {
                        coder.problemStatus[i] = "Unopened";
                    }
                }
                return coder;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
