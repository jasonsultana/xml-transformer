using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace XMLTransformer.Services
{
    public class XmlTransformService
    {
        public string Transform(string sourceXml, string transformXml)
        {
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
    }
}