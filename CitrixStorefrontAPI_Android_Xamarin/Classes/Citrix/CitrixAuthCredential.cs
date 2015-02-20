using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitrixStorefrontAPI_Android_Xamarin
{
    public class CitrixAuthCredential
    {
        public string AuthToken { get; set; }
        public string SessionID { get; set; }
        public string CSRFToken { get; set; }

        public CitrixAuthCredential()
        {
            this.AuthToken = null;
            this.SessionID = null;
            this.CSRFToken = null;
        }
    }
}
