using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Xamarin.CardIO.Payment;

namespace Xamarin.CardIO.Test
{
	[Activity(Label = "Xamarin.CardIO.Test", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		const int REQUEST_CODE_CARD_SCAN = 4;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate {
				Intent intent = new Intent(this, typeof(CardIOActivity));

				intent.PutExtra(CardIOActivity.ExtraRequireExpiry, true);
				intent.PutExtra(CardIOActivity.ExtraRequireCvv, true);

				this.StartActivityForResult(intent, REQUEST_CODE_CARD_SCAN);
			};
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (requestCode == REQUEST_CODE_CARD_SCAN)
			{
				if (data == null)
				{

					Toast.MakeText(this, "The user canceled.", ToastLength.Short).Show();
					return;
				}
				var card = (CreditCard)data.GetParcelableExtra(CardIOActivity.ExtraScanResult);
				if (card != null)
				{
					Toast.MakeText(this, $"Card:{card.CardNumber}, Cvv:{card.Cvv}, Exp. Date:{card.ExpiryMonth}/{card.ExpiryYear}", ToastLength.Short).Show();
				}
				else
				{
					Toast.MakeText(this, "The user canceled.", ToastLength.Short).Show();
				}
			}
		}
	}
}

