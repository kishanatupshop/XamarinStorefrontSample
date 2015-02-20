using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;

namespace CitrixStorefrontAPI_Android_Xamarin
{
	public class CitrixApplicationsAdapter : BaseAdapter<CitrixApplicationInfo>
	{
		List<CitrixApplicationInfo> _applications = null;
		Activity _context = null;

		public CitrixApplicationsAdapter (Activity context, List<CitrixApplicationInfo> Applications )
		{
			this._applications = Applications;
			this._context = context;
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View _view = convertView;

			if (_view == null) 
			{
				_view = this._context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem1, null);
			}

			_view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = this._applications [position].AppTitle;

			return _view;
		}

		public override int Count {
			get {
				return this._applications.Count;
			}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override CitrixApplicationInfo this [int index] {
			get {
				return this._applications [index];
			}
		}

		#endregion
	}
}

