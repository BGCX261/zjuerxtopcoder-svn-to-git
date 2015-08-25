using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ZJUerXTopCoder
{
    class XmlHelper
    {
        public static Dictionary<string, string> ParseXml(XmlNode root)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (XmlNode node in root.ChildNodes)
            {
                string name = node.Name;
                string val;
                if (node.FirstChild == null)
                {
                    val = "";
                }
                else
                {
                    val = node.FirstChild.Value;
                }
                dict[name] = val;
            }
            return dict;
        }

        public static XmlDocument LoadXML(string URL, string xmlFileName)
        {
            XmlDocument xml = new XmlDocument();
            string dire = Directory.GetCurrentDirectory();
            string xmlFile = dire + "\\XML\\" + xmlFileName;
            if (File.Exists(xmlFile))
            {
                FileStream stream = new FileStream(xmlFile, FileMode.Open);
                xml.Load(stream);
                stream.Close();
            }
            else
            {
                try
                {
                    xml.Load(URL);
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Date Feed Not Found.");
                    xml = null;
                    return xml;
                }
                Console.WriteLine("Date Feed Updated.");
                FileStream stream = new FileStream(xmlFile, FileMode.Create);
                xml.Save(stream);
                stream.Close();
            }
            return xml;
        }
    }
}
