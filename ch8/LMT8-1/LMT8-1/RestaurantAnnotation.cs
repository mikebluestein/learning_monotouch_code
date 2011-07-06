using System;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace LMT81
{
    public enum RestaurantKind
    {
        Pizza,
        Seafood
    }
    
    public class RestaurantAnnotation : MKAnnotation
    {
        CLLocationCoordinate2D _coordinate;
        string _title;
        string _subtitle;
        RestaurantKind _kind;
        
        public RestaurantAnnotation (string title, string subtitle, CLLocationCoordinate2D coordinate, RestaurantKind kind)
        {
            _title = title;
            _subtitle = subtitle;
            _coordinate = coordinate;
            _kind = kind;
        }
        
        [MonoTouch.Foundation.Export("_original_setCoordinate:")]
        public void SetCoordinate(CLLocationCoordinate2D coordinate)
        {
            this.Coordinate = coordinate;
        }
         
        public override CLLocationCoordinate2D Coordinate {
            get {
                return _coordinate;
            }
            set {
                _coordinate = value;
            }
        }
        
        public override string Title {
            get {
                return _title;
            }
        }
        
        public override string Subtitle {
            get {
                return _subtitle;
            }
        }
        
        public RestaurantKind Kind {
            get{
                return _kind;
            }
        }
    }
}

