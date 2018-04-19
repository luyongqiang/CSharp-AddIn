using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Syncfusion.Addin.Utility
{
    /// <summary>
    /// XML文件转换
    /// </summary>
    public static class XmlConvertor
    {
        /// <summary>
        /// 序列号对像为字符串 serialize an object to string.
        /// </summary>
        /// <param name="obj">
        ///  对像 the object.
        /// </param>
        /// <returns>
        /// 返回序列化字符串  the serialized string.
        /// </returns>
        public static string ObjectToXml(object obj)
        {
            return XmlConvertor.ObjectToXml(obj, false);
        }

        /// <summary>
        /// 序列号对像为字符串  serialize an object to string.
        /// </summary>
        /// <param name="obj">
        /// 对像 the object.
        /// </param>
        /// <param name="toBeIndented">
        /// 是否被缩进。     whether to be indented.
        /// </param>
        public static string ObjectToXml(object obj, bool toBeIndented)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            String content = String.Empty;
            UTF8Encoding encoding = new UTF8Encoding(false);
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, encoding))
                {
                    xmlTextWriter.Formatting = toBeIndented ? Formatting.Indented : Formatting.None;
                    xmlSerializer.Serialize(xmlTextWriter, obj);

                    content = encoding.GetString(memoryStream.ToArray());
                }
            }
            return content;
        }

        /// <summary>
        /// 反序列化一个对象  deserialize string.to an object.
        /// </summary>
        /// <param name="type">
        /// the type of the object.
        /// </param>
        /// <param name="xml">
        /// 字符串需要反序列化。
        /// </param>
        /// <returns>
        /// 反序列化的对象。
        /// </returns>
        public static object XmlToObject(Type type, string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            object obj = null;
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = new XmlTextReader(stringReader))
                {
                    obj = xmlSerializer.Deserialize(xmlReader);
                }
            }
            return obj;
        }
    }
}