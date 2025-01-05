Shader "Custom/WhiteWithBlackOutline"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Float) = 0.02
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }

        // Pass for the outline
        Pass
        {
            Name "OutlinePass"
            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float _OutlineThickness;

            v2f vert(appdata v)
            {
                v2f o;

                // Transform normal to world space
                float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));

                // Calculate object scale to keep thickness consistent
                float3 worldScale = float3(
                    length(unity_ObjectToWorld[0].xyz),
                    length(unity_ObjectToWorld[1].xyz),
                    length(unity_ObjectToWorld[2].xyz)
                );
                float scaledThickness = _OutlineThickness / max(max(worldScale.x, worldScale.y), worldScale.z);

                // Offset vertex position outward along the normal
                float3 offset = worldNormal * scaledThickness;
                float4 displacedVertex = v.vertex + float4(offset, 0.0);

                // Transform to clip space
                o.pos = UnityObjectToClipPos(displacedVertex);

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return float4(0, 0, 0, 1); // Black outline color
            }
            ENDHLSL
        }

        // Pass for the fill
        Pass
        {
            Name "FillPass"
            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _MainColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return _MainColor; // White fill color
            }
            ENDHLSL
        }
    }
        FallBack "Diffuse"
}
