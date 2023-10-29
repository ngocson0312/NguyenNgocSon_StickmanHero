// filename BuildPostProcessor.cs
// put it in a folder Assets/Editor/
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
#if UNITY_IOS || UNITY_IPHONE
using UnityEditor.iOS.Xcode;
#endif

#if UNITY_IOS || UNITY_IPHONE
public class BuildPostProcessor {

    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string path) {
        if (buildTarget == BuildTarget.iOS) {

            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;

            // example of changing a value:
            // rootDict.SetString("CFBundleVersion", "6.6.6");

            // example of adding a boolean key...
            // < key > ITSAppUsesNonExemptEncryption </ key > < false />
            rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-2777953690987264~6759076705");
            rootDict.SetString("NSUserTrackingUsageDescription", "This identifier will be used to deliver personalized ads to you.");
#if ENABLE_ADS_IRON
            PlistElementArray arr = rootDict.CreateArray("SKAdNetworkItems");
            string[] dicnet = {
                "su67r6k2v3.skadnetwork"//iron
                , "4pfyvq9l8r.skadnetwork" //adcolony
                , "cstr6suwn9.skadnetwork"//admob
                , "ludvb6z3bs.skadnetwork"//applovin
                // , "f38h382jlk.skadnetwork"//chartboost
                , "v9wttpbfk9.skadnetwork"//fb
                , "n38lu8286q.skadnetwork"//fb
                // ,"9g2aggbj52.skadnetwork"//Flyber
                // ,"nu4557a4je.skadnetwork"//HyprMx
                // ,"wzmmz9fp6w.skadnetwork"//inMobi
                // ,"v4nxqhlyqp.skadnetwork"//Maio
                // , "V4NXQHLYQP.skadnetwork"//Maio
                , "22mmun2rn5.skadnetwork"//Pangle non cn
                , "238da6jt44.skadnetwork"//Pangle cn
                // ,"424m5254lk.skadnetwork"//Snap
                // ,"ecpz2srf59.skadnetwork"//Tapjoy
                , "4DZT52R2T5.skadnetwork"//Unity
                //, "GTA9LK7P23.skadnetwork"//Vungle
                };
            for (int i = 0; i < dicnet.Length; i++)
            {
                PlistElementDict dicarr = arr.AddDict();
                dicarr.SetString("SKAdNetworkIdentifier", dicnet[i]);
            }
#endif

            File.WriteAllText(plistPath, plist.WriteToString());

        }
    }

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // We need to construct our own PBX project path that corrently refers to the Bridging header
            // var projPath = PBXProject.GetPBXProjectPath(buildPath);
            var projPath = buildPath + "/Unity-iPhone.xcodeproj/project.pbxproj";
            var proj = new PBXProject();
            proj.ReadFromFile(projPath);

            //var targetGuid = proj.GetUnityFrameworkTargetGuid();
            var targetGuid = proj.GetUnityMainTargetGuid();

            //// Configure build settings
            proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            //proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/iOS/MyUnityPlugin/Source/MyUnityPlugin-Bridging-Header.h");
            //proj.SetBuildProperty(targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "MyUnityPlugin-Swift.h");
            //proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");

            proj.WriteToFile(projPath);
            try
            {
                string orien = SettingBuildAndroid.getMemSetting("key_mem_df_orien", "0");
                if (!orien.Contains("0") && !orien.Contains("1") && !orien.Contains("4"))
                {
                    FixBannerFalliOSLanscape(buildPath);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    static void FixBannerFalliOSLanscape(string path) {
        string pfilem = path + "/Libraries/MaxSdk/AppLovin/Plugins/iOS/MAUnityAdManager.m";
        if (File.Exists(pfilem))
        {
            string[] linesRead = File.ReadAllLines(pfilem);
            List<string> linesWrite = new List<string>();
            int stateCheck = 0;
            bool isw = false;
            for (int i = 0; i < linesRead.Length; i++)
            {
                if (stateCheck == 0)
                {
                    if (linesRead[i].Contains("- (void)positionAdViewForAdUnitIdentifier:(NSString *)adUnitIdentifier adFormat:(MAAdFormat *)adFormat")) 
                    {
                        stateCheck = 1;
                    }
                    linesWrite.Add(linesRead[i]);
                }
                else if (stateCheck == 1)
                {
                    if (linesRead[i].Contains("CGSize adViewSize = CGSizeMake(adViewWidth, adViewHeight);")) 
                    {
                        if (!linesRead[i - 1].Contains("[self setPosBanner4Show:adView bannerWidth:adViewWidth bannerHeight:adViewHeight];"))
                        {
                            linesWrite.Add("        [self setPosBanner4Show:adView bannerWidth:adViewWidth bannerHeight:adViewHeight];");
                            isw = true;
                        }
                        linesWrite.Add(linesRead[i]);
                    }
                    else if (linesRead[i].Contains("UILayoutGuide *layoutGuide;"))
                    {
                        linesWrite.Add(linesRead[i]);
                        for (int j = 0; j < 8; j++)
                        {
                            i++;
                            if (!linesRead[i].StartsWith("//")) 
                            {
                                linesRead[i] = "//" + linesRead[i];
                                isw = true;
                            }
                            linesWrite.Add(linesRead[i]);
                        }
                        linesWrite.Add("        layoutGuide = superview.layoutMarginsGuide;");
                    }
                    else if (linesRead[i].Contains("[NSLayoutConstraint activateConstraints: constraints];"))
                    {
                        linesWrite.Add(linesRead[i]);
                        if (!linesRead[i + 1].Contains("[self setPosBanner4Show:adView bannerWidth:adViewWidth bannerHeight:adViewHeight];"))
                        {
                            linesWrite.Add("        [self setPosBanner4Show:adView bannerWidth:adViewWidth bannerHeight:adViewHeight];");
                            isw = true;
                        }
                        stateCheck = 2;
                    }
                    else
                    {
                        linesWrite.Add(linesRead[i]);
                    }
                }
                else if (stateCheck == 2)
                {
                    linesWrite.Add(linesRead[i]);
                    if (linesRead[i].CompareTo("}") == 0)
                    {
                        bool isadd = false;
                        for (int j = 0; j < 4; j++)
                        {
                            if (linesRead[i].Contains("-(void)setPosBanner4Show:(UIView*)bn bannerWidth:(float)bnw bannerHeight:(float)bnh"))
                            {
                                isadd = true;
                            }
                        }
                        if (!isadd)
                        {
                            isw = true;
                            linesWrite.Add("");
                            AddCodeFix(linesWrite);
                        }
                        stateCheck = -1;
                    }
                }
                else
                {
                    linesWrite.Add(linesRead[i]);
                }
            }
            if (isw) {
                System.IO.File.WriteAllLines(pfilem, linesWrite);
                linesWrite.Clear();
            }
        }
    }

    static void AddCodeFix(List<string> linesWrite)
    {
        string[] all = {
            "-(void)setPosBanner4Show:(UIView*)bn bannerWidth:(float)bnw bannerHeight:(float)bnh",
            "{",
            "    UIView *unityView = [self unityViewController].view;",
            "    float wscr = unityView.bounds.size.width;",
            "    if (wscr < unityView.bounds.size.height) {",
            "        wscr = unityView.bounds.size.height;",
            "    }",
            "    float xbn = (wscr - bnw)/2;",
            "//    NSLog(@\"checkmax: parent=%f, bnw=%f, xbn=%f\", wscr, bnw, xbn);",
            "//    NSLog(@\"checkmax: x=%f, y=%f, w=%f, h=%f\", bn.frame.origin.x, bn.frame.origin.y, bn.frame.size.width, bn.frame.size.height);",
            "//    NSLog(@\"checkmax: bound x=%f, y=%f, w=%f, h=%f\", bn.bounds.origin.x, bn.bounds.origin.y, bn.bounds.size.width, bn.bounds.size.height);",
            "//    NSLog(@\"checkmax: cx=%f, cy=%f\", bn.center.x, bn.center.y);",
            "//    for (UIView *i in bn.subviews){",
            "//        //        NSLog(@\"checkmax: before subview=%@\", i);",
            "//        i.frame = CGRectMake(0, 0, bnw, bnh);",
            "//        for (UIView *vvv in i.subviews){",
            "//            //            NSLog(@\"checkmax: vvv before subview=%@\", vvv);",
            "//            vvv.frame = CGRectMake(0, 0, bnw, bnh);",
            "//",
            "//            for (UIView *kkk in vvv.subviews){",
            "//                //                NSLog(@\"checkmax: kkk before subview=%@\", kkk);",
            "//                kkk.frame = CGRectMake(0, 0, bnw, bnh);",
            "//                //                NSLog(@\"checkmax: kkk subview=%@\", kkk);",
            "//                for (UIView *ppp in kkk.subviews){",
            "//                    NSLog(@\"checkmax: ppp before subview=%@\", ppp);",
            "//                    ppp.frame = CGRectMake(0, 0, bnw, bnh);",
            "//                    //                NSLog(@\"checkmax: kkk subview=%@\", ppp);",
            "//                    if ([ppp isKindOfClass:[UIScrollView class]]) {",
            "//                        UIScrollView *scrv = (UIScrollView *)ppp;",
            "//                        scrv.contentSize = CGSizeMake(bnw, bnh);",
            "//                        scrv.contentOffset = CGPointMake(160, 0);",
            "//                        for (UIView *qqq in ppp.subviews){",
            "//                            NSLog(@\"checkmax: qqq before subview=%@\", qqq);",
            "//                            qqq.frame = CGRectMake(0, 0, bnw, bnh);",
            "//                        }",
            "//                    }",
            "//                }",
            "//            }",
            "//        }",
            "//    }",
            "//    NSLog(@\"checkmax:====================================\");",
            "    bn.frame = CGRectMake(xbn, 0, bnw, bnh);",
            "}",
        };
        for (int i = 0; i < all.Length; i++) 
        {
            linesWrite.Add(all[i]);
        }
    }
}
#endif
