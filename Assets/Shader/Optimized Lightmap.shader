Shader "Optimized/LightMap" {
Properties {
 _texBase ("MainTex", 2D) = "" {}
 _texLightmap ("LightMap", 2D) = "" {}
}
SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 100

    Pass
    {
CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        struct input
        {
            float2 uv : TEXCOORD0;
            float2 lm : TEXCOORD1;
            float4 vertex : POSITION;
        };

        sampler2D _texBase, _texLightmap;
        float4 _texLightmap_ST;

        input vert(input v)
        {
            input o;

            o.uv = v.uv;
            o.lm = v.lm * _texLightmap_ST.xy + _texLightmap_ST.zw;

            o.vertex = UnityObjectToClipPos(v.vertex);

            return o;
        }

        half3 frag(input i) : SV_TARGET
        {
            return tex2D(_texBase, i.uv).rgb * (tex2D(_texLightmap, i.lm).rgb * 2);
        }
ENDCG
    }
}
}