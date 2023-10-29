//
//  MyGameAppController.mm
//
//  Created by Shachar Aharon on 15/05/2017.
//
//
#import "MyGameAppController.h"
#import <GoogleMobileAds/GoogleMobileAds.h>
#import <FBAudienceNetwork/FBAdSettings.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>

bool isOpenAdLoading = false;
bool isOpenAdShowing = false;
bool isGameOpen = true;
NSTimeInterval openAdTimeShow = 0;
bool isOpenAdShowOpen = true;
bool isOpenAdShowFirst = true;
bool _isOpenAdAllowShow = true;
bool _isFirstOpenAdShowed = false;

static MyGameAppController *MyGameAppControllerInstance = nil;

@implementation MyGameAppController

- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(nullable NSDictionary<UIApplicationLaunchOptionsKey, id> *)launchOptions
{
    BOOL re = [super application:application willFinishLaunchingWithOptions:launchOptions];
    
    //[MyGameAppController mylog:@"application willFinishLaunchingWithOptions"];
    [self setDefaultOpenAd];
    MyGameAppControllerInstance = self;
    isOpenAdShowOpen = true;
    isOpenAdShowFirst = true;
    _isFirstOpenAdShowed = false;
    
    NSInteger co = [[NSUserDefaults standardUserDefaults] integerForKey:@"count_game_open"];
    co++;
    [[NSUserDefaults standardUserDefaults] setInteger:co forKey:@"count_game_open"];
    
    [FBAdSettings setAdvertiserTrackingEnabled:YES];
    
    return re;
}

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(nullable NSDictionary<UIApplicationLaunchOptionsKey, id> *)launchOptions
{
    BOOL re = [super application:application didFinishLaunchingWithOptions:launchOptions];
    //[MyGameAppController mylog:@"application didFinishLaunchingWithOptions"];
    return re;
}

- (void)applicationDidBecomeActive:(UIApplication*)application
{
    [super applicationDidBecomeActive:application];
    //[MyGameAppController mylog:@"applicationDidBecomeActive"];
    UnitySendMessage(GameAdsHelper_NAME, "gameIsBecomeActive", "");
    if (isOpenAdShowOpen) {
        isOpenAdShowOpen = false;
        NSInteger isremoveads = [[NSUserDefaults standardUserDefaults] integerForKey:@"is_remove_ads_native"];
        if (isremoveads != 1) {
            [self _showOpenAd];
        }
    }
}

- (void)applicationWillResignActive:(UIApplication *)application
{
    [super applicationWillResignActive:application];
    //[MyGameAppController mylog:@"applicationWillResignActive"];
    UnitySendMessage(GameAdsHelper_NAME, "gameIsResignActive", "");
}

- (void)applicationDidEnterBackground:(UIApplication *)application
{
    [super applicationDidEnterBackground:application];
    isOpenAdShowOpen = true;
    //[MyGameAppController mylog:@"applicationDidEnterBackground"];
}

- (void)applicationWillEnterForeground:(UIApplication *)application
{
    [super applicationWillEnterForeground:application];
    //[MyGameAppController mylog:@"applicationWillEnterForeground"];
}

-(void) _showOpenAd
{
    NSString* adid = [[NSUserDefaults standardUserDefaults] stringForKey:@"openad_id"];
    NSInteger tyshow = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_type_show"];
    if ((tyshow == 0 || tyshow == 2 || (tyshow == 4 && isOpenAdShowFirst)) && adid != nil && adid.length > 10) {
        [MyGameAppController mylog:@"openad _showOpenAd 1"];
        isOpenAdShowFirst = false;
        [MyGameAppControllerInstance tryToPresentAd:false];
    } else {
        [MyGameAppController mylog:@"openad _showOpenAd 2"];
        if (tyshow == 3) {
            [self requestOpenAd];
        }
        if (!isOpenAdLoading) {
            UnitySendMessage(GameAdsHelper_NAME, "_showOpenAd", "");
        }
    }
    isGameOpen = false;
}

- (void)requestOpenAd
{
    [MyGameAppController mylog:@"openad requestOpenAd 1"];
    if ([self isAdAvailable]) {
        return;
    }
    [MyGameAppController mylog:@"openad requestOpenAd 2"];
    NSString* adunitid = [[NSUserDefaults standardUserDefaults] stringForKey:@"openad_id"];
    // adunitid =@"ca-app-pub-3940256099942544/5662855259";
    if (!isOpenAdLoading && adunitid != nil && [adunitid length] > 10) {
        [MyGameAppController mylog:[NSString stringWithFormat:@"openad requestOpenAd 2=%@", adunitid]];
        self.gameOpenAd = nil;
        NSInteger orien = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_orentation"];
        UIInterfaceOrientation uiorien;
        if (orien == 0) {
            uiorien = UIInterfaceOrientationPortrait;
        } else {
            uiorien = UIInterfaceOrientationLandscapeLeft;
        }
        isOpenAdLoading = true;
        [GADAppOpenAd loadWithAdUnitID:adunitid
                               request:[GADRequest request]
                           orientation:uiorien
                     completionHandler:^(GADAppOpenAd *_Nullable appOpenAd, NSError *_Nullable error) {
            if (error) {
                [MyGameAppController mylog:[NSString stringWithFormat:@"openad Failed to load app open ad: %@", error]];
                isOpenAdLoading = false;
            } else {
                self.gameOpenAd = appOpenAd;
                self.gameOpenAd.fullScreenContentDelegate = self;
                isOpenAdLoading = false;
                [MyGameAppController mylog:@"openad requestOpenAd ok"];
            }
        }];
    }
}

- (void)tryToPresentAd:(bool)isForceShow
{
    [MyGameAppController mylog:[NSString stringWithFormat:@"openad tryToPresentAd isForceShow=%d, _isAllowShowOpenAd=%d", isForceShow, _isOpenAdAllowShow]];
    int statecheck = [self check4ShowOpenad];
    bool isShowAds = false;
    if ((statecheck > 0 || isForceShow) && _isOpenAdAllowShow) {
        if (!isOpenAdShowing && [self isAdAvailable]) {
            isShowAds = true;
            if (!_isFirstOpenAdShowed) {
                UnitySendMessage(GameAdsHelper_NAME, "onShowOpenNative", "onFirstAdOpen");
            } else {
                UnitySendMessage(GameAdsHelper_NAME, "onShowOpenNative", "onAdOpen");
            }
            _isFirstOpenAdShowed = true;
            openAdTimeShow = [self SystemCurrentMilisecond];
            GADAppOpenAd *ad = self.gameOpenAd;
            ad.fullScreenContentDelegate = self;
            UIViewController *rootController = self.window.rootViewController;
            [ad presentFromRootViewController:rootController];
        } else {
            if (isOpenAdShowing) {
                [MyGameAppController mylog:@"openad tryToPresentAd is ads showing"];
            } else {
                [MyGameAppController mylog:@"openad tryToPresentAd is ads no avaiable"];
                NSInteger tyshow = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_type_show"];
                if (tyshow == 2) {
                    UnitySendMessage(GameAdsHelper_NAME, "showOpenAdsRe", "showRe");
                }
            }
        }
    }
    if ((statecheck >= 0 || isForceShow) && !isShowAds) {
        [self requestOpenAd];
    }
}

- (bool)isAdAvailable
{
    return (self.gameOpenAd != nil);
}

- (int)check4ShowOpenad
{
    NSInteger co = [[NSUserDefaults standardUserDefaults] integerForKey:@"count_game_open"];
    NSInteger showAt = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_show_at"];
    NSInteger isShowFirst = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_is_showfirst"];
    NSInteger delt = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_deltime"];
    if (co < showAt) {
        [MyGameAppController mylog:[NSString stringWithFormat:@"open ad checkConditionShow show at count < cf, c=%ld, cf=%ld", co, showAt]];
        isGameOpen = false;
        return -1;
    }
    if (isGameOpen) {
        isGameOpen = false;
        if (isShowFirst == 0) {
            [MyGameAppController mylog:@"openad checkConditionShow not show fist"];
            return 0;
        }
    }
    NSTimeInterval t = [self SystemCurrentMilisecond];
    if ((t - openAdTimeShow) <= delt) {
        [MyGameAppController mylog:[NSString stringWithFormat:@"openad checkConditionShow not allow deltime cf==%ld", delt]];
        return 0;
    }
    
    [MyGameAppController mylog:@"openad checkConditionShow ok"];
    return 1;
}

-(void)setDefaultOpenAd {
    NSString* adid = [[NSUserDefaults standardUserDefaults] stringForKey:@"openad_id"];
    if (adid == nil || adid.length < 3) {
        [[NSUserDefaults standardUserDefaults] setInteger:1 forKey:@"openad_orentation"];
        [[NSUserDefaults standardUserDefaults] setInteger:4 forKey:@"openad_type_show"];
        [[NSUserDefaults standardUserDefaults] setInteger:3 forKey:@"openad_show_at"];
        [[NSUserDefaults standardUserDefaults] setInteger:0 forKey:@"openad_is_showfirst"];
        [[NSUserDefaults standardUserDefaults] setInteger:30 forKey:@"openad_deltime"];
        [[NSUserDefaults standardUserDefaults] setObject:@"ca-app-pub-2777953690987264/8708602831" forKey:@"openad_id"];
        [MyGameAppController mylog:@"setDefaultOpenAd"];
    }
}

+(void)mylog:(NSString*) msg
{
    NSLog(@"mysdk: %@", msg);
}
- (NSTimeInterval) SystemCurrentMilisecond
{
    NSTimeInterval timeStamp = [[NSDate date] timeIntervalSince1970];
    return timeStamp;
}

+ (bool)openAdLoaded
{
    if (MyGameAppControllerInstance != nil) {
        bool re = [MyGameAppControllerInstance isAdAvailable];
        return re;
    }
    else
    {
        return false;
    }
}

+(void) showOpenAd:(bool)isShowFirst
{
    if (MyGameAppControllerInstance != nil) {
        if (!isShowFirst) {
            [MyGameAppControllerInstance tryToPresentAd:true];
        } else {
            if (!_isFirstOpenAdShowed) {
                [MyGameAppControllerInstance tryToPresentAd:true];
            }
        }
    }
}

+ (void)checkFetchOpenAd
{
    if (MyGameAppControllerInstance != nil) {
        [MyGameAppControllerInstance requestOpenAd];
    }
}

+ (void)setOpenAdAllowShow:(bool)isShow
{
    _isOpenAdAllowShow = isShow;
}

+ (void)requestIDFA:(int)allversion
{
    // dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(1.0 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
    dispatch_async(dispatch_get_main_queue(), ^{
        if (allversion == 0) {
            if (@available(iOS 14.5, *)) {
                [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                    // Tracking authorization completed. Start loading ads here.
                    // [self loadAd];
                    [MyGameAppController mylog:[NSString stringWithFormat:@"requestIDFA: %lu", (unsigned long)status]];
                    NSString* nsstatus = [NSString stringWithFormat:@"%lu", (unsigned long)status];
                    UnitySendMessage(GameAdsHelper_NAME, "requestIDFACallBack", [nsstatus UTF8String]);
                }];
            } else {
                UnitySendMessage(GameAdsHelper_NAME, "requestIDFACallBack", "3");
            }
            } else {
            if (@available(iOS 14, *)) {
                [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                    // Tracking authorization completed. Start loading ads here.
                    // [self loadAd];
                    [MyGameAppController mylog:[NSString stringWithFormat:@"requestIDFA: %lu", (unsigned long)status]];
                    NSString* nsstatus = [NSString stringWithFormat:@"%lu", (unsigned long)status];
                    UnitySendMessage(GameAdsHelper_NAME, "requestIDFACallBack", [nsstatus UTF8String]);
                }];
            } else {
                UnitySendMessage(GameAdsHelper_NAME, "requestIDFACallBack", "3");
            }
        }
    });
}

#pragma mark - GADFullScreenContentDelegate

/// Tells the delegate that the ad failed to present full screen content.
- (void)ad:(nonnull id<GADFullScreenPresentingAd>)ad
didFailToPresentFullScreenContentWithError:(nonnull NSError *)error {
    [MyGameAppController mylog:@"openad didFailToPresentFullSCreenCContentWithError"];
    self.gameOpenAd = nil;
    isOpenAdShowing = false;
    [self requestOpenAd];
}

/// Tells the delegate that the ad presented full screen content.
- (void)adDidPresentFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad {
    [MyGameAppController mylog:@"openad adDidPresentFullScreenContent"];
    isOpenAdShowing = true;
}

/// Tells the delegate that the ad dismissed full screen content.
- (void)adDidDismissFullScreenContent:(nonnull id<GADFullScreenPresentingAd>)ad {
    [MyGameAppController mylog:@"openad adDidDismissFullScreenContent"];
    self.gameOpenAd = nil;
    isOpenAdShowing = false;
    [self requestOpenAd];
    UnitySendMessage(GameAdsHelper_NAME, "onShowOpenNative", "onAdClose");
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(MyGameAppController)


