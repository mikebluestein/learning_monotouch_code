//
//  LMT1_1AppDelegate.h
//  LMT1-1
//
//  Created by Michael Bluestein on 12/31/10.
//  Copyright 2010 Michael Bluestein. All rights reserved.
//

#import <UIKit/UIKit.h>

@class LMT1_1ViewController;

@interface LMT1_1AppDelegate : NSObject <UIApplicationDelegate> {
    UIWindow *window;
    LMT1_1ViewController *viewController;
}

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet LMT1_1ViewController *viewController;

@end

