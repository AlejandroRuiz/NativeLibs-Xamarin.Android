using Android.App;
using Android.Widget;
using Android.OS;
using TV.Danmaku.Ijk.Media.Player;
using Android.Media;
using Android.Content;
using Android.Net;
using System;

namespace IJKMediaFramework.Android.Test
{
	[Activity(Label = "IJKMediaFramework.Android.Test", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, IMediaPlayerOnPreparedListener, IMediaPlayerOnBufferingUpdateListener
	{

		// all possible internal states
		private static int STATE_ERROR = -1;
		private static int STATE_IDLE = 0;
		private static int STATE_PREPARING = 1;
		private static int STATE_PREPARED = 2;
		private static int STATE_PLAYING = 3;
		private static int STATE_PAUSED = 4;
		private static int STATE_PLAYBACK_COMPLETED = 5;

		// mCurrentState is a VideoView object's current state.
		// mTargetState is the state that a method caller intends to reach.
		// For instance, regardless the VideoView object's current state,
		// calling pause() intends to bring the object to a target state
		// of STATE_PAUSED.
		private int mCurrentState = STATE_IDLE;
		private int mTargetState = STATE_IDLE;

		private IMediaPlayer mMediaPlayer = null;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			button.Click += delegate {
				CreatePlayer(global::Android.Net.Uri.Parse("http://cdnapi.kaltura.com/p/243342/sp/24334200/playManifest/entryId/0_uka1msg4/flavorIds/1_vqhfu6uy,1_80sohj7p,1_ry9w1l0b/format/applehttp/protocol/http/a.m3u8"));
			};
		}

		global::Android.Net.Uri mUri;
		int mSeekWhenPrepared;

		void CreatePlayer(global::Android.Net.Uri uri)
		{

			// we shouldn't clear the target state, because somebody might have
			// called start() previously
			release(false);

			mUri = uri;
			mSeekWhenPrepared = 0;

			AudioManager am = (AudioManager)this.GetSystemService(Context.AudioService);
			am.RequestAudioFocus(null, Stream.Music, AudioFocus.Gain);

			mMediaPlayer = createPlayer();
			mMediaPlayer.SetOnPreparedListener(this);
			mMediaPlayer.SetOnBufferingUpdateListener(this);
			mMediaPlayer.SetKeepInBackground(true);
			mMediaPlayer.SetDataSource(this, uri);
			mMediaPlayer.SetAudioStreamType((int)Stream.Music);
			mMediaPlayer.SetScreenOnWhilePlaying(true);
			mMediaPlayer.PrepareAsync();
		}

		IMediaPlayer createPlayer()
		{
			IMediaPlayer mediaPlayer = null;
			IjkMediaPlayer ijkMediaPlayer = null;
			ijkMediaPlayer = new IjkMediaPlayer();

			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryCodec, "mediacodec", 0);

			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryPlayer, "opensles", 0);

			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryPlayer, "overlay-format", IjkMediaPlayer.SdlFccRv32);

			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryPlayer, "framedrop", 1);
			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryPlayer, "start-on-prepared", 0);

			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryFormat, "http-detect-range-support", 0);

			ijkMediaPlayer.SetOption(IjkMediaPlayer.OptCategoryCodec, "skip_loop_filter", 48);

			mediaPlayer = ijkMediaPlayer;
			return mediaPlayer;
		}

		/*
     	* release the media player in any state
     	*/
		public void release(bool cleartargetstate)
		{
			if (mMediaPlayer != null)
			{
				mMediaPlayer.Reset();
				mMediaPlayer.Release();
				mMediaPlayer = null;
				mCurrentState = STATE_IDLE;
				if (cleartargetstate)
				{
					mTargetState = STATE_IDLE;
				}
				AudioManager am = (AudioManager)this.GetSystemService(Context.AudioService);
				am.AbandonAudioFocus(null);
			}
		}

		public void OnPrepared(IMediaPlayer p0)
		{
			mMediaPlayer.Start();
		}

		public void OnBufferingUpdate(IMediaPlayer p0, int p1)
		{
		}
	}
}

