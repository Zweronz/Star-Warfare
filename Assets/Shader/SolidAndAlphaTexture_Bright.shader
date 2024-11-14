Shader "iPhone/SolidAndAlphaTexture_Bright" {
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
   Bind "texcoord1", TexCoord1
  }
  Color [_TintColor]
  SetTexture [_tex2] { combine texture * primary double }
  SetTexture [_texBase] { combine previous + texture }
 }
}
}