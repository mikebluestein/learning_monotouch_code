
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MessageUI;

namespace LMT47
{
    public partial class MailDemoController : UIViewController
    {
        MFMailComposeViewController _mail;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public MailDemoController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public MailDemoController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public MailDemoController () : base("MailDemoController", null)
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
            
            mailButton.TouchUpInside += (o, e) =>
            {
                if (MFMailComposeViewController.CanSendMail) {
                    _mail = new MFMailComposeViewController ();
                    _mail.SetToRecipients (new string[] { "person1@foo.com", "person2@foo.com" });
                    _mail.SetCcRecipients (new string[] { "person3@foo.com" });
                    _mail.SetBccRecipients (new string[] { "person4@foo.com" });
                    _mail.SetMessageBody ("body of the email", false);
                    _mail.SetSubject ("test email");
                    _mail.Finished += HandleMailFinished;
                    this.PresentModalViewController (_mail, true);
                    
                } else {
                    var alert = new UIAlertView ("Mail Alert", "Mail Not Sent", null, "Mail Demo", null);
                    alert.Show ();
                }
            };
        }

        void HandleMailFinished (object sender, MFComposeResultEventArgs e)
        {
            if (e.Result == MFMailComposeResult.Sent) {
                var alert = new UIAlertView ("Mail Alert", "Mail Sent", null, "Mail Demo", null);
                alert.Show ();
            }
            e.Controller.DismissModalViewControllerAnimated (true);
        }
        
    }
}

