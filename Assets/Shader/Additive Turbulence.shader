¼Ì
Shader "FX PACK 1/Particles/Additive Turbulence" {
Properties {
 _MainTex ("Main_Texture", 2D) = "white" {}
 _Color01 ("Color", Color) = (1,1,1,1)
 _Blend_Texture ("Blend_Texture_01", 2D) = "white" {}
 _Color02 ("Color", Color) = (1,1,1,1)
 _Blend_Texture01 ("Blend_Texture_02", 2D) = "black" {}
 _Color03 ("Color", Color) = (1,1,1,1)
 _Speed01 ("Blend_Texture_01_Speed", Float) = 1
 _Speed02 ("Blend_Texture_02_Speed", Float) = 1
 _LightenMain ("Brightness_Main", Float) = 1
 _Lighten ("Brightness_Blend", Float) = 1
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="False" "RenderType"="Transparent" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "QUEUE"="Transparent" "IGNOREPROJECTOR"="False" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = tmpvar_1.xyz;
  tmpvar_9 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_2.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_2.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_2.z;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  shlight_3 = tmpvar_14;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_10 * ((
    (_World2Object * tmpvar_12)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = tmpvar_1.xyz;
  tmpvar_9 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_2.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_2.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_2.z;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  shlight_3 = tmpvar_14;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_10 * ((
    (_World2Object * tmpvar_12)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  c_1.w = tmpvar_5;
  c_1.xyz = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  c_1.w = tmpvar_5;
  c_1.xyz = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = tmpvar_1.xyz;
  tmpvar_9 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_2.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_2.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_2.z;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  shlight_3 = tmpvar_14;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_10 * ((
    (_World2Object * tmpvar_12)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  c_1.w = tmpvar_5;
  c_1.xyz = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_8;
  mediump vec3 tmpvar_15;
  mediump vec4 normal_16;
  normal_16 = tmpvar_14;
  highp float vC_17;
  mediump vec3 x3_18;
  mediump vec3 x2_19;
  mediump vec3 x1_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAr, normal_16);
  x1_20.x = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAg, normal_16);
  x1_20.y = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAb, normal_16);
  x1_20.z = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24 = (normal_16.xyzz * normal_16.yzzx);
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBr, tmpvar_24);
  x2_19.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBg, tmpvar_24);
  x2_19.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBb, tmpvar_24);
  x2_19.z = tmpvar_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((normal_16.x * normal_16.x) - (normal_16.y * normal_16.y));
  vC_17 = tmpvar_28;
  highp vec3 tmpvar_29;
  tmpvar_29 = (unity_SHC.xyz * vC_17);
  x3_18 = tmpvar_29;
  tmpvar_15 = ((x1_20 + x2_19) + x3_18);
  shlight_3 = tmpvar_15;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_30;
  tmpvar_30 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosX0 - tmpvar_30.x);
  highp vec4 tmpvar_32;
  tmpvar_32 = (unity_4LightPosY0 - tmpvar_30.y);
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosZ0 - tmpvar_30.z);
  highp vec4 tmpvar_34;
  tmpvar_34 = (((tmpvar_31 * tmpvar_31) + (tmpvar_32 * tmpvar_32)) + (tmpvar_33 * tmpvar_33));
  highp vec4 tmpvar_35;
  tmpvar_35 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_31 * tmpvar_8.x) + (tmpvar_32 * tmpvar_8.y)) + (tmpvar_33 * tmpvar_8.z))
   * 
    inversesqrt(tmpvar_34)
  )) * (1.0/((1.0 + 
    (tmpvar_34 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_36;
  tmpvar_36 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_35.x) + (unity_LightColor[1].xyz * tmpvar_35.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_35.z)
  ) + (unity_LightColor[3].xyz * tmpvar_35.w)));
  tmpvar_6 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_11 * ((
    (_World2Object * tmpvar_13)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_8;
  mediump vec3 tmpvar_15;
  mediump vec4 normal_16;
  normal_16 = tmpvar_14;
  highp float vC_17;
  mediump vec3 x3_18;
  mediump vec3 x2_19;
  mediump vec3 x1_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAr, normal_16);
  x1_20.x = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAg, normal_16);
  x1_20.y = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAb, normal_16);
  x1_20.z = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24 = (normal_16.xyzz * normal_16.yzzx);
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBr, tmpvar_24);
  x2_19.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBg, tmpvar_24);
  x2_19.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBb, tmpvar_24);
  x2_19.z = tmpvar_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((normal_16.x * normal_16.x) - (normal_16.y * normal_16.y));
  vC_17 = tmpvar_28;
  highp vec3 tmpvar_29;
  tmpvar_29 = (unity_SHC.xyz * vC_17);
  x3_18 = tmpvar_29;
  tmpvar_15 = ((x1_20 + x2_19) + x3_18);
  shlight_3 = tmpvar_15;
  tmpvar_6 = shlight_3;
  highp vec3 tmpvar_30;
  tmpvar_30 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosX0 - tmpvar_30.x);
  highp vec4 tmpvar_32;
  tmpvar_32 = (unity_4LightPosY0 - tmpvar_30.y);
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosZ0 - tmpvar_30.z);
  highp vec4 tmpvar_34;
  tmpvar_34 = (((tmpvar_31 * tmpvar_31) + (tmpvar_32 * tmpvar_32)) + (tmpvar_33 * tmpvar_33));
  highp vec4 tmpvar_35;
  tmpvar_35 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_31 * tmpvar_8.x) + (tmpvar_32 * tmpvar_8.y)) + (tmpvar_33 * tmpvar_8.z))
   * 
    inversesqrt(tmpvar_34)
  )) * (1.0/((1.0 + 
    (tmpvar_34 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_36;
  tmpvar_36 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_35.x) + (unity_LightColor[1].xyz * tmpvar_35.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_35.z)
  ) + (unity_LightColor[3].xyz * tmpvar_35.w)));
  tmpvar_6 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_11 * ((
    (_World2Object * tmpvar_13)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_8;
  mediump vec3 tmpvar_15;
  mediump vec4 normal_16;
  normal_16 = tmpvar_14;
  highp float vC_17;
  mediump vec3 x3_18;
  mediump vec3 x2_19;
  mediump vec3 x1_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAr, normal_16);
  x1_20.x = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAg, normal_16);
  x1_20.y = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAb, normal_16);
  x1_20.z = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24 = (normal_16.xyzz * normal_16.yzzx);
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBr, tmpvar_24);
  x2_19.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBg, tmpvar_24);
  x2_19.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBb, tmpvar_24);
  x2_19.z = tmpvar_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((normal_16.x * normal_16.x) - (normal_16.y * normal_16.y));
  vC_17 = tmpvar_28;
  highp vec3 tmpvar_29;
  tmpvar_29 = (unity_SHC.xyz * vC_17);
  x3_18 = tmpvar_29;
  tmpvar_15 = ((x1_20 + x2_19) + x3_18);
  shlight_3 = tmpvar_15;
  tmpvar_6 = shlight_3;
  highp vec4 cse_30;
  cse_30 = (_Object2World * _glesVertex);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosX0 - cse_30.x);
  highp vec4 tmpvar_32;
  tmpvar_32 = (unity_4LightPosY0 - cse_30.y);
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosZ0 - cse_30.z);
  highp vec4 tmpvar_34;
  tmpvar_34 = (((tmpvar_31 * tmpvar_31) + (tmpvar_32 * tmpvar_32)) + (tmpvar_33 * tmpvar_33));
  highp vec4 tmpvar_35;
  tmpvar_35 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_31 * tmpvar_8.x) + (tmpvar_32 * tmpvar_8.y)) + (tmpvar_33 * tmpvar_8.z))
   * 
    inversesqrt(tmpvar_34)
  )) * (1.0/((1.0 + 
    (tmpvar_34 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_36;
  tmpvar_36 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_35.x) + (unity_LightColor[1].xyz * tmpvar_35.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_35.z)
  ) + (unity_LightColor[3].xyz * tmpvar_35.w)));
  tmpvar_6 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_11 * ((
    (_World2Object * tmpvar_13)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * cse_30);
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = tmpvar_1.xyz;
  tmpvar_9 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_2.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_2.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_2.z;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  shlight_3 = tmpvar_14;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_10 * ((
    (_World2Object * tmpvar_12)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_8 = tmpvar_1.xyz;
  tmpvar_9 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_10;
  tmpvar_10[0].x = tmpvar_8.x;
  tmpvar_10[0].y = tmpvar_9.x;
  tmpvar_10[0].z = tmpvar_2.x;
  tmpvar_10[1].x = tmpvar_8.y;
  tmpvar_10[1].y = tmpvar_9.y;
  tmpvar_10[1].z = tmpvar_2.y;
  tmpvar_10[2].x = tmpvar_8.z;
  tmpvar_10[2].y = tmpvar_9.z;
  tmpvar_10[2].z = tmpvar_2.z;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  shlight_3 = tmpvar_14;
  tmpvar_6 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_10 * ((
    (_World2Object * tmpvar_12)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  c_1.w = tmpvar_5;
  c_1.xyz = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  c_1.w = tmpvar_5;
  c_1.xyz = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_8;
  mediump vec3 tmpvar_15;
  mediump vec4 normal_16;
  normal_16 = tmpvar_14;
  highp float vC_17;
  mediump vec3 x3_18;
  mediump vec3 x2_19;
  mediump vec3 x1_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAr, normal_16);
  x1_20.x = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAg, normal_16);
  x1_20.y = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAb, normal_16);
  x1_20.z = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24 = (normal_16.xyzz * normal_16.yzzx);
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBr, tmpvar_24);
  x2_19.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBg, tmpvar_24);
  x2_19.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBb, tmpvar_24);
  x2_19.z = tmpvar_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((normal_16.x * normal_16.x) - (normal_16.y * normal_16.y));
  vC_17 = tmpvar_28;
  highp vec3 tmpvar_29;
  tmpvar_29 = (unity_SHC.xyz * vC_17);
  x3_18 = tmpvar_29;
  tmpvar_15 = ((x1_20 + x2_19) + x3_18);
  shlight_3 = tmpvar_15;
  tmpvar_6 = shlight_3;
  highp vec4 cse_30;
  cse_30 = (_Object2World * _glesVertex);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosX0 - cse_30.x);
  highp vec4 tmpvar_32;
  tmpvar_32 = (unity_4LightPosY0 - cse_30.y);
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosZ0 - cse_30.z);
  highp vec4 tmpvar_34;
  tmpvar_34 = (((tmpvar_31 * tmpvar_31) + (tmpvar_32 * tmpvar_32)) + (tmpvar_33 * tmpvar_33));
  highp vec4 tmpvar_35;
  tmpvar_35 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_31 * tmpvar_8.x) + (tmpvar_32 * tmpvar_8.y)) + (tmpvar_33 * tmpvar_8.z))
   * 
    inversesqrt(tmpvar_34)
  )) * (1.0/((1.0 + 
    (tmpvar_34 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_36;
  tmpvar_36 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_35.x) + (unity_LightColor[1].xyz * tmpvar_35.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_35.z)
  ) + (unity_LightColor[3].xyz * tmpvar_35.w)));
  tmpvar_6 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_11 * ((
    (_World2Object * tmpvar_13)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * cse_30);
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out highp vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  highp vec4 tmpvar_4;
  lowp vec3 tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_4.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_4.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_5 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_8;
  mediump vec3 tmpvar_15;
  mediump vec4 normal_16;
  normal_16 = tmpvar_14;
  highp float vC_17;
  mediump vec3 x3_18;
  mediump vec3 x2_19;
  mediump vec3 x1_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAr, normal_16);
  x1_20.x = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAg, normal_16);
  x1_20.y = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHAb, normal_16);
  x1_20.z = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24 = (normal_16.xyzz * normal_16.yzzx);
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBr, tmpvar_24);
  x2_19.x = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBg, tmpvar_24);
  x2_19.y = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = dot (unity_SHBb, tmpvar_24);
  x2_19.z = tmpvar_27;
  mediump float tmpvar_28;
  tmpvar_28 = ((normal_16.x * normal_16.x) - (normal_16.y * normal_16.y));
  vC_17 = tmpvar_28;
  highp vec3 tmpvar_29;
  tmpvar_29 = (unity_SHC.xyz * vC_17);
  x3_18 = tmpvar_29;
  tmpvar_15 = ((x1_20 + x2_19) + x3_18);
  shlight_3 = tmpvar_15;
  tmpvar_6 = shlight_3;
  highp vec4 cse_30;
  cse_30 = (_Object2World * _glesVertex);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosX0 - cse_30.x);
  highp vec4 tmpvar_32;
  tmpvar_32 = (unity_4LightPosY0 - cse_30.y);
  highp vec4 tmpvar_33;
  tmpvar_33 = (unity_4LightPosZ0 - cse_30.z);
  highp vec4 tmpvar_34;
  tmpvar_34 = (((tmpvar_31 * tmpvar_31) + (tmpvar_32 * tmpvar_32)) + (tmpvar_33 * tmpvar_33));
  highp vec4 tmpvar_35;
  tmpvar_35 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_31 * tmpvar_8.x) + (tmpvar_32 * tmpvar_8.y)) + (tmpvar_33 * tmpvar_8.z))
   * 
    inversesqrt(tmpvar_34)
  )) * (1.0/((1.0 + 
    (tmpvar_34 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_36;
  tmpvar_36 = (tmpvar_6 + ((
    ((unity_LightColor[0].xyz * tmpvar_35.x) + (unity_LightColor[1].xyz * tmpvar_35.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_35.z)
  ) + (unity_LightColor[3].xyz * tmpvar_35.w)));
  tmpvar_6 = tmpvar_36;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = (tmpvar_11 * ((
    (_World2Object * tmpvar_13)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * cse_30);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
void main ()
{
  lowp vec4 c_1;
  highp vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = xlv_TEXCOORD0.zw;
  tmpvar_2 = xlv_COLOR0;
  mediump vec3 tmpvar_4;
  mediump float tmpvar_5;
  highp vec4 Tex2D2_6;
  highp vec4 Tex2D1_7;
  highp vec4 Tex2D0_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_8 = tmpvar_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (_Color01 * Tex2D0_8);
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_12;
  tmpvar_12.x = tmpvar_3.x;
  tmpvar_12.y = (xlv_TEXCOORD0.w + tmpvar_11.x);
  tmpvar_12.z = (xlv_TEXCOORD0.z + tmpvar_11.x);
  tmpvar_12.w = tmpvar_3.y;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture (_Blend_Texture, tmpvar_12.xy);
  Tex2D1_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_Color02 * Tex2D1_7);
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_16;
  tmpvar_16.x = (xlv_TEXCOORD1.x + tmpvar_15.x);
  tmpvar_16.y = (xlv_TEXCOORD1.y + tmpvar_15.x);
  tmpvar_16.z = xlv_TEXCOORD1.x;
  tmpvar_16.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture (_Blend_Texture01, tmpvar_16.xy);
  Tex2D2_6 = tmpvar_17;
  highp vec4 tmpvar_18;
  tmpvar_18 = (_Color03 * Tex2D2_6);
  highp vec4 tmpvar_19;
  tmpvar_19 = (vec4(_LightenMain) * (tmpvar_10 + (
    (tmpvar_10 * ((tmpvar_14 + tmpvar_18) * (tmpvar_14 * tmpvar_18)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * tmpvar_2).xyz;
  tmpvar_4 = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = (tmpvar_19 * tmpvar_2.wwww).x;
  tmpvar_5 = tmpvar_21;
  mediump vec4 c_22;
  c_22.xyz = vec3(0.0, 0.0, 0.0);
  c_22.w = tmpvar_5;
  c_1 = c_22;
  mediump vec3 tmpvar_23;
  tmpvar_23 = c_1.xyz;
  c_1.xyz = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = (c_1.xyz + tmpvar_4);
  c_1.xyz = tmpvar_24;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="False" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
out mediump vec3 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
out mediump vec3 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
out mediump vec3 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
out mediump vec3 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * ((
    (_World2Object * _WorldSpaceLightPos0)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
varying mediump vec3 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex)).xy;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesTANGENT;
uniform highp vec3 _WorldSpaceCameraPos;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
out mediump vec3 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  mediump vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec3 tmpvar_6;
  tmpvar_5 = tmpvar_1.xyz;
  tmpvar_6 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_7;
  tmpvar_7[0].x = tmpvar_5.x;
  tmpvar_7[0].y = tmpvar_6.x;
  tmpvar_7[0].z = tmpvar_2.x;
  tmpvar_7[1].x = tmpvar_5.y;
  tmpvar_7[1].y = tmpvar_6.y;
  tmpvar_7[1].z = tmpvar_2.y;
  tmpvar_7[2].x = tmpvar_5.z;
  tmpvar_7[2].y = tmpvar_6.z;
  tmpvar_7[2].z = tmpvar_2.z;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_3 = tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_7 * ((
    (_World2Object * tmpvar_9)
  .xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_4 = tmpvar_10;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_3;
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = (_LightMatrix0 * (_Object2World * _glesVertex)).xy;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
void main ()
{
  lowp vec4 c_1;
  highp vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  highp vec2 tmpvar_4;
  highp vec4 tmpvar_5;
  mediump float tmpvar_6;
  highp vec4 Tex2D2_7;
  highp vec4 Tex2D1_8;
  highp vec4 Tex2D0_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture (_MainTex, tmpvar_2);
  Tex2D0_9 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11 = (_Color01 * Tex2D0_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_13;
  tmpvar_13.x = tmpvar_3.x;
  tmpvar_13.y = (tmpvar_3.y + tmpvar_12.x);
  tmpvar_13.z = (tmpvar_3.x + tmpvar_12.x);
  tmpvar_13.w = tmpvar_3.y;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_Blend_Texture, tmpvar_13.xy);
  Tex2D1_8 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color02 * Tex2D1_8);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_17;
  tmpvar_17.x = (tmpvar_4.x + tmpvar_16.x);
  tmpvar_17.y = (tmpvar_4.y + tmpvar_16.x);
  tmpvar_17.z = tmpvar_4.x;
  tmpvar_17.w = tmpvar_4.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture01, tmpvar_17.xy);
  Tex2D2_7 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color03 * Tex2D2_7);
  highp float tmpvar_20;
  tmpvar_20 = ((vec4(_LightenMain) * (tmpvar_11 + 
    ((tmpvar_11 * ((tmpvar_15 + tmpvar_19) * (tmpvar_15 * tmpvar_19))) * vec4(_Lighten))
  )) * tmpvar_5.wwww).x;
  tmpvar_6 = tmpvar_20;
  mediump vec4 c_21;
  c_21.xyz = vec3(0.0, 0.0, 0.0);
  c_21.w = tmpvar_6;
  c_1.xyz = c_21.xyz;
  c_1.w = 0.0;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3"
}
}
 }
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="False" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend DstColor SrcColor, One One
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesTANGENT;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  tmpvar_3 = tmpvar_1.xyz;
  tmpvar_4 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_5;
  tmpvar_5[0].x = tmpvar_3.x;
  tmpvar_5[0].y = tmpvar_4.x;
  tmpvar_5[0].z = tmpvar_2.x;
  tmpvar_5[1].x = tmpvar_3.y;
  tmpvar_5[1].y = tmpvar_4.y;
  tmpvar_5[1].z = tmpvar_2.y;
  tmpvar_5[2].x = tmpvar_3.z;
  tmpvar_5[2].y = tmpvar_4.z;
  tmpvar_5[2].z = tmpvar_2.z;
  highp vec3 v_6;
  v_6.x = _Object2World[0].x;
  v_6.y = _Object2World[1].x;
  v_6.z = _Object2World[2].x;
  highp vec3 v_7;
  v_7.x = _Object2World[0].y;
  v_7.y = _Object2World[1].y;
  v_7.z = _Object2World[2].y;
  highp vec3 v_8;
  v_8.x = _Object2World[0].z;
  v_8.y = _Object2World[1].z;
  v_8.z = _Object2World[2].z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((tmpvar_5 * v_6) * unity_Scale.w);
  xlv_TEXCOORD1 = ((tmpvar_5 * v_7) * unity_Scale.w);
  xlv_TEXCOORD2 = ((tmpvar_5 * v_8) * unity_Scale.w);
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 res_1;
  lowp vec3 worldN_2;
  mediump vec3 tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD0.z;
  worldN_2.x = tmpvar_4;
  highp float tmpvar_5;
  tmpvar_5 = xlv_TEXCOORD1.z;
  worldN_2.y = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = xlv_TEXCOORD2.z;
  worldN_2.z = tmpvar_6;
  tmpvar_3 = worldN_2;
  mediump vec3 tmpvar_7;
  tmpvar_7 = ((tmpvar_3 * 0.5) + 0.5);
  res_1.xyz = tmpvar_7;
  res_1.w = 0.0;
  gl_FragData[0] = res_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesTANGENT;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
out highp vec3 xlv_TEXCOORD0;
out highp vec3 xlv_TEXCOORD1;
out highp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  tmpvar_3 = tmpvar_1.xyz;
  tmpvar_4 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_5;
  tmpvar_5[0].x = tmpvar_3.x;
  tmpvar_5[0].y = tmpvar_4.x;
  tmpvar_5[0].z = tmpvar_2.x;
  tmpvar_5[1].x = tmpvar_3.y;
  tmpvar_5[1].y = tmpvar_4.y;
  tmpvar_5[1].z = tmpvar_2.y;
  tmpvar_5[2].x = tmpvar_3.z;
  tmpvar_5[2].y = tmpvar_4.z;
  tmpvar_5[2].z = tmpvar_2.z;
  highp vec3 v_6;
  v_6.x = _Object2World[0].x;
  v_6.y = _Object2World[1].x;
  v_6.z = _Object2World[2].x;
  highp vec3 v_7;
  v_7.x = _Object2World[0].y;
  v_7.y = _Object2World[1].y;
  v_7.z = _Object2World[2].y;
  highp vec3 v_8;
  v_8.x = _Object2World[0].z;
  v_8.y = _Object2World[1].z;
  v_8.z = _Object2World[2].z;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((tmpvar_5 * v_6) * unity_Scale.w);
  xlv_TEXCOORD1 = ((tmpvar_5 * v_7) * unity_Scale.w);
  xlv_TEXCOORD2 = ((tmpvar_5 * v_8) * unity_Scale.w);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
in highp vec3 xlv_TEXCOORD0;
in highp vec3 xlv_TEXCOORD1;
in highp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 res_1;
  lowp vec3 worldN_2;
  mediump vec3 tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD0.z;
  worldN_2.x = tmpvar_4;
  highp float tmpvar_5;
  tmpvar_5 = xlv_TEXCOORD1.z;
  worldN_2.y = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = xlv_TEXCOORD2.z;
  worldN_2.z = tmpvar_6;
  tmpvar_3 = worldN_2;
  mediump vec3 tmpvar_7;
  tmpvar_7 = ((tmpvar_3 * 0.5) + 0.5);
  res_1.xyz = tmpvar_7;
  res_1.w = 0.0;
  _glesFragData[0] = res_1;
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
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "QUEUE"="Transparent" "IGNOREPROJECTOR"="False" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = (tmpvar_7 * (normalize(_glesNormal) * unity_Scale.w));
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  tmpvar_2 = tmpvar_9;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec4 tmpvar_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = xlv_TEXCOORD0.zw;
  tmpvar_4 = xlv_COLOR0;
  mediump vec3 tmpvar_6;
  mediump float tmpvar_7;
  highp vec4 Tex2D2_8;
  highp vec4 Tex2D1_9;
  highp vec4 Tex2D0_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_10 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Color01 * Tex2D0_10);
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_14;
  tmpvar_14.x = tmpvar_5.x;
  tmpvar_14.y = (xlv_TEXCOORD0.w + tmpvar_13.x);
  tmpvar_14.z = (xlv_TEXCOORD0.z + tmpvar_13.x);
  tmpvar_14.w = tmpvar_5.y;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_Blend_Texture, tmpvar_14.xy);
  Tex2D1_9 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Color02 * Tex2D1_9);
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_18;
  tmpvar_18.x = (xlv_TEXCOORD1.x + tmpvar_17.x);
  tmpvar_18.y = (xlv_TEXCOORD1.y + tmpvar_17.x);
  tmpvar_18.z = xlv_TEXCOORD1.x;
  tmpvar_18.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_Blend_Texture01, tmpvar_18.xy);
  Tex2D2_8 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Color03 * Tex2D2_8);
  highp vec4 tmpvar_21;
  tmpvar_21 = (vec4(_LightenMain) * (tmpvar_12 + (
    (tmpvar_12 * ((tmpvar_16 + tmpvar_20) * (tmpvar_16 * tmpvar_20)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * tmpvar_4).xyz;
  tmpvar_6 = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = (tmpvar_21 * tmpvar_4.wwww).x;
  tmpvar_7 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
  light_3.w = tmpvar_25.w;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_25.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_26;
  mediump vec4 c_27;
  c_27.xyz = vec3(0.0, 0.0, 0.0);
  c_27.w = tmpvar_7;
  c_2.w = c_27.w;
  c_2.xyz = tmpvar_6;
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = (tmpvar_7 * (normalize(_glesNormal) * unity_Scale.w));
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  tmpvar_2 = tmpvar_9;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
in highp vec4 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec4 tmpvar_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = xlv_TEXCOORD0.zw;
  tmpvar_4 = xlv_COLOR0;
  mediump vec3 tmpvar_6;
  mediump float tmpvar_7;
  highp vec4 Tex2D2_8;
  highp vec4 Tex2D1_9;
  highp vec4 Tex2D0_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_10 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Color01 * Tex2D0_10);
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_14;
  tmpvar_14.x = tmpvar_5.x;
  tmpvar_14.y = (xlv_TEXCOORD0.w + tmpvar_13.x);
  tmpvar_14.z = (xlv_TEXCOORD0.z + tmpvar_13.x);
  tmpvar_14.w = tmpvar_5.y;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture (_Blend_Texture, tmpvar_14.xy);
  Tex2D1_9 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Color02 * Tex2D1_9);
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_18;
  tmpvar_18.x = (xlv_TEXCOORD1.x + tmpvar_17.x);
  tmpvar_18.y = (xlv_TEXCOORD1.y + tmpvar_17.x);
  tmpvar_18.z = xlv_TEXCOORD1.x;
  tmpvar_18.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture (_Blend_Texture01, tmpvar_18.xy);
  Tex2D2_8 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Color03 * Tex2D2_8);
  highp vec4 tmpvar_21;
  tmpvar_21 = (vec4(_LightenMain) * (tmpvar_12 + (
    (tmpvar_12 * ((tmpvar_16 + tmpvar_20) * (tmpvar_16 * tmpvar_20)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * tmpvar_4).xyz;
  tmpvar_6 = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = (tmpvar_21 * tmpvar_4.wwww).x;
  tmpvar_7 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
  light_3.w = tmpvar_25.w;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_25.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_26;
  mediump vec4 c_27;
  c_27.xyz = vec3(0.0, 0.0, 0.0);
  c_27.w = tmpvar_7;
  c_2.w = c_27.w;
  c_2.xyz = tmpvar_6;
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  tmpvar_2.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_2.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec4 tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD0.zw;
  tmpvar_7 = xlv_COLOR0;
  mediump vec3 tmpvar_9;
  mediump float tmpvar_10;
  highp vec4 Tex2D2_11;
  highp vec4 Tex2D1_12;
  highp vec4 Tex2D0_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_13 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color01 * Tex2D0_13);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_17;
  tmpvar_17.x = tmpvar_8.x;
  tmpvar_17.y = (xlv_TEXCOORD0.w + tmpvar_16.x);
  tmpvar_17.z = (xlv_TEXCOORD0.z + tmpvar_16.x);
  tmpvar_17.w = tmpvar_8.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture, tmpvar_17.xy);
  Tex2D1_12 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color02 * Tex2D1_12);
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_21;
  tmpvar_21.x = (xlv_TEXCOORD1.x + tmpvar_20.x);
  tmpvar_21.y = (xlv_TEXCOORD1.y + tmpvar_20.x);
  tmpvar_21.z = xlv_TEXCOORD1.x;
  tmpvar_21.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_Blend_Texture01, tmpvar_21.xy);
  Tex2D2_11 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23 = (_Color03 * Tex2D2_11);
  highp vec4 tmpvar_24;
  tmpvar_24 = (vec4(_LightenMain) * (tmpvar_15 + (
    (tmpvar_15 * ((tmpvar_19 + tmpvar_23) * (tmpvar_19 * tmpvar_23)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * tmpvar_7).xyz;
  tmpvar_9 = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_24 * tmpvar_7.wwww).x;
  tmpvar_10 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_27;
  mediump vec4 tmpvar_28;
  tmpvar_28 = -(log2(max (light_6, vec4(0.001, 0.001, 0.001, 0.001))));
  light_6.w = tmpvar_28.w;
  highp float tmpvar_29;
  tmpvar_29 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_29;
  lowp vec3 tmpvar_30;
  tmpvar_30 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = (2.0 * texture2D (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_31;
  light_6.xyz = (tmpvar_28.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  mediump vec4 c_32;
  c_32.xyz = vec3(0.0, 0.0, 0.0);
  c_32.w = tmpvar_10;
  c_2.w = c_32.w;
  c_2.xyz = tmpvar_9;
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out highp vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  tmpvar_2.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_2.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
in highp vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec4 tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD0.zw;
  tmpvar_7 = xlv_COLOR0;
  mediump vec3 tmpvar_9;
  mediump float tmpvar_10;
  highp vec4 Tex2D2_11;
  highp vec4 Tex2D1_12;
  highp vec4 Tex2D0_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_13 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color01 * Tex2D0_13);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_17;
  tmpvar_17.x = tmpvar_8.x;
  tmpvar_17.y = (xlv_TEXCOORD0.w + tmpvar_16.x);
  tmpvar_17.z = (xlv_TEXCOORD0.z + tmpvar_16.x);
  tmpvar_17.w = tmpvar_8.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture, tmpvar_17.xy);
  Tex2D1_12 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color02 * Tex2D1_12);
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_21;
  tmpvar_21.x = (xlv_TEXCOORD1.x + tmpvar_20.x);
  tmpvar_21.y = (xlv_TEXCOORD1.y + tmpvar_20.x);
  tmpvar_21.z = xlv_TEXCOORD1.x;
  tmpvar_21.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture (_Blend_Texture01, tmpvar_21.xy);
  Tex2D2_11 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23 = (_Color03 * Tex2D2_11);
  highp vec4 tmpvar_24;
  tmpvar_24 = (vec4(_LightenMain) * (tmpvar_15 + (
    (tmpvar_15 * ((tmpvar_19 + tmpvar_23) * (tmpvar_19 * tmpvar_23)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * tmpvar_7).xyz;
  tmpvar_9 = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_24 * tmpvar_7.wwww).x;
  tmpvar_10 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_27;
  mediump vec4 tmpvar_28;
  tmpvar_28 = -(log2(max (light_6, vec4(0.001, 0.001, 0.001, 0.001))));
  light_6.w = tmpvar_28.w;
  highp float tmpvar_29;
  tmpvar_29 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_29;
  lowp vec3 tmpvar_30;
  tmpvar_30 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = (2.0 * texture (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_31;
  light_6.xyz = (tmpvar_28.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  mediump vec4 c_32;
  c_32.xyz = vec3(0.0, 0.0, 0.0);
  c_32.w = tmpvar_10;
  c_2.w = c_32.w;
  c_2.xyz = tmpvar_9;
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = (tmpvar_7 * (normalize(_glesNormal) * unity_Scale.w));
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  tmpvar_2 = tmpvar_9;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec4 tmpvar_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = xlv_TEXCOORD0.zw;
  tmpvar_4 = xlv_COLOR0;
  mediump vec3 tmpvar_6;
  mediump float tmpvar_7;
  highp vec4 Tex2D2_8;
  highp vec4 Tex2D1_9;
  highp vec4 Tex2D0_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_10 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Color01 * Tex2D0_10);
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_14;
  tmpvar_14.x = tmpvar_5.x;
  tmpvar_14.y = (xlv_TEXCOORD0.w + tmpvar_13.x);
  tmpvar_14.z = (xlv_TEXCOORD0.z + tmpvar_13.x);
  tmpvar_14.w = tmpvar_5.y;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_Blend_Texture, tmpvar_14.xy);
  Tex2D1_9 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Color02 * Tex2D1_9);
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_18;
  tmpvar_18.x = (xlv_TEXCOORD1.x + tmpvar_17.x);
  tmpvar_18.y = (xlv_TEXCOORD1.y + tmpvar_17.x);
  tmpvar_18.z = xlv_TEXCOORD1.x;
  tmpvar_18.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_Blend_Texture01, tmpvar_18.xy);
  Tex2D2_8 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Color03 * Tex2D2_8);
  highp vec4 tmpvar_21;
  tmpvar_21 = (vec4(_LightenMain) * (tmpvar_12 + (
    (tmpvar_12 * ((tmpvar_16 + tmpvar_20) * (tmpvar_16 * tmpvar_20)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * tmpvar_4).xyz;
  tmpvar_6 = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = (tmpvar_21 * tmpvar_4.wwww).x;
  tmpvar_7 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
  light_3.w = tmpvar_25.w;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_25.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_26;
  mediump vec4 c_27;
  c_27.xyz = vec3(0.0, 0.0, 0.0);
  c_27.w = tmpvar_7;
  c_2.w = c_27.w;
  c_2.xyz = tmpvar_6;
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = (tmpvar_7 * (normalize(_glesNormal) * unity_Scale.w));
  mediump vec3 tmpvar_9;
  mediump vec4 normal_10;
  normal_10 = tmpvar_8;
  highp float vC_11;
  mediump vec3 x3_12;
  mediump vec3 x2_13;
  mediump vec3 x1_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAr, normal_10);
  x1_14.x = tmpvar_15;
  highp float tmpvar_16;
  tmpvar_16 = dot (unity_SHAg, normal_10);
  x1_14.y = tmpvar_16;
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHAb, normal_10);
  x1_14.z = tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = (normal_10.xyzz * normal_10.yzzx);
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBr, tmpvar_18);
  x2_13.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHBg, tmpvar_18);
  x2_13.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHBb, tmpvar_18);
  x2_13.z = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y));
  vC_11 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = (unity_SHC.xyz * vC_11);
  x3_12 = tmpvar_23;
  tmpvar_9 = ((x1_14 + x2_13) + x3_12);
  tmpvar_2 = tmpvar_9;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
in highp vec4 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec4 tmpvar_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = xlv_TEXCOORD0.zw;
  tmpvar_4 = xlv_COLOR0;
  mediump vec3 tmpvar_6;
  mediump float tmpvar_7;
  highp vec4 Tex2D2_8;
  highp vec4 Tex2D1_9;
  highp vec4 Tex2D0_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_10 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12 = (_Color01 * Tex2D0_10);
  highp vec4 tmpvar_13;
  tmpvar_13 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_14;
  tmpvar_14.x = tmpvar_5.x;
  tmpvar_14.y = (xlv_TEXCOORD0.w + tmpvar_13.x);
  tmpvar_14.z = (xlv_TEXCOORD0.z + tmpvar_13.x);
  tmpvar_14.w = tmpvar_5.y;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture (_Blend_Texture, tmpvar_14.xy);
  Tex2D1_9 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Color02 * Tex2D1_9);
  highp vec4 tmpvar_17;
  tmpvar_17 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_18;
  tmpvar_18.x = (xlv_TEXCOORD1.x + tmpvar_17.x);
  tmpvar_18.y = (xlv_TEXCOORD1.y + tmpvar_17.x);
  tmpvar_18.z = xlv_TEXCOORD1.x;
  tmpvar_18.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture (_Blend_Texture01, tmpvar_18.xy);
  Tex2D2_8 = tmpvar_19;
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Color03 * Tex2D2_8);
  highp vec4 tmpvar_21;
  tmpvar_21 = (vec4(_LightenMain) * (tmpvar_12 + (
    (tmpvar_12 * ((tmpvar_16 + tmpvar_20) * (tmpvar_16 * tmpvar_20)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_21 * tmpvar_4).xyz;
  tmpvar_6 = tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = (tmpvar_21 * tmpvar_4.wwww).x;
  tmpvar_7 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_24;
  mediump vec4 tmpvar_25;
  tmpvar_25 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
  light_3.w = tmpvar_25.w;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_25.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_26;
  mediump vec4 c_27;
  c_27.xyz = vec3(0.0, 0.0, 0.0);
  c_27.w = tmpvar_7;
  c_2.w = c_27.w;
  c_2.xyz = tmpvar_6;
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  tmpvar_2.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_2.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR0;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec4 tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD0.zw;
  tmpvar_7 = xlv_COLOR0;
  mediump vec3 tmpvar_9;
  mediump float tmpvar_10;
  highp vec4 Tex2D2_11;
  highp vec4 Tex2D1_12;
  highp vec4 Tex2D0_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_13 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color01 * Tex2D0_13);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_17;
  tmpvar_17.x = tmpvar_8.x;
  tmpvar_17.y = (xlv_TEXCOORD0.w + tmpvar_16.x);
  tmpvar_17.z = (xlv_TEXCOORD0.z + tmpvar_16.x);
  tmpvar_17.w = tmpvar_8.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_Blend_Texture, tmpvar_17.xy);
  Tex2D1_12 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color02 * Tex2D1_12);
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_21;
  tmpvar_21.x = (xlv_TEXCOORD1.x + tmpvar_20.x);
  tmpvar_21.y = (xlv_TEXCOORD1.y + tmpvar_20.x);
  tmpvar_21.z = xlv_TEXCOORD1.x;
  tmpvar_21.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_Blend_Texture01, tmpvar_21.xy);
  Tex2D2_11 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23 = (_Color03 * Tex2D2_11);
  highp vec4 tmpvar_24;
  tmpvar_24 = (vec4(_LightenMain) * (tmpvar_15 + (
    (tmpvar_15 * ((tmpvar_19 + tmpvar_23) * (tmpvar_19 * tmpvar_23)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * tmpvar_7).xyz;
  tmpvar_9 = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_24 * tmpvar_7.wwww).x;
  tmpvar_10 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_27;
  mediump vec4 tmpvar_28;
  tmpvar_28 = max (light_6, vec4(0.001, 0.001, 0.001, 0.001));
  light_6.w = tmpvar_28.w;
  highp float tmpvar_29;
  tmpvar_29 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_29;
  lowp vec3 tmpvar_30;
  tmpvar_30 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = (2.0 * texture2D (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_31;
  light_6.xyz = (tmpvar_28.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  mediump vec4 c_32;
  c_32.xyz = vec3(0.0, 0.0, 0.0);
  c_32.w = tmpvar_10;
  c_2.w = c_32.w;
  c_2.xyz = tmpvar_9;
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _Blend_Texture_ST;
uniform highp vec4 _Blend_Texture01_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
out lowp vec4 xlv_COLOR0;
out highp vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _Blend_Texture_ST.xy) + _Blend_Texture_ST.zw);
  highp vec4 o_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_4.xy = (tmpvar_6 + tmpvar_5.w);
  o_4.zw = tmpvar_3.zw;
  tmpvar_2.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_2.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _Blend_Texture01_ST.xy) + _Blend_Texture01_ST.zw);
  xlv_COLOR0 = _glesColor;
  xlv_TEXCOORD2 = o_4;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform sampler2D _MainTex;
uniform highp vec4 _Color01;
uniform sampler2D _Blend_Texture;
uniform highp vec4 _Color02;
uniform sampler2D _Blend_Texture01;
uniform highp vec4 _Color03;
uniform highp float _Speed01;
uniform highp float _Speed02;
uniform highp float _LightenMain;
uniform highp float _Lighten;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
in highp vec4 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
in lowp vec4 xlv_COLOR0;
in highp vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec4 tmpvar_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD0.zw;
  tmpvar_7 = xlv_COLOR0;
  mediump vec3 tmpvar_9;
  mediump float tmpvar_10;
  highp vec4 Tex2D2_11;
  highp vec4 Tex2D1_12;
  highp vec4 Tex2D0_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_MainTex, xlv_TEXCOORD0.xy);
  Tex2D0_13 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_Color01 * Tex2D0_13);
  highp vec4 tmpvar_16;
  tmpvar_16 = (_Time * vec4(_Speed01));
  highp vec4 tmpvar_17;
  tmpvar_17.x = tmpvar_8.x;
  tmpvar_17.y = (xlv_TEXCOORD0.w + tmpvar_16.x);
  tmpvar_17.z = (xlv_TEXCOORD0.z + tmpvar_16.x);
  tmpvar_17.w = tmpvar_8.y;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture (_Blend_Texture, tmpvar_17.xy);
  Tex2D1_12 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19 = (_Color02 * Tex2D1_12);
  highp vec4 tmpvar_20;
  tmpvar_20 = (_Time * vec4(_Speed02));
  highp vec4 tmpvar_21;
  tmpvar_21.x = (xlv_TEXCOORD1.x + tmpvar_20.x);
  tmpvar_21.y = (xlv_TEXCOORD1.y + tmpvar_20.x);
  tmpvar_21.z = xlv_TEXCOORD1.x;
  tmpvar_21.w = xlv_TEXCOORD1.y;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture (_Blend_Texture01, tmpvar_21.xy);
  Tex2D2_11 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23 = (_Color03 * Tex2D2_11);
  highp vec4 tmpvar_24;
  tmpvar_24 = (vec4(_LightenMain) * (tmpvar_15 + (
    (tmpvar_15 * ((tmpvar_19 + tmpvar_23) * (tmpvar_19 * tmpvar_23)))
   * vec4(_Lighten))));
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * tmpvar_7).xyz;
  tmpvar_9 = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = (tmpvar_24 * tmpvar_7.wwww).x;
  tmpvar_10 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_27;
  mediump vec4 tmpvar_28;
  tmpvar_28 = max (light_6, vec4(0.001, 0.001, 0.001, 0.001));
  light_6.w = tmpvar_28.w;
  highp float tmpvar_29;
  tmpvar_29 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_29;
  lowp vec3 tmpvar_30;
  tmpvar_30 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_30;
  lowp vec3 tmpvar_31;
  tmpvar_31 = (2.0 * texture (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_31;
  light_6.xyz = (tmpvar_28.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  mediump vec4 c_32;
  c_32.xyz = vec3(0.0, 0.0, 0.0);
  c_32.w = tmpvar_10;
  c_2.w = c_32.w;
  c_2.xyz = tmpvar_9;
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3"
}
}
 }
}
Fallback "Diffuse"
}