//
// (c) LiveAid allrights Reserved.
//

#import <UIKit/UIKit.h>

#define SP_BANNER_HEIGHT 50
#define SP_ICON_WIDTH    57
#define SP_ICON_HEIGHT   57

@interface SPInterstitial : NSObject

- (void)show;

@end

@interface SPAdContext : NSObject

- (id)initWithMediaId:(NSString *)mediaId;
- (id)initWithMediaId:(NSString *)mediaId debug:(BOOL)debug;
- (SPInterstitial *)createInterstitial:(NSInteger)locationId;
- (UIView *)createIconView:(NSInteger)locationId origin:(CGPoint)origin;
- (UIView *)createBannerView:(NSInteger)locationId y:(CGFloat)y;

@end
