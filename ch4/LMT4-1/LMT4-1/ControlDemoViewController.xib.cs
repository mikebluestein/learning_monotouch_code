
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace LMT41
{
    public partial class ControlDemoViewController : UIViewController
    {
        UILabel _testLabel;  
        UISegmentedControl _segmentedControl;      
        UISlider _slider;   
        UISwitch _switch;
        
        string _text;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public ControlDemoViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public ControlDemoViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public ControlDemoViewController () : base("ControlDemoViewController", null)
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
            
            //CreateSegmentedControl ();      
            //CreateUISlider ();     
            CreateUISwitch ();
        }

        void CreateSegmentedControl ()
        {
//            _segmentedControl = new UISegmentedControl (new object[] { "one", "two", "three", "four" });
//            _segmentedControl.ControlStyle = UISegmentedControlStyle.Bezeled;
//            _segmentedControl.TintColor = UIColor.Black;
//            _segmentedControl.SetImage (UIImage.FromFile ("Star.png"), 0);
//            _segmentedControl.SelectedSegment = 0;
//            _segmentedControl.Frame = new RectangleF (10, 10, View.Frame.Width - 20, 50);
            
            _testLabel = new UILabel { Frame = new RectangleF (10, 200, 100, 50) };
            
            _segmentedControl = new UISegmentedControl (new object[] { UIImage.FromFile ("Star.png"), "two", "three", "four" });
            _segmentedControl.ControlStyle = UISegmentedControlStyle.Bezeled;
            _segmentedControl.TintColor = UIColor.Black;
            _segmentedControl.Frame = new RectangleF (10, 10, View.Frame.Width - 20, 50);
            
            _segmentedControl.ValueChanged += (o, e) =>
            {
                _text = _segmentedControl.TitleAt (_segmentedControl.SelectedSegment) ?? "title not set";
                _testLabel.Text = _text;
            };
            
            _segmentedControl.SelectedSegment = 0;
            _segmentedControl.AddSubview (_testLabel);
            
            View.AddSubview (_segmentedControl);
        }
        
        void CreateUISlider ()
        {
            _slider = new UISlider { Frame = new RectangleF (10, 10, View.Frame.Width - 20, 50) };
            _slider.MinValue = 0.0f;
            _slider.MaxValue = 20.0f; 
            _slider.SetValue (10.0f, false);
            
            _slider.ValueChanged += delegate {
                _text = _slider.Value.ToString ();
                _testLabel.Text = _text;         
            };
            
//            _slider.SetThumbImage (UIImage.FromFile("Thumb0.png"), UIControlState.Normal);
//            _slider.SetThumbImage (UIImage.FromFile("Thumb1.png"), UIControlState.Highlighted);
//            _slider.SetMaxTrackImage (UIImage.FromFile("MaxTrack.png"), UIControlState.Normal);
//            _slider.SetMinTrackImage (UIImage.FromFile("MinTrack.png"), UIControlState.Normal);
//            _slider.MaxValueImage = UIImage.FromFile("Max.png");
//            _slider.MinValueImage = UIImage.FromFile("Min.png");
   
            View.AddSubview (_slider);
            
            _testLabel = new UILabel { Frame = new RectangleF (10, 200, 100, 50) };
            View.AddSubview (_testLabel);
        }
        
        void CreateUISwitch ()
        {
            _switch = new UISwitch {Frame = new RectangleF (new PointF(10,10), SizeF.Empty)};
            _switch.SetState (true, false);
            _switch.ValueChanged += delegate {
                _text = _switch.On.ToString ();
                _testLabel.Text = _text;
            };
            
            View.AddSubview (_switch);
            
            _testLabel = new UILabel { Frame = new RectangleF (10, 200, 100, 50) };
            View.AddSubview (_testLabel);
        }
        
    }
}

