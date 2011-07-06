
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AddressBook;
using MonoTouch.AddressBookUI;

namespace LMT48
{
    public partial class ContactDemoController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public ContactDemoController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public ContactDemoController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public ContactDemoController () : base("ContactDemoController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        ABPeoplePickerNavigationController _peoplePicker;
        ABPerson _person;
        string _phoneNumber;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            //direct use of ABAddressBook
//            ABAddressBook ab = new ABAddressBook ();
//            
//            ABPerson[] people = ab.GetPeople ();
//            
//            foreach (ABPerson person in people) {
//                
//                Console.WriteLine("{0} {1}", person.FirstName, person.LastName);
//                
//                var phones = person.GetPhones ();
//                
//                if (phones.Count > 0) {
//
//                    foreach(var phone in phones)
//                        Console.WriteLine ("  {0}, {1}", phone.Label.ToString(), phone.Value);
//                }  
//            }
            
            _peoplePicker = new ABPeoplePickerNavigationController ();
            
            showPeoplePicker.TouchUpInside += delegate { this.PresentModalViewController (_peoplePicker, true); };
            
            _peoplePicker.Cancelled += delegate { this.DismissModalViewControllerAnimated (true); };
            
            _peoplePicker.SelectPerson += delegate(object sender, ABPeoplePickerSelectPersonEventArgs e) {
                
                // Setting Continue to true allows the picker to navigate to the person details,
                // in which case you wouldn't dimiss the controller below.
                //e.Continue = true;
                
                _person = e.Person;
                
                nameLabel.Text = String.Format ("{0} {1}", _person.FirstName, _person.LastName);
                
                var phones = _person.GetPhones ();
                
                if (phones.Count > 0) {
                    //just using the first phone for demo
                    _phoneNumber = phones[0].Value;
                    phoneLabel.Text = _phoneNumber;
                } else {
                    _phoneNumber = String.Empty;
                }
                
                this.DismissModalViewControllerAnimated (true);
            };
            
            callPerson.TouchUpInside += delegate {
                
                if (!String.IsNullOrEmpty (_phoneNumber)) {
                    
                    NSUrl phoneUrl = new NSUrl (String.Format ("tel:{0}", EscapePhoneNumber (_phoneNumber)));
                    
                    if (UIApplication.SharedApplication.CanOpenUrl (phoneUrl))
                        UIApplication.SharedApplication.OpenUrl (phoneUrl);
                }
            };
        }

        string EscapePhoneNumber (string phoneNum)
        {
            return phoneNum.Replace (" ", "-").Replace ("(", "").Replace (")", "");
        }
        
    }
}
