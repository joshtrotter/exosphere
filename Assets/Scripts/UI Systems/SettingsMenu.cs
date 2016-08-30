using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SettingsMenu : UISystem {

	public struct Settings {
		public bool muted, inverted, shakeEnabled, tutEnabled;
		public float volume, camSensitivity, shakeSensitivity;

		public Settings(bool muted, float volume, bool inverted, float camSensitivity, bool shakeEnabled, float shakeSensitivity, bool tutEnabled){
			this.muted = muted;
			this.volume = volume;
			this.inverted = inverted;
			this.camSensitivity = camSensitivity;
			this.shakeEnabled = shakeEnabled;
			this.shakeSensitivity = shakeSensitivity;
			this.tutEnabled = tutEnabled;
		}
	}

	public static SettingsMenu controller;

	public GameObject dropPanel;

	public Toggle muteToggle;
	public Slider volumeSlider;
	public Text volumeText;

	public Toggle invertToggle;
	public Slider camSensitivitySlider;

	public Toggle shakeToggle;
	public Slider shakeSensitivitySlider;

	public Toggle tutToggle;

	private Settings currentSettings;

	//default settings
	private readonly int SETTINGS_VER = 1; //increment this on a change of default settings to reset to new defaults, don't use zero
	private readonly Settings DEFAULT_SETTINGS = new Settings(false, 0.5f, false, 0.5f, false, 0.5f, true);

	public override void Awake(){
		//set up singleton instance
		if (controller == null) {
			controller = this;
			DontDestroyOnLoad (this);
			if (PlayerPrefs.GetInt ("SettingsVer", defaultValue: 0) == SETTINGS_VER) {
				currentSettings.muted = PlayerPrefs.GetInt ("Muted") == 1 ? true : false;
				currentSettings.volume = PlayerPrefs.GetFloat ("Volume");
				currentSettings.inverted = PlayerPrefs.GetInt ("Inverted") == 1 ? true : false;
				currentSettings.camSensitivity = PlayerPrefs.GetFloat ("CamSensitivity");
				currentSettings.shakeEnabled = PlayerPrefs.GetInt ("ShakeEnabled") == 1 ? true : false;
				currentSettings.shakeSensitivity = PlayerPrefs.GetFloat ("ShakeSensitivity");
				currentSettings.muted = PlayerPrefs.GetInt ("Muted") == 1 ? true : false;
				ApplySettings(currentSettings);
			} else {
				PlayerPrefs.SetInt("SettingsVer", SETTINGS_VER);
				ApplySettings(DEFAULT_SETTINGS);
			}
		} else if (controller != this) {
			Destroy(gameObject);
		}
		base.Awake ();
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0).Play ();
	}

	public override void Show ()
	{
		Time.timeScale = 0f;
		dropPanel.SetActive (true);
		dropPanel.transform.DOLocalMoveY (0, 0.5f).SetUpdate(true).Play ();
	}

	public override void Hide ()
	{
		//Time.timeScale = 1f;
		dropPanel.SetActive (false);
	}

	public override void BackKey ()
	{
		dropPanel.transform.DOLocalMoveY ((Screen.height * 1.2f), 0.5f).SetUpdate(true).OnComplete(Deregister).Play ();
	}

	public void ResetToDefaults(){
		ApplySettings (DEFAULT_SETTINGS);
	}

	public void SetMuted(bool muted){
		currentSettings.muted = muted;
		muteToggle.isOn = muted;
		AudioListener.pause = muted;
		PlayerPrefs.SetInt ("Muted", muted ? 1 : 0);
	}

	public void SetVolume(float volume){
		currentSettings.volume = volume;
		volumeSlider.value = volume;
		volumeText.text = (int)(volume * 100) + "%"; 
		AudioListener.volume = volume;
		PlayerPrefs.SetFloat ("Volume", volume);
	}

	public void SetInverted(bool inverted){
		currentSettings.inverted = inverted;
		invertToggle.isOn = inverted;
		PlayerPrefs.SetInt("Inverted", inverted ? 1 : 0);
	}

	public void SetCamSensitivity (float sensitivity){
		currentSettings.camSensitivity = sensitivity;
		camSensitivitySlider.value = sensitivity;
		PlayerPrefs.SetFloat ("CamSensitivity", sensitivity);
	}

	public void SetShakeEnabled(bool enabled){
		currentSettings.shakeEnabled = enabled;
		shakeToggle.isOn = enabled;
		PlayerPrefs.SetInt("ShakeEnabled", enabled ? 1 : 0);
	}
	
	public void SetShakeSensitivity (float sensitivity){
		currentSettings.shakeSensitivity = sensitivity;
		shakeSensitivitySlider.value = sensitivity;
		PlayerPrefs.SetFloat ("ShakeSensitivity", sensitivity);
	}

	public void SetTutEnabled(bool enabled){
		currentSettings.tutEnabled = enabled;
		tutToggle.isOn = enabled;
		PlayerPrefs.SetInt("TutEnabled", enabled ? 1 : 0);
	}

	private void ApplySettings(Settings settings){
		SetMuted (settings.muted);
		SetVolume (settings.volume);
		SetInverted (settings.inverted);
		SetCamSensitivity (settings.camSensitivity);
		SetShakeEnabled (settings.shakeEnabled);
		SetShakeSensitivity (settings.shakeSensitivity);
		SetTutEnabled (settings.tutEnabled);
	}

}
