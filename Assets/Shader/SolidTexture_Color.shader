Shader "iPhone/SolidTexture_Color" {
Properties {
 _TintColor ("Main Color", Color) = (0.8,0.8,0.8,1)
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Pass {
  Color [_TintColor]
  SetTexture [_MainTex] { combine texture * primary }
 }
}
}