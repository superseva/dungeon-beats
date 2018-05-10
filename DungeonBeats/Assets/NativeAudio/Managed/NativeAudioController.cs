// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E7.Native
{
    /// <summary>
    /// A reference to the native audio source used to play a sound. You can for example, Stop() it before it ends.
    /// 
    /// The native implementation round robins the audio players, so if you hold onto this variable for a long time
    /// it is possible that it will be reused to play other sound. A method like Replay() (if exists someday) might produce unexpected result.
    /// </summary>
    public class NativeAudioController
    {
        private int instanceIndex;
        /// <param name="instanceIndex">All actions issued on this `NativeAudioController` will be for a specific sound output instance at the native side. (Not source, unlike `NativeAudioPointer`'s `index`)</param>
        public NativeAudioController(int instanceIndex)
        {
            this.instanceIndex = instanceIndex;
        }

        /// <summary>
        /// Immediately stop a specific played sound.
        /// [iOS] One of the total 32 OpenAL sources will stop.
        /// [Android] One of all AudioTrack will stop.
        /// </summary>
        public void Stop()
        {
#if UNITY_IOS
            NativeAudio._StopAudio(instanceIndex);
#elif UNITY_ANDROID
            NativeAudio.AndroidNativeAudio.CallStatic(NativeAudio.AndroidStopAudio, instanceIndex);
#endif
        }
    }
}