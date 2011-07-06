using System;
using MonoTouch.UIKit;
using System.Reflection;

namespace LMT33
{
    public class SecondViewController : UIViewController
    {
        public SecondViewController ()
        {
        }

        public override void LoadView ()
        {
            base.LoadView ();
            
            var sv = new SecondView { Title = "This is the title" };
            this.View = sv;
            this.View.BackgroundColor = UIColor.White;
        }
    }
}