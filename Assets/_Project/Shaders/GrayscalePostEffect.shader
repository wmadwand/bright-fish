Shader "Greyscale Post Effect" {
   
Properties {
    _Opacity ( "Opacity", Range(0,1) ) = 1
}
 
SubShader {
    Tags { "Queue" = "Overlay" }
    ZWrite Off
    Pass {
        GLSLPROGRAM
        #extension GL_EXT_shader_framebuffer_fetch : require
               
        #ifdef VERTEX
        void main() {
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
        }
        #endif
       
        #ifdef FRAGMENT
        uniform lowp float _Opacity;
        void main() {
            lowp float greyscale = dot( gl_LastFragData[0].rgb, vec3(.222, .707, .071) );
            gl_FragColor.rgb = mix(gl_LastFragData[0].rgb, vec3(greyscale), _Opacity);
        }
        #endif     
        ENDGLSL
    }
}
}