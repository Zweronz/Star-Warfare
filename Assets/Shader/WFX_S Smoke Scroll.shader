‘Shader "WFX/Scroll/Smoke" {
Properties {
 _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Texture", 2D) = "white" {}
 _ScrollSpeed ("Scroll Speed", Float) = 2
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend DstColor SrcAlpha
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  highp vec2 tmpvar_2;
  tmpvar_2 = tmpvar_1;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _TintColor;
uniform sampler2D _MainTex;
uniform highp float _ScrollSpeed;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = xlv_TEXCOORD0;
  lowp vec4 tex_2;
  highp float mask_3;
  lowp float tmpvar_4;
  tmpvar_4 = (texture2D (_MainTex, xlv_TEXCOORD0).w * xlv_COLOR.w);
  mask_3 = tmpvar_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (_Time * _ScrollSpeed);
  highp vec4 tmpvar_6;
  tmpvar_6 = fract(abs(tmpvar_5));
  highp float tmpvar_7;
  if ((tmpvar_5.x >= 0.0)) {
    tmpvar_7 = tmpvar_6.x;
  } else {
    tmpvar_7 = -(tmpvar_6.x);
  };
  tmpvar_1.y = (xlv_TEXCOORD0.y - tmpvar_7);
  tex_2.xyz = (texture2D (_MainTex, tmpvar_1).xyz * (xlv_COLOR.xyz * _TintColor.xyz));
  tex_2.w = mask_3;
  highp vec4 tmpvar_8;
  tmpvar_8 = mix (vec4(0.5, 0.5, 0.5, 0.5), tex_2, vec4(mask_3));
  tex_2 = tmpvar_8;
  gl_FragData[0] = tex_2;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0.xy;
  highp vec2 tmpvar_2;
  tmpvar_2 = tmpvar_1;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_COLOR = _glesColor;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _TintColor;
uniform sampler2D _MainTex;
uniform highp float _ScrollSpeed;
in highp vec2 xlv_TEXCOORD0;
in lowp vec4 xlv_COLOR;
void main ()
{
  highp vec2 tmpvar_1;
  tmpvar_1 = xlv_TEXCOORD0;
  lowp vec4 tex_2;
  highp float mask_3;
  lowp float tmpvar_4;
  tmpvar_4 = (texture (_MainTex, xlv_TEXCOORD0).w * xlv_COLOR.w);
  mask_3 = tmpvar_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (_Time * _ScrollSpeed);
  highp vec4 tmpvar_6;
  tmpvar_6 = fract(abs(tmpvar_5));
  highp float tmpvar_7;
  if ((tmpvar_5.x >= 0.0)) {
    tmpvar_7 = tmpvar_6.x;
  } else {
    tmpvar_7 = -(tmpvar_6.x);
  };
  tmpvar_1.y = (xlv_TEXCOORD0.y - tmpvar_7);
  tex_2.xyz = (texture (_MainTex, tmpvar_1).xyz * (xlv_COLOR.xyz * _TintColor.xyz));
  tex_2.w = mask_3;
  highp vec4 tmpvar_8;
  tmpvar_8 = mix (vec4(0.5, 0.5, 0.5, 0.5), tex_2, vec4(mask_3));
  tex_2 = tmpvar_8;
  _glesFragData[0] = tex_2;
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