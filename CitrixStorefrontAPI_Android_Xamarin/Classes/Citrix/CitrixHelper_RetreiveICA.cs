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
        public static string RetreiveICA(string WebURL, CitrixAuthCredential Creds,CitrixApplicationInfo AppInfo)
        {
            string SFURL = string.Format("{0}/{1}", WebURL, AppInfo.AppLaunchURL);
            bool IsSSL = false;

            if (SFURL.ToLower().IndexOf("https:") != -1)
            {
                IsSSL = true;
            }
            else
            {
                IsSSL = false;
            }

            var _ica = GetICAFromStoreFront(SFURL, Creds, IsSSL);

            return _ica;

        }
        public static string RetreiveICA(string Server, string WebLocation, CitrixAuthCredential Creds,CitrixApplicationInfo AppInfo, bool UseSSL)
        {
            string SFURL = null;
            if (UseSSL)
            {
                SFURL = string.Format("https://{0}/{1}/{2}", Server, WebLocation, AppInfo.AppLaunchURL);
            }
            else
            {
                SFURL = string.Format("http://{0}/{1}/{2}", Server, WebLocation,AppInfo.AppLaunchURL);
            }
            var _ica = GetICAFromStoreFront(SFURL, Creds, UseSSL);

            return _ica;
        }
        private static string GetICAFromStoreFront(string SFURL, CitrixAuthCredential Creds, bool UseSSL)
        {
            RestClient _rc = new RestClient(SFURL);
            RestRequest _getICAReq = new RestRequest(Method.GET);

            if (UseSSL)
            {
                _getICAReq.AddHeader("X-Citrix-IsUsingHTTPS", "Yes");
            }
            else
            {
                _getICAReq.AddHeader("X-Citrix-IsUsingHTTPS", "No");
            }

            _getICAReq.AddHeader("Accept", "application/octet-stream");
            _getICAReq.AddHeader("Csrf-Token", Creds.CSRFToken);
            _getICAReq.AddCookie("csrftoken", Creds.CSRFToken);
            _getICAReq.AddCookie("asp.net_sessionid", Creds.SessionID);
            _getICAReq.AddCookie("CtxsAuthId", Creds.AuthToken);

            IRestResponse _getICAResp = _rc.Execute(_getICAReq);

            string _icaFile = _getICAResp.Content;

            return _icaFile;
        }
    }
}
