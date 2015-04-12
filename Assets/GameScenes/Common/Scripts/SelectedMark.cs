using System;
using UnityEngine;
using Mazzaroth.Ships;

namespace Mazzaroth {
    public class SelectedMark : BaseMonoBehaviour {
        public float Radius = 1f;
        public Color Color = Color.white;
        public float Width = 1f;

        Ship ship;
        LineRenderer lineRenderer;

        void Start() {
            ship = GetComponent<Ship>();
            if (ship) {
				/*Color TeamColor = ship.Color;
                TeamColor.a = Color.a;
                Color = TeamColor;*/
            }

            float theta_scale = 0.1f;             //Set lower to add more points
            int size = (int) ((2.0f * Mathf.PI) / theta_scale); //Total number of points in circle.

            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Mobile/Particles/Additive"));
            lineRenderer.SetColors(Color, Color);
            lineRenderer.SetWidth(Width, Width);
            lineRenderer.SetVertexCount(size + 2);

            lineRenderer.useWorldSpace = false;

            float WorldRaius = Radius / transform.localScale.z;

            int i = 0;
            for(float theta = 0; theta <= (2f * Mathf.PI + theta_scale); theta += theta_scale) {
                float x = WorldRaius * Mathf.Cos(theta);
                float y = WorldRaius * Mathf.Sin(theta);

                Vector3 pos = new Vector3(x, 0, y);
                lineRenderer.SetPosition(i, pos);
                i += 1;
            }
        }

        void Update() {
            if (ship) {
                lineRenderer.enabled = ship.Group.Selected;
            }
        }
    }
}

