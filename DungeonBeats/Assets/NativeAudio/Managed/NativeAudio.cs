// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace E7.Native
{
    /// <summary>
    /// Place your audio (.wav 44100Hz PCM 16-bit) in [`StreamingAssets`](https://docs.unity3d.com/Manual/StreamingAssets.html) folder (it will be copied directly to native). Then call Load to get a pointer. Which you can then use it with `NativeAudio.Play`.
    /// </summary>
    public class NativeAudio
    {

#if UNITY_IOS
        [DllImport("__Internal")]
        internal static extern int _Initialize();

        [DllImport("__Internal")]
        internal static extern int _LoadAudio(string soundUrl);

        [DllImport("__Internal")]
        internal static extern int _PrepareAudio(int index);

        [DllImport("__Internal")]
        internal static extern int _PlayAudio(int index, float volume, float pan);

        [DllImport("__Internal")]
        internal static extern void _PlayAudioWithSourceIndex(int sourceIndex, float volume, float pan);

        [DllImport("__Internal")]
        internal static extern void _UnloadAudio(int index);

        [DllImport("__Internal")]
        internal static extern void _StopAudio(int sourceIndex);
#endif

#if UNITY_ANDROID
        /// <summary>
        /// [Android] Set the total number of AudioTrack you wants to instantiate here. It equals to a concurrent sound you can play. The maximum per device is 32 but just to be safe set it around under 24 if you really need more.
        /// </summary>
        private const int androidAudioTrackInstances = 8;

        /// <summary>
        /// I decided on MODE_STATIC of AudioTrack since it potentially has the lowest latency from being able to preload.
        /// But we have to face some trade offs :
        /// 1. The size needs to be given while creating it. How large should it be?
        /// 2. The common strategy is to create a new AudioTrack every time you play a sound then we could make the size fit an audio perfectly.
        /// 3. To minimize latency, I want to create all AudioTrack at once at Initialize and not when playing. So we can't do that.
        /// 4. An approach is we will have a fixed size that is large enough for all possible audio in your game. We instantiate all
        ///AudioTrack to this size. We lose more memory than we have to in exchange for not having to worry about overflowing bufferSize
        /// 5. There is a backup measure when an incoming sound is larger. We will reinstantiate only that AudioTrack to fit that sound and left it at that size. Other AudioTrack remains at starting fixed size.
        /// This is that size in seconds. If you play any audio longer than this much seconds it needs to reinstantiate 1 AudioTrack.
        /// 
        /// For memory consideration : 3 seconds at 16-bit PCM Stereo 44000Hz is : ((16 * 440000 * 2) * 5)/8 = 529200 bytes = 0.53 MB
        /// If at `androidAudioTrackInstances` you have set 8 instances of AudioTrack, you lose 8 * 0.53 = 4.24 MB of memory immediately on instantiate even without any sound loaded.
        /// </summary>
        private const float androidMaxAudioLength = 3.0f;

        private static AndroidJavaClass androidNativeAudio;
        internal static AndroidJavaClass AndroidNativeAudio
        {
            get
            {
                if (androidNativeAudio == null)
                {
                    androidNativeAudio = new AndroidJavaClass("com.Exceed7.NativeAudio.NativeAudio");
                }
                return androidNativeAudio;
            }
        }

        internal const string AndroidInitialize = "Initialize";
        internal const string AndroidLoadAudio = "LoadAudio";
        internal const string AndroidStopAudio = "StopAudio";
        internal const string AndroidPrepareAudio = "PrepareAudio";
        internal const string AndroidPlayAudio = "PlayAudio";
        internal const string AndroidUnloadAudio = "UnloadAudio";

#endif

        private static bool initialized = false;
        /// <summary>
        /// [iOS] Initializes OpenAL. Then 32 OpenAL sources will be allocated all at once. You have a maximum of 32 concurrency shared for all sounds.
        /// [Android] Instantiate a set number of AudioTrack. Since the hard limit of 32 instantiable AudioTrack is shared for the whole device we don't know how many can Native Audio use. The default is 8 which means you can play 8 sounds concurrently. You can configure this number in NativeAudio.cs's constant variable `androidAudioTrackInstances`. All AudioTrack can handle up to `androidMaxAudioLength` which you can also set. If while instantiating AudioTrack it fails to create more, it will have just that as many as possible AudioTrack.
        /// </summary>
        public static void Initialize()
        {
            if (!initialized)
            {
                int errorCode;
#if UNITY_IOS
                errorCode = _Initialize();
                if (errorCode == -1)
                {
                    throw new System.Exception("There is an error initializing Native Audio.");
                }
                //There is also a check at native side but just to be safe here.
                initialized = true;
#elif UNITY_ANDROID
                errorCode = AndroidNativeAudio.CallStatic<int>(AndroidInitialize, androidAudioTrackInstances, androidMaxAudioLength);
                if(errorCode == -1)
                {
                    throw new System.Exception("There is an error initializing Native Audio.");
                }
                else if(errorCode > 0)
                {
                    Debug.LogWarning("Wants to instantiate " + androidAudioTrackInstances + " AudioTracks but reched the limit and instantiated only " + errorCode + " AudioTracks.");
                }
                //There is also a check at native side but just to be safe here.
                initialized = true;
#endif
            }
        }

        /// <summary>
        /// Loads audio at `audioPath`. If this is the first time loading any audio it will call `NativeAudio.Initialize()` automatically which might take a bit more time.
        /// [iOS] Loads an audio into OpenAL's output audio buffer. (Max 256) This buffer will be paired to one of 32 OpenAL source when you play it.
        /// [Android] Loads an audio into a `byte[]` array at native side. This array will be `write` into one of available `AudioTrack` when you play it.
        /// </summary>
        /// <param name="audioPath">The file must be in `StreamingAssets` folder and in .wav PCM 16-bit format with 44100 Hz sampling rate. If the file is `SteamingAssets/Hit.wav` use "Hit.wav" (WITH the extension).</param>
        /// <returns> An object that stores a number. Native side can pair this number with an actual loaded audio data when you want to play it. You can `Play`, `Prepare`, or `Unload` with this object. `Load` returns null on error, for example : wrong name, not existing in StreamingAssets, calling in Editor </returns>
        public static NativeAudioPointer Load(string audioPath)
        {
            if (!initialized)
            {
                NativeAudio.Initialize();
            }

#if UNITY_IOS
            int startingIndex = _LoadAudio(audioPath);
            if (startingIndex == -1)
            {
                //Debug.LogError("You have either Load-ed a nonexistent audio file or allocated over the iOS's OpenAL buffer amount hard limit. Check your StreamingAssets folder for the correct name, or call nativeAudioPointer.Unload() to free up the quota."); 
                return null;
            }
            else
            {
                return new NativeAudioPointer(audioPath, startingIndex);
            }
#elif UNITY_ANDROID
            int startingIndex = AndroidNativeAudio.CallStatic<int>(AndroidLoadAudio, audioPath);
            if(startingIndex == -1)
            {
                //This "error" is expected, it is not recomended to make it verbose but you should handle the returned null.

                //Debug.LogError("You have either Load-ed a nonexistent audio file or allocated over the Android's AudioTrack hard limit. Check your StreamingAssets folder for the correct name, or call nativeAudioPointer.Unload() to free up the quota."); 
                return null;
            }
            else
            {
                return new NativeAudioPointer(audioPath, startingIndex);
            }
#else
            //Load is defined on editor so that autocomplete shows up, but it is a stub. If you mistakenly use the pointer in editor instead of forwarding to normal sound playing method you will get a null reference error.
            return null;
#endif
        }

    }
}