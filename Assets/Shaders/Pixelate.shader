Shader "Hidden/Custom/Pixelate"
{
    Properties
    {
        _PixelSize ("Pixel Size", Float) = 8
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/ShaderPass/ShaderPass.cs.hlsl"

            // Входная структура для вершинной функции
            struct appdata
            {
                float3 positionOS : POSITION; // Позиция в объектных координатах
                float2 uv : TEXCOORD0;       // Текстурные координаты
            };

            // Выходная структура для вершинной функции
            struct v2f
            {
                float4 positionCS : SV_POSITION; // Позиция в экранных координатах
                float2 uv : TEXCOORD0;          // Текстурные координаты
            };

            // Объявление переменных
            float _PixelSize;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Вершинная функция
            v2f vert(appdata v)
            {
                v2f o;
                // Преобразование позиции из объектных в клип-координаты
                float3 positionWS = TransformObjectToWorld(v.positionOS);
                o.positionCS = TransformWorldToHClip(positionWS);

                // Передача текстурных координат
                o.uv = v.uv;
                return o;
            }

            // Пиксельная функция
            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                // Понижение разрешения
                uv = floor(uv * _PixelSize) / _PixelSize;
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            }

            ENDHLSL
        }
    }
}