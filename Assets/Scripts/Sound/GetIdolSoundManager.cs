using UnityEngine;
using System.Collections;

public class GetIdolSoundManager : MonoSingleton<GetIdolSoundManager> {

	public AudioClip[] SEclipArray;
	private AudioSource[] mSEsourceArray;

	public override	void OnInitialize () {
		mSEsourceArray = new AudioSource[SEclipArray.Length];
		for (int i = 0; i < mSEsourceArray.Length; i++) {
			mSEsourceArray [i] = gameObject.AddComponent<AudioSource> ();    
			mSEsourceArray [i].clip = SEclipArray [i];
		}
	}

	public void PlayVoice (int id) {
		AudioSource audioSource = mSEsourceArray [id];
		audioSource.Play ();
	}
}
