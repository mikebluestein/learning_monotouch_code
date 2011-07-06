using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;
using MonoTouch.UIKit;

namespace LMT73
{

    public sealed class LocationHelper
    {
        static LocationHelper locationHelperInstance = new LocationHelper ();

        public static LocationHelper Instance {
            get { return locationHelperInstance; }
        }

        public static void Initialize ()
        {
            var instance = LocationHelper.Instance;
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
            _locationManager.Delegate = new LMTLocationManagerDelegate (this);
        }

        public void StartLocationUpdates ()
        {
            if (CLLocationManager.SignificantLocationChangeMonitoringAvailable)
                _locationManager.StartMonitoringSignificantLocationChanges ();
            else {
                UIAlertView alert = new UIAlertView ("Cannot determine location", "Significant-change location services are not available", null, "OK");
                
                alert.Show ();
            }
        }

        public void StopLocationUpdates ()
        {
            _locationManager.StopMonitoringSignificantLocationChanges ();
        }

        public void StartRegionUpdates (CLRegion region)
        {        
            if (CLLocationManager.RegionMonitoringAvailable && CLLocationManager.RegionMonitoringEnabled) {
                _locationManager.StartMonitoring (region, CLLocation.AccuracyHundredMeters);
            }
        }

        public void StopRegionUpdates (CLRegion region)
        {
            _locationManager.StopMonitoring (region);
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

            public override void Failed (CLLocationManager manager, NSError error)
            {
                if (error.Code == (int)CLError.Denied) {
                    Console.WriteLine ("Access to location services denied");
                    
                    manager.StopUpdatingLocation ();
                    manager.Delegate = null;
                }
            }
            
            public override void RegionEntered (CLLocationManager manager, CLRegion region)
            {
                Console.WriteLine("entered region {0}", region.Identifier);
            }
            
            public override void RegionLeft (CLLocationManager manager, CLRegion region)
            {
                 Console.WriteLine("exited region {0}", region.Identifier);
            }
            
            public override void MonitoringFailed (CLLocationManager manager, CLRegion region, NSError error)
            {
                Console.WriteLine ("region monitoring failed for region {0}", region.Identifier);
                
                if (error.Code == (int)CLError.RegionMonitoringDenied){
                    Console.WriteLine("RegionMonitoringDenied");
                }
                else if(error.Code == (int)CLError.RegionMonitoringFailure){
                    Console.WriteLine("RegionMonitoringFailure"); 
                }
                else if(error.Code == (int)CLError.RegionMonitoringSetupDelayed){
                    Console.WriteLine("RegionMonitoringSetupDelayed"); 
                }
            }
        }
    }
}

