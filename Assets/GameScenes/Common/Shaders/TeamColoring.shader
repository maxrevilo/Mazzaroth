// By Oliver Pérez
// Based on the MatCap Shader by Jean Moreno

Shader "Mazzaroth/Vertex/TeamColoring"
{
	Properties
	{
		_Color ("Color", Color) = (0.5,0.5,0.5,1)
		_TeamColor ("TeamColor", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MatCap ("MatCap (RGB)", 2D) = "white" {}
		_TeamMask ("TeamMask (RGB)", 2D) = "black" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf Lambert vertex:vert
		
		sampler2D _MainTex;
		sampler2D _MatCap;
		sampler2D _TeamMask;
		float4 _Color;
		float4 _TeamColor;
		
		struct Input
		{
			half2 uv_MainTex : TEXCOORD0;
			float2 matcapUV;
		};
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
			UNITY_INITIALIZE_OUTPUT(Input,o);
			#endif
			
			o.matcapUV = float2(dot(UNITY_MATRIX_IT_MV[0].xyz, v.normal), dot(UNITY_MATRIX_IT_MV[1].xyz, v.normal)) * 0.5 + 0.5;
		}
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			half4 mc = tex2D(_MatCap, IN.matcapUV);
			half4 tm = tex2D(_TeamMask, IN.uv_MainTex);
			o.Albedo = c.rgb * mc.rgb * _Color.rgb * 2.0;
			o.Emission = tm.rgb * tm.a * _TeamColor;
		}
		ENDCG
	}
	
	Fallback "VertexLit"
}
