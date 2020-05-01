Shader "Custom/TerrainShader" {
	Properties{
		_MainTex("Albedo", 2D) = "white" {}
		_Color1("Color 1", Color) = (1,1,1,1)
		_Color2("Color 2", Color) = (1,1,1,1)
		_Angle("Slope Angle", float) = 0
		_SlopeMinValue("Slope Min Value", float) = 0.3
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows vertex:vert
			#pragma target 3.0
			struct Input {
				float2 uv_MainTex;
				float3 normal;
			};

			struct v2f {
			  float4 pos : SV_POSITION;
			  fixed4 color : COLOR;
			};

			inline float angleBetween(fixed3 a, fixed3 b) {
				return acos(dot(a, b) / (length(a)*length(b)));
			}

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input,o);
				o.normal = v.normal;
			}

			fixed4 _Color1;
			fixed4 _Color2;
			float _Angle;
			float _SlopeMinValue;
			uniform sampler2D _MainTex;

			float invLerp(float from, float to, float value){
			  return (value - from) / (to - from);
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);

				fixed3 n = IN.normal;
				float angle = angleBetween(n, fixed3(0, 1, 0));
				float slope = clamp(angle / _Angle, 0, 1);
				if(slope<_SlopeMinValue){
					slope = 0;
				}else{
					slope = invLerp(_SlopeMinValue, 1, slope);
				}

				float4 color = tex * lerp(_Color1, _Color2, slope);
				o.Albedo = color.rgb;
				o.Alpha = 1;
			}
			ENDCG
	}
		FallBack "Diffuse"
}