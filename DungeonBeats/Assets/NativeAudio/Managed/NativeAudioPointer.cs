// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace E7.Native
{
	/// <summary>
	/// Please do not create an instance of this class by yourself!
	/// Call NativeAudio.Load to get it.
	/// </summary>
    public class NativeAudioPointer 
    {
		private string soundPath;
		private int startingIndex;

		//Amount is always 1 for now. It has no effect.
		private int amount;
		private bool isUnloaded;

		//[iOS] When preparing to play, OpenAL need you to remember some information so that the next Play will use the prepared things correctly.
#if UNITY_IOS
		private int prepareIndex;
		private bool prepared;
#endif
		private int currentIndex;

        /// <summary>
        /// This will automatically cycles for you if the amount is not 1.
        /// </summary>
        public int NextIndex
		{
            get
            {
				int toReturn = currentIndex;
				currentIndex = currentIndex + 1;
                if (currentIndex > startingIndex + amount - 1)
				{
					currentIndex = startingIndex;
				}
				return toReturn;
			}
		}

        /// <param name="amount">Right now amount is not used anywhere yet.</param>
        public NativeAudioPointer(string soundPath, int index, int amount = 1)
		{
            this.soundPath = soundPath;
			this.startingIndex = index;
			this.amount = amount;

			this.currentIndex = index;
		}

        /// <summary>
        /// Plays an audio using the underlying OS's native method. A number stored in this object will be used to determine which loaded audio data at native side that we would like to play. If you previously call `Prepare()` it will take effect here.

		/// [iOS] (Unprepared) Native Audio remembered which of the total of 32 OpenAL source that we just played. It will get the next one (wraps to the first instance when already at 32nd instance), find the correct sound buffer and assign it to that source, then play it.
		/// [iOS] (Prepared) Instead of using buffer index (the sound, not the sound player) it will play the source at the time you call `Prepare` immediately without caring if the sound in that source is currently the same as when you call `Prepare` or not. After calling this `Play`, the prepare status will be reset to unprepared, and the next `Play` will use buffer index as usual.

		/// [Android] (Unprepared) It searches for `AudioTrack` which already contains the `byte[]` representing this sound that is not currently playing. If not found, we load the byte array into any non playing `AudioTrack`. If every `AudioTrack` is playing, it stops and replaces the first instance. Then play it. We use this "searching" approach instead of just assign and play round-robin style of iOS because on Android we are bounded with very low number of AudioTrack and the `write` method takes more time than looping.
		/// [Android] (Prepared) Same as unprepared, but `Prepare` method have already make sure such `AudioTrack` with the correct `byte[]` data exists so the search should found it quickly. Prepare status is never reset when calling `Play()`.

        /// </summary>
        /// <param name="volume">From 0.0f to 1.0f. The interpolation is in linear space.</param>
        /// <param name="pan">-1.0f : Left, 0.0f : Center, 1.0f : Right [iOS] Not implemented yet. All sounds will be at center 0.0f. [Android] It works.</param>
		/// <returns> Please treat this return as `void` for now. The class will be usable in the future version.</returns>
        public NativeAudioController Play(float volume = 1, float pan = 0)
        {
			if(isUnloaded)
			{
				throw new System.Exception("You cannot play an unloaded NativeAudio.");
			}

            int playedSourceIndex = -1;
#if UNITY_IOS
            if (prepared)
            {
				//This is using source index. It means we have already loaded our sound to that source with Prepare.
                NativeAudio._PlayAudioWithSourceIndex(this.prepareIndex, volume, pan);
				playedSourceIndex = this.prepareIndex;
            }
            else
            {
				//This is using buffer index. Which source it use will be determined at native side.
                playedSourceIndex = NativeAudio._PlayAudio(this.NextIndex, volume, pan);
            }
            prepared = false;
#elif UNITY_ANDROID
            playedSourceIndex = NativeAudio.AndroidNativeAudio.CallStatic<int>(NativeAudio.AndroidPlayAudio, this.NextIndex, volume, pan);
#endif
            return new NativeAudioController(playedSourceIndex); //Empty object for now
        }

        /// <summary>
        /// Shave off as much start up time as possible to play a sound. The majority of load time is already in `Load` but `Prepare` might help a bit more, or not at all. You can also call `Play()` without calling this first. The effectiveness depends on platform's audio library's approach :

        /// [iOS] Assigns OpenAL audio buffer to a source. `NativeAudioPointer` then remembers this source index. The next `Play()` you call will immediately play this remembered source without caring what sound is in it instead of using a buffer index to get sound to pair with the next available source. This means if in between `Prepare()` and `Play()` you have played 32 sounds, the resulting sound will be something else as other sound has already put their buffer into the source you have remembered.

        /// [Android] `write` a loaded audio byte array to any non-playing `AudioTrack` so that the next `Play()` does not require a `write` and can play right away. If all `AudioTrack` is playing the first `AudioTrack` will immediately stop to receive a new `byte[]` data.

        /// </summary>
        public void Prepare()
        {
#if UNITY_IOS
            prepareIndex = NativeAudio._PrepareAudio(NextIndex);
			prepared = true;
#elif UNITY_ANDROID
            NativeAudio.AndroidNativeAudio.CallStatic(NativeAudio.AndroidPrepareAudio, NextIndex);
#endif
        }

		public override string ToString()
		{
			return soundPath;
		}

        /// <summary>
        /// Do not call `Play` anymore after unloading.

		/// [iOS] Unload OpenAL buffer. The total number of 32 OpenAL source does not change.
		/// [Android] Unload the `byte[]` array by dereferencing it. We have to wait for the actual unload is by Java's garbage collector. The total number of `AudioTrack` does not change.

        /// </summary>
        public void Unload()
        {
#if UNITY_IOS
			if(!isUnloaded)
			{
				NativeAudio._UnloadAudio(startingIndex);
				isUnloaded = true;
			}
#elif UNITY_ANDROID
			if(!isUnloaded)
			{
				for(int i = startingIndex; i < startingIndex + amount; i++)
				{
                    NativeAudio.AndroidNativeAudio.CallStatic(NativeAudio.AndroidUnloadAudio, i);
				}
				isUnloaded = true;
			}
#endif
        }

    }
}