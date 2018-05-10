using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using E7.Native;

public class NativeAudioPlayer : MonoBehaviour {

	public static NativeAudioPlayer Instance;
	public bool initialized = false;
	public string[] audioNames;

	private List<NativeAudioPointer> audioPointers = new List<NativeAudioPointer>();

	//private NativeAudioPointer nativeAudioPointer;
    private NativeAudioController nativeAudioController;

	private void Awake()
	{
		if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start()
	{
#if UNITY_EDITOR
        Debug.Log("Please try this in a real device!");
#elif (UNITY_IOS || UNITY_ANDROID)
		StartCoroutine(DoInitializing());
#endif
	}

	IEnumerator DoInitializing(){
		Debug.Log("Initialize Native Audio");
		NativeAudio.Initialize();
		yield return new WaitForSeconds(1.0f);
		initialized = true;
		Debug.Log("native audio initialized : "+ initialized);
		if (initialized)
		{
			StartCoroutine(LoadSounds());
		}
	}

	IEnumerator LoadSounds(){

		audioPointers.Clear();

		int soundsCount = audioNames.Length;
		int soundNum = 0;
		Debug.Log("LOAD " + soundsCount + " SOUNDS:");

		while(soundNum<soundsCount){
			Debug.Log("LOADING SOUND "+ audioNames[soundNum] + "  " + Time.time);
			NativeAudioPointer nativeAudioPointer = NativeAudio.Load(audioNames[soundNum]);
			audioPointers.Add(nativeAudioPointer);
			if(nativeAudioPointer==null){
				Debug.Log("ERROR: " + audioNames[soundNum] + " NOT LOADED ");
			}
			yield return new WaitForSeconds(1.0f);
			if (nativeAudioPointer != null)
			{
				Debug.Log("PREPARE SOUND " + audioNames[soundNum] + "  " + Time.time);
				nativeAudioPointer.Prepare();
			}else{
				Debug.Log("CAN'T PREPARE NULL SOUND " + audioNames[soundNum] + "  " + Time.time);
			}
			yield return new WaitForSeconds(1.0f);
			soundNum += 1;
		}

		Debug.Log("LOADING DONE " + +Time.time);
		yield return null;
	}


	// PLAY 0 or 1
	public void PlaySound(int soundIndex)
	{

#if UNITY_EDITOR
		Debug.Log("TRY ON PHONE");
#elif UNITY_IOS
		if (audioPointers[soundIndex] != null)
		{
			nativeAudioController = audioPointers[soundIndex].Play(0.5f, 0.0f);
		}
#endif
	}


	public void StopLatestPlay()
    {

#if UNITY_EDITOR
        Debug.Log("TRY ON PHONE");
#elif UNITY_IOS
        if (nativeAudioController != null)
        {
            nativeAudioController.Stop();
        }
#endif
    }


	public void Unload()
    {
#if UNITY_EDITOR
        Debug.Log("TRY ON PHONE");
#elif UNITY_IOS
		//Debug.Log("NATIVE POINTERS COUNT :::" + audioPointers.Count);
		for (int i = 0; i < audioPointers.Count; i++)
        {
			if(audioPointers[i]!=null)
				audioPointers[i].Unload();
        }
		audioPointers.Clear();
#endif
    }

	// Update is called once per frame
	void Update () {
		
	}
}
