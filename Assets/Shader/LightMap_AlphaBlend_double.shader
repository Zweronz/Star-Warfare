Shader "iPhone/LightMap_AlphaBlend_double" {
Properties {
 _Color ("Main Color", Color) = (0.8,0.8,0.8,1)
 _texBase ("MainTex", 2D) = "" {}
 _texLightmap ("LightMap", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  Color [_Color]
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_texBase] { combine texture * primary }
  SetTexture [_texLightmap] { combine previous * texture double }
 }
}
}