// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace LMT49 {
	
	
	// Base type probably should be MonoTouch.UIKit.UIViewController or subclass
	[MonoTouch.Foundation.Register("CameraDemoController")]
	public partial class CameraDemoController {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		private MonoTouch.UIKit.UIButton __mt_showPicker;
		
		private MonoTouch.UIKit.UIImageView __mt_imageView;
		
		private MonoTouch.UIKit.UIButton __mt_playMovie;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("view")]
		private MonoTouch.UIKit.UIView view {
			get {
				this.__mt_view = ((MonoTouch.UIKit.UIView)(this.GetNativeField("view")));
				return this.__mt_view;
			}
			set {
				this.__mt_view = value;
				this.SetNativeField("view", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("showPicker")]
		private MonoTouch.UIKit.UIButton showPicker {
			get {
				this.__mt_showPicker = ((MonoTouch.UIKit.UIButton)(this.GetNativeField("showPicker")));
				return this.__mt_showPicker;
			}
			set {
				this.__mt_showPicker = value;
				this.SetNativeField("showPicker", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("imageView")]
		private MonoTouch.UIKit.UIImageView imageView {
			get {
				this.__mt_imageView = ((MonoTouch.UIKit.UIImageView)(this.GetNativeField("imageView")));
				return this.__mt_imageView;
			}
			set {
				this.__mt_imageView = value;
				this.SetNativeField("imageView", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("playMovie")]
		private MonoTouch.UIKit.UIButton playMovie {
			get {
				this.__mt_playMovie = ((MonoTouch.UIKit.UIButton)(this.GetNativeField("playMovie")));
				return this.__mt_playMovie;
			}
			set {
				this.__mt_playMovie = value;
				this.SetNativeField("playMovie", value);
			}
		}
	}
}
