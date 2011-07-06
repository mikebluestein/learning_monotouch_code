using System;
using MonoTouch.Dialog;

namespace LMT55
{
    public class Customer
    {
        [Section("Customer Name")]

        [Entry("Enter first name")]
        public string FirstName;

        [Entry("Enter last name")]
        public string LastName;

        [Section("More Customer Details")]

        [Entry("Enter customer note")]
        public string Note;

        [Checkbox]
        public bool IsFavorite = true;
    }
}
