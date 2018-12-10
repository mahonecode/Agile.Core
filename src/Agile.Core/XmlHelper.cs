using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Agile.Core
{
    public class XmlHelper
    {
        #region --增、删、改操作--

        /// <summary>
        /// 追加节点
        /// </summary>
        /// <param name="filePath">XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="xmlNode">XmlNode节点</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, XmlNode xmlNode)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNode n = doc.ImportNode(xmlNode, true);
                xn.AppendChild(n);
                doc.Save(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 从XML文档中读取节点追加到另一个XML文档中
        /// </summary>
        /// <param name="filePath">需要读取的XML文档绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="toFilePath">被追加节点的XML文档绝对路径</param>
        /// <param name="toXPath">范例: @"Skill/First/SkillItem"</param>
        /// <returns></returns>
        public static bool AppendChild(string filePath, string xPath, string toFilePath, string toXPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(toFilePath);
                XmlNode xn = doc.SelectSingleNode(toXPath);

                XmlNodeList xnList = ReadNodes(filePath, xPath);
                if (xnList != null)
                {
                    foreach (XmlElement xe in xnList)
                    {
                        XmlNode n = doc.ImportNode(xe, true);
                        xn.AppendChild(n);
                    }
                    doc.Save(toFilePath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改节点的InnerText的值
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <param name="value">节点的值</param>
        /// <returns></returns>
        public static bool UpdateNodeInnerText(string filePath, string xPath, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlElement xe = (XmlElement)xn;
                xe.InnerText = value;
                doc.Save(filePath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 读取XML文档
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <returns></returns>
        public static XmlDocument LoadXmlDoc(string filePath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                return doc;
            }
            catch
            {
                return null;
            }
        }
        #endregion 增、删、改操作

        #region --读取XML的所有子节点--
        /// <summary>
        /// 读取XML的所有子节点
        /// </summary>
        /// <param name="filePath">XML文件绝对路径</param>
        /// <param name="xPath">范例: @"Skill/First/SkillItem"</param>
        /// <returns></returns>
        public static XmlNodeList ReadNodes(string filePath, string xPath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);
                XmlNode xn = doc.SelectSingleNode(xPath);
                XmlNodeList xnList = xn.ChildNodes;  //得到该节点的子节点
                return xnList;
            }
            catch
            {
                return null;
            }
        }

        #endregion 扩展方法




        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                // open the stream...
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void Save(object obj, string filename)
        {
            FileStream fs = null;
            // serialize it...
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }
    }
}
