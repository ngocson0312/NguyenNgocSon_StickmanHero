#import <Foundation/Foundation.h>

@interface MyAdmobLowiOS : NSObject

+(instancetype)sharedInstance;

#ifdef __cplusplus
extern "C" {
#endif
    void LowInitializeNative();
    void LowSetBannerPosNative(int pos, float dxcenter);
    void LowShowBannerNative(char* adsId, int pos, int orien, bool iPad, float dxcenter);
    void LowHideBannerNative();
    void LowClearCurrFullNative();
    void LowLoadFullNative(char* adsId);
    bool LowShowFullNative();
    void LowClearCurrGiftNative();
    void LowLoadGiftNative(char* adsId);
    bool LowShowGiftNative();
    
#ifdef __cplusplus
}
#endif


@end
