using Android.App;
using Android.Widget;
using Android.OS;

namespace Xamarin.Picasso.Test
{
	[Activity(Label = "Xamarin.Picasso.Test", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);
			ImageView imageView = FindViewById<ImageView>(Resource.Id.imageView1);

			//Xam.Picasso

			button.Click += delegate {
				Xam.Picasso.Picasso.With(this)
				   .Load("http://k30.kn3.net/taringa/B/2/B/2/F/2/RICKOSOSA/DE0.png")
				   .Into(imageView);
			};
		}
	}
}

