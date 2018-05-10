// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using E7.Native;

public class NativeAudioDemo : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    private NativeAudioPointer nativeAudioPointer;
    private NativeAudioController nativeAudioController;

    public void Initialize()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
        NativeAudio.Initialize();
#endif
    }

    public void PlayUnityAudioSource()
    {
        audioSource.Play();
    }

    public void LoadAudio1()
    {
        Debug.Log(Application.streamingAssetsPath);
        LoadAudio("NativeAudioDemo1.wav");
    }

    public void LoadAudio2()
    {
        Debug.Log(Application.streamingAssetsPath);
        LoadAudio("NativeAudioDemo2.wav");
    }

    public void StopLatestPlay()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
        if(nativeAudioController != null)
        {
            nativeAudioController.Stop();
        }
#endif
    }

    private void LoadAudio(string name)
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
		nativeAudioPointer = NativeAudio.Load(name);
		if(nativeAudioPointer == null)
		{
			Debug.LogError("You have either Load-ed a nonexistent audio file or allocated over the Android's AudioTrack hard limit. Check your StreamingAssets folder for the correct name, or call nativeAudioPointer.Unload() to free up the quota."); 
		}
#endif
    }

    public void Prepare()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
		nativeAudioPointer.Prepare();
#endif
    }

    public void PlayAudio1()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
		nativeAudioController = nativeAudioPointer.Play();
#endif
    }

    public void PlayAudio2()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
		//Pan 1f is fully to the right.
        nativeAudioController = nativeAudioPointer.Play(0.5f, 0.5f);
#endif
    }

    public void Unload()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
		//Pan 1f is fully to the right.
        nativeAudioPointer.Unload();
#endif
    }

}
