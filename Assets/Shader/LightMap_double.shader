Shader "iPhone/LightMap_double" {
Properties {
 _texBase ("MainTex", 2D) = "" {}
 _texLightmap ("LightMap", 2D) = "" {}
}
SubShader { 
 Pass {
  Tags { "RenderType"="Geometry" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  SetTexture [_texBase] { combine texture }
  SetTexture [_texLightmap] { combine previous * texture double }
 }
}
}