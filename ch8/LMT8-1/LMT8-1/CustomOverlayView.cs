using MonoTouch.MapKit;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace LMT81
{
    public class CustomOverlayView : MKOverlayView
    {
        CustomOverlay _overlay;
            
        public CustomOverlayView (CustomOverlay overlay)
        { 
            _overlay = overlay;
        }
        
        public override void DrawMapRect (MKMapRect mapRect, float zoomScale, CGContext context)
        {
            UIGraphics.PushContext(context);
            
            context.SetLineWidth (4000);    
            
            UIColor.Blue.SetFill();  
            CGPath path = new CGPath ();
            
            RectangleF r = this.RectForMapRect(_overlay.BoundingMapRect());
            PointF _origin = r.Location;
                                 
            path.AddLines (new PointF[] { 
                _origin, 
                new PointF (_origin.X + 35000, _origin.Y + 80000),
                new PointF (_origin.X - 50000, _origin.Y + 30000),
                new PointF (_origin.X + 50000, _origin.Y + 30000),
                new PointF (_origin.X - 35000, _origin.Y + 80000) });
                     
            context.AddPath (path);
            context.DrawPath (CGPathDrawingMode.Fill);
            
            UIGraphics.PopContext();
        }
       
    }
}

