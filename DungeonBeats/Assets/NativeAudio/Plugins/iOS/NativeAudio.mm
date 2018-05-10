// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

// Special thanks to Con for written this wonderful OpenAL tutorial : http://ohno789.blogspot.com/2013/08/playing-audio-samples-using-openal-on.html

#import "NativeAudio.h"

//#define LOG_NATIVE_AUDIO

@implementation NativeAudio

static ALCdevice *openALDevice;
static ALCcontext *openALContext;

//OpenAL sources starts at number 2400
#define kMaxConcurrentSources 32

//OpenAL buffer index starts at number 2432. (Now you know then source limit is implicitly 32)
//This number will be remembered in NativeAudioPointer at managed side.
//As far as I know there is no limit. I can allocate way over 500 sounds and it does not seems to cause any bad things.
//But of course every sound will cost memory and that should be your real limit.
//This is limit for just in case someday we discover a real hard limit, then Native Audio could warn us.
#define kMaxBuffers 1024

static NSMutableArray *openALSources;

//Error when this goes to max
static int bufferAllocationCount = 0;

//Currently "amount" is disregarded on iOS. It is always 32 but shared for all sounds played with Native Audio.

//OpenAL can play at most 32 concurrent sounds while having separated audio buffer loaded. (at most kMaxBuffers buffers)
//Unlike Android's AudioTrack or iOS's AVAudioPlayer where each buffer has its own sound channel.

//We will cycle through these 32 channels when we play any sounds.

+ (AudioFileID) openAudioFile:(NSString *)audioFilePathAsString
{
    NSURL *audioFileURL = [NSURL fileURLWithPath:audioFilePathAsString];
    
    AudioFileID afid;
    OSStatus openAudioFileResult = AudioFileOpenURL((__bridge CFURLRef)audioFileURL, kAudioFileReadPermission, 0, &afid);
    
    if (0 != openAudioFileResult)
    {
        NSLog(@"An error occurred when attempting to open the audio file %@: %d", audioFilePathAsString, (int)openAudioFileResult);
    }
    
    return afid;
}

+ (UInt32) getSizeOfAudioComponent:(AudioFileID)afid
{
    UInt64 audioDataSize = 0;
    UInt32 propertySize = sizeof(UInt64);
    
    OSStatus getSizeResult = AudioFileGetProperty(afid, kAudioFilePropertyAudioDataByteCount, &propertySize, &audioDataSize);
    
    if (0 != getSizeResult)
    {
        NSLog(@"An error occurred when attempting to determine the size of audio file.");
    }
    
    return (UInt32)audioDataSize;
}

+ (int) Initialize
{
    openALDevice = alcOpenDevice(NULL);
    openALContext = alcCreateContext(openALDevice, NULL);
    alcMakeContextCurrent(openALContext);
    
    openALSources = [[NSMutableArray alloc] init];
    
    ALuint sourceID;
    for (int i = 0; i < kMaxConcurrentSources; i++) {
        alGenSources(1, &sourceID);
        [openALSources addObject:[NSNumber numberWithUnsignedInt:sourceID]];
    }
    
#ifdef LOG_NATIVE_AUDIO
    NSLog(@"Initialized OpenAL");
#endif

    return 0; //0 = success
}

+ (void) UnloadAudio: (int) index
{
    ALuint bufferId = (ALuint)index;
    alDeleteBuffers(1, &bufferId);
    bufferAllocationCount--;
}

+ (int) LoadAudio:(const char*) soundUrl
{
    if (bufferAllocationCount > kMaxBuffers) {
        NSLog(@"Fail to load because OpenAL reaches the maximum sound buffers limit. Raise the limit or use unloading to free up the quota.");
        return -1;
    }
    
    if(openALDevice == nil)
    {
        [NativeAudio Initialize];
    }
    
    NSString *audioFilePath = [NSString stringWithFormat:@"%@/Data/Raw/%@", [[NSBundle mainBundle] resourcePath], [NSString stringWithUTF8String:soundUrl] ];

    AudioFileID afid = [NativeAudio openAudioFile:audioFilePath];
    UInt32 audioSize = [NativeAudio getSizeOfAudioComponent:afid];
    void *audioData = malloc(audioSize);
    OSStatus readBytesResult = AudioFileReadBytes(afid, false, 0, &audioSize, audioData);
    if (0 != readBytesResult)
    {
        NSLog(@"An error occurred when attempting to read data from audio file %@: %d", audioFilePath, (int)readBytesResult);
    }
    
    AudioFileClose(afid);
    
    ALuint bufferId;
    alGenBuffers(1, &bufferId);
    bufferAllocationCount++;
    
    //Here is where you need to expand if you wish to support flexible sound format other than PCM 16bit 44100Hz
    alBufferData(bufferId, AL_FORMAT_STEREO16, audioData, audioSize, 44100);
    
    if (audioData)
    {
        free(audioData);
        audioData = NULL;
    }
    
#ifdef LOG_NATIVE_AUDIO
    NSLog(@"Loaded OpenAL sound: %@ bufferId: %d size: %d",[NSString stringWithUTF8String:soundUrl], bufferId, audioSize);
#endif
    
    return bufferId;
}

static ALuint sourceCycleIndex = 0;

//Sources are selected sequentially.
//Searching for non-playing source might be a better idea to reduce sound cutoff chance
//(For example, by the time we reach 33rd sound some sound earlier must have finished playing, and we can select that one safely)
//But for performance concern I don't want to run a for...in loop everytime I play sounds.
+ (ALuint) CycleThroughSources
{
    ALuint sourceID = (ALuint)[[openALSources objectAtIndex:sourceCycleIndex] unsignedIntegerValue];
    alSourceStop(sourceID);
    sourceCycleIndex = (sourceCycleIndex + 1)%kMaxConcurrentSources;
    return sourceID;
}

+ (int)PrepareAudio:(int) alBufferIndex
{
    ALuint sourceIndex = [NativeAudio CycleThroughSources];
    alSourcei(sourceIndex, AL_BUFFER, alBufferIndex);
#ifdef LOG_NATIVE_AUDIO
    NSLog(@"Pairing OpenAL buffer: %d with source: %d", alBufferIndex, sourceIndex);
#endif
    return sourceIndex;
}

//Currently pan does not work yet. It will work once I can figure out how to pan with stereo buffer.
+ (int)PlayAudio:(int) alBufferIndex withVolume:(float)volume andPan : (float) pan
{
    ALuint sourceIndex = [NativeAudio PrepareAudio:alBufferIndex];
    [NativeAudio PlayAudioWithSourceIndex:sourceIndex withVolume:volume andPan:pan];
    return sourceIndex;
}

+ (void)StopAudio:(int) sourceIndex
{
    ALuint sourceIndexALuint = sourceIndex;
    alSourceStop(sourceIndexALuint);
}

//If you have prepared and have a source index in hand it is possible to call this and play whatever sound that is in a that source. Directly after preparing it should be the sound that you want, but later it might be something else when sources cycle to the same point again.
+ (void)PlayAudioWithSourceIndex:(int) alSourceIndex withVolume:(float)volume andPan : (float) pan
{
    alSourcef(alSourceIndex, AL_GAIN, volume);
    alSourcePlay(alSourceIndex);
    
#ifdef LOG_NATIVE_AUDIO
    NSLog(@"Played OpenAL at source index: %d volume: %f pan: %f", alSourceIndex,volume,pan);
#endif
}

// The old implementation that still uses AVAudioPlayer. I might completely remove this later.
/*
static NSMutableArray<AVAudioPlayer *> * audioPlayers;
+ (int)LoadAudioOld:(const char*) soundUrl amount:(int)amount
{
    if(amount <= 0)
    {
        return -1;
    }

    NSString *path = [NSString stringWithFormat:@"%@/Data/Raw/%@", [[NSBundle mainBundle] resourcePath], [NSString stringWithUTF8String:soundUrl] ];
    
    NSURL *url = [NSURL fileURLWithPath:path];

    if(audioPlayers == nil)
    {
        audioPlayers = [[NSMutableArray<AVAudioPlayer*> alloc] init];
    }
    
    int startingIndex = (int)[audioPlayers count];
    
    for(int i = 0; i < amount; i++)
    {
        AVAudioPlayer* newAudioPlayer = [[AVAudioPlayer alloc] initWithContentsOfURL:url error:nil];
        [newAudioPlayer setEnableRate:NO]; //Disabling pitch shifting function might help with the performance.
        [newAudioPlayer setMeteringEnabled:NO]; //Just in case this adds overhead.
        [newAudioPlayer prepareToPlay]; //With this the first play is fast, but when the playback ends it will undo the memory.

        [audioPlayers addObject:newAudioPlayer];
    }
    
#ifdef LOG_NATIVE_AUDIO
    NSLog(@"Native Audio Loaded : %@", [NSString stringWithUTF8String:soundUrl]);
#endif
    
    return startingIndex;
}

+ (void)PlayAudioOld:(int) index withVolume:(float)volume andPan : (float) pan
{
    if( index < 0 || index >= [audioPlayers count])
    {
        NSLog(@"Native Sound Play Error! Index out of range!");
        return;
    }

    AVAudioPlayer* audio = [audioPlayers objectAtIndex:index];
    
    //Since one source is playing the same sound over and over Stop is not required, it will just add a bit overhead
    //[audio stop];
    
    //AVAudioPlayer play sound with low latency, but it cause a frame skip whenever you use play or even setCurrentTime:0 without play

    [audio setPan:pan];
    [audio setVolume:volume];
    
    //If it is playing, just seek back to 0 is enough. We don't have to reload audio hardware.
    if([audio isPlaying] == NO)
    {
        [audio prepareToPlay];
        [audio play]; //Calls prepareForPlay automatically if not called yet
    }
    else
    {
        [audio setCurrentTime:0];
    }

    //When an audio finished playing it undoes prepareForPlay. We can't stop audio from finish playing so instead
    //we will load them again immediately so that the next play is fast.
}
*/

@end


extern "C" {
    int _Initialize() {
        return [NativeAudio Initialize];
    }
    
    int _LoadAudio(const char* soundUrl) {
        //return [NativeAudio LoadAudioOld:soundUrl amount:1];
        return [NativeAudio LoadAudio:soundUrl];
    }

    void _PrepareAudio(int index) {
        [NativeAudio PrepareAudio:index];
    }
    
    int _PlayAudio(int index, float volume, float pan) {
        //[NativeAudio PlayAudioOld:index withVolume: volume andPan: pan];
        return [NativeAudio PlayAudio:index withVolume: volume andPan: pan];
    }

    void _StopAudio(int sourceIndex) {
        [NativeAudio StopAudio:sourceIndex];
    }

    void _PlayAudioWithSourceIndex(int sourceIndex, float volume, float pan) {
        //[NativeAudio PlayAudioOld:index withVolume: volume andPan: pan];
        [NativeAudio PlayAudioWithSourceIndex:sourceIndex withVolume: volume andPan: pan];
        
    }

    void _UnloadAudio(int index) {
        [NativeAudio UnloadAudio:index];
    }
}
