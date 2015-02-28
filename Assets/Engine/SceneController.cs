using UnityEngine;
using System.Collections;

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

	public void FadeOutAndChangeScene(string sceneName) {
		timeToFadeOut = TotalTimeToFadeOut;
		sceneToChangeAfterFadeOut = sceneName;
	}

	public void ChangeScene(string sceneName) {
		Application.LoadLevel(sceneName);
	}

	void Start () {
		fadeTexture = Texture2D.whiteTexture;


		if (FadeInOnEnabled) {
			timeToFadeIn = TotalTimeToFadeIn;
		}

	}

	void Update () {
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
