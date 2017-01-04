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
        static private Dictionary<string, XmlNode> enums = new Dictionary<string, XmlNode>();
        static private Dictionary<string, XmlNode> structs = new Dictionary<string, XmlNode>();
        static private Dictionary<string, XmlNode> c2sFuncs = new Dictionary<string, XmlNode>();
        static private Dictionary<string, XmlNode> s2cFuncs = new Dictionary<string, XmlNode>();

        static public void Export(string fileName)
        {
            string txt = File.ReadAllText(fileName);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(txt);
            XmlNodeList xmlNodeList = xmlDoc.LastChild.ChildNodes;
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
                        if (nodes.Name == "Enum")
                        {
                            enums.Add(nodes.Attributes["name"].Value, nodes);
                        }
                        else if (nodes.Name == "Struct")
                        {
                            structs.Add(nodes.Attributes["name"].Value, nodes);
                        }
                    }
                }
                else if (xmlNode.Name == "FuncC2S")
                {
                    foreach (XmlNode point in xmlNode.ChildNodes)
                    {
                        c2sFuncs.Add(point.Attributes["name"].Value, point);
                    }
                }
                else if (xmlNode.Name == "FuncS2C")
                {
                    foreach (XmlNode point in xmlNode.ChildNodes)
                    {
                        s2cFuncs.Add(point.Attributes["name"].Value, point);
                    }
                }
            }

            ExportClient(fileName);
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
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("class {0}\n", fileName);
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine();
            foreach (KeyValuePair<string , XmlNode> value in c2sFuncs)
            {
                stringBuilder.AppendLine("\t /**>");
                stringBuilder.AppendFormat("\t * {0}" , value.Value.Attributes["description"].Value);
                stringBuilder.AppendLine();
                foreach (XmlNode xmlNode in value.Value.ChildNodes)
                {
                    string str = "";
                    if (xmlNode.Attributes["type"].Value.Equals("List"))
                    {
                        str = xmlNode.Attributes["type"].Value + "<" + xmlNode.Attributes["class"].Value + ">" + " " + xmlNode.Attributes["name"].Value;
                    }
                    else
                    {
                        str = xmlNode.Attributes["type"].Value + " " + xmlNode.Attributes["name"].Value;
                    }
                    stringBuilder.AppendFormat("\t @ param {0} ({1})" , str , xmlNode.Attributes["description"].Value);
                    stringBuilder.AppendLine();
                }
                stringBuilder.AppendLine("\t **/");
                stringBuilder.AppendFormat("\tpublic void Send_{0}({1})\n", value.Key , GetParamStr(value.Value.ChildNodes));
                stringBuilder.AppendLine("\t{");
                foreach(XmlNode xmlNode in value.Value.ChildNodes)
                {
                    stringBuilder.AppendFormat("\t\t//public {0} {1};", xmlNode.Attributes["type"].Value , xmlNode.Attributes["name"].Value);
                    stringBuilder.AppendLine();
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
            }
            foreach (KeyValuePair<string, XmlNode> value in s2cFuncs)
            {
                stringBuilder.AppendLine("\t /**>");
                stringBuilder.AppendFormat("\t * {0}", value.Value.Attributes["description"].Value);
                stringBuilder.AppendLine();
                foreach (XmlNode xmlNode in value.Value.ChildNodes)
                {
                    string str = "";
                    if (xmlNode.Attributes["type"].Value.Equals("List"))
                    {
                        str = xmlNode.Attributes["type"].Value + "<" + xmlNode.Attributes["class"].Value + ">" + " " + xmlNode.Attributes["name"].Value;
                    }
                    else
                    {
                        str = xmlNode.Attributes["type"].Value + " " + xmlNode.Attributes["name"].Value;
                    }
                    stringBuilder.AppendFormat("\t @ param {0} ({1})", str, xmlNode.Attributes["description"].Value);
                    stringBuilder.AppendLine();
                }
                stringBuilder.AppendLine("\t **/");
                stringBuilder.AppendFormat("\tprivate void Receive_{0}({1})\n", value.Key, GetParamStr(value.Value.ChildNodes));
                stringBuilder.AppendLine("\t{");
                foreach (XmlNode xmlNode in value.Value)
                {
                    stringBuilder.AppendFormat("\t\t//public {0} {1};", xmlNode.Attributes["type"].Value, xmlNode.Attributes["name"].Value);
                    stringBuilder.AppendLine();
                }
                stringBuilder.AppendLine("\t}");
                stringBuilder.AppendLine();
            }
            stringBuilder.Append('}');
            stringBuilder.AppendLine();

            Save("Out/Client/Code/" + fileName + "/", fileName, stringBuilder);
        }

        static private void ExportEnum(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            foreach (KeyValuePair<string, XmlNode> value in enums)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine("// Auto Generated Code By ProtocolTool");
                stringBuilder.AppendLine("// Copyright(c) Cao ChunYang  All rights reserved.");
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("using system;");
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat("class {0}\n", value.Key);
                stringBuilder.AppendLine("{");
                foreach (XmlNode xmlNode in value.Value.ChildNodes)
                {
                    stringBuilder.AppendFormat("\tpublic const int {0} = {1}; //{2}", xmlNode.Attributes["name"].Value, xmlNode.Attributes["value"].Value , xmlNode.Attributes["description"].Value);
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append('}');
                stringBuilder.AppendLine();

                Save("Out/Client/Code/Enum/", value.Key, stringBuilder);
                Save("Out/Server/Code/Enum/", value.Key, stringBuilder);
            }
        }

        static private void ExportStruct(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            foreach (KeyValuePair<string, XmlNode> value in structs)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine("// Auto Generated Code By ProtocolTool");
                stringBuilder.AppendLine("// Copyright(c) Cao ChunYang  All rights reserved.");
                stringBuilder.AppendLine("//************************************************************");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("using System;");
                stringBuilder.AppendLine("using System.Collections.Generic;");
                stringBuilder.AppendLine();
                stringBuilder.AppendFormat("class {0}\n", value.Key);
                stringBuilder.AppendLine("{");
                foreach (XmlNode xmlNode in value.Value.ChildNodes)
                {
                    string str = "";
                    if (xmlNode.Attributes["type"].Value.Equals("List"))
                    {
                        str = xmlNode.Attributes["type"].Value + "<" + xmlNode.Attributes["class"].Value + ">" + " " + xmlNode.Attributes["name"].Value;
                    }
                    else
                    {
                        str = xmlNode.Attributes["type"].Value + " " + xmlNode.Attributes["name"].Value;
                    }
                    stringBuilder.AppendFormat("\tpublic {0}; //{1}", str , xmlNode.Attributes["description"].Value);
                    stringBuilder.AppendLine();
                }
                stringBuilder.Append('}');
                stringBuilder.AppendLine();

                Save("Out/Client/Code/Struct/", value.Key, stringBuilder);
                Save("Out/Server/Code/Struct/", value.Key, stringBuilder);
            }
        }

        static private void Save(string path , string fileName , StringBuilder stringBuilder)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream stream = new FileStream(path + fileName + ".cs", FileMode.Create, FileAccess.Write);
            TextWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(stringBuilder.ToString());
            writer.Close();
            stream.Close();
        }
        
        static private string GetParamStr(XmlNodeList nodeList)
        {
            string str = "";
            uint count = 0;
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["type"].Value.Equals("List"))
                {
                    str += (count > 0 ? " , " : "") + node.Attributes["type"].Value + "<" + node.Attributes["class"].Value + ">" + " " + node.Attributes["name"].Value;
                }
                else
                {
                    str += (count > 0 ? " , " : "") + node.Attributes["type"].Value + " " + node.Attributes["name"].Value;
                }
                count++;
            }
            return str;
        }
    }
}
