Ç$Shader "Advanced/Lightmap" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _SpecOffset ("Specular Offset from Camera", Vector) = (1,10,2,0)
 _SpecRange ("Specular Range", Float) = 20
 _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
 _Shininess ("Shininess", Range(0.01,1)) = 0.078125
 _ScrollingSpeed ("Scrolling speed", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp vec4 unity_LightmapST;
uniform highp vec3 _SpecOffset;
uniform highp float _SpecRange;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp vec4 _ScrollingSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 tmpvar_1;
  highp mat3 tmpvar_2;
  tmpvar_2[0] = glstate_matrix_modelview0[0].xyz;
  tmpvar_2[1] = glstate_matrix_modelview0[1].xyz;
  tmpvar_2[2] = glstate_matrix_modelview0[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = ((glstate_matrix_modelview0 * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_4;
  tmpvar_4 = (((_SpecColor * 
    pow (clamp (dot ((tmpvar_2 * 
      normalize(_glesNormal)
    ), normalize(
      ((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_3))) * 0.5)
    )), 0.0, 1.0), (_Shininess * 128.0))
  ) * 2.0) * (1.0 - clamp (
    (sqrt(dot (tmpvar_3, tmpvar_3)) / _SpecRange)
  , 0.0, 1.0)));
  tmpvar_1 = tmpvar_4;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (xlv_TEXCOORD2 * tmpvar_2.w));
  c_1.xyz = (c_1.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp vec4 unity_LightmapST;
uniform highp vec3 _SpecOffset;
uniform highp float _SpecRange;
uniform highp vec3 _SpecColor;
uniform highp float _Shininess;
uniform highp vec4 _ScrollingSpeed;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 tmpvar_1;
  highp mat3 tmpvar_2;
  tmpvar_2[0] = glstate_matrix_modelview0[0].xyz;
  tmpvar_2[1] = glstate_matrix_modelview0[1].xyz;
  tmpvar_2[2] = glstate_matrix_modelview0[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = ((glstate_matrix_modelview0 * _glesVertex).xyz - (_SpecOffset * vec3(1.0, 1.0, -1.0)));
  highp vec3 tmpvar_4;
  tmpvar_4 = (((_SpecColor * 
    pow (clamp (dot ((tmpvar_2 * 
      normalize(_glesNormal)
    ), normalize(
      ((vec3(0.0, 0.0, 1.0) + normalize(-(tmpvar_3))) * 0.5)
    )), 0.0, 1.0), (_Shininess * 128.0))
  ) * 2.0) * (1.0 - clamp (
    (sqrt(dot (tmpvar_3, tmpvar_3)) / _SpecRange)
  , 0.0, 1.0)));
  tmpvar_1 = tmpvar_4;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD2 = tmpvar_1;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz + (xlv_TEXCOORD2 * tmpvar_2.w));
  c_1.xyz = (c_1.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
}