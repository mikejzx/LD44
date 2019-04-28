/*
	Nice little effect that will seperate some colour channels
	to make the game look a bit psychedelic :P
*/

Shader "Hidden/TripEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Width("Effect Width", Range(0.0, 1.0)) = 0.087
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform half _Width;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 a = tex2D(_MainTex, i.uv + float2(_Width * 0.01, 0.0));
				a.rgb = fixed3(a.r / 3.0, a.g, a.b);
				fixed4 b = tex2D(_MainTex, i.uv - float2(_Width * 0.01, 0.0));
				b.rgb = fixed3(b.r, 0.0, b.b / 3.0);
				return fixed4((a.rgb + b.rgb) * 0.9, 0.0);
			}
			ENDCG
		}
	}
}
