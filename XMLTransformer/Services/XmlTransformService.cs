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

        public string Transform(string sourceXml, string transformXml, string transformXml2)
        {
            var res = Transform(sourceXml, transformXml);

            if (!string.IsNullOrWhiteSpace(transformXml2))
            {
                // After the first Transform, a BOM (Byte Order Mark) gets added to the start of the text.
                // We need to remove the BOM before we can transform a second time.
                res = RemoveBOM(res);
                res = Transform(res, transformXml2);
            }
            
            return res;
        }

        private string RemoveBOM(string xml)
        {
            // https://stackoverflow.com/questions/17795167/xml-loaddata-data-at-the-root-level-is-invalid-line-1-position-1
            string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (xml.StartsWith(byteOrderMarkUtf8))
            {
                xml = xml.Remove(0, byteOrderMarkUtf8.Length);
            }

            return xml;
        }
    }
}