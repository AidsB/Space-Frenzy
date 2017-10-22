Shader "Unlit/Space"
{
	Properties
	{

	}
	SubShader
	{
		Tags { "Queue"="Background" }

		Pass
		{
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 vertex : SV_POSITION;
				float3 viewRay : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.viewRay = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
				return o;
			}

			float4 frag (v2f i) : SV_Target
			{
				float3 rd = normalize(i.viewRay);



				return float4(0,0,0, 1);
			}
			ENDCG
		}
	}
}
