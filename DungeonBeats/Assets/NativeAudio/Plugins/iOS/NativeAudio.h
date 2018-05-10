// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

#import <AVFoundation/AVFoundation.h>
#import <OpenAl/al.h>
#import <OpenAl/alc.h>
#include <AudioToolbox/AudioToolbox.h>

@interface NativeAudio : NSObject
{
}

+ (int)Initialize;
+ (int)LoadAudio:(const char *)soundUrl;
+ (int)PrepareAudio:(int)alBufferIndex;
+ (int)PlayAudio:(int)alBufferIndex withVolume:(float)volume andPan:(float)pan;
+ (void)PlayAudioWithSourceIndex:(int)alSourceIndex withVolume:(float)volume andPan:(float)pan;
+ (void)UnloadAudio:(int)index;
+ (void)StopAudio:(int)sourceIndex;

@end
