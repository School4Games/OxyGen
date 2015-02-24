using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuState : MonoBehaviour, IMenuMessageTarget 
{
	public Slider soundVolume;
	public Slider musicVolume;
	public Toggle showTutorial;

	public Text text;

	public GameObject mainMenu;
	public GameObject optionsMenu;

	void loadPrefs ()
	{
		soundVolume.value = PlayerPrefs.GetFloat ("soundVolume", 1);
		musicVolume.value = PlayerPrefs.GetFloat ("musicVolume", 1);
		showTutorial.isOn = System.Convert.ToBoolean (PlayerPrefs.GetInt ("showTutorial", 1));
	}

	#region IMenuMessageTarget implementation

	public void OnStart ()
	{
		Application.LoadLevel (1);
	}

	public void OnApply ()
	{
		PlayerPrefs.SetFloat ("soundVolume", soundVolume.value);
		PlayerPrefs.SetFloat ("musicVolume", musicVolume.value);
		PlayerPrefs.SetInt ("showTutorial", System.Convert.ToInt16 (showTutorial.isOn));
		mainMenu.SetActive (true);
		optionsMenu.SetActive (false);
	}

	public void OnOptions ()
	{
		mainMenu.SetActive (false);
		optionsMenu.SetActive (true);
	}

	#endregion
}
