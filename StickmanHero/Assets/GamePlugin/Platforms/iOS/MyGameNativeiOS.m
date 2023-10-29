#define Enable_openAds_NT
//#define Test_Show_CMP

#import "MyGameNativeiOS.h"

#import <UIKit/UIKit.h>
#import <StoreKit/StoreKit.h>
#import <CoreTelephony/CTCarrier.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <AudioToolbox/AudioServices.h>
#import <AdSupport/ASIdentifierManager.h>
#import <UnityFramework/UnityFramework-Swift.h>
#import <objc/runtime.h>
#import <mach/mach.h>
#import <mach/mach_host.h>

#include <UserMessagingPlatform/UserMessagingPlatform.h>

#import "MyGameAppController.h"
#import "MyAdmobUtiliOS.h"

@implementation MyGameNativeiOS

+ (instancetype)sharedInstance
{
    static MyGameNativeiOS *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[MyGameNativeiOS alloc] init];
        // Do any other initialisation stuff here
    });
    return sharedInstance;
}

-(BOOL) isStringValideBase64:(NSString*)string
{
    NSString *regExPattern = @"^(?:[A-Za-z0-9+/]{4})*(?:[A-Za-z0-9+/]{2}==|[A-Za-z0-9+/]{3}=)?$";
    
    NSRegularExpression *regEx = [[NSRegularExpression alloc] initWithPattern:regExPattern options:NSRegularExpressionCaseInsensitive error:nil];
    NSUInteger regExMatches = [regEx numberOfMatchesInString:string options:0 range:NSMakeRange(0, [string length])];
    return regExMatches != 0;
}

-(char*) getLanguageCode {
    NSString * language = [[NSLocale preferredLanguages] firstObject];
    if (language != NULL) {
        if ([language containsString:@"-"]) {
            NSUInteger idx = [language rangeOfString:@"-"].location;
            language = [language substringToIndex:idx];
        }
        const char* code = [language UTF8String];
        char* re = malloc([language length]);
        strcpy(re, code);
        return re;
    }
    NSLocale *local = [NSLocale currentLocale];
    if (local != NULL) {
        NSString *countryCode = [local objectForKey:NSLocaleLanguageCode];
        if(countryCode == NULL) {
            countryCode = @"";
        }
        const char* code = [countryCode UTF8String];
        char* re = malloc([countryCode length]);
        strcpy(re, code);
        return re;
    } else {
        NSString *nsre = @"";
        const char* cre = [nsre UTF8String];
        char* re = malloc([nsre length]);
        strcpy(re, cre);
        return re;
    }
}

-(char*) getCountryCode {
    NSLocale *local = [NSLocale currentLocale];  // get the current locale.
    if (local != NULL) {
        NSString *countryCode = [local objectForKey:NSLocaleCountryCode];
        if(countryCode == NULL) {
            countryCode = @"";
        }
        const char* code = [countryCode UTF8String];
        char* re = malloc([countryCode length]);
        strcpy(re, code);
        return re;
    } else {
        NSString *nsre = @"";
        const char* cre = [nsre UTF8String];
        char* re = malloc([nsre length]);
        strcpy(re, cre);
        return re;
    }
}

-(char*) getAdsIdentify {
    //@try
    //{
    NSUUID *identifier = [[ASIdentifierManager sharedManager] advertisingIdentifier];
    NSString *str = [identifier UUIDString];
    const char* cadid = [str UTF8String];
    char* re = malloc([str length]);
    strcpy(re, cadid);
    return re;
    //}
    //@catch(NSException *exception)
    //{
    
    //}
}

-(char*) getGiftBox {
    NSString* sub = [[NSUserDefaults standardUserDefaults] objectForKey:@"mem_gift_box"];
    if(sub == NULL) {
        sub = @"";
    }
    const char* cub = [sub UTF8String];
    char* re = malloc([sub length]);
    strcpy(re, cub);
    return re;
}

-(void) vibrate:(int)type {
    if (type == 0) {
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    } else {
        [[MyGameiOSSwift shared] VibrateWithType:type];
    }
}

-(long)getMemoryLimit {
    mach_port_t host_port;
    mach_msg_type_number_t host_size;
    vm_size_t pagesize;
    
    host_port = mach_host_self();
    host_size = sizeof(vm_statistics_data_t) / sizeof(integer_t);
    host_page_size(host_port, &pagesize);
    
    vm_statistics_data_t vm_stat;
    
    if (host_statistics(host_port, HOST_VM_INFO, (host_info_t)&vm_stat, &host_size) != KERN_SUCCESS) {
        NSLog(@"mysdk: Failed to fetch vm statistics");
        return 0;
    } else {
        /* Stats in bytes */
        natural_t mem_used = (vm_stat.active_count +
                              vm_stat.inactive_count +
                              vm_stat.wire_count) * pagesize;
        natural_t mem_free = vm_stat.free_count * pagesize;
        natural_t mem_total = mem_used + mem_free;
        NSLog(@"mysdk: used: %u free: %u total: %u", mem_used / 1024 / 1024, mem_free / 1024 / 1024, mem_total / 1024 / 1024);
        return mem_total;
    }
}

-(long)getPhysicMemoryInfo {
    struct mach_task_basic_info info;
    mach_msg_type_number_t size = sizeof(info);
    kern_return_t kerr = task_info(mach_task_self(), MACH_TASK_BASIC_INFO, (task_info_t)&info, &size);
    if (kerr == KERN_SUCCESS)
    {
        float used_bytes = info.resident_size;
        float total_bytes = [NSProcessInfo processInfo].physicalMemory;
        NSLog(@"mysdk: Used: %f MB out of %f MB (%f%%)", used_bytes / 1024.0f / 1024.0f, total_bytes / 1024.0f / 1024.0f, used_bytes * 100.0f / total_bytes);
        return (long)total_bytes;
    } else {
        NSLog(@"mysdk: Failed to fetch vm statistics");
        return 0;
    }
}

-(float)getScreenWidth{
    UIView *unityView = [MyAdmobUtiliOS unityGLViewController].view;
    float w = unityView.bounds.size.width;
    return w;
}

-(bool)appReview {
    return [[MyGameiOSSwift shared] MyAppReview];
}

-(void)setRemoveAds4OpenAdsNative:(int)isRemove {
    [[NSUserDefaults standardUserDefaults] setInteger:isRemove forKey:@"is_remove_ads_native"];
}

-(void)configAppOpenAdNative:(int)typeShow
                      showAt:(int)showat
             isShowFirstOpen:(int)isShow
                     delTime:(int)deltime
                        adId:(NSString*)adId
{
    NSInteger co = [[NSUserDefaults standardUserDefaults] integerForKey:@"count_game_open"];
    NSInteger oldshowAt = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_show_at"];
    NSInteger oldshowfirst = [[NSUserDefaults standardUserDefaults] integerForKey:@"openad_is_showfirst"];
    
    [[NSUserDefaults standardUserDefaults] setInteger:typeShow forKey:@"openad_type_show"];
    [[NSUserDefaults standardUserDefaults] setInteger:showat forKey:@"openad_show_at"];
    [[NSUserDefaults standardUserDefaults] setInteger:isShow forKey:@"openad_is_showfirst"];
    [[NSUserDefaults standardUserDefaults] setInteger:deltime forKey:@"openad_deltime"];
    if (adId != nil && [adId length] > 10)
    {
        [[NSUserDefaults standardUserDefaults] setObject:adId forKey:@"openad_id"];
    }
    NSLog(@"mysdk: setDefaultOpenAd typeShow=%d, showat=%d, showfirst=%d, del=%d", typeShow, showat, isShow, deltime);
    if (typeShow == 0 || typeShow == 2 || typeShow == 4) {
        if (co < oldshowAt || oldshowfirst == 0) {
            if (co >= showat && isShow >= 1) {
                [MyGameAppController checkFetchOpenAd];
            }
        }
    }
}

-(void)showCMP
{
    // Create a UMPRequestParameters object.
    UMPRequestParameters *parameters = [[UMPRequestParameters alloc] init];
#ifdef Test_Show_CMP
    UMPDebugSettings *debugSettings = [[UMPDebugSettings alloc] init];
    UIDevice *device = [UIDevice currentDevice];
    debugSettings.testDeviceIdentifiers = @[ [[device identifierForVendor] UUIDString] ];
    debugSettings.geography = UMPDebugGeographyEEA;
    parameters.debugSettings = debugSettings;
#endif
    // Set tag for under age of consent. Here NO means users are not under age.
    parameters.tagForUnderAgeOfConsent = NO;
    
    // Request an update to the consent information.
    [UMPConsentInformation.sharedInstance
     requestConsentInfoUpdateWithParameters:parameters
     completionHandler:^(NSError *_Nullable error) {
        if (error) {
            // Handle the error.
            NSLog(@"mysdk: showCMP err=%@", error);
            UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMPFinish", "request Error");
        } else {
            // The consent information state was updated.
            // You are now ready to check if a form is
            // available.
            UMPFormStatus formStatus = UMPConsentInformation.sharedInstance.formStatus;
            NSLog(@"mysdk: showCMP formStatus=%ld", formStatus);
            if (formStatus == UMPFormStatusAvailable) {
                [self loadForm];
            } else {
                UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMPFinish", "request form status not available");
            }
        }
    }];
}

- (void)loadForm {
    NSLog(@"mysdk: showCMP loadForm");
    [UMPConsentForm loadWithCompletionHandler:^(UMPConsentForm *form, NSError *loadError) {
        if (loadError) {
            // Handle the error
            NSLog(@"mysdk: showCMP loadForm err=%@", loadError);
            UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMPFinish", "load form error");
        } else {
            // Present the form
            NSLog(@"mysdk: showCMP loadForm show consentStatus = %ld", UMPConsentInformation.sharedInstance.consentStatus);
            if (UMPConsentInformation.sharedInstance.consentStatus == UMPConsentStatusUnknown
                || UMPConsentInformation.sharedInstance.consentStatus == UMPConsentStatusRequired)
            {
                UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMP", "show");
                [form presentFromViewController:[MyAdmobUtiliOS unityGLViewController]
                              completionHandler:^(NSError *_Nullable dismissError) {
                    NSLog(@"mysdk: showCMP loadForm show com consentStatus=%ld", UMPConsentInformation.sharedInstance.consentStatus);
                    if (UMPConsentInformation.sharedInstance.consentStatus == UMPConsentStatusObtained) {
                        // App can start requesting ads.
                        NSString* tcv2 = [[NSUserDefaults standardUserDefaults] stringForKey:@"IABTCF_TCString"];
                        NSLog(@"mysdk: showCMP loadForm show tcv2=%@", tcv2);
                        if (tcv2 != nil) {
                            //NSLog(@"mysdk: showCMP loadForm show tcv2=%@", tcv2);
                            UnitySendMessage(GameAdsHelper_NAME, "iOSCBCMP", [tcv2 UTF8String]);
                        } else {
                            UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMPFinish", "tcv2 is invalid");
                        }
                    } else {
                        UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMPFinish", "consent status not obtained");
                    }
                }];
            } else {
                UnitySendMessage(GameAdsHelper_NAME, "iOSCBOnShowCMPFinish", "form not show");
            }
        }
    }];
}

# pragma mark - C API
char* getLanguageCodeNative() {
    return [[MyGameNativeiOS sharedInstance] getLanguageCode];
}

char* getCountryCodeNative() {
    return [[MyGameNativeiOS sharedInstance] getCountryCode];
}

char* getAdsIdentifyNative() {
    return [[MyGameNativeiOS sharedInstance] getAdsIdentify];
}

char* getGiftBoxNative() {
    return [[MyGameNativeiOS sharedInstance] getGiftBox];
}

void vibrateNative(int type) {
    [[MyGameNativeiOS sharedInstance] vibrate:type];
}

long getMemoryLimit() {
    return [[MyGameNativeiOS sharedInstance] getMemoryLimit];
}

long getPhysicMemoryInfo() {
    return [[MyGameNativeiOS sharedInstance] getPhysicMemoryInfo];
}

float getScreenWidthNative() {
    return [[MyGameNativeiOS sharedInstance] getScreenWidth];
}

void setRemoveAds4OpenAdsNative(int isRemove) {
    [[MyGameNativeiOS sharedInstance] setRemoveAds4OpenAdsNative:isRemove];
}

void configAppOpenAdNative(int typeShow, int showat, int isShowFirstOpen, int deltime, char* adid) {
    NSString* nsadsid = [NSString stringWithUTF8String:adid];
    return [[MyGameNativeiOS sharedInstance] configAppOpenAdNative:typeShow showAt:showat isShowFirstOpen:isShowFirstOpen delTime:deltime adId:nsadsid];
}

void showOpenAdsNative(bool isShowFirst) {
#ifdef Enable_openAds_NT
    [MyGameAppController showOpenAd:isShowFirst];
#endif
}

bool isOpenAdLoadedNative() {
#ifdef Enable_openAds_NT
    return [MyGameAppController openAdLoaded];
#else
    return false;
#endif
}

void setAllowShowOpenNative(bool isAllow)
{
    [MyGameAppController setOpenAdAllowShow:isAllow];
}

bool appReviewNative() {
    return[[MyGameNativeiOS sharedInstance] appReview];
}

void requestIDFANative(int isallversion) {
    [MyGameAppController requestIDFA:isallversion];
}

void showCMPNative() {
    [[MyGameNativeiOS sharedInstance] showCMP];
}

@end


