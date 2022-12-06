using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Services
{
    public class WorkWithXml
    {
        public static XDocument GetXmlFileFromDictionary(Dictionary<string, object> elements, string rootName = "root")
        {
            XDocument xdoc = new();
            XElement root = new(rootName);
            foreach(var el in elements)
            {
                XElement element = new("parameter");
                element.Add(new XAttribute("name", el.Key));
                element.Add(new XElement("value", el.Value));
                root.Add(element);
            }
            xdoc.Add(root);
            return xdoc;
        }
        public static Dictionary<string, object> GetDictionaryFromXmlFile(XDocument xdoc, string rootName = "root")
        {
            Dictionary<string, object> pairs = new();
            XElement? root = xdoc.Element(rootName);
            if(root == null) { throw new Exception($"Корневой элемент {rootName} не найден."); }
            foreach(var el in root.Elements("parameter"))
            {
                pairs.Add(el.Attribute("name").Value, el.Element("value").Value);
            }
            return pairs;
        }
        public static Dictionary<string, object> GetDictionaryFromXmlFile(Stream stream, string rootName = "root")
        {
            XDocument xdoc = XDocument.Load(stream);
            Dictionary<string, object> pairs = new();
            XElement? root = xdoc.Element(rootName);
            if (root == null) { throw new Exception($"Корневой элемент {rootName} не найден."); }
            foreach (var el in root.Elements("parameter"))
            {
                pairs.Add(el.Attribute("name").Value, el.Element("value").Value);
            }
            return pairs;
        }
    }
}
