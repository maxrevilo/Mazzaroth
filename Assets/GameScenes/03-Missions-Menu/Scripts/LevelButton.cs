using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Mazzaroth {
	public class LevelButton : BaseMonoBehaviour {

		public Text percentageField;
		public string LevelName = "";

		void Start() {

		}

		void Update() {
			Debug.Log(Application.GetStreamProgressForLevel(LevelName));
			if (Application.GetStreamProgressForLevel(LevelName) == 1)
				percentageField.enabled = false;
			else {
				percentageField.enabled = true;
				percentageField.text = Application.GetStreamProgressForLevel(1) * 100 + "%";
			}
		}
	}
}

