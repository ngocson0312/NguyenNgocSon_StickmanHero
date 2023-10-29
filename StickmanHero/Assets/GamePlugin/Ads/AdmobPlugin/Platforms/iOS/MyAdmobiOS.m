#import "MyAdmobiOS.h"

#import <UIKit/UIKit.h>
#import <StoreKit/StoreKit.h>
#import <CoreTelephony/CTCarrier.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <AudioToolbox/AudioServices.h>
#import <AdSupport/ASIdentifierManager.h>
#import <objc/runtime.h>
#import <mach/mach.h>
#import <mach/mach_host.h>

#import <GoogleMobileAds/GoogleMobileAds.h>
#import "MyAdmobUtiliOS.h"

const char* const MyAdmobMyBridge_NAME = "AdsAdmobMyBridge";

@interface MyAdmobiOS () <GADBannerViewDelegate, GADFullScreenContentDelegate>

@property BOOL bannerLoading;
@property BOOL bannerShow;
@property (nonatomic) int bannerOrien;
@property (nonatomic) int bannerPosShow;
@property (nonatomic) float bannerDxCenter;
@property int bannerCurrLvEcpm;
@property(nonatomic, strong) GADBannerView *bannerView;
@property(nonatomic, strong) NSMutableDictionary<NSString*, GADBannerView*> *dicBannerView;
@property(nonatomic, strong) UIView *bannerParent;

@property BOOL fullLoading;
@property BOOL fullLoaded;
@property(nonatomic, strong) GADInterstitialAd *interstitial;

@property BOOL giftLoading;
@property BOOL giftLoaded;
@property(nonatomic, strong) GADRewardedAd *rewardedAd;

@end

@implementation MyAdmobiOS

+ (instancetype)sharedInstance
{
    static MyAdmobiOS *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[MyAdmobiOS alloc] init];
        // Do any other initialisation stuff here
    });
    return sharedInstance;
}

-(void) Initialize
{
    [[GADMobileAds sharedInstance] startWithCompletionHandler:nil];
}

-(void) setBannerPos:(int)pos dxCenter:(float)dxCenter
{
    self.bannerShow = YES;
    self.bannerPosShow = pos;
    self.bannerDxCenter = dxCenter;
    if (self.bannerParent != nil) {
        self.bannerParent.hidden = NO;
    }
    if (self.bannerView != nil) {
        self.bannerView.hidden = NO;
    }
    [self setPosBanner4Show:self.bannerParent pos:pos dxCenter:dxCenter];
}

-(void) showBanner:(NSString*)adsId pos:(int)pos orien:(int)orien iPad:(bool)iPad dxCenter:(float)dxCenter
{
    NSLog(@"mysdk: admobmy showBanner id=%@ pos=%d ipad=%d", adsId, pos, iPad);
    if (self.dicBannerView == nil) {
        self.dicBannerView = [[NSMutableDictionary alloc]init];
    }
    if(adsId == nil) {
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerLoadFail", "adsid nil");
        return;
    }
    self.bannerView = [self.dicBannerView valueForKey:adsId];
    self.bannerShow = YES;
    self.bannerOrien = orien;
    self.bannerPosShow = pos;
    self.bannerDxCenter = dxCenter;
    if (self.bannerView == nil && !self.bannerLoading) {
        NSLog(@"mysdk: admobmy showBanner create and load");
        UIViewController* vcon = [MyAdmobUtiliOS unityGLViewController];
        if (!iPad) {
            self.bannerView = [[GADBannerView alloc] initWithAdSize:GADAdSizeBanner];
        } else {
            self.bannerView = [[GADBannerView alloc] initWithAdSize:GADAdSizeLeaderboard];
        }
        if (self.bannerParent == nil) {
            float xbn = (vcon.view.bounds.size.width - self.bannerView.bounds.size.width)/2 + dxCenter*vcon.view.bounds.size.width;
            if (pos == 0) {
                self.bannerParent = [[UIView alloc] initWithFrame:CGRectMake(xbn, 0, vcon.view.bounds.size.width, self.bannerView.bounds.size.height)];
            } else {
                self.bannerParent = [[UIView alloc] initWithFrame:CGRectMake(xbn, vcon.view.bounds.size.height - self.bannerView.bounds.size.height, vcon.view.bounds.size.width, self.bannerView.bounds.size.height)];
            }
            // self.bannerParent.backgroundColor = [UIColor redColor];
            //self.bannerParent.userInteractionEnabled = NO;
            self.bannerParent.clipsToBounds = true;
            [vcon.view addSubview:self.bannerParent];
        } else {
            self.bannerParent.hidden = NO;
        }
        self.bannerView.translatesAutoresizingMaskIntoConstraints = NO;
        self.bannerView.adUnitID = adsId;
        self.bannerView.rootViewController = vcon;
        self.bannerView.delegate = self;
        self.bannerView.paidEventHandler = ^(GADAdValue * _Nonnull value) {
            NSString* paidParam = [NSString stringWithFormat:@"%ld, %@, %lld", [value precision], [value currencyCode], [[value value] longLongValue]];
            UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerPaid", [paidParam UTF8String]);
        };
        self.bannerView.hidden = NO;
        self.bannerLoading = YES;
        [self setPosBanner4Show:self.bannerParent pos:pos dxCenter:dxCenter];
        [self.bannerView loadRequest:[GADRequest request]];
    } else {
        if (self.bannerView != nil) {
            NSLog(@"mysdk: admobmy showBanner loaded and show");
            if (self.bannerParent != nil) {
                self.bannerParent.hidden = NO;
            }
            if (self.bannerView != nil) {
                self.bannerView.hidden = NO;
            }
            [self setPosBanner4Show:self.bannerParent pos:pos dxCenter:dxCenter];
        } else {
            NSLog(@"mysdk: admobmy showBanner bannerView nil");
        }
    }
}
-(void)setPosBanner4Show:(UIView*)view pos:(int)pos dxCenter:(float)dxCenter
{
    UIView *unityView = [MyAdmobUtiliOS unityGLViewController].view;
    self.bannerView.frame = CGRectMake(0, 0, self.bannerView.bounds.size.width, self.bannerView.bounds.size.height);
    float wscr = unityView.bounds.size.width;
    if (self.bannerOrien == 0) {
        if (wscr > unityView.bounds.size.height) {
            wscr = unityView.bounds.size.height;
        }
    } else {
        if (wscr < unityView.bounds.size.height) {
            wscr = unityView.bounds.size.height;
        }
    }
    float xbn = (wscr - self.bannerView.bounds.size.width)/2 + dxCenter*wscr;
    if (pos == 0) {
        float safetop = 0;
        if (self.bannerOrien == 0) {
            if (@available(iOS 11.0, *)) {
                safetop = unityView.safeAreaInsets.top;
            }
        }
        view.frame = CGRectMake(xbn, safetop, self.bannerView.bounds.size.width, self.bannerView.bounds.size.height);
        NSLog(@"mysdk: admobmy banner setPosBanner4Show xbn=%f safetop=%f", xbn, safetop);
    } else {
        float safebot = 0;
        if (self.bannerOrien == 0) {
            if (@available(iOS 11.0, *)) {
                safebot = unityView.safeAreaInsets.bottom;
            }
        }
        view.frame = CGRectMake(xbn, unityView.bounds.size.height - self.bannerView.bounds.size.height - safebot, self.bannerView.bounds.size.width, self.bannerView.bounds.size.height);
        NSLog(@"mysdk: admobmy banner setPosBanner4Show xbn=%f safebot=%f", xbn, safebot);
    }
}

-(void)hideBanner
{
    self.bannerShow = NO;
    if (self.bannerParent != nil) {
        self.bannerParent.hidden = YES;
    }
    if (self.bannerView != nil) {
        self.bannerView.hidden = YES;
    }
}

-(void) destroyBanner
{
    self.bannerShow = NO;
    self.bannerLoading = NO;
    if (self.bannerParent != nil) {
        self.bannerParent.hidden = YES;
    }
    if (self.bannerView == nil) {
        self.bannerView.hidden = YES;
        [self.bannerView removeFromSuperview];
        self.bannerView = nil;
    }
}

-(void) loadFull:(NSString*)adsId
{
    NSLog(@"mysdk: admobmy loadFull id=%@", adsId);
    if (self.interstitial == nil && self.fullLoading == NO && self.fullLoaded == NO) {
        self.fullLoaded = NO;
        self.fullLoading = YES;
        GADRequest *request = [GADRequest request];
        [GADInterstitialAd loadWithAdUnitID:adsId
                                    request:request
                          completionHandler:^(GADInterstitialAd *ad, NSError *error) {
            if (error) {
                NSLog(@"mysdk: admobmy Failed to load interstitial ad with error: %@", [error localizedDescription]);
                self.fullLoaded = NO;
                self.fullLoading = NO;
                self.interstitial = nil;
                UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBonFullLoadFail", "-1");
                return;
            } else {
                self.fullLoaded = YES;
                self.fullLoading = NO;
                self.interstitial = ad;
                self.interstitial.fullScreenContentDelegate = self;
                UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBFullLoaded", "");
                self.interstitial.paidEventHandler = ^(GADAdValue * _Nonnull value) {
                    NSString* paidParam = [NSString stringWithFormat:@"%ld, %@, %lld", [value precision], [value currencyCode], [[value value] longLongValue]];
                    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBFullPaid", [paidParam UTF8String]);
                };
            }
        }];
    }
}

-(bool) showFull
{
    NSLog(@"mysdk: admobmy show full");
    if (self.interstitial != nil) {
        [self.interstitial presentFromRootViewController:[MyAdmobUtiliOS unityGLViewController]];
        return true;
    } else {
        return false;
    }
}

-(void) loadGift:(NSString*)adsId
{
    NSLog(@"mysdk: admobmy load gift id=%@", adsId);
    if (self.rewardedAd == nil && self.giftLoading == NO && self.giftLoaded == NO) {
        self.giftLoaded = NO;
        self.giftLoading = YES;
        GADRequest *request = [GADRequest request];
        [GADRewardedAd loadWithAdUnitID:adsId
                                request:request
                      completionHandler:^(GADRewardedAd *ad, NSError *error) {
            if (error) {
                NSLog(@"mysdk: admobmy Rewarded ad failed to load with error: %@", [error localizedDescription]);
                self.giftLoaded = NO;
                self.giftLoading = NO;
                self.rewardedAd = nil;
                UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftLoadFail", "-1");
                return;
            } else {
                self.giftLoaded = YES;
                self.giftLoading = NO;
                self.rewardedAd = ad;
                self.rewardedAd.fullScreenContentDelegate = self;
                UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftLoaded", "");
                self.rewardedAd.paidEventHandler = ^(GADAdValue * _Nonnull value) {
                    NSString* paidParam = [NSString stringWithFormat:@"%ld, %@, %lld", [value precision], [value currencyCode], [[value value] longLongValue]];
                    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftPaid", [paidParam UTF8String]);
                };
            }
        }];
    }
}

-(bool) showGift
{
    NSLog(@"mysdk: admobmy show gift");
    if (self.rewardedAd) {
        [self.rewardedAd presentFromRootViewController:[MyAdmobUtiliOS unityGLViewController]
                              userDidEarnRewardHandler:^{
            UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftReward", "");
        }];
        return true;
    } else {
        return false;
    }
}
#pragma Cb Banner
- (void)bannerViewDidReceiveAd:(GADBannerView *)bannerView {
    NSLog(@"mysdk: admobmy bannerViewDidReceiveAd=%@", [bannerView adUnitID]);
    self.bannerLoading = NO;
    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerLoaded", "");
    GADBannerView *bn = [self.dicBannerView valueForKey:[bannerView adUnitID]];
    if (bn == nil) {
        if(self.bannerView != bannerView) {
            if (self.bannerView != nil) {
                NSLog(@"mysdk: admobmy bannerViewDidReceiveAd banner new load != banner curr");
            } else {
                NSLog(@"mysdk: admobmy bannerViewDidReceiveAd banner new load curr nil");
            }
        }
        NSLog(@"mysdk: admobmy bannerViewDidReceiveAd new load curr = load");
        self.bannerView = bannerView;
        [_dicBannerView setValue:bannerView forKey:[bannerView adUnitID]];
        [self.bannerView removeFromSuperview];
        [self.bannerParent addSubview:self.bannerView];
        [self setPosBanner4Show:self.bannerParent pos:self.bannerPosShow dxCenter:self.bannerDxCenter];
    } else {
        // Add the new banner view.
        if(self.bannerView == bannerView) {
            [self.bannerView removeFromSuperview];
            [self.bannerParent addSubview:self.bannerView];
            [self setPosBanner4Show:self.bannerParent pos:self.bannerPosShow dxCenter:self.bannerDxCenter];
            NSLog(@"mysdk: admobmy bannerViewDidReceiveAd old load curr = load");
        } else {
            if (self.bannerView != nil) {
                NSLog(@"mysdk: admobmy bannerViewDidReceiveAd old load curr != load, cur=%@ load=%@", [bannerView adUnitID], [self.bannerView adUnitID]);
            } else {
                NSLog(@"mysdk: admobmy bannerViewDidReceiveAd old load curr != load curr nil");
                self.bannerView = bannerView;
            }
        }
    }
    if (!self.bannerShow) {
        if (self.bannerParent != nil) {
            self.bannerParent.hidden = YES;
        }
        if (self.bannerView != nil) {
            self.bannerView.hidden = YES;
        }
    }
}

- (void)bannerView:(GADBannerView *)bannerView didFailToReceiveAdWithError:(NSError *)error {
    NSLog(@"mysdk: admobmy bannerView:didFailToReceiveAdWithError: %@", [error localizedDescription]);
    self.bannerLoading = NO;
    GADBannerView *bn = [self.dicBannerView valueForKey:[bannerView adUnitID]];
    if (bn == nil) {
        [bannerView removeFromSuperview];
        if (bannerView == self.bannerView) {
            self.bannerView = nil;
        }
    }
    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerLoadFail", [[error localizedDescription] UTF8String]);
}

- (void)bannerViewDidRecordImpression:(GADBannerView *)bannerView {
    NSLog(@"mysdk: admobmy bannerViewDidRecordImpression");
    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerImpression", "");
}

- (void)bannerViewWillPresentScreen:(GADBannerView *)bannerView {
    NSLog(@"mysdk: admobmy bannerViewWillPresentScreen");
    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerOpen", "");
}

- (void)bannerViewWillDismissScreen:(GADBannerView *)bannerView {
    NSLog(@"mysdk: admobmy bannerViewWillDismissScreen");
}

- (void)bannerViewDidDismissScreen:(GADBannerView *)bannerView {
    NSLog(@"mysdk: admobmy bannerViewDidDismissScreen");
    UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBBannerClose", "");
}
#pragma CB full
/// Tells the delegate that the ad failed to present full screen content.
- (void)ad:(nonnull id<GADFullScreenPresentingAd>)ad
didFailToPresentFullScreenContentWithError:(nonnull NSError *)error {
    NSLog(@"mysdk: admobmy Ad did fail to present full screen content.");
    if ([ad isEqual:self.interstitial]) {
        self.fullLoaded = NO;
        self.interstitial = nil;
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBFullFailedToShow", "-1");
    } else if ([ad isEqual:self.rewardedAd]) {
        self.giftLoaded = NO;
        self.rewardedAd = nil;
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftFailedToShow", "-1");
    } else {
        NSLog(@"mysdk: admobmy Ad did fail to present full screen content err not detect ads");
    }
}

/// Tells the delegate that the ad presented full screen content.
- (void)adDidPresentFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad {
    NSLog(@"mysdk: admobmy Ad did present full screen content.");
    if ([ad isEqual:self.interstitial]) {
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBFullShowed", "");
    } else if ([ad isEqual:self.rewardedAd]) {
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftShowed", "");
    } else {
        NSLog(@"mysdk: admobmy Ad did present full screen content. Err not detet ads");
    }
}

/// Tells the delegate that the ad dismissed full screen content.
- (void)adDidDismissFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad {
    NSLog(@"mysdk: admobmy Ad did dismiss full screen content.");
    if ([ad isEqual:self.interstitial]) {
        self.fullLoaded = NO;
        self.interstitial = nil;
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBFullDismissed", "");
    } else if ([ad isEqual:self.rewardedAd]) {
        self.giftLoaded = NO;
        self.rewardedAd = nil;
        UnitySendMessage(MyAdmobMyBridge_NAME, "iOSCBGiftDismissed", "");
    } else {
        NSLog(@"mysdk: admobmy Ad did dismiss full screen content. Err not detect ads");
    }
}

#pragma mark - C API
void InitializeNative() {
    [[MyAdmobiOS sharedInstance] Initialize];
}

void setBannerPosNative(int pos, float dxcenter) {
    [[MyAdmobiOS sharedInstance] setBannerPos:pos dxCenter:dxcenter];
}
void showBannerNative(char* adsId, int pos, int orien, bool iPad, float dxcenter) {
    NSString* nsadsid = [NSString stringWithUTF8String:adsId];
    [[MyAdmobiOS sharedInstance] showBanner:nsadsid pos:pos orien:orien iPad:iPad dxCenter:dxcenter];
}
void hideBannerNative() {
    [[MyAdmobiOS sharedInstance] hideBanner];
}
void clearCurrFullNative() {
    
}
void loadFullNative(char* adsId) {
    NSString* nsadsid = [NSString stringWithUTF8String:adsId];
    [[MyAdmobiOS sharedInstance] loadFull:nsadsid];
}
bool showFullNative() {
    return [[MyAdmobiOS sharedInstance] showFull];
}
void clearCurrGiftNative() {
    
}
void loadGiftNative(char* adsId) {
    NSString* nsadsid = [NSString stringWithUTF8String:adsId];
    [[MyAdmobiOS sharedInstance] loadGift:nsadsid];
}
bool showGiftNative() {
    return [[MyAdmobiOS sharedInstance] showGift];
}

@end
