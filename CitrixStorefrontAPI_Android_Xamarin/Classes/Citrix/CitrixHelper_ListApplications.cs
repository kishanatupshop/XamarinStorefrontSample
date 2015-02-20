using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitrixStorefrontAPI_Android_Xamarin
{
    public partial class CitrixHelper
    {
        public static List<CitrixApplicationInfo> ListResources(string WebURL, CitrixAuthCredential Creds, bool UseSSL)
        {
            string SFURL = WebURL;
            bool IsSSL = false;

            if (SFURL.ToLower().IndexOf("https:") != -1)
            {
                IsSSL = true;
            }
            else
            {
                IsSSL = false;
            }

            var _resources = CitrixHelper.GetResourcesFromStoreFront(SFURL, Creds, UseSSL);

            return _resources;
        }
        public static List<CitrixApplicationInfo> ListResources(string Server, string WebLocation, CitrixAuthCredential Creds, bool UseSSL)
        {
            string SFURL = null;
            if (UseSSL)
            {
                SFURL = string.Format("https://{0}/{1}", Server, WebLocation);
            }
            else
            {
                SFURL = string.Format("http://{0}/{1}", Server, WebLocation);
            }

            var _resources = CitrixHelper.GetResourcesFromStoreFront(SFURL, Creds, UseSSL);

            return _resources;

        }
        private static List<CitrixApplicationInfo> GetResourcesFromStoreFront(string SFURL, CitrixAuthCredential Creds, bool UseSSL)
        {
            List<CitrixApplicationInfo> _applicationList = new List<CitrixApplicationInfo>();
            RestClient _rc = new RestClient(SFURL);
            RestRequest _getResourcesReq = new RestRequest(@"Resources/List", Method.POST);

            if (UseSSL)
            {
                _getResourcesReq.AddHeader("X-Citrix-IsUsingHTTPS", "Yes");
            }
            else
            {
                _getResourcesReq.AddHeader("X-Citrix-IsUsingHTTPS", "No");
            }

            _getResourcesReq.AddHeader("Accept", "application/json");
            _getResourcesReq.AddHeader("Csrf-Token", Creds.CSRFToken);
            _getResourcesReq.AddCookie("csrftoken", Creds.CSRFToken);
            _getResourcesReq.AddCookie("asp.net_sessionid", Creds.SessionID);
            _getResourcesReq.AddCookie("CtxsAuthId", Creds.AuthToken);

            IRestResponse _resourceListResp = _rc.Execute(_getResourcesReq);

            string json = _resourceListResp.Content;

            JObject a = JObject.Parse(json);
            JArray resources = (JArray)a["resources"];

            foreach (var o in resources)
            {
                CitrixApplicationInfo _appInfo = new CitrixApplicationInfo();
                _appInfo.AppTitle = o["name"].ToString();
                try
                {
                    _appInfo.AppDesc = o["description"].ToString();
                }
                catch (Exception e)
                {
                    _appInfo.AppDesc = "";
                }
                _appInfo.AppIcon = o["iconurl"].ToString();
                _appInfo.AppLaunchURL = o["launchurl"].ToString();
                _appInfo.ID = o["id"].ToString();
                _applicationList.Add(_appInfo);
            }

            return _applicationList;
        }
        
    }
}
