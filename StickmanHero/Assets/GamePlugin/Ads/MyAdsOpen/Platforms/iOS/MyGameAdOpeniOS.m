#import "MyGameAdOpeniOS.h"

#import <UIKit/UIKit.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <UnityFramework/UnityFramework-Swift.h>
#import <objc/runtime.h>
#import <mach/mach.h>
#import <mach/mach_host.h>

#import "UnityAppController.h"

#import <WebKit/WKPreferences.h>
#import <WebKit/WKWebpagePreferences.h>
#import <WebKit/WKWebView.h>
#import <WebKit/WKWebViewConfiguration.h>
#import <WebKit/WKUserContentController.h>
#import <WebKit/WKScriptMessageHandler.h>

const char* const MyAdsOpenBridge_NAME = "MyAdsOpenBridge";
bool isLoading = false;
bool isShowWhenLoad = false;
int _flagBtNo = 1;
int _isFull = 1;

@interface PlayableAdsModel ()

@property BOOL isCache;
@property NSString* path;
@property WKWebView* webView;
@property UIView* adsView;
@property UIButton* btClose;

@end

@implementation PlayableAdsModel
@end

@implementation WaitLoadingModel
@end

//----------------------------------

@interface MyGameAdOpeniOS () <WKScriptMessageHandler>

@property (strong) NSMutableDictionary<NSString*, PlayableAdsModel*>* dicPlayads;
@property (strong) NSString* pathLoading;
@property (strong) PlayableAdsModel* adsLoading;
@property (strong) PlayableAdsModel* adsShowing;
@property (strong) NSMutableArray<WaitLoadingModel*>* listWaitLoading;

@end

@implementation MyGameAdOpeniOS

+ (instancetype)sharedInstance
{
    static MyGameAdOpeniOS *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[MyGameAdOpeniOS alloc] init];
        // Do any other initialisation stuff here
    });
    return sharedInstance;
}

- (id)init
{
    self = [super init];
    
    _dicPlayads = [[NSMutableDictionary alloc] init];
    _pathLoading = @"";
    _adsLoading = nil;
    _adsShowing = nil;
    _listWaitLoading = [[NSMutableArray alloc] init];
    
    return self;
}

-(void) load:(NSString*)path cache:(BOOL)isCache
{
    NSLog(@"mysdk: myopen load path=%@ iscache=%d", path, isCache);
    if ([self.dicPlayads objectForKey:path]) {
        NSLog(@"mysdk: myopen load has cache");
        UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBLoaded", "");
    } else {
        if (!isLoading) {
            NSLog(@"mysdk: myopen load loading");
            isLoading = true;
            _pathLoading = path;
            isShowWhenLoad = false;
            [self loadWeb:path cache:isCache];
        } else {
            if (_pathLoading == nil || [_pathLoading length] < 5 || [_pathLoading compare:path] == 0) {
                NSLog(@"mysdk: myopen load add to wait");
                WaitLoadingModel* wait = [[WaitLoadingModel alloc] init];
                [_listWaitLoading arrayByAddingObject:wait];
            }
        }
    }
}

-(bool) show:(NSString*)path flagClose:(int)flagBtno isFull:(int)isFull
{
    NSLog(@"mysdk: myopen show path=%@ close=%d full=%d", path, flagBtno, isFull);
    if (_adsShowing) {
        NSLog(@"mysdk: myopen ads is showing");
        return false;
    } else {
        _adsShowing = [_dicPlayads objectForKey:path];
        if (!_adsShowing) {
            _adsShowing = nil;
            return false;
        }
        UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBOpen", "");
        [self callJavaScript:_adsShowing.webView method:@"show" param:nil];
        if (isFull == 1) {
            _adsShowing.webView.frame = CGRectMake(0, 0, _adsShowing.adsView.bounds.size.width, _adsShowing.adsView.bounds.size.height);
        } else {
            _adsShowing.webView.frame = CGRectMake(0, 50, _adsShowing.adsView.bounds.size.width - 20, _adsShowing.adsView.bounds.size.height - 100);
        }
        if (flagBtno == 0) {
            _adsShowing.btClose.hidden = YES;
        } else {
            int sbt =_adsShowing.btClose.bounds.size.width;
            if (isFull == 1) {
                _adsShowing.btClose.center = CGPointMake(_adsShowing.adsView.bounds.size.width - sbt/2, sbt/2);
            } else {
                _adsShowing.btClose.center = CGPointMake(_adsShowing.adsView.bounds.size.width - 20 - sbt/2, 50 + sbt/2);
            }
            NSTimeInterval delayInSeconds = 3.0;
            dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, (int64_t)(delayInSeconds * NSEC_PER_SEC));
            dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
                if (_adsShowing) {
                    _adsShowing.btClose.hidden = NO;
                }
            });
        }
        _adsShowing.adsView.hidden = NO;
        return true;
    }
}

-(void)loadAndShow:(NSString*)path cache:(BOOL)isCache flagClose:(int)flagBtno isFull:(int)isFull
{
    NSLog(@"mysdk: myopen loadAndShow path=%@ iscache=%d close=%d full=%d", path, isCache, flagBtno, isFull);
    if ([self.dicPlayads objectForKey:path]) {
        NSLog(@"mysdk: myopen loadAndShow has cache");
        UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBLoaded", "");
        [self show:path flagClose:flagBtno isFull:isFull];
    } else {
        if (!isLoading) {
            NSLog(@"mysdk: myopen loadAndShow loading");
            NSFileManager *filemgr = [NSFileManager defaultManager];
            if ([filemgr fileExistsAtPath:path] == NO) {
                UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBLoadFail", "1");
            } else {
                isLoading = true;
                _pathLoading = path;
                isShowWhenLoad = true;
                _flagBtNo = flagBtno;
                _isFull = isFull;
                [self loadWeb:path cache:isCache];
            }
        } else {
            UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBLoadFail", "0");
        }
    }
}

-(void)loadWeb:(NSString*)path cache:(BOOL)isCache
{
    NSLog(@"mysdk: myopen loadWeb path=%@ iscache=%d ", path, isCache);
    UIViewController* rootview = ((UnityAppController *)[UIApplication sharedApplication].delegate).rootViewController;
    
    _adsLoading = [[PlayableAdsModel alloc] init];
    _adsLoading.isCache = isCache;
    _adsLoading.path = path;
    _adsLoading.adsView = [[UIView alloc] initWithFrame:rootview.view.bounds];
    _adsLoading.adsView.backgroundColor=[UIColor colorWithRed:0 green:0 blue:0 alpha:0.4];
    [rootview.view addSubview: _adsLoading.adsView];
    [rootview.view bringSubviewToFront:_adsLoading.adsView];
    if (!isShowWhenLoad) {
        _adsLoading.adsView.hidden = YES;
    }
    
    WKPreferences *preferences = [[WKPreferences alloc] init];
    preferences.javaScriptEnabled = YES; // Here its set
    preferences.javaScriptCanOpenWindowsAutomatically = YES;
    WKWebViewConfiguration *theConfiguration = [[WKWebViewConfiguration alloc] init];
    theConfiguration.preferences = preferences;
    [theConfiguration.userContentController addScriptMessageHandler:self name:@"CBLoadOk"];
    if (@available(iOS 14.0, *)) {
        WKWebpagePreferences *pagepre = [[WKWebpagePreferences alloc] init];
        pagepre.allowsContentJavaScript = YES;
        theConfiguration.defaultWebpagePreferences = pagepre;
    }
    NSFileManager *filemgr = [NSFileManager defaultManager];
    NSData *databuffer = [filemgr contentsAtPath:path];
    NSString *contenthtml = [[NSString alloc] initWithData:databuffer encoding:NSUTF8StringEncoding];
    
    CGRect webRect = CGRectMake(0, 50, rootview.view.bounds.size.width - 20, rootview.view.bounds.size.height - 100);
    int xbt = rootview.view.bounds.size.width - 20;
    int ybt = 50;
    if (isShowWhenLoad) {
        if (_isFull) {
            xbt = rootview.view.bounds.size.width;
            ybt = 0;
            webRect = CGRectMake(0, 0, rootview.view.bounds.size.width, rootview.view.bounds.size.height);
        }
    }
    
    _adsLoading.webView = [[WKWebView alloc] initWithFrame:webRect configuration:theConfiguration];
    [_adsLoading.webView loadHTMLString:contenthtml baseURL:nil];
    [_adsLoading.adsView addSubview:_adsLoading.webView];
    
    int ww = rootview.view.bounds.size.width;
    int hh = rootview.view.bounds.size.height;
    int sizebt;
    if (ww < hh) {
        sizebt = 30*375/ww;
    } else {
        sizebt = 30*667/hh;
    }
    
    _adsLoading.btClose = [[UIButton alloc] initWithFrame:CGRectMake(xbt - sizebt, ybt, sizebt, sizebt)];
    _adsLoading.btClose.hidden = YES;
    [_adsLoading.btClose setBackgroundImage:[UIImage imageNamed:@"button_close.png"] forState:UIControlStateNormal];
    
    // Add an action in current code file (i.e. target)
    [_adsLoading.btClose addTarget:self action:@selector(buttonPressed:)
                  forControlEvents:UIControlEventTouchUpInside];
    [_adsLoading.adsView addSubview:_adsLoading.btClose];
    
    NSString* tmpPath = path;
    NSTimeInterval delayInSeconds = 30.0;
    dispatch_time_t popTime = dispatch_time(DISPATCH_TIME_NOW, (int64_t)(delayInSeconds * NSEC_PER_SEC));
    dispatch_after(popTime, dispatch_get_main_queue(), ^(void){
        if (isLoading && _adsLoading != nil && [tmpPath compare:_adsLoading.path] == 0) {
            isLoading = false;
            _adsLoading.path = nil;
            [_adsLoading.adsView removeFromSuperview];
            [_adsLoading.webView removeFromSuperview];
            [_adsLoading.btClose removeFromSuperview];
            _adsLoading.adsView = nil;
            _adsLoading.webView = nil;
            _adsLoading.btClose = nil;
            NSLog(@"mysdk: myopen loadWeb not load ok- not call cb loadok");
            UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBLoadFail", "2");
        }
    });
}

-(void)continueLoad
{
    if (_listWaitLoading.count > 0) {
        NSString* path = _listWaitLoading[0].path;
        BOOL isc = _listWaitLoading[0].isCache;
        [_listWaitLoading removeObjectAtIndex:0];
        [self load:path cache:isc];
    }
}

- (void)buttonPressed:(UIButton *)button
{
    [self closeAds];
}

-(void)closeAds
{
    if (_adsShowing != nil) {
        if (!_adsShowing.isCache) {
            [_dicPlayads removeObjectForKey:_adsShowing.path];
            [_adsShowing.webView removeFromSuperview];
            [_adsShowing.adsView removeFromSuperview];
            _adsShowing.webView = nil;
            _adsShowing.webView = nil;
            _adsShowing.path = nil;
        } else {
            _adsShowing.adsView.hidden = YES;
            [self callJavaScript:_adsShowing.webView method:@"close" param:nil];
        }
        UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBClose", "");
    }
    _adsShowing = nil;
}

- (void)userContentController:(WKUserContentController *)userContentController
      didReceiveScriptMessage:(WKScriptMessage *)message
{
    NSLog(@"mysdk: myopen loadok");
    isLoading = false;
    if (_pathLoading != nil && [_pathLoading length] >= 5 && _adsLoading != nil) {
        [_dicPlayads setValue:_adsLoading forKey:_pathLoading];
        NSString* tmppath = _pathLoading;
        _pathLoading = @"";
        _adsLoading = nil;
        UnitySendMessage(MyAdsOpenBridge_NAME, "iOSCBLoaded", "");
        if (isShowWhenLoad) {
            isShowWhenLoad = false;
            [self show:tmppath flagClose:_flagBtNo isFull:_isFull];
        }
    }
    [self continueLoad];
}

-(void) callJavaScript:(WKWebView*)view method:(NSString*)methodName param:(NSMutableArray*)params
{
    NSString* contencall = @"javascript:try{";
    contencall = [contencall stringByAppendingString:methodName];
    contencall = [contencall stringByAppendingString:@"("];
    if (params != nil) {
        bool isbegin = true;
        for (id obj in params) {
            if ([obj isKindOfClass:[NSString class]]) {
                // NSString-specific code.
                if (!isbegin) {
                    contencall = [contencall stringByAppendingString:@",'"];
                } else {
                    contencall = [contencall stringByAppendingString:@"'"];
                }
                NSString* nsobj = (NSString*)obj;
                [nsobj stringByReplacingOccurrencesOfString:@"'" withString:@"\\'"];
                contencall = [contencall stringByAppendingString:nsobj];
                
                contencall = [contencall stringByAppendingString:@"'"];
                isbegin = false;
            } else if ([obj isKindOfClass:[NSNumber class]]) {
                // NSNumber-specific code.
                if (!isbegin) {
                    contencall = [contencall stringByAppendingString:@","];
                }
                NSNumber* num = (NSNumber*)obj;
                contencall = [contencall stringByAppendingString:[num stringValue]];
                isbegin = false;
            }
        }
    }
    contencall = [contencall stringByAppendingFormat:@")}catch(error){console.error(error.message);}"];
    [view evaluateJavaScript:contencall completionHandler:nil];
}

# pragma mark - C API

void loadNative(char* path, bool isCache) {
    NSString* nspath = [NSString stringWithUTF8String:path];
    [[MyGameAdOpeniOS sharedInstance] load:nspath cache:isCache];
}
bool showNative(char* path, int flagBtNo, int isFull) {
    NSString* nspath = [NSString stringWithUTF8String:path];
    return [[MyGameAdOpeniOS sharedInstance] show:nspath flagClose:flagBtNo isFull:isFull];
}

void loadAndShowNative(char* path, bool isCache, int flagBtNo, int isFull) {
    NSString* nspath = [NSString stringWithUTF8String:path];
    [[MyGameAdOpeniOS sharedInstance] loadAndShow:nspath cache:isCache flagClose:flagBtNo isFull:isFull];
}

@end
