﻿using UnityEngine;
using System.Collections;

public class SoundManager : MonoSingleton<SoundManager> {

	public enum SE_CHANNEL {
		Button,
		GetCoin,
		GetIdol_1,
		GetIdol_2,
		Katsu,
		Hanauta}

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
		mSEsourceArray [seChannelId].Play ();
	}
}