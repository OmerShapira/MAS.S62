Shader "Custom/Seleciton" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert
//		#include "UnityCG.cginc"
		
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = half4(.9,.9,.2,1);
			half a = clamp(sin(dot(IN.uv_MainTex + half2(_Time * .5), half2(20.))),0,1);
			clip (a < 0.2 ? -1 : 1);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
