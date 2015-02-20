using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CitrixStorefrontAPI_Android_Xamarin
{
    public partial class CitrixHelper
    {
        public static CitrixAuthCredential AuthenticateWithPost(string WebURL, string Username, string Password, string Domain)
        {
            string SFURL = WebURL;
            bool IsSSL = false;
            if ( SFURL.ToLower().IndexOf("https:") != -1)
            {
                IsSSL = true;
            }
            else
            {
                IsSSL = false;
            }

            var _creds = GetCredentialsFromStoreFront(SFURL, Username, Password, Domain, IsSSL);

            return _creds;

        }
        public static CitrixAuthCredential AuthenticateWithPost(string Server, string WebLocation, string Username, string Password, string Domain, bool IsSSL)
        {
            string SFURL = null;
            if (IsSSL)
            {
                SFURL = string.Format("https://{0}/{1}", Server, WebLocation);
            }
            else
            {
                SFURL = string.Format("http://{0}/{1}", Server, WebLocation);
            }

            var _creds = GetCredentialsFromStoreFront(SFURL, Username, Password, Domain, IsSSL);

            return _creds;

        }
        private static CitrixAuthCredential GetCredentialsFromStoreFront(string SFURL,string Username, string Password, string Domain, bool IsSSL)
        {
            CitrixAuthCredential _sfCredential = null;

            Dictionary<string, string> _returnedValues = new Dictionary<string, string>();
            string _csrfToken = Guid.NewGuid().ToString();
            string _aspnetSessionID = Guid.NewGuid().ToString();

            string _username = Username;
            string _password = Password;
            string _domain = Domain;

            string _authenticationBody = string.Format("username={0}\\{1}&password={2}", _domain, _username, _password);

            RestClient _rc = new RestClient(SFURL);
            //set the cookie container
            _rc.CookieContainer = new System.Net.CookieContainer();

            RestRequest _authReq = new RestRequest("/PostCredentialsAuth/Login", Method.POST);
            if (IsSSL)
            {
                SFURL = "https://" + SFURL;
                _authReq.AddHeader("X-Citrix-IsUsingHTTPS", "Yes");
            }
            else
            {
                _authReq.AddHeader("X-Citrix-IsUsingHTTPS", "No");
            }
            _authReq.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            _authReq.AddHeader("Csrf-Token", _csrfToken);
            _authReq.AddCookie("csrtoken", _csrfToken);
            _authReq.AddCookie("asp.net_sessionid", _aspnetSessionID);
            _authReq.AddParameter("application/x-www-form-urlencoded", _authenticationBody, ParameterType.RequestBody);

            RestSharp.IRestResponse _authResp = _rc.Execute(_authReq);
            //parse cookie values

            if (_authResp.ResponseStatus == ResponseStatus.Error)
            {
                throw new Exception(String.Format("Error: {0}", _authResp.ErrorMessage));
            }
            else
            {
                string _returnedContent = _authResp.Content;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(_returnedContent);



				/*
<?xml version="1.0" encoding="UTF-8"?>
<AuthenticationStatus>
 <Result>success</Result>
 <AuthType>PostCredentials</AuthType>
</AuthenticationStatus>
*/

//                string _jsonString = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
  //              Newtonsoft.Json.Linq.JObject _rootXmlObject = Newtonsoft.Json.Linq.JObject.Parse(_jsonString);
    //            JObject _authenticationStatus = (JObject)_rootXmlObject["AuthenticationStatus"];
      //          JValue _authResult = (JValue)_authenticationStatus["Result"];

        //        if (_authResult.Value.ToString().ToLower() == "success")

				XmlNodeList _statusNodeList = doc.GetElementsByTagName("Result");
				if (_statusNodeList.Count == 1) 
				{
					if (_statusNodeList [0].InnerText.ToLower () == "success") 
					{
						//Restsharp is not returning the headers correctly. So let's parse them
						//manually through the header property
						foreach (var header in _authResp.Headers.Where(i => i.Name == "Set-Cookie"))
						{
							string[] cookieValues = header.Value.ToString().Split(',');
							foreach (string cookieValue in cookieValues)
							{
								string[] cookieElements = cookieValue.Split(';');
								string[] keyValueElements = cookieElements[0].Split('=');
								_returnedValues.Add(keyValueElements[0], keyValueElements[1]);
							}
						}

						_sfCredential = new CitrixAuthCredential
						{
							AuthToken = _returnedValues["CtxsAuthId"].ToString(),
							CSRFToken = _returnedValues["CsrfToken"].ToString(),
							SessionID = _returnedValues["ASP.NET_SessionId"].ToString()
						};
					}
				}

            }
            return _sfCredential;
        }
        
        private void ParseXML(string XML)
        {

        }
    }


}
