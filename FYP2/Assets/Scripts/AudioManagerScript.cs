using UnityEngine;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {
	
	static AudioManagerScript instance;
	
	public AudioClip[] sList;
	private bool soundPlaying = false;
	private string loadedLevelName_temp;
	


	void Awake()
	{
		if (instance)
		{
			Destroy (gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
	
	void Start()
	{
		loadedLevelName_temp = Application.loadedLevelName;
		PlayerPrefs.SetFloat ("bg_Volume",100);
	}
	
	void Update ()
	{
		if (loadedLevelName_temp != Application.loadedLevelName)
		{
			soundPlaying = false;
			loadedLevelName_temp = Application.loadedLevelName;
		}
		// Handle one song playing at a time
		if (!soundPlaying)
		{
			switch (Application.loadedLevelName)
			{
			case "MainPage":
				playSong ((int) 0);
				break;

			case "FYP2":
			case "Level0":
			case "Level1":
			case "Level2":
			case "Level3":
			case "Level4":
			case "Level5":
			case "Level6":
				playSong ((int) 1);
				break;

		
				
			default:
				break;
			}
			
			soundPlaying = true;
		}
		
		GetComponent<AudioSource> ().volume =  PlayerPrefs.GetFloat ("bg_Volume");
	}
	
	void playSong (int songName, bool isLooping = true) // array list with songs - scene dependent(play depend on scene)
	{
		if (GetComponent<AudioSource> ().clip != sList [songName] ) //check for songs played at different scenes
		{
			GetComponent<AudioSource> ().clip = sList [songName];
			GetComponent<AudioSource> ().loop = isLooping;
			GetComponent<AudioSource> ().Play ();
		}
	}
	
	public void playOnce (int soundName) // array list with sounds
	{
		GetComponent<AudioSource> ().PlayOneShot (sList [soundName], PlayerPrefs.GetFloat ("effect_Volume"));
	}                                //play sound one shot
	public void playOnceCustom (AudioClip soundName) // array list with sounds
	{
		GetComponent<AudioSource> ().PlayOneShot (soundName);
	}                                //play sound one shot

}
