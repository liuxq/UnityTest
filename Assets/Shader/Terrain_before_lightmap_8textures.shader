Shader "UE/Terrain/Before LightMap/8Textures" {
Properties {
	_Splat0 ("Layer 1", 2D) = "white" {}
	_Splat1 ("Layer 2", 2D) = "white" {}
	_Splat2 ("Layer 3", 2D) = "white" {}
	_Splat3 ("Layer 4", 2D) = "white" {}
	_Splat4 ("Layer 5", 2D) = "white" {}
	_Splat5 ("Layer 6", 2D) = "white" {}
	_Splat6 ("Layer 7", 2D) = "white" {}
	_Splat7 ("Layer 8", 2D) = "white" {}
	_Control ("Control (RGBA)", 2D) = "white" {}
	_Control2 ("Control (RGBA)", 2D) = "white" {}
}
                
SubShader {
	Tags {
   "SplatCount" = "4"
   "RenderType" = "Opaque"
	}

	//cull Off
CGPROGRAM
#pragma surface surf Lambert 
#pragma exclude_renderers xbox360 ps3

struct Input {
	float2 uv_Control : TEXCOORD0;
	float2 uv_Splat0 : TEXCOORD1;
	float2 uv_Splat1 : TEXCOORD2;
	float2 uv_Splat2 : TEXCOORD3;
	float2 uv_Splat3 : TEXCOORD4;
};
 
sampler2D _Control, _Control2;
sampler2D _Splat0,_Splat1,_Splat2,_Splat3,_Splat4,_Splat5,_Splat6,_Splat7;
float _globalDarkFactor;
 
void surf (Input IN, inout SurfaceOutput o) {
	fixed4 splat_control = tex2D (_Control, IN.uv_Control).rgba;
	fixed4 splat_control2 = tex2D (_Control2, IN.uv_Control).rgba;
		
	fixed3 lay1 = tex2D(_Splat0, IN.uv_Splat0);
	fixed3 lay2 = tex2D(_Splat1, IN.uv_Splat1);
	fixed3 lay3 = tex2D(_Splat2, IN.uv_Splat2);
	fixed3 lay4 = tex2D(_Splat3, IN.uv_Control);
	fixed3 lay5 = tex2D(_Splat4, IN.uv_Control);
	fixed3 lay6 = tex2D(_Splat5, IN.uv_Control);
	fixed3 lay7 = tex2D(_Splat6, IN.uv_Control);
	fixed3 lay8 = tex2D(_Splat7, IN.uv_Control);

	o.Alpha = 0.0;
	o.Albedo.rgb = (lay1 * splat_control.r + lay2 * splat_control.g + lay3 * splat_control.b + lay4 * splat_control.a + lay5 * splat_control2.r + lay6 * splat_control2.g + lay7 * splat_control2.b + lay8 * splat_control2.a);
}
ENDCG 
}
Fallback "Diffuse"
}
