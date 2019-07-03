Shader "Custom/Screen/Monochromize" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Intensity ("Intensity", float) = 1.0
		_BrightnessFactor ("Brightness Factor", float) = 1.0
	}

	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
				
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			float _Intensity;
			float _BrightnessFactor;
			
			fixed4 frag (v2f_img i) : COLOR
			{
				fixed4 original = tex2D(_MainTex, i.uv);
				fixed lum = saturate(Luminance(original.rgb) * _BrightnessFactor);

				fixed4 output;
				output.rgb = lerp(original.rgb, fixed3(lum,lum,lum), _Intensity);
				output.a = original.a;
				return output;
			}
			ENDCG

		}
	}
	Fallback off
}