
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MediaPlayer;

namespace MusicDemo
{
    public partial class MusicDemoController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public MusicDemoController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public MusicDemoController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public MusicDemoController () : base("MusicDemoController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        MPMusicPlayerController _musicPlayer;
        MPMediaPickerController _mediaController;
        MediaPickerDelegate _mpDelegate;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            _musicPlayer = new MPMusicPlayerController ();
            _musicPlayer.Volume = volumeSlider.Value;
            _mediaController = new MPMediaPickerController (MPMediaType.MPMediaTypeMusic);
            _mediaController.AllowsPickingMultipleItems = false;
            _mpDelegate = new MediaPickerDelegate (this);
            _mediaController.Delegate = _mpDelegate;
            
            volumeSlider.ValueChanged += delegate { _musicPlayer.Volume = volumeSlider.Value; };
            
            open.Clicked += (o, e) => { this.PresentModalViewController (_mediaController, true); };
            
            play.Clicked += (o, e) => { _musicPlayer.Play (); };
            
            pause.Clicked += (o, e) => { _musicPlayer.Pause (); };
            
            stop.Clicked += (o, e) => { _musicPlayer.Stop (); };
        }

        public class MediaPickerDelegate : MPMediaPickerControllerDelegate
        {
            MusicDemoController _viewController;

            public MediaPickerDelegate (MusicDemoController viewController) : base()
            {
                _viewController = viewController;
            }

            public override void MediaItemsPicked (MPMediaPickerController sender, MPMediaItemCollection mediaItemCollection)
            {
                _viewController._musicPlayer.SetQueue (mediaItemCollection);
                _viewController.DismissModalViewControllerAnimated (true);
                
                MPMediaItem mediaItem = mediaItemCollection.Items[0];
                
                //see MPMediaItem.h for various string property names (search for MPMediaItem.h in Mac Spotlight)
                
                string artist = mediaItem.ValueForProperty ("artist").ToString ();
                string title = mediaItem.ValueForProperty ("title").ToString ();
                
                _viewController.artistLabel.Text = artist;
                _viewController.titleLabel.Text = title;
            }

            public override void MediaPickerDidCancel (MPMediaPickerController sender)
            {
                _viewController.DismissModalViewControllerAnimated (true);
            }
        }  
    }
}

