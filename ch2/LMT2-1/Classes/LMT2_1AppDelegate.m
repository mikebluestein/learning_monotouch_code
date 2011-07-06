#import "LMT2_1AppDelegate.h"

@implementation LMT2_1AppDelegate

@synthesize window;
@synthesize imageView;


- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {    

    // Override point for customization after application launch
	
    [window makeKeyAndVisible];
	
	return YES;
}

-(IBAction) changePicture: (id) sender{
    
    NSLog(@"placeholder for UIActionSheet code");

    UIActionSheet *changeImageSheet = [[UIActionSheet alloc] initWithTitle:@"Change Image" 
                                                                  delegate:self cancelButtonTitle:@"Cancel" 
                                                    destructiveButtonTitle:NULL 
                                                         otherButtonTitles:@"Image 1", @"Image 2", NULL];
    
    
    [changeImageSheet showInView:imageView];
    [changeImageSheet release];
}

- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex{
    switch (buttonIndex) {
        case 0:
            imageView.image = [UIImage imageNamed: @"image1.jpg"];
            break;
        case 1:
            imageView.image = [UIImage imageNamed: @"image2.jpg"];
            break;
        case 2:
            NSLog(@"cancel");
            break;
        default:
            break;
    }
}

- (void)dealloc {
    [window release];
    [imageView release];
    [super dealloc];
}


@end
