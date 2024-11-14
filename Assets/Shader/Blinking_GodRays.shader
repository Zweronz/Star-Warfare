©?Shader "Advanced/Blinking_Atmosphere" {
Properties {
 _MainTex ("Base texture", 2D) = "white" {}
 _FadeOutDistNear ("Near fadeout dist", Float) = 10
 _FadeOutDistFar ("Far fadeout dist", Float) = 10000
 _Multiplier ("Color multiplier", Float) = 1
 _Bias ("Bias", Float) = 0
 _TimeOnDuration ("ON duration", Float) = 0.5
 _TimeOffDuration ("OFF duration", Float) = 0.5
 _BlinkingTimeOffsScale ("Blinking time offset scale (seconds)", Float) = 5
 _SizeGrowStartDist ("Size grow start dist", Float) = 5
 _SizeGrowEndDist ("Size grow end dist", Float) = 50
 _MaxGrowSize ("Max grow size", Float) = 2.5
 _NoiseAmount ("Noise amount (when zero, pulse wave is used)", Range(0,0.5)) = 0
 _Color ("Color", Color) = (1,1,1,1)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp float _FadeOutDistNear;
uniform highp float _FadeOutDistFar;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _SizeGrowStartDist;
uniform highp float _SizeGrowEndDist;
uniform highp float _MaxGrowSize;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0;
  highp vec4 mdlPos_4;
  lowp vec4 tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_7;
  tmpvar_7 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp float tmpvar_8;
  tmpvar_8 = sqrt(dot (tmpvar_7, tmpvar_7));
  highp float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_8 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp ((
    max ((tmpvar_8 - _FadeOutDistFar), 0.0)
   * 0.2), 0.0, 1.0));
  highp float y_11;
  y_11 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_12;
  tmpvar_12 = (tmpvar_6 / y_11);
  highp float tmpvar_13;
  tmpvar_13 = (fract(abs(tmpvar_12)) * y_11);
  highp float tmpvar_14;
  if ((tmpvar_12 >= 0.0)) {
    tmpvar_14 = tmpvar_13;
  } else {
    tmpvar_14 = -(tmpvar_13);
  };
  highp float tmpvar_15;
  tmpvar_15 = clamp ((tmpvar_14 / (_TimeOnDuration * 0.25)), 0.0, 1.0);
  highp float edge0_16;
  edge0_16 = (_TimeOnDuration * 0.75);
  highp float tmpvar_17;
  tmpvar_17 = clamp (((tmpvar_14 - edge0_16) / (_TimeOnDuration - edge0_16)), 0.0, 1.0);
  highp float tmpvar_18;
  tmpvar_18 = ((tmpvar_15 * (tmpvar_15 * 
    (3.0 - (2.0 * tmpvar_15))
  )) * (1.0 - (tmpvar_17 * 
    (tmpvar_17 * (3.0 - (2.0 * tmpvar_17)))
  )));
  highp float tmpvar_19;
  tmpvar_19 = (tmpvar_6 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_20;
  tmpvar_20 = ((_NoiseAmount * (
    sin(tmpvar_19)
   * 
    ((0.5 * cos((
      (tmpvar_19 * 0.6366)
     + 56.7272))) + 0.5)
  )) + (1.0 - _NoiseAmount));
  highp float tmpvar_21;
  tmpvar_21 = min ((max (
    (tmpvar_8 - _SizeGrowStartDist)
  , 0.0) / _SizeGrowEndDist), 1.0);
  highp float tmpvar_22;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_22 = tmpvar_18;
  } else {
    tmpvar_22 = tmpvar_20;
  };
  highp float tmpvar_23;
  tmpvar_23 = (tmpvar_9 * tmpvar_9);
  mdlPos_4.w = tmpvar_1.w;
  mdlPos_4.xyz = (_glesVertex.xyz + ((
    ((tmpvar_21 * tmpvar_21) * _MaxGrowSize)
   * _glesColor.w) * tmpvar_2));
  highp vec4 tmpvar_24;
  tmpvar_24 = (((
    ((tmpvar_23 * tmpvar_23) * (tmpvar_10 * tmpvar_10))
   * _Color) * _Multiplier) * (tmpvar_22 + _Bias));
  tmpvar_5 = tmpvar_24;
  gl_Position = (glstate_matrix_mvp * mdlPos_4);
  xlv_TEXCOORD0 = tmpvar_3.xy;
  xlv_TEXCOORD1 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp float _FadeOutDistNear;
uniform highp float _FadeOutDistFar;
uniform highp float _Multiplier;
uniform highp float _Bias;
uniform highp float _TimeOnDuration;
uniform highp float _TimeOffDuration;
uniform highp float _BlinkingTimeOffsScale;
uniform highp float _SizeGrowStartDist;
uniform highp float _SizeGrowEndDist;
uniform highp float _MaxGrowSize;
uniform highp float _NoiseAmount;
uniform highp vec4 _Color;
out highp vec2 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesVertex;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec4 tmpvar_3;
  tmpvar_3 = _glesMultiTexCoord0;
  highp vec4 mdlPos_4;
  lowp vec4 tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = (_Time.y + (_BlinkingTimeOffsScale * _glesColor.z));
  highp vec3 tmpvar_7;
  tmpvar_7 = (glstate_matrix_modelview0 * _glesVertex).xyz;
  highp float tmpvar_8;
  tmpvar_8 = sqrt(dot (tmpvar_7, tmpvar_7));
  highp float tmpvar_9;
  tmpvar_9 = clamp ((tmpvar_8 / _FadeOutDistNear), 0.0, 1.0);
  highp float tmpvar_10;
  tmpvar_10 = (1.0 - clamp ((
    max ((tmpvar_8 - _FadeOutDistFar), 0.0)
   * 0.2), 0.0, 1.0));
  highp float y_11;
  y_11 = (_TimeOnDuration + _TimeOffDuration);
  highp float tmpvar_12;
  tmpvar_12 = (tmpvar_6 / y_11);
  highp float tmpvar_13;
  tmpvar_13 = (fract(abs(tmpvar_12)) * y_11);
  highp float tmpvar_14;
  if ((tmpvar_12 >= 0.0)) {
    tmpvar_14 = tmpvar_13;
  } else {
    tmpvar_14 = -(tmpvar_13);
  };
  highp float tmpvar_15;
  tmpvar_15 = clamp ((tmpvar_14 / (_TimeOnDuration * 0.25)), 0.0, 1.0);
  highp float edge0_16;
  edge0_16 = (_TimeOnDuration * 0.75);
  highp float tmpvar_17;
  tmpvar_17 = clamp (((tmpvar_14 - edge0_16) / (_TimeOnDuration - edge0_16)), 0.0, 1.0);
  highp float tmpvar_18;
  tmpvar_18 = ((tmpvar_15 * (tmpvar_15 * 
    (3.0 - (2.0 * tmpvar_15))
  )) * (1.0 - (tmpvar_17 * 
    (tmpvar_17 * (3.0 - (2.0 * tmpvar_17)))
  )));
  highp float tmpvar_19;
  tmpvar_19 = (tmpvar_6 * (6.28319 / _TimeOnDuration));
  highp float tmpvar_20;
  tmpvar_20 = ((_NoiseAmount * (
    sin(tmpvar_19)
   * 
    ((0.5 * cos((
      (tmpvar_19 * 0.6366)
     + 56.7272))) + 0.5)
  )) + (1.0 - _NoiseAmount));
  highp float tmpvar_21;
  tmpvar_21 = min ((max (
    (tmpvar_8 - _SizeGrowStartDist)
  , 0.0) / _SizeGrowEndDist), 1.0);
  highp float tmpvar_22;
  if ((_NoiseAmount < 0.01)) {
    tmpvar_22 = tmpvar_18;
  } else {
    tmpvar_22 = tmpvar_20;
  };
  highp float tmpvar_23;
  tmpvar_23 = (tmpvar_9 * tmpvar_9);
  mdlPos_4.w = tmpvar_1.w;
  mdlPos_4.xyz = (_glesVertex.xyz + ((
    ((tmpvar_21 * tmpvar_21) * _MaxGrowSize)
   * _glesColor.w) * tmpvar_2));
  highp vec4 tmpvar_24;
  tmpvar_24 = (((
    ((tmpvar_23 * tmpvar_23) * (tmpvar_10 * tmpvar_10))
   * _Color) * _Multiplier) * (tmpvar_22 + _Bias));
  tmpvar_5 = tmpvar_24;
  gl_Position = (glstate_matrix_mvp * mdlPos_4);
  xlv_TEXCOORD0 = tmpvar_3.xy;
  xlv_TEXCOORD1 = tmpvar_5;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
in highp vec2 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture (_MainTex, xlv_TEXCOORD0) * xlv_TEXCOORD1);
  _glesFragData[0] = tmpvar_1;
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