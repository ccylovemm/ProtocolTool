using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace ProtocolTool
{
    class ProtocolExport
    {
        static private List<string> namespaces = new List<string>();
        static private Dictionary<string, XmlNodeList> enums = new Dictionary<string, XmlNodeList>();
        static private Dictionary<string, XmlNodeList> structs = new Dictionary<string, XmlNodeList>();
        static private Dictionary<string, XmlNodeList> c2sFuncs = new Dictionary<string, XmlNodeList>();
        static private Dictionary<string, XmlNodeList> s2cFuncs = new Dictionary<string, XmlNodeList>();

        static public void Export(string fileName)
        {
            string txt = File.ReadAllText(fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(txt);
            XmlNodeList xmlNodeList = xmlDoc.FirstChild.ChildNodes;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Name == "NameSpace")
                {
                    foreach (XmlNode point in xmlNode.ChildNodes)
                    {
                        namespaces.Add(point.Attributes["name"].Value);
                    }
                }
                else if (xmlNode.Name == "TypeDefine")
                {
                    foreach (XmlNode nodes in xmlNode.ChildNodes)
                    {
                        if (xmlNode.Name == "Enum")
                        {
                            foreach (XmlNode point in nodes.ChildNodes)
                            {
                                enums.Add(point.Attributes["name"].Value, point.ChildNodes);
                            }
                        }
                        else if (xmlNode.Name == "Struct")
                        {
                            foreach (XmlNode point in nodes.ChildNodes)
                            {
                                structs.Add(point.Attributes["name"].Value, point.ChildNodes);
                            }
                        }
                    }
                }
                else if (xmlNode.Name == "FuncC2S")
                {
                    foreach (XmlNode point in xmlNode.ChildNodes)
                    {
                        c2sFuncs.Add(point.Attributes["name"].Value, point.ChildNodes);
                    }
                }
                else if (xmlNode.Name == "FuncS2C")
                {
                    foreach (XmlNode point in xmlNode.ChildNodes)
                    {
                        s2cFuncs.Add(point.Attributes["name"].Value, point.ChildNodes);
                    }
                }
            }

            ExportClient(fileName);
            ExportServer(fileName);
            ExportEnum(fileName);
            ExportStruct(fileName);
        }

        static private void ExportClient(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("//************************************************************");
            stringBuilder.AppendLine("// Auto Generated Code By ProtocolTool");
            stringBuilder.AppendLine("// Copyright(c) Cao ChunYang  All rights reserved.");
            stringBuilder.AppendLine("//************************************************************");
            stringBuilder.AppendLine();
            foreach (string name in namespaces)
            {
                stringBuilder.AppendFormat("using {0};", name);
            }
            stringBuilder.AppendLine();
            foreach (KeyValuePair<string , XmlNodeList> value in c2sFuncs)
            {
                stringBuilder.AppendFormat("\tpublic void {0}({1})", value.Key , GetParamStr(value.Value));
                stringBuilder.AppendLine("\t{");
                foreach(XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\t//public {0} {1};", xmlNode.Attributes["type"].Value , xmlNode.Attributes["name"].Value);
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
            }
            foreach (KeyValuePair<string, XmlNodeList> value in s2cFuncs)
            {
                stringBuilder.AppendFormat("\tprivate void {0}({1})", value.Key, GetParamStr(value.Value));
                stringBuilder.AppendLine("\t{");
                foreach (XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\t//public {0} {1};", xmlNode.Attributes["type"].Value, xmlNode.Attributes["name"].Value);
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
            }
            stringBuilder.Append('}');
            stringBuilder.AppendLine();

            if (!Directory.Exists("Out/Client/Code/" + fileName))
            {
                Directory.CreateDirectory("Out/Client/Code/" + fileName);
            }
            FileStream streamClient = new FileStream("Out/Client/Code/" + fileName + "/" + fileName + ".cs", FileMode.Create, FileAccess.Write);
            TextWriter writerClient = new StreamWriter(streamClient);
            writerClient.Write(stringBuilder.ToString());
            writerClient.Close();
            streamClient.Close();
        }

        static private void ExportServer(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("//************************************************************");
            stringBuilder.AppendLine("// Auto Generated Code By ProtocolTool");
            stringBuilder.AppendLine("// Copyright(c) Cao ChunYang  All rights reserved.");
            stringBuilder.AppendLine("//************************************************************");
            stringBuilder.AppendLine();
            foreach (string name in namespaces)
            {
                stringBuilder.AppendFormat("using {0};", name);
            }
            stringBuilder.AppendLine();
            foreach (KeyValuePair<string, XmlNodeList> value in c2sFuncs)
            {
                stringBuilder.AppendFormat("\tpublic void {0}({1})", value.Key, GetParamStr(value.Value));
                stringBuilder.AppendLine("\t{");
                foreach (XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\t//public {0} {1};", xmlNode.Attributes["type"].Value, xmlNode.Attributes["name"].Value);
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
            }
            foreach (KeyValuePair<string, XmlNodeList> value in s2cFuncs)
            {
                stringBuilder.AppendFormat("\tprivate void {0}({1})", value.Key, GetParamStr(value.Value));
                stringBuilder.AppendLine("\t{");
                foreach (XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\t//public {0} {1};", xmlNode.Attributes["type"].Value, xmlNode.Attributes["name"].Value);
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
            }
            stringBuilder.Append('}');
            stringBuilder.AppendLine();

            if (!Directory.Exists("Out/Server/Code/" + fileName))
            {
                Directory.CreateDirectory("Out/Server/Code/" + fileName);
            }
            FileStream streamClient = new FileStream("Out/Server/Code/" + fileName + "/" + fileName + ".cs", FileMode.Create, FileAccess.Write);
            TextWriter writerClient = new StreamWriter(streamClient);
            writerClient.Write(stringBuilder.ToString());
            writerClient.Close();
            streamClient.Close();
        }

        static private void ExportEnum(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            foreach (KeyValuePair<string, XmlNodeList> value in enums)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine("// Auto Generated Code By ProtocolTool");
                stringBuilder.AppendLine("// Copyright(c) Cao ChunYang  All rights reserved.");
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("using system;");
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat("\tpublic Class {0}", value.Key);
                stringBuilder.AppendLine("\t{");
                foreach (XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\tpublic const int {0} {1};", xmlNode.Attributes["name"].Value, xmlNode.Attributes["value"].Value);
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
                stringBuilder.Append('}');
                stringBuilder.AppendLine();

                if (!Directory.Exists("Out/Server/Code/Enum"))
                {
                    Directory.CreateDirectory("Out/Server/Code/Enum");
                }
                FileStream stream = new FileStream("Out/Server/Code/Enum/" + value.Key + ".cs", FileMode.Create, FileAccess.Write);
                TextWriter writer = new StreamWriter(stream);
                writer.Write(stringBuilder.ToString());
                writer.Close();
                stream.Close();
            }
        }

        static private void ExportStruct(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            foreach (KeyValuePair<string, XmlNodeList> value in enums)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine("// Auto Generated Code By ProtocolTool");
                stringBuilder.AppendLine("// Copyright(c) Cao ChunYang  All rights reserved.");
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("using system;");
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat("\tpublic Class {0}", value.Key);
                stringBuilder.AppendLine("\t{");
                foreach (XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\tpublic {0} {1};", xmlNode.Attributes["type"].Value, xmlNode.Attributes["name"].Value);
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
                stringBuilder.Append('}');
                stringBuilder.AppendLine();

                if (!Directory.Exists("Out/Server/Code/Struct"))
                {
                    Directory.CreateDirectory("Out/Server/Code/Struct");
                }
                FileStream stream = new FileStream("Out/Server/Code/Struct/" + value.Key + ".cs", FileMode.Create, FileAccess.Write);
                TextWriter writer = new StreamWriter(stream);
                writer.Write(stringBuilder.ToString());
                writer.Close();
                stream.Close();
            }
        }

        static private string GetParamStr(XmlNodeList nodeList)
        {
            string str = "";
            uint count = 0;
            foreach (XmlNode node in nodeList)
            {
                str += (count > 0 ? " , " : "") + node.Attributes["type"].Value + " " + node.Attributes["name"].Value;
                count++;
            }
            return str;
        }
    }
}
