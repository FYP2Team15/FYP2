using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScript : MonoBehaviour {
	
	void Start()
	{
		GameObject.Find ("Bg volume").GetComponentInChildren<Slider> ().value = PlayerPrefs.GetFloat ("bg_Volume");
		GameObject.Find ("effect volume").GetComponentInChildren<Slider> ().value = PlayerPrefs.GetFloat ("effect_Volume");
	}
	
	void Update()
	{

	}
	
	public void ResetUserData()
	{
		float bgVolume_temp, effectVolume_temp;
		bgVolume_temp = GameObject.Find ("Bg volume").GetComponentInChildren<Slider> ().value = PlayerPrefs.GetFloat ("bg_Volume");
		effectVolume_temp = GameObject.Find ("effect volume").GetComponentInChildren<Slider> ().value = PlayerPrefs.GetFloat ("effect_Volume");
		
		PlayerPrefs.DeleteAll ();
		
		// Set back player's volume settings
		PlayerPrefs.SetFloat ("bg_Volume", bgVolume_temp);
		PlayerPrefs.SetFloat ("effect_Volume", effectVolume_temp);
		
	}
	
	public void adjustBackgroundVolume(float volume)
	{
		PlayerPrefs.SetFloat ("bg_Volume", volume);
	}
	
	public void adjustEffecVolume(float volume)
	{
		PlayerPrefs.SetFloat ("effect_Volume", volume);
	}
	
	public void GoBack()
	{
		Application.LoadLevel ("MainPage");
	}
}