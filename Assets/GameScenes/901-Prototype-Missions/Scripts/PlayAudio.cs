using UnityEngine;
using System.Collections;
using Mazzaroth.Ships;

namespace Mazzaroth {
	public class PlayAudio : BaseMonoBehaviour {
		public AudioSource AudioToPlay;
		void Start () {
			AudioToPlay.Play();
		}
	}
}
