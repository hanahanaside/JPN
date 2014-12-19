using UnityEngine;
using System.Collections;

public class SoundManager : MonoSingleton<SoundManager> {

	public enum SE_CHANNEL {
		Button,
		GetCoin,
		GetIdol_1,
		GetIdol_2,
		Katsu,
		Hanauta,
		Plane,
		Cheer,
		GenerateCoin
	}

	;

	public enum BGM_CHANNEL {
		Main,
		Live,
		Puzzle}

	;

	public AudioClip[] bgmClipArray;
	public AudioClip[] SEclipArray;
	private AudioSource[] mSEsourceArray;
	private AudioSource mBGMAudioSource;

	public override	void OnInitialize () {
		DontDestroyOnLoad (gameObject);
		mBGMAudioSource = gameObject.AddComponent<AudioSource> ();
		mBGMAudioSource.loop = true;
		mSEsourceArray = new AudioSource[SEclipArray.Length];
		for (int i = 0; i < mSEsourceArray.Length; i++) {
			mSEsourceArray [i] = gameObject.AddComponent<AudioSource> ();    
			mSEsourceArray [i].clip = SEclipArray [i];
		}
	}

	public void PlayBGM (BGM_CHANNEL bgmChannel) {
		int channelId = (int)bgmChannel;
		mBGMAudioSource.clip = bgmClipArray [channelId];
		mBGMAudioSource.Play ();
	}

	public void StopBGM () {
		mBGMAudioSource.Stop ();
	}

	public void PlaySE (SE_CHANNEL seChannel) {
		int seChannelId = (int)seChannel;
		AudioSource audioSource = mSEsourceArray [seChannelId];
		switch(seChannel){
		case SE_CHANNEL.GetCoin:
			audioSource.volume = 0.5f;
			break;
		case SE_CHANNEL.Cheer:
			audioSource.volume = 0.5f;
			break;
		}
		audioSource.Play ();
	}

	public void FadeoutSE(SE_CHANNEL seChannel){
		StartCoroutine ("Fadeout",seChannel);
	}

	IEnumerator Fadeout(SE_CHANNEL seChannel)
	{
		int seChannelId = (int)seChannel;
		AudioSource audioSource = mSEsourceArray [seChannelId];
		float duration = 1f;
		float currentTime = 0.0f;
		float waitTime = 0.02f;
		float firstVol = audioSource.volume;
		while (duration > currentTime)
		{
			currentTime += Time.fixedDeltaTime;
			audioSource.volume = Mathf.Clamp01(firstVol * (duration - currentTime) / duration);
			yield return new WaitForSeconds(waitTime);
		}
	}
}
