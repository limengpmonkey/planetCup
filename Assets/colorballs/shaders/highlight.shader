//Shader "Unlit/highlight"
//{
//    Properties
//    {
//        _Color ("Color", Color) = (1,1,1,0.1)
//        _MainTex ("Albedo (RGB)", 2D) = "white" {}
//        _Glossiness ("Smoothness", Range(0,1)) = 0.5
//        _Metallic ("Metallic", Range(0,1)) = 0.0
//        _RimPow ("RimPow", float) = 2
//        _RimColor ("RimColor", Color) = (1,1,0,1)
//        _RimColorSpeed ("RimColorSpeed", float) = 100
//        [Toggle]_Highlighted("Highlighted", Float) = 0
//    }
//    SubShader
//    {
//        Tags
//        {
//            "Queue"="Transparent"
//            "IgnoreProjector"="True"
//            "RenderType"="Transparent"
//        }
//        LOD 200
//
//        CGPROGRAM
//        #pragma surface surf Standard fullforwardshadows
//        #pragma target 3.0
//
//        sampler2D _MainTex;
//
//        struct Input
//        {
//            float3 worldPos;
//            float3 worldNormal;
//            float2 uv_MainTex;
//        };
//
//        half _Glossiness;
//        half _Metallic;
//        fixed4 _Color;
//        float _RimPow;
//        fixed4 _RimColor;
//        float _Highlighted;
//        float _RimColorSpeed;
//
//
//        void surf(Input IN, inout SurfaceOutputStandard o)
//        {
//            float3 viewDir = normalize(UnityWorldSpaceViewDir(IN.worldPos));
//            float3 normal = normalize(IN.worldNormal);
//            float rimPower = pow(1 - saturate(dot(normal, viewDir)), _RimPow);
//            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
//            o.Albedo = c.rgb;
//            o.Metallic = _Metallic;
//            o.Smoothness = _Glossiness;
//            o.Alpha = c.a;
//            // o.Emission = _Highlighted * rimPower * _RimColor * lerp(0.5,1,(sin(_Time.x*_RimColorSpeed)+1)/2)*_CosTime.w;
//            o.Emission = _Highlighted * rimPower * _RimColor * lerp(0.5,1.5,_CosTime.w*_RimColorSpeed+1);
//        }
//        ENDCG
//    }
//    FallBack "Diffuse"
//}
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Factor("宽度", float) = 0
		_Color("边缘颜色", Color) = (0,0,0,1)
	}
	SubShader
	{
		LOD 100
		tags {"Queue" = "Transparent"}
		Pass
		{
			ZWrite On
			ColorMask 0
		}

		Pass
		{	
			Tags{  "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Back        // 剔除背面
			CGPROGRAM
			//ZWrite Off
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
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
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.vertex.z;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				//discard;
				return float4(col.xyz, 0.2);
			}
			ENDCG
		}

		Pass
		{
			Tags{ "Queue" = "Geometry" "LightMode" = "ForwardBase" }
			Cull Front   //剔除正面
			ZWrite On
							//Offset -300, -5
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 vetex : POSITION;
			};
			float _Factor;
			half4 _Color;
			v2f vert(appdata_full data)
			{
				v2f o;
				float4 view_vetex = UnityObjectToClipPos(data.vertex);
				float3 view_normal = mul(UNITY_MATRIX_IT_MV, data.normal);
				float2 offset = mul(UNITY_MATRIX_P, view_normal.xy);
				o.vetex = view_vetex;
				o.vetex.xy += offset * _Factor;
				//o.vetex = UnityObjectToClipPos(data.vertex);
				//o.vetex.z += 0.01;
				return o;
			}
			half4 frag(v2f o) :COLOR
			{
				return _Color;
			}

			ENDCG
		}
	}
}

