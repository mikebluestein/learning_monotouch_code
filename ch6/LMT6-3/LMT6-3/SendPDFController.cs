// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace LMT63 {
    
    
    // Base type probably should be MonoTouch.UIKit.UIViewController or subclass
    [MonoTouch.Foundation.Register("SendPDFController")]
    public partial class SendPDFController {
        
        private PDFView __mt_view;
        
        private MonoTouch.UIKit.UIToolbar __mt_toolbar;
        
        private MonoTouch.UIKit.UITextView __mt_tv;
        
        #pragma warning disable 0169
        [MonoTouch.Foundation.Connect("view")]
        private PDFView view {
            get {
                this.__mt_view = ((PDFView)(this.GetNativeField("view")));
                return this.__mt_view;
            }
            set {
                this.__mt_view = value;
                this.SetNativeField("view", value);
            }
        }
        
        [MonoTouch.Foundation.Connect("toolbar")]
        private MonoTouch.UIKit.UIToolbar toolbar {
            get {
                this.__mt_toolbar = ((MonoTouch.UIKit.UIToolbar)(this.GetNativeField("toolbar")));
                return this.__mt_toolbar;
            }
            set {
                this.__mt_toolbar = value;
                this.SetNativeField("toolbar", value);
            }
        }
        
        [MonoTouch.Foundation.Connect("tv")]
        private MonoTouch.UIKit.UITextView tv {
            get {
                this.__mt_tv = ((MonoTouch.UIKit.UITextView)(this.GetNativeField("tv")));
                return this.__mt_tv;
            }
            set {
                this.__mt_tv = value;
                this.SetNativeField("tv", value);
            }
        }
    }
}