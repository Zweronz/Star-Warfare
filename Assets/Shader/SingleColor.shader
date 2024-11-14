Shader "iPhone/SingleColor" {
Properties {
 _TintColor ("Main Color", Color) = (0.8,0.8,0.8,1)
}
SubShader { 
 Pass {
  SetTexture [_MainTex] { ConstantColor [_TintColor] combine constant }
 }
}
}