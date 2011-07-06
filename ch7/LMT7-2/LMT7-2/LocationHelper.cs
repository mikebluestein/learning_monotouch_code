using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace LMT72
{

    public sealed class LocationHelper
    {
        static LocationHelper locationHelperInstance = new LocationHelper ();

        public static LocationHelper Instance {
            get { return locationHelperInstance; }
        }

        CLLocationManager _locationManager;

        List<CLLocation> _locations;
        public List<CLLocation> Locations {
            get { return _locations; }
        }

        public event EventHandler LocationAdded;

        LocationHelper ()
        {
            _locations = new List<CLLocation> ();
            
            _locationManager = new CLLocationManager ();
            _locationManager.Purpose = "This is the purpose string.";
            _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
            _locationManager.DistanceFilter = CLLocation.AccuracyBest;
            _locationManager.HeadingFilter = 10; //min degrees to genreate heading callbacks
            _locationManager.Delegate = new LMTLocationManagerDelegate (this);
        }

        public void StartLocationUpdates ()
        {
            if (CLLocationManager.LocationServicesEnabled)
                _locationManager.StartUpdatingLocation ();
            else {
                UIAlertView alert = new UIAlertView ("Cannot determine location", "Location services are disabled", null, "OK");            
                alert.Show ();
            }
        }

        public void StartHeadingUpdates ()
        {
            if (CLLocationManager.HeadingAvailable)
                _locationManager.StartUpdatingHeading ();
            else {
                UIAlertView alert = new UIAlertView ("Cannot determine location", "Location services are disabled", null, "OK");         
                alert.Show ();
            }
        }

        public void StopLocationUpdates ()
        {
            _locationManager.StopUpdatingLocation ();
        }
        
        public void StopHeadingUpdates ()
        {
            _locationManager.StopUpdatingHeading ();
        }

        class LMTLocationManagerDelegate : CLLocationManagerDelegate
        {
            LocationHelper _helper;

            public LMTLocationManagerDelegate (LocationHelper lh)
            {
                _helper = lh;
            }

            public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
            {
                Console.WriteLine ("New location data = {0}", newLocation.Description ());
                
                _helper.Locations.Add (newLocation);
                
                if (_helper.LocationAdded != null)
                    _helper.LocationAdded (_helper, new EventArgs ());
            }
            
            public override void UpdatedHeading (CLLocationManager manager, CLHeading newHeading)
            {
                Console.WriteLine ("Magnetic Heading = {0}", newHeading.MagneticHeading);
                Console.WriteLine ("True Heading = {0}", newHeading.TrueHeading);
                Console.WriteLine ("Heading Accuracy = +/-{0} degress", newHeading.HeadingAccuracy);
            }

            public override void Failed (CLLocationManager manager, NSError error)
            {
                if (error.Code == (int)CLError.Denied) {
                    Console.WriteLine ("Access to location services denied");
                    
                    manager.StopUpdatingLocation ();
                    manager.StopUpdatingLocation ();
                    manager.Delegate = null;
                }
            }
        }
    }
}

