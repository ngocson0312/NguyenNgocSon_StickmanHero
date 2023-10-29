#import "UnityAppController.h"
#import <GoogleMobileAds/GoogleMobileAds.h>

const char *const GameAdsHelper_NAME = "GameAdsHelperBridge";

//@class VibrateHelper;
@interface MyGameAppController : UnityAppController <GADFullScreenContentDelegate>

@property(nonatomic) GADAppOpenAd *gameOpenAd;

+ (bool)openAdLoaded;
+ (void)showOpenAd:(bool)isShowFirst;
+ (void)checkFetchOpenAd;
+ (void)setOpenAdAllowShow:(bool)isShow;
+ (void)requestIDFA:(int)allversion;

@end

