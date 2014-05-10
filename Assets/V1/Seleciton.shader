Shader "Custom/Seleciton" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert addshadow
		#define THRESHOLD 0.2
//		#include "UnityCG.cginc"
		
//		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = half4(.9,.9,.2,1);
			half a = saturate(sin(dot(IN.uv_MainTex + half2(_Time * .5), half2(15.7))));
			c.a = smoothstep(THRESHOLD,0.8, a);
			clip (a < THRESHOLD ? -1 : 1);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
