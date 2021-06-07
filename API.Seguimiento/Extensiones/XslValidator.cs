using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace API.Seguimiento.Extensiones
{
    public class XslValidator
    {
        private readonly string _xslPath;
        private readonly string _catalogosPath;

        public XslValidator(string xslPath, string catalogosPath)
        {
            _xslPath = xslPath;
            _catalogosPath = catalogosPath;
        }

        public string Validate(string filename, Stream sourceDoc)
        {
            var xsltArguments = CreateXsltArguments(filename);

            XPathDocument myXPathDocument = new XPathDocument(sourceDoc);
            XslTransform myXslTransform = new XslTransform();
            var resolver = new XmlUrlResolver();
            myXslTransform.Load(_xslPath, resolver);

            try
            {
                using (var output = new MemoryStream())
                    myXslTransform.Transform(myXPathDocument, xsltArguments, output);
            }
            catch (XsltException ex)
            {
                return ex.Message;
            }

            return null;
        }

        private XsltArgumentList CreateXsltArguments(string filename)
        {
            XsltArgumentList xsltArguments = null;
            XsltExtension xsltExtension = new XsltExtension(_catalogosPath);
            xsltArguments = new XsltArgumentList();
            xsltArguments.AddExtensionObject("urn:actl-xslt", xsltExtension);
            xsltArguments.AddParam("nombreArchivoEnviado", "", filename);

            return xsltArguments;
        }
    }
}
