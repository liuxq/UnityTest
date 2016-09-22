Shader "UE/Editor/ModelMark" {
	Properties {
		_mark("mark", 2D) = "White"
		_xCount("xcount", int) = 7
		_zCount("zcount", int) = 7
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		pass{

	        lighting off
	        Cull Off
	        CGPROGRAM
	        #pragma vertex vert
	        #pragma fragment frag
	        #include "UnityCG.cginc"
	        //#include "../UECGInclude/UECGInclude.cginc"

			sampler2D _mark;
			int _xCount;
			int _zCount;

	        struct v2f {
	            fixed4 pos:SV_POSITION;
	            fixed2 uv : TEXCOORD3;
	        };

	        v2f vert (appdata_full v) {
	            v2f o;
	            o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
	            o.uv = v.texcoord;

	            return o;
	        }

			bool isMarked(float2 uv)
			{
				return tex2D(_mark, uv).r > 0.5f;
			}

	        float4 frag(v2f i):COLOR
	        {
				float x = floor(i.uv.x * _xCount)/_xCount + 0.5f/_xCount;
				float z = floor(i.uv.y * _zCount)/_zCount + 0.5f/_zCount;
				
				if(frac(i.uv.x * _xCount) < 0.03 || frac(i.uv.y * _zCount) < 0.03)
				{
					return float4(1,1,1,1);
				}
				else
				{
					if(isMarked(float2(x,z)))
						return float4(1,0,0,1);
					else
						return float4(0,1,0,1);
				}
			}
			ENDCG
		}

	}
	FallBack "Diffuse"
}
