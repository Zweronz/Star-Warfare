Shader "Foam_add" {
Properties {
 _Offset ("Offset", Range(0,1)) = 1
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _Cutout ("Mask (A)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent-110" }
 Pass {
  Tags { "QUEUE"="Transparent-110" }
  Color [_Color]
  ZWrite Off
  Cull Off
  Blend SrcAlpha One
  ColorMask RGB
  Offset -1, -1
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture * primary double, texture alpha * constant alpha double }
  SetTexture [_Cutout] { combine previous * texture }
 }
}
}