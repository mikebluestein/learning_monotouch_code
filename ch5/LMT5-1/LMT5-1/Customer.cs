using System;

namespace LMT51
{
    public class Customer
    {   
        string _fName;
        string _lName;

        public string FName {
            get { return this._fName; }
            set { _fName = value; }
        }

        public string LName {
            get { return this._lName; }
            set { _lName = value; }
        }

        public Customer (string fName, string lName)
        {
            _fName = fName;
            _lName = lName;
        }

        public string Note { get; set; }
        
    }
}

