using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * The base class of all SceneControllers.
 **/
public class SceneController : BaseMonoBehaviour {
	public float TotalTimeToFadeOut = 1f;
	public float TotalTimeToFadeIn = 3f;

	public bool FadeInOnEnabled = true;

	private float timeToFadeIn = -1;
	private float timeToFadeOut = -1;
	private string sceneToChangeAfterFadeOut;
	private Texture2D fadeTexture;
	private float fadeAlpha;

	public static Infinario.Infinario infinario;

	public void FadeOutAndChangeScene(string sceneName) {
		timeToFadeOut = TotalTimeToFadeOut;
		sceneToChangeAfterFadeOut = sceneName;
	}

	public void ChangeScene(string sceneName) {
		Application.LoadLevel(sceneName);
	}

	protected void Awake() {
		if (SceneController.infinario == null) {
			SceneController.infinario = new Infinario.Infinario("13e58d18-e5e0-11e4-b263-b083fedeed2e");
			SceneController.infinario.Track("session_start", new Dictionary<string, string>() {
				{"platform", SystemInfo.operatingSystem},
				{"device", SystemInfo.deviceType.ToString()}
			});

			SceneController.infinario.Track("hi");
			Debug.Log("Infinario started");
		}
	}

	protected void OnApplicationQuit() {
		SceneController.infinario.Track("session_end", new Dictionary<string, string>() {
			{"duration", Time.realtimeSinceStartup.ToString()}
		});
		Debug.Log("Infinario closed");
	}

	protected void Start () {
		fadeTexture = Texture2D.whiteTexture;
		Application.targetFrameRate = 60;

		if (FadeInOnEnabled) {
			timeToFadeIn = TotalTimeToFadeIn;
		}

	}

	protected void Update () {
		computeFadeIn();
		computeFadeOut();
	}

	void computeFadeIn() {
		if (FadeInOnEnabled && timeToFadeIn > 0) {
			timeToFadeIn -= Time.deltaTime;

			fadeAlpha = Mathf.InverseLerp(0, TotalTimeToFadeIn, timeToFadeIn);

			if (timeToFadeIn <= 0) {
				timeToFadeIn = -1;
				fadeAlpha = 0;
			}

		}
	}

	void computeFadeOut() {
		if (timeToFadeOut != -1) {
			timeToFadeOut -= Time.deltaTime;

			fadeAlpha = Mathf.InverseLerp(TotalTimeToFadeOut, 0, timeToFadeOut);

			if (timeToFadeOut <= 0) {
				timeToFadeOut = -1;
				fadeAlpha = 1;
				ChangeScene(sceneToChangeAfterFadeOut);
			}
		}
	}

	void OnGUI() {
		Color originalColor = GUI.color;
		GUI.color= new Color(0f, 0f, 0f, fadeAlpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
		GUI.color = originalColor;
	}
}
