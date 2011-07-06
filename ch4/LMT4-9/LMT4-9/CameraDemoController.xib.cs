
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MediaPlayer;

namespace LMT49
{
    public partial class CameraDemoController : UIViewController
    {
        UIImagePickerController _picker;
        PickerDelegate _pickerDel;
        UIActionSheet _actionSheet;
        MPMoviePlayerController _mp;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public CameraDemoController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public CameraDemoController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public CameraDemoController () : base("CameraDemoController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            _picker = new UIImagePickerController ();
            _pickerDel = new PickerDelegate (this);
            _picker.Delegate = _pickerDel;
            
            _actionSheet = new UIActionSheet ();
            _actionSheet.AddButton ("Library");
            _actionSheet.AddButton ("Camera");
            _actionSheet.AddButton ("Cancel");
            _actionSheet.CancelButtonIndex = 2;
            _actionSheet.Delegate = new ActionSheetDelegate (this);
            
            showPicker.TouchUpInside += delegate { _actionSheet.ShowInView (this.View); };
            
            playMovie.Hidden = true;
            
            playMovie.TouchUpInside += delegate {
                if (_mp != null) {
                    View.AddSubview (_mp.View);
                    _mp.SetFullscreen (true, true);
                    _mp.Play ();
                }
            };
        }

        class ActionSheetDelegate : UIActionSheetDelegate
        {
            CameraDemoController _controller;

            public ActionSheetDelegate (CameraDemoController controller)
            {
                _controller = controller;
            }

            void ShowPicker (UIImagePickerControllerSourceType sourceType)
            {
                if (!UIImagePickerController.IsSourceTypeAvailable (sourceType)) {
                    
                    var alert = new UIAlertView ("Image Picker", "Source type not available", null, "Close");
                    alert.Show ();
                    
                } else {
                    
                    _controller._picker.SourceType = sourceType;
                    
                    string[] availableMediaTypes = UIImagePickerController.AvailableMediaTypes (sourceType);
                    string[] requestedMediaTypes = new string[] { "public.image", "public.movie" };
                    List<string> mediaTypes = new List<string> ();
                    
                    foreach (string mediaType in requestedMediaTypes) {
                        if (availableMediaTypes.Contains (mediaType))
                            mediaTypes.Add (mediaType);
                    }
                    
                    _controller._picker.MediaTypes = mediaTypes.ToArray ();
                    
                    _controller.PresentModalViewController (_controller._picker, true);
                }
            }

            public override void Clicked (UIActionSheet actionSheet, int buttonIndex)
            {
                switch (buttonIndex) {
                case 0:
                    ShowPicker (UIImagePickerControllerSourceType.PhotoLibrary);
                    break;
                case 1:
                    ShowPicker (UIImagePickerControllerSourceType.Camera);
                    break;
                }
                actionSheet.DismissWithClickedButtonIndex (buttonIndex, true);
            }
        }

        class PickerDelegate : UIImagePickerControllerDelegate
        {
            CameraDemoController _controller;

            public PickerDelegate (CameraDemoController controller)
            {
                _controller = controller;
            }

            public override void FinishedPickingMedia (UIImagePickerController picker, NSDictionary info)
            {
                picker.DismissModalViewControllerAnimated (true);
                
                string mediaType = info[new NSString ("UIImagePickerControllerMediaType")].ToString ();
                UIImage img = null;
                
                if (mediaType == "public.image") {
                    
                    img = (UIImage)info[new NSString ("UIImagePickerControllerOriginalImage")];
                    _controller.playMovie.Hidden = true;
                    
                } else if (mediaType == "public.movie") {
                    
                    NSUrl videoUrl = (NSUrl)info[new NSString ("UIImagePickerControllerMediaURL")];
                    _controller._mp = new MPMoviePlayerController (videoUrl);
                    img = _controller._mp.ThumbnailImageAt (0, MPMovieTimeOption.NearestKeyFrame);
                    _controller.playMovie.Hidden = false;
                }
                
                if (img != null)
                    _controller.imageView.Image = img;
            }
            
        }
    }
}

