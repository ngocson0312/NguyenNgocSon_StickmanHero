#import <Foundation/Foundation.h>

@interface MyGameAdOpeniOS : NSObject

+(instancetype)sharedInstance;

#ifdef __cplusplus
extern "C" {
#endif
    
    void loadNative(char* path, bool isCache);
    bool showNative(char* path, int flagBtNo, int isFull);
    void loadAndShowNative(char* path, bool isCache, int flagBtNo, int isFull);
    
#ifdef __cplusplus
}
#endif


@end

@interface PlayableAdsModel : NSObject
@end

@interface WaitLoadingModel : NSObject
@property BOOL isCache;
@property NSString* path;
@end

