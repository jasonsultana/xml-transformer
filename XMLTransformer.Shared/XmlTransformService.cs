using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace XMLTransformer.Shared
{
    public class XmlTransformService
    {
        public string Transform(string sourceXml, string transformXml)
        {
            // Remove the BOM (Byte Order Mark) from the source XML if it's there.
            // This will most likely be the case when you try to call this multiple times.
            sourceXml = TrimStart(sourceXml);
            
            using (var document = new XmlTransformableDocument())
            {
                document.PreserveWhitespace = true;
                document.LoadXml(sourceXml);

                using (var transform = new XmlTransformation(transformXml, false, null))
                {
                    if (transform.Apply(document))
                    {
                        var stringBuilder = new StringBuilder();
                        var xmlWriterSettings = new XmlWriterSettings();
                        xmlWriterSettings.Indent = true;
                        xmlWriterSettings.IndentChars = "    ";
                        
                        using (var xmlTextWriter = XmlTextWriter.Create(stringBuilder, xmlWriterSettings))
                        using (var ms = new MemoryStream())
                        {
                            document.Save(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Flush();

                            var bytes = ms.ToArray();
                            return System.Text.Encoding.UTF8.GetString(bytes);
                        }
                    }
                    else
                    {
                        throw new Exception("Transformation failed.");
                    }
                }
            }
        }

        private string TrimStart(string xml)
        {
            int index = xml.IndexOf('<');
            if (index > 0)
            {
                xml = xml.Substring(index, xml.Length - index);
            }

            return xml;
        }
    }
}