using System;
using UnityEngine;

namespace Mazzaroth {
	public class PlayerMarker : BaseMonoBehaviour {

		public Color MoveColor = Color.green;
		public Color AtackColor = Color.red;

		public Vector3 Position { get{ return transform.position; } }

		public void MoveTo(Vector3 position, bool aggressive = false) {
			particleSystem.Stop();
			particleSystem.Play();
			position.y = 0;
			transform.position = position;

			setColor(aggressive ? AtackColor : MoveColor);
		}

		void Start() {
//			Color PlayerColor = player.Color;
//			Material partoclesaterial = GetComponent<ParticleSystem>().renderer.renderer.material;
//			Color color = partoclesaterial.GetColor("_TintColor");
//			PlayerColor.a = color.a;
//			partoclesaterial.SetColor("_TintColor", PlayerColor);
		}

		void setColor(Color color) {
			particleSystem.startColor = color;
		}
	}
}

