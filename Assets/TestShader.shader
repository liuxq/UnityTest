Shader "Custom/TestShader" {
	Properties {
		_Color("颜色", Color) = (1,1,1,1)
		_NormalTex ("法线贴图", 2D) = "bump" {}
		_TransVal("Transparency Value",Range(0.01,1))=0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		float4 _Color;
		sampler2D _NormalTex;
		float  _TransVal;

		struct Input {
			float2 uv_NormalTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float3 normalMap = UnpackNormal(tex2D(_NormalTex,IN.uv_NormalTex));
			normalMap = float3(normalMap.x, normalMap.y, normalMap.z);
			o.Normal = normalMap.rgb;
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a * _TransVal;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
