Shader "Custom/ColoredGlassSurface"
{
    Properties
    {
        _Color ("Glass Color", Color) = (0, 0.7, 1, 0.4)
        _Emission ("Emission Strength", Range(0,5)) = 1
        _Smoothness ("Smoothness", Range(0,1)) = 0.9
        _Metallic ("Metallic", Range(0,1)) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade

        fixed4 _Color;
        float _Emission;
        float _Smoothness;
        float _Metallic;

        struct Input
        {
            float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Tint
            o.Albedo = _Color.rgb;

            // Transparent
            o.Alpha = _Color.a;

            // Glossy glass
            o.Smoothness = _Smoothness;
            o.Metallic = _Metallic;

            // Fake light transmission so it's colored even in shadow
            o.Emission = _Color.rgb * _Emission;
        }
        ENDCG
    }
}