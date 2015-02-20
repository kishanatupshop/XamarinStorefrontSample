
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace CitrixStorefrontAPI_Android_Xamarin
{
	[Activity (Label = "ListApplicationsActivity")]			
	public class ListApplicationsActivity : Activity
	{
		List<CitrixApplicationInfo> _applications = null;
		CitrixAuthCredential _creds = null;

		//[Android.Runtime.Register("DIRECTORY_DOWNLOADS")]
		//public static string DirectoryDownloads { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.ListApplications);

			string _credsString = Intent.GetStringExtra("authcreds");

			this._creds = Newtonsoft.Json.JsonConvert.DeserializeObject<CitrixAuthCredential> (_credsString);

			this._applications = CitrixHelper.ListResources(@"http://devchallenge.citrixcloud.net/citrix/citrixdevweb",_creds,false);

			var _adp = new CitrixApplicationsAdapter (this, this._applications);

			ListView lv = (ListView)FindViewById (Resource.Id.listView1);

			//
			lv.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				CitrixApplicationInfo _appInfo = this._applications[e.Position];
				//launch URL.
				var _ica = CitrixHelper.RetreiveICA(@"http://devchallenge.citrixcloud.net/citrix/citrixdevweb",this._creds,_appInfo);

				var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);


				if (!path.Exists())
				{
					path.Mkdirs();
				}

				//var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
				string _icaFile = string.Format(@"{0}/test.ica",path,Guid.NewGuid().ToString());
				if ( File.Exists(_icaFile ))
				{
					File.Delete(_icaFile);
				}

				File.Create(_icaFile);
				FileInfo _fi = new FileInfo(_icaFile);

				Intent launchIcaFileIntent = new Intent();
				launchIcaFileIntent.SetAction(Intent.ActionView);
				launchIcaFileIntent.AddFlags(ActivityFlags.ClearTop);
				launchIcaFileIntent.SetDataAndType(Android.Net.Uri.Parse("file://" + _fi.FullName), "application/x-ica");
				StartActivity(launchIcaFileIntent);

				Console.WriteLine(_ica);
			
			};

			lv.Adapter = _adp;
		}
	}
}

