Shader "Custom/LeavesAlphaCutout" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_LightsColor ("Lights Color", Color) = (1,1,1,1)
		_MainTex ("Diffuse (RGB) Alpha (A)", 2D) = "white" {}
		_Cutout ("Illumination Map", 2D) = "white" {}
	}

	SubShader{
		Tags { "RenderType" = "Opaque"}
		
		Blend SrcAlpha OneMinusSrcAlpha 
		
		CGPROGRAM
			#pragma surface surf TF3 alphatest:Off
			#pragma target 3.0

			struct Input
			{
				float2 uv_MainTex;
				float3 worldNormal;
				fixed3 viewDir;
				fixed3 lightDir;
			};
			
			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Lighting2;
			};
			
			sampler2D _MainTex;
			sampler2D _Cutout;
			fixed4 _Color;
			
			inline fixed4 LightingTF3(EditorSurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
			{
				fixed4 c;
				
				c.rgb = s.Albedo.rgb;
				return c;
			}
			
			void surf (Input IN, inout EditorSurfaceOutput o)
			{
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
				float3 cutoutCol = tex2D(_Cutout, IN.uv_MainTex).rgb;
				o.Alpha = (cutoutCol.r + cutoutCol.g + cutoutCol.b) / 3;
			}
		ENDCG
	}
	Fallback "Transparent/Cutout/Bumped Specular"
}
