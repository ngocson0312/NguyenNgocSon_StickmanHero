#import <Foundation/Foundation.h>

//@class VibrateHelper;
@interface MyAdmobiOS : NSObject

+(instancetype)sharedInstance;

#ifdef __cplusplus
extern "C" {
#endif
    void InitializeNative();
    void setBannerPosNative(int pos, float dxcenter);
    void showBannerNative(char* adsId, int pos, int orien, bool iPad, float dxcenter);
    void hideBannerNative();
    void clearCurrFullNative();
    void loadFullNative(char* adsId);
    bool showFullNative();
    void clearCurrGiftNative();
    void loadGiftNative(char* adsId);
    bool showGiftNative();
    
#ifdef __cplusplus
}
#endif


@end
