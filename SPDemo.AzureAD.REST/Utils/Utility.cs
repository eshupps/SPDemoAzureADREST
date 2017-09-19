using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;
using System.IO;
using SPDemo.AzureAD.REST.Utils;

namespace SPDemo.AzureAD.REST.Utils
{
    public class Utility
    {
        public static string FixupUrl(string Url)
        {
            string endChar = Url.Substring(Url.Length - 1);
            if (endChar != "/")
                Url = Url + "/";
            return Url;
        }

        public static string GetBaseUrl(string Url)
        {
            string _url = Url;

            if (Url.Contains("http"))
            {
                Uri _uri = new Uri(Url);
                _url = _uri.GetLeftPart(UriPartial.Authority);
                _url = _url.Substring(_url.IndexOf("//") + 2);
            }
            return _url;
        }

        public static string GetSPHostUrl(HttpRequest Request)
        {
            string result = string.Empty;

            try
            {
                foreach (string key in Request.QueryString.Keys)
                {
                    if (key.ToLower().Contains("hosturl"))
                    {
                        result = Utility.GetBaseUrl(Request.QueryString[key]);
                        break;
                    }
                    else if (key.ToLower() == "x")
                    {
                        string enc = System.Text.ASCIIEncoding.UTF8.GetString(System.Convert.FromBase64String(Request.QueryString[key]));
                        string[] encs = enc.Split('~');
                        string val = encs[0];
                        if (val.Contains("@"))
                        {
                            string[] vals = val.Split('@');
                            result = vals[1];
                        }
                        else
                            result = val;

                        break;
                    }
                }
            }
            catch { }

            return result;
        }

        public static bool IsTrustedUrl(string Referrer)
        {
            bool _result = false;

            string _trustedUrls = ConfigurationManager.AppSettings["TrustedUrls"];

            if (!string.IsNullOrEmpty(_trustedUrls))
            {
                string[] _urls = _trustedUrls.Split(';');

                for (int i = 0; i < _urls.Length; i++)
                {
                    if (Referrer.ToLower().Contains(_urls[i].ToLower()))
                    {
                        _result = true;
                        break;
                    }
                }
            }

            return _result;
        }

        public static bool IsSystemUrl(string Referrer)
        {
            bool _result = false;

            string _systemUrl = ConfigurationManager.AppSettings["SystemUrl"];

            if (!string.IsNullOrEmpty(_systemUrl))
            {
                if (Referrer.ToLower().Contains(_systemUrl.ToLower()))
                {
                    _result = true;
                }
            }

            return _result;
        }

        public static string DecodeParameter(string parameterValue)
        {
            string decodedValue = parameterValue;
            try
            {
                string encodedValue = parameterValue.Replace(" ", "+");
                decodedValue = System.Text.ASCIIEncoding.UTF8.GetString(System.Convert.FromBase64String(encodedValue));
            }
            catch { }

            return decodedValue;
        }

        public static ResultObject GetValuesFromWResult(string ResultXml)
        {
            ResultObject ro = new Utils.ResultObject();

            try
            {
                if (!string.IsNullOrEmpty(ResultXml))
                {
                    string emailAddress = string.Empty;
                    string accessToken = string.Empty;
                    int start = ResultXml.IndexOf("<AttributeStatement>");
                    int end = ResultXml.IndexOf("</AttributeStatement>") + 21;
                    int diff = end - start;
                    string modXml = ResultXml.Substring(start, diff);
                    XmlReader reader = XmlReader.Create(new StringReader(modXml));
                    XElement doc = XElement.Load(reader);

                    //Get tenant ID
                    var tenantNode = (from n in doc.Descendants("Attribute") where n.Attribute("Name").Value == "http://schemas.microsoft.com/identity/claims/tenantid" select n).FirstOrDefault();
                    var tenantId = tenantNode.Value;

                    //Get user email address
                    var emailNode = (from n in doc.Descendants("Attribute") where n.Attribute("Name").Value == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" select n).FirstOrDefault();
                    if (emailNode == null)
                    {
                        var nameNode = (from n in doc.Descendants("Attribute") where n.Attribute("Name").Value == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" select n).FirstOrDefault();
                        if (nameNode != null)
                            emailAddress = nameNode.Value;
                    }
                    else
                        emailAddress = emailNode.Value;

                    //Get user display name
                    var displayNameNode = (from n in doc.Descendants("Attribute") where n.Attribute("Name").Value == "http://schemas.microsoft.com/identity/claims/displayname" select n).FirstOrDefault();
                    var displayName = displayNameNode.Value;

                    //Get object identifier
                    var objectNode = (from n in doc.Descendants("Attribute") where n.Attribute("Name").Value == "http://schemas.microsoft.com/identity/claims/objectidentifier" select n).FirstOrDefault();
                    var objectId = objectNode.Value;

                    ro.TenantId = tenantId;
                    ro.EmailAddress = emailAddress;
                    ro.DisplayName = displayName;
                    ro.ObjectId = objectId;
                    ro.AccessToken = accessToken;
                }
            }
            catch { }

            return ro;
        }
    }
}