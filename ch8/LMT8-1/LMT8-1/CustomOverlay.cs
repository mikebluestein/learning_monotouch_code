using System;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LMT81
{
    public class CustomOverlay : MKShape
    {       
        const string MapKitDll = "/System/Library/Frameworks/MapKit.framework/MapKit";

        [DllImport(MapKitDll)]
        public static extern MKMapPoint MKMapPointForCoordinate (CLLocationCoordinate2D coordinate);
        
        MKMapSize MKMapSizeWorld = new MKMapSize(268435456, 268435456);
   
        MKMapRect _boundingMapRect;
        
        public CustomOverlay (CLLocationCoordinate2D coordinate)
        {       
            MKMapPoint mp = MKMapPointForCoordinate(coordinate);       
            
            _boundingMapRect = new MKMapRect(mp, new MKMapSize(MKMapSizeWorld.Height/4, MKMapSizeWorld.Width/4));
        }
        
        [MonoTouch.Foundation.Export("boundingMapRect")]
        public MKMapRect BoundingMapRect (){
            return _boundingMapRect;
        }
    }
}

