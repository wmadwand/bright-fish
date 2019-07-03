Shader "Custom/Progressive Desaturation" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Saturation("Saturation (0,1)", Range(0,1)) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
        float _Saturation;
 
        struct Input {
            float2 uv_MainTex;
        };
 
        // Converts color to luminance (grayscale)
        float Luminance( float3 c )
        {
            return dot( c, float3(0.22, 0.707, 0.071) );
        }
 
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            half4 g = Luminance(c.rgb);
            o.Albedo = lerp(g.rgb, c.rgb, _Saturation);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}