Shader "iPhone/SolidTextureTwoSides" {
Properties {
 _TintColor ("Main Color", Color) = (0.8,0.8,0.8,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Pass {
  Cull Off
  SetTexture [_MainTex] { ConstantColor [_TintColor] combine texture * constant }
 }
}
}