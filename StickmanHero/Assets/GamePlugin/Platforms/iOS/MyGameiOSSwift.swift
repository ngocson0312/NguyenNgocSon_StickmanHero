//
//  TestHaptic.swift
//  UnityFramework
//
//  Created by Nguyen Viet on 9/29/20.
//

import Foundation
import UIKit
import StoreKit

@objc public class MyGameiOSSwift : NSObject {
    
    @objc public static let shared = MyGameiOSSwift()
    
    @objc public func Vibrate(type:Int) {
        switch type {
        case 1:
            //print("haptic error")
            let generator = UINotificationFeedbackGenerator()
            generator.notificationOccurred(.error)
            
        case 2:
            //print("haptic success")
            let generator = UINotificationFeedbackGenerator()
            generator.notificationOccurred(.success)
            
        case 3:
            //print("haptic warning")
            let generator = UINotificationFeedbackGenerator()
            generator.notificationOccurred(.warning)
            
        case 4:
            //print("haptic impactOccurred light")
            let generator = UIImpactFeedbackGenerator(style: .light)
            generator.impactOccurred()
            
        case 5:
            //print("haptic impactOccurred medium")
            let generator = UIImpactFeedbackGenerator(style: .medium)
            generator.impactOccurred()
            
        case 6:
            //print("haptic impactOccurred heavy")
            let generator = UIImpactFeedbackGenerator(style: .heavy)
            generator.impactOccurred()
            
        default:
            //print("haptic selectionChanged")
            let generator = UISelectionFeedbackGenerator()
            generator.selectionChanged()
        }
    }
    
    @objc public func MyAppReview() -> Bool {
        if #available(iOS 10.3, *) {
            let twoSecondsFromNow = DispatchTime.now() + 0.75
            DispatchQueue.main.asyncAfter(deadline: twoSecondsFromNow) {
                SKStoreReviewController.requestReview()
            }
            return true;
        } else {
            return false;
        }
    }
}
