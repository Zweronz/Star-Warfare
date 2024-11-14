Shader "iPhone/AlphaBlend_Color_TwoSides" {
Properties {
 _TintColor ("Tint Color", Color) = (1,1,1,0.5)
 _MainTex ("Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  Color [_TintColor]
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
}
}