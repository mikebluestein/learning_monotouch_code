
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT46
{

	// The name AppDelegateIPhone is referenced in the MainWindowIPhone.xib file.
	public partial class AppDelegateIPhone : UIApplicationDelegate
	{
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// If you have defined a view, add it here:
		    window.AddSubview (demoAdIPhone.View);
					
			window.MakeKeyAndVisible ();
			
			return true;
		}
	
	}
}