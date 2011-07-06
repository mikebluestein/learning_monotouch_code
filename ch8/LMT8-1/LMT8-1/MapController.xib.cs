
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;
using MonoTouch.ObjCRuntime;
//using System.Runtime.InteropServices;

namespace LMT81
{
    public partial class MapController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public MapController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public MapController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public MapController () : base("MapController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        MapDelegate _md;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            _md = new MapDelegate ();
            map.Delegate = _md;
           
            map.AddAnnotation (new RestaurantAnnotation[] { 
                new RestaurantAnnotation ("Mike's Pizza", "Gourmet Pizza Kitchen", new CLLocationCoordinate2D (41.86337816, -72.56874647), RestaurantKind.Pizza), 
                new RestaurantAnnotation ("Barb's Seafod", "Best Seafood in New England", new CLLocationCoordinate2D (41.96337816, -72.96874647), RestaurantKind.Seafood),
                new RestaurantAnnotation ("John's Pizza", "Deep Dish Style", new CLLocationCoordinate2D (41.45537816, -72.76874647), RestaurantKind.Pizza)});
           
            AddOverlays ();
            
            map.MapType = MKMapType.Hybrid;
        }

        public void AddOverlays ()
        {
            // sample coordinates
            CLLocationCoordinate2D c1 = new CLLocationCoordinate2D (41.86337816, -72.56874647);
            CLLocationCoordinate2D c2 = new CLLocationCoordinate2D (41.96337816, -72.96874647);
            CLLocationCoordinate2D c3 = new CLLocationCoordinate2D (41.45537816, -72.76874647);
            CLLocationCoordinate2D c4 = new CLLocationCoordinate2D (42.34994, -71.09292);
            
            // circle
            MKCircle circle = MKCircle.Circle (c1, 10000.0); // 10000 meter radius
            map.AddOverlay (circle);
            
            // polygon
            MKPolygon polygon = MKPolygon.FromCoordinates(new CLLocationCoordinate2D[]{c1,c2,c3});
            map.AddOverlay(polygon);
            
            // triangle
            MKPolyline polyline = MKPolyline.FromCoordinates(new CLLocationCoordinate2D[]{c1,c2,c3});
            map.AddOverlay(polyline);
           
            CustomOverlay co = new CustomOverlay(c4);
            map.AddOverlay(co);
        }

        class MapDelegate : MKMapViewDelegate
        {
            static string annotationId = "restaurauntAnnotation";

            public override void DidSelectAnnotationView (MKMapView mapView, MKAnnotationView view)
            {
                MKUserLocation userLocationAnnotation = view.Annotation as MKUserLocation;
                
                if (userLocationAnnotation != null) {
                    CLLocationCoordinate2D coord = userLocationAnnotation.Location.Coordinate;
                    MKCoordinateRegion region = MKCoordinateRegion.FromDistance (coord, 500, 500);
                    
                    mapView.CenterCoordinate = coord;
                    mapView.Region = region;
                    
                    userLocationAnnotation.Title = "I am here";
                }
            }

            public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, NSObject annotation)
            {
                if (annotation is MKUserLocation)
                    return null;             
 
                
                // stock pin annotation view
                
//                MKPinAnnotationView annotationView = mapView.DequeueReusableAnnotation (annotationId) as MKPinAnnotationView;
//                if (annotationView == null)
//                    annotationView = new MKPinAnnotationView (annotation, annotationId);
//                
//                annotationView.PinColor = MKPinAnnotationColor.Purple;
//                annotationView.CanShowCallout = true;
//                annotationView.Draggable = true;
//                annotationView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
                
                
                // annotation view with custom image set
                var restaurantAnnotation = annotation as RestaurantAnnotation;
                
                MKAnnotationView annotationView = mapView.DequeueReusableAnnotation (annotationId);
                if (annotationView == null)
                    annotationView = new MKAnnotationView (annotation, annotationId);
                      
                switch (restaurantAnnotation.Kind) {
                case RestaurantKind.Pizza:
                    annotationView.Image = UIImage.FromFile ("images/Pizza.png");
                    break;
                case RestaurantKind.Seafood:
                    annotationView.Image = UIImage.FromFile ("images/Seafood.png");
                    break;
                }
                
                annotationView.CanShowCallout = true;
                annotationView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
                
                return annotationView;
            }

            public override MKOverlayView GetViewForOverlay (MKMapView mapView, NSObject overlay)
            {
                MKOverlayView overlayView = null;
                
                if(overlay is MKPolygon){
                    MKPolygon polygon = overlay as MKPolygon;
                    var polygonView = new MKPolygonView(polygon);
                    polygonView.FillColor = UIColor.Purple;
                    polygonView.Alpha = 0.7f;
                    overlayView = polygonView;
                }
                else if(overlay is MKCircle){
                    MKCircle circle = overlay as MKCircle;
                    var circleView = new MKCircleView (circle);
                    circleView.FillColor = UIColor.Green;
                    overlayView = circleView;
                }
                else if(overlay is MKPolyline){
                    MKPolyline polyline = overlay as MKPolyline;
                    var polylineView = new MKPolylineView (polyline);
                    polylineView.StrokeColor = UIColor.Black;
                    overlayView = polylineView;
                }   
                else if(overlay is CustomOverlay)
                {
                    CustomOverlay co = overlay as CustomOverlay;
                    var v = new CustomOverlayView(co);
                    overlayView = v;
                }
                
                return overlayView;
            }

            public override void DidAddAnnotationViews (MKMapView mapView, MKAnnotationView[] views)
            {
                Console.WriteLine ("TODO: add region code to zoom in here...");
            }

            public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
            {
                var annotation = view.Annotation as RestaurantAnnotation;
                
                if (annotation != null) {
                    string message = String.Format ("{0} tapped", annotation.Title);
                    UIAlertView alert = new UIAlertView ("Annotation Tapped", message, null, "OK");
                    alert.Show ();
                }
            }

            public override void ChangedDragState (MKMapView mapView, MKAnnotationView annotationView, MKAnnotationViewDragState newState, MKAnnotationViewDragState oldState)
            {
                Console.WriteLine ("drag state changed");
            }
        }
    }
}

