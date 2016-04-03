﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Planets/Rings"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		[HDR] _Ambient ("Ambient Color", Color) = (0,0,0,0)
		_PlanetRadius ("Planet Radius", Float) = 1
		_SunDirection("Sun Direction", Vector) = (1,0,0,0)
		_TransitionAlpha("TransitionAlpha", Float) = 1
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "../../Shaders/cginc/NearClip.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 side : TEXCOORD1;
				float  clipAmount : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _PlanetRadius;
			float4 _SunDirection;
			float3 _Ambient;
			float _TransitionAlpha;
			
			v2f vert (appdata v)
			{
				float scale = length(mul(_Object2World, float4(1, 0, 0, 0)));

				float3 alignedVertex = mul((float3x3)_Object2World, v.vertex);

				float alongLight = dot(alignedVertex, _SunDirection);
				float3 alongSides = alignedVertex - alongLight * _SunDirection;

				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.side = float2(length(alongSides), alongLight) / scale;
				float3 wPos = mul(_Object2World, v.vertex);
				o.clipAmount = CalcVertClipAmount(wPos);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				float light = i.side.y > 0 ? 1 : (i.side.x < _PlanetRadius ? 0 : 1);

				float4 col = tex2D(_MainTex, i.uv);
				
				min16float4 finalColor = min16float4(col.xyz * saturate(light.xxx + _Ambient), col.a * _TransitionAlpha);

				return ApplyVertClipAmount(finalColor, i.clipAmount);
			}
			ENDCG
		}
	}
}
