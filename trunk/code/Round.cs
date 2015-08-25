using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace ZJUerXTopCoder
{
    class Round
    {
        public string roundID;
        public string name;
        public string type;
        public string fullname;
        public string date;
        public int roundIndex;
        public static int ind = 0;

        public static Round ParseRoundWithXML(XmlNode node)
        {
            Round round = new Round();
            Dictionary<string, string> dict = XmlHelper.ParseXml(node);
            round.roundID = dict["round_id"];
            round.name = dict["short_name"];
            round.name = Regex.Replace(round.name, "Single Round Match", "SRM");
            round.type = dict["round_type_desc"];
            round.fullname = dict["full_name"];
            round.date = dict["date"];
            round.roundIndex = ind++;
            return round;
        }
    }
}
