// Copyright 2016 Google Inc. All Rights Reserved.

#import <Foundation/Foundation.h>

#import <GoogleMobileAds/GoogleMobileAds.h>

@interface MyAdmobUtiliOS : NSObject

/// Whether the Unity app should be paused when a full screen ad is displayed.
@property(class) BOOL pauseOnBackground;

/// Returns YES if the operating system is at least the supplied major version.
+(BOOL)GADUIsOperatingSystemAtLeastVersion:(NSInteger) majorVersion;

/// Returns an NSString copying the characters from |bytes|, a C array of UTF8-encoded bytes.
/// Returns nil if |bytes| is NULL.
+ (NSString *)GADUStringFromUTF8String:(const char *)bytes;

/// Returns the Unity view controller.
+ (UIViewController *)unityGLViewController;

/// Position view in the parent view, corresponding to the adPosition.
+ (void)positionView:(UIView *)view
        inParentView:(UIView *)parentView
          adPosition:(int)adPosition;

/// Position view in the parent view, corresponding to the CGPoint.
+ (void)positionView:(UIView *)view
        inParentView:(UIView *)parentView
      customPosition:(CGPoint)adPosition;

/// If requesting smart banner landscape, returns the custom size for landscape smart banners which
/// is full width of the safe area and auto height. Assumes that the application window is visible.
/// If requesting any other ad size, returns the un-modified ad size.
+ (GADAdSize)safeAdSizeForAdSize:(GADAdSize)adSize;

@end
