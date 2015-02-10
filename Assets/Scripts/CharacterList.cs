using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterList : MonoBehaviour {

	private List<Character> mCharacterList = new List<Character>();

	public void WakeUp () {
		foreach (Character character in mCharacterList) {
			character.Wakeup ();
		}
	}

	public void Remove(Character character){
		mCharacterList.Remove (character);
	}

	public void Add(Character character){
		mCharacterList.Add (character);
	}

	public void DestroyAll(){
		foreach (Character character in mCharacterList) {
			Destroy (character.gameObject);
		}
	}

	public void CreateNewInstance(){
		mCharacterList = new List<Character> ();
	}

	public void StartLive(){
		foreach (Character character in mCharacterList) {
			character.StartLive ();
		}
	}

	public void FinishLive(){
		foreach (Character character in mCharacterList) {
			character.FinishLive ();
		}
	}

	public void Sleep(){
		foreach (Character character in mCharacterList) {
			character.Sleep ();
		}
	}

	public void Remove(int index){
		Character character = mCharacterList [0];
		mCharacterList.Remove (character);
		Destroy (character.gameObject);
	}
}
