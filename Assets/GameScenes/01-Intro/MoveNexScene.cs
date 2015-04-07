using UnityEngine;
using System.Collections;

public class MoveNexScene : MonoBehaviour {
	public int TimeToNextScene = 3;
	public SceneController Scene;
	public string NextSceneName;

	void Start () {
		Invoke("changeScene", TimeToNextScene);
	}
	private void changeScene() {
		Scene.FadeOutAndChangeScene (NextSceneName);
	}
}
