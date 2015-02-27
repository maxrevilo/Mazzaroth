using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mazzaroth {
	public class GUIShipStatus : BaseMonoBehaviour {

		public ShipState ship;
		public RectTransform LifeBarContainer;
		public RectTransform LifeBar;
		public Color FUllLifeColor = Color.green;
		public Color LowLifeColor = Color.red;


		private RectTransform rectTransform;
		private Image LifeBarImage;
		private Canvas canvas;

		void Awake() {
			if (!ship) ship = GetComponentInParent<ShipState>();
			rectTransform = GetComponent<RectTransform>();
			LifeBarImage = LifeBar.GetComponent<Image>();
			canvas = GetComponent<Canvas>();
			canvas.enabled = false;
		}

		void Update() {
			if (ship) {
				Vector3 scale = LifeBar.localScale;
				scale.x = Mathf.Clamp01(ship.HealthPoints / ship.GetComponent<ShipStats>().HealthPoints);

				canvas.enabled = scale.x < 1f;// && ship.Group.Selected;

				LifeBar.localScale = scale;

				LifeBarImage.color = Color.Lerp(LowLifeColor, FUllLifeColor, scale.x);

				if (!ship.gameObject.activeSelf) {
					gameObject.DestroyAPS();
					gameObject.SetActive(false);
				}
			}


			if (ship) {
				Vector2 canvasSize = rectTransform.offsetMax - rectTransform.offsetMin;
				Vector3 viewPortPoint = Camera.main.WorldToViewportPoint(ship.transform.position);
				Vector2 screenPosition = Vector2.Scale(canvasSize, new Vector2(viewPortPoint.x, viewPortPoint.y)) - new Vector2(0, 600);
				LifeBarContainer.anchoredPosition = screenPosition;
			}

		}

		void OnGUI() {
		}
	}
}

