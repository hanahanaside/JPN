using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterVoiceManager : MonoSingleton<CharacterVoiceManager> {

	private List<AudioSource> mIdolVoiceList;

	public override	void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		mIdolVoiceList = new List<AudioSource> ();
		for (int i = 0; i < 49; i++) {
			AudioClip audioClip = Resources.Load<AudioClip> ("Audios/Voice/Idol/20" + (i + 1));
			AudioSource audioSource = gameObject.AddComponent<AudioSource> ();
			audioSource.clip = audioClip;
			mIdolVoiceList.Add(audioSource);    
		}
	}

	public void PlayVoice (int index) {
		if (PrefsManager.instance.SE_ON) {
			AudioSource audioSource = mIdolVoiceList [index];
			audioSource.Play ();
		}
	}
}
