
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT32
{
    public partial class SampleViewController : UIViewController
    {
        MyAccelerometerDelegate _accelDelegate;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public SampleViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public SampleViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public SampleViewController () : base("SampleViewController", null)
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
            
            UIAccelerometer accelerometer = UIAccelerometer.SharedAccelerometer;
            accelerometer.UpdateInterval = 0.25;
            _accelDelegate = new MyAccelerometerDelegate (this);
            accelerometer.Delegate = _accelDelegate;
            
//            accelerometer.Acceleration += HandleAccelerometerAcceleration;
        }

//        void HandleAccelerometerAcceleration (object sender, 
//            UIAccelerometerEventArgs e)
//        {
//            loggingView.AppendTextLine (
//                String.Format ("x = {0:f}, y={1:f}, z={2:f}", 
//                    e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z));
//        }

        class MyAccelerometerDelegate : UIAccelerometerDelegate
        {
            SampleViewController _controller;

            public MyAccelerometerDelegate (SampleViewController controller)
            {
                _controller = controller;
            }

            public override void DidAccelerate (UIAccelerometer accelerometer, 
                UIAcceleration acceleration)
            {
                _controller.loggingView.AppendTextLine (
                    String.Format ("x = {0:f}, y={1:f}, z={2:f}", 
                        acceleration.X, acceleration.Y, acceleration.Z));
            }
        }
    }
}

