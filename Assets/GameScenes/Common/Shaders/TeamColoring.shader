// By Oliver Pérez
// Based on the MatCap Shader by Jean Moreno

Shader "Mazzaroth/Mobile/TeamColoring"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MatCap ("MatCap (RGB)", 2D) = "grey" {}
		_TeamColor ("TeamColor", Color) = (1,1,1,1)
		_TeamMask ("TeamMask (RGB)", 2D) = "black" {}
	}
	
	Subshader
	{
		Tags { "RenderType"="Opaque" }
		
		Pass
		{
			Tags { "LightMode" = "Always" }
			
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				uniform float4 _MainTex_ST;

				uniform sampler2D _MatCap;

				fixed4 _TeamColor;
				sampler2D _TeamMask;

				struct v2f
				{
					float4 pos	: SV_POSITION;
					float2 uv 	: TEXCOORD0;
					float2 cap	: TEXCOORD1;
				};

				
				v2f vert (appdata_base v)
				{
					v2f o;
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					
					half2 capCoord;
					capCoord.x = dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal);
					capCoord.y = dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal);
					o.cap = capCoord * 0.5 + 0.5;
					
					return o;
				}

				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 tex = tex2D(_MainTex, i.uv);
					fixed4 tm = tex2D(_TeamMask, i.uv);
					fixed4 mc = tex2D(_MatCap, i.cap);
					
					return tex * mc * 2.0 + tm * _TeamColor;
				}
			ENDCG
		}
	}
	
	Fallback "VertexLit"
}
