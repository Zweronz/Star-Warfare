Shader "iPhone/SolidAndAlphaTexture" {
Properties {
 _TintColor ("Tint Color", Color) = (1,1,1,1)
 _texBase ("MainTex", 2D) = "" {}
 _tex2 ("Texture2", 2D) = "" {}
}
SubShader { 
 Pass {
  Tags { "RenderType"="Geometry" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord
  }
  Color [_TintColor]
  SetTexture [_tex2] { combine texture * primary }
  SetTexture [_texBase] { combine previous + texture }
 }
}
}