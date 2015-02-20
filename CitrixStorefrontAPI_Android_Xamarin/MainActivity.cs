using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace CitrixStorefrontAPI_Android_Xamarin
{
	[Activity (Label = "CitrixStorefrontAPI_Android_Xamarin", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			//Button button = FindViewById<Button> (Resource.Id);
			
			//button.Click += delegate {
			//	button.Text = string.Format ("{0} clicks!", count++);
			//};
			Button _loginButton = FindViewById<Button>(Resource.Id.btnLogin);
			//Get the SF URL
			EditText _storefrontURLEt = FindViewById<EditText> (Resource.Id.etSFURL);
			string _sfURL = _storefrontURLEt.Text;
			//get the Username
			EditText _sfUsernameEt = FindViewById<EditText> (Resource.Id.etUsername);
			string _sfUsername = _sfUsernameEt.Text;
			//get the password
			EditText _sfPasswordEt = FindViewById<EditText> (Resource.Id.etPassword);
			string _sfPassword = _sfPasswordEt.Text;
			//get the domain
			EditText _sfDomainEt = FindViewById<EditText> (Resource.Id.etDomain);
			string _sfDomain = _sfDomainEt.Text;

			_loginButton.Click += delegate {
				//perform login and get credential object back
				var _creds = CitrixHelper.AuthenticateWithPost(_sfURL,_sfUsername,_sfPassword,_sfDomain);
				//serialize cred object into json string and pass that to new intent
				string _authJson = Newtonsoft.Json.JsonConvert.SerializeObject(_creds);
				//create the intent to list the available resources
				Intent _listResourcesIntent = new Intent(this,typeof(ListApplicationsActivity));

				_listResourcesIntent.PutExtra("authcreds",_authJson);
				StartActivity(_listResourcesIntent);


			};
		}
	}
}


