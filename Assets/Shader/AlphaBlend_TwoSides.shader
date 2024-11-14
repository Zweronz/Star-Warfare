Shader "iPhone/AlphaBlend_Bright_TwoSides" {
Properties {
 _MainTex ("Texture", 2D) = "white" {}
 _TintColor ("Tint Color", Color) = (1,1,1,0.5)
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Color [_TintColor]
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture double }
 }
}
}