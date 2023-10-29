#import <Foundation/Foundation.h>

//@class VibrateHelper;
@interface MyGameNativeiOS : NSObject

+ (instancetype)sharedInstance;

#ifdef __cplusplus
extern "C"
{
#endif

    char *getLanguageCodeNative();
    char *getCountryCodeNative();
    char *getAdsIdentifyNative();
    char *getGiftBoxNative();
    void vibrateNative(int type);
    long getMemoryLimit();
    long getPhysicMemoryInfo();
    float getScreenWidthNative();
    void setRemoveAds4OpenAdsNative(int isRemove);
    void configAppOpenAdNative(int typeShow, int showat, int isShowFirstOpen, int deltime, char* adid);
    void showOpenAdsNative(bool isShowFirst);
    bool isOpenAdLoadedNative();
    void setAllowShowOpenNative(bool isAllow);
    bool appReviewNative();
    void requestIDFANative(int isallversion);
    void showCMPNative();

#ifdef __cplusplus
}
#endif

@end
