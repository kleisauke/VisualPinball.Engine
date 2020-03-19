﻿Shader "Visual Pinball/DMD Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DmdBlockCount ("Block Count", Float) = 128
		_DmdAspectRatio ("Aspect Ratio", Float) = 4
		_DmdMax ("Max", Float) = 1.25
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _DmdBlockCount;
			float _DmdMax;
			float _DmdAspectRatio;
			float4 FilterColor;
			float _IsMonochrome;

			// Static computed vars for optimization
			static float2 BlockCount2 = float2(_DmdBlockCount, _DmdBlockCount / _DmdAspectRatio);
			static float2 BlockSize2 = 1.0f / BlockCount2;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float4 setMonochrome(float4 color) : COLOR
			{
				float4 monochrome = color;
				if (((int)_IsMonochrome) == 1)
				{
					float3 rgb = color.rgb;
					float3 luminance = dot(rgb, float3(0.30, 0.59, 0.11));
					monochrome = float4(luminance * FilterColor.rgb, color.a);
				}
				return monochrome;
			}

			float4 setDmd (float2 uv, sampler2D samp) : COLOR
			{
				// Calculate block center
				float2 blockPos = floor(uv * BlockCount2);
				float2 blockCenter = blockPos * BlockSize2 + BlockSize2 * 0.5;

				// Scale coordinates back to original ratio for rounding
				float2 uvScaled = float2(uv.x * _DmdAspectRatio, uv.y);
				float2 blockCenterScaled = float2(blockCenter.x * _DmdAspectRatio, blockCenter.y);

				// Round the block by testing the distance of the pixel coordinate to the center
				float dist = length(uvScaled - blockCenterScaled) * BlockCount2;

				float4 insideColor = tex2D(samp, blockCenter);

				float4 outsideColor = insideColor;
				outsideColor.r = 0;
				outsideColor.g = 0;
				outsideColor.b = 0;
				outsideColor.a = 1;

				float distFromEdge = _DmdMax - dist;  // positive when inside the circle
				float thresholdWidth = .22;  // a constant you'd tune to get the right level of softness
				float antialiasedCircle = saturate((distFromEdge / thresholdWidth) + 0.5);

				return lerp(outsideColor, insideColor, antialiasedCircle);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float4 DMD = setDmd(i.uv, _MainTex);
				DMD = setMonochrome(DMD);
				return DMD;
			}
			ENDCG
		}
	}
}
