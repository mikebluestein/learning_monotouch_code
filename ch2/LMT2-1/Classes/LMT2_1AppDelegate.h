#import <UIKit/UIKit.h>

@interface LMT2_1AppDelegate : NSObject <UIApplicationDelegate, UIActionSheetDelegate> {
    UIWindow *window;
    UIImageView *imageView;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet UIImageView *imageView;

-(IBAction) changePicture: (id) sender;
-(void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex;

@end

