ÑKShader "Advanced/Scroll_2Layers" {
Properties {
 _MainTex ("Base layer (RGB)", 2D) = "white" {}
 _DetailTex ("2nd layer (RGB)", 2D) = "white" {}
 _ScrollX ("Base layer Scroll speed X", Float) = 1
 _ScrollY ("Base layer Scroll speed Y", Float) = 0
 _Scroll2X ("2nd layer Scroll speed X", Float) = 1
 _Scroll2Y ("2nd layer Scroll speed Y", Float) = 0
 _SineAmplX ("Base layer sine amplitude X", Float) = 0.5
 _SineAmplY ("Base layer sine amplitude Y", Float) = 0.5
 _SineFreqX ("Base layer sine freq X", Float) = 10
 _SineFreqY ("Base layer sine freq Y", Float) = 10
 _SineAmplX2 ("2nd layer sine amplitude X", Float) = 0.5
 _SineAmplY2 ("2nd layer sine amplitude Y", Float) = 0.5
 _SineFreqX2 ("2nd layer sine freq X", Float) = 10
 _SineFreqY2 ("2nd layer sine freq Y", Float) = 10
 _MMultiplier ("Layer Multiplier", Float) = 2
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
Keywords { "LIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _DetailTex_ST;
uniform highp float _ScrollX;
uniform highp float _ScrollY;
uniform highp float _Scroll2X;
uniform highp float _Scroll2Y;
uniform highp float _MMultiplier;
uniform highp float _SineAmplX;
uniform highp float _SineAmplY;
uniform highp float _SineFreqX;
uniform highp float _SineFreqY;
uniform highp float _SineAmplX2;
uniform highp float _SineAmplY2;
uniform highp float _SineFreqX2;
uniform highp float _SineFreqY2;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3.x = _ScrollX;
  tmpvar_3.y = _ScrollY;
  tmpvar_1.xy = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((tmpvar_3 * _Time.xy)));
  highp vec2 tmpvar_4;
  tmpvar_4.x = _Scroll2X;
  tmpvar_4.y = _Scroll2Y;
  tmpvar_1.zw = (((_glesMultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + fract((tmpvar_4 * _Time.xy)));
  tmpvar_1.x = (tmpvar_1.x + (sin(
    (_Time * _SineFreqX)
  ) * _SineAmplX).x);
  tmpvar_1.y = (tmpvar_1.y + (sin(
    (_Time * _SineFreqY)
  ) * _SineAmplY).x);
  tmpvar_1.z = (tmpvar_1.z + (sin(
    (_Time * _SineFreqX2)
  ) * _SineAmplX2).x);
  tmpvar_1.w = (tmpvar_1.w + (sin(
    (_Time * _SineFreqY2)
  ) * _SineAmplY2).x);
  highp vec4 tmpvar_5;
  tmpvar_5 = vec4(_MMultiplier);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _DetailTex;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0.xy) * texture2D (_DetailTex, xlv_TEXCOORD0.zw)) * xlv_TEXCOORD1);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _DetailTex_ST;
uniform highp float _ScrollX;
uniform highp float _ScrollY;
uniform highp float _Scroll2X;
uniform highp float _Scroll2Y;
uniform highp float _MMultiplier;
uniform highp float _SineAmplX;
uniform highp float _SineAmplY;
uniform highp float _SineFreqX;
uniform highp float _SineFreqY;
uniform highp float _SineAmplX2;
uniform highp float _SineAmplY2;
uniform highp float _SineFreqX2;
uniform highp float _SineFreqY2;
out highp vec4 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3.x = _ScrollX;
  tmpvar_3.y = _ScrollY;
  tmpvar_1.xy = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((tmpvar_3 * _Time.xy)));
  highp vec2 tmpvar_4;
  tmpvar_4.x = _Scroll2X;
  tmpvar_4.y = _Scroll2Y;
  tmpvar_1.zw = (((_glesMultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + fract((tmpvar_4 * _Time.xy)));
  tmpvar_1.x = (tmpvar_1.x + (sin(
    (_Time * _SineFreqX)
  ) * _SineAmplX).x);
  tmpvar_1.y = (tmpvar_1.y + (sin(
    (_Time * _SineFreqY)
  ) * _SineAmplY).x);
  tmpvar_1.z = (tmpvar_1.z + (sin(
    (_Time * _SineFreqX2)
  ) * _SineAmplX2).x);
  tmpvar_1.w = (tmpvar_1.w + (sin(
    (_Time * _SineFreqY2)
  ) * _SineAmplY2).x);
  highp vec4 tmpvar_5;
  tmpvar_5 = vec4(_MMultiplier);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _DetailTex;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture (_MainTex, xlv_TEXCOORD0.xy) * texture (_DetailTex, xlv_TEXCOORD0.zw)) * xlv_TEXCOORD1);
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _DetailTex_ST;
uniform highp float _ScrollX;
uniform highp float _ScrollY;
uniform highp float _Scroll2X;
uniform highp float _Scroll2Y;
uniform highp float _MMultiplier;
uniform highp float _SineAmplX;
uniform highp float _SineAmplY;
uniform highp float _SineFreqX;
uniform highp float _SineFreqY;
uniform highp float _SineAmplX2;
uniform highp float _SineAmplY2;
uniform highp float _SineFreqX2;
uniform highp float _SineFreqY2;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3.x = _ScrollX;
  tmpvar_3.y = _ScrollY;
  tmpvar_1.xy = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((tmpvar_3 * _Time.xy)));
  highp vec2 tmpvar_4;
  tmpvar_4.x = _Scroll2X;
  tmpvar_4.y = _Scroll2Y;
  tmpvar_1.zw = (((_glesMultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + fract((tmpvar_4 * _Time.xy)));
  tmpvar_1.x = (tmpvar_1.x + (sin(
    (_Time * _SineFreqX)
  ) * _SineAmplX).x);
  tmpvar_1.y = (tmpvar_1.y + (sin(
    (_Time * _SineFreqY)
  ) * _SineAmplY).x);
  tmpvar_1.z = (tmpvar_1.z + (sin(
    (_Time * _SineFreqX2)
  ) * _SineAmplX2).x);
  tmpvar_1.w = (tmpvar_1.w + (sin(
    (_Time * _SineFreqY2)
  ) * _SineAmplY2).x);
  highp vec4 tmpvar_5;
  tmpvar_5 = vec4(_MMultiplier);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _DetailTex;
varying highp vec4 xlv_TEXCOORD0;
varying lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0.xy) * texture2D (_DetailTex, xlv_TEXCOORD0.zw)) * xlv_TEXCOORD1);
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _DetailTex_ST;
uniform highp float _ScrollX;
uniform highp float _ScrollY;
uniform highp float _Scroll2X;
uniform highp float _Scroll2Y;
uniform highp float _MMultiplier;
uniform highp float _SineAmplX;
uniform highp float _SineAmplY;
uniform highp float _SineFreqX;
uniform highp float _SineFreqY;
uniform highp float _SineAmplX2;
uniform highp float _SineAmplY2;
uniform highp float _SineFreqX2;
uniform highp float _SineFreqY2;
out highp vec4 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3.x = _ScrollX;
  tmpvar_3.y = _ScrollY;
  tmpvar_1.xy = (((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw) + fract((tmpvar_3 * _Time.xy)));
  highp vec2 tmpvar_4;
  tmpvar_4.x = _Scroll2X;
  tmpvar_4.y = _Scroll2Y;
  tmpvar_1.zw = (((_glesMultiTexCoord0.xy * _DetailTex_ST.xy) + _DetailTex_ST.zw) + fract((tmpvar_4 * _Time.xy)));
  tmpvar_1.x = (tmpvar_1.x + (sin(
    (_Time * _SineFreqX)
  ) * _SineAmplX).x);
  tmpvar_1.y = (tmpvar_1.y + (sin(
    (_Time * _SineFreqY)
  ) * _SineAmplY).x);
  tmpvar_1.z = (tmpvar_1.z + (sin(
    (_Time * _SineFreqX2)
  ) * _SineAmplX2).x);
  tmpvar_1.w = (tmpvar_1.w + (sin(
    (_Time * _SineFreqY2)
  ) * _SineAmplY2).x);
  highp vec4 tmpvar_5;
  tmpvar_5 = vec4(_MMultiplier);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _DetailTex;
in highp vec4 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture (_MainTex, xlv_TEXCOORD0.xy) * texture (_DetailTex, xlv_TEXCOORD0.zw)) * xlv_TEXCOORD1);
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" }
"!!GLES3"
}
}
 }
}
}