Shader "Sentata/SentataStarfield"
{
    Properties
    {
        _StarfieldTex ("Starfield Texture (RGB)", 2D) = "black" {}
        _ConstellationTex ("Constellation Texture (RGB)", 2D) = "black" {}
        _ConstellationWeight ("Weight of Constellation", Range(0.0, 1.0)) = 0
        _Latitude ("Latitude", Range(-90, 90)) = 35
        _RotHorizontal ("Horizontal Rotation", Range(-180, 180)) = 0
        _RotPolar ("Rotation around Polar", Range(0, 360)) = 0
    }

    SubShader
    {
        Tags
        { 
            "RenderType"="Background"
            "Queue"="Background"
            "PreviewType"="SkyBox"
            "ForceNoShadowCasting" = "True"
            "IsEmissive" = "true"
        }
        Cull Off
        CGPROGRAM
        
        #pragma multi_compile_instancing
        #pragma target 3.0
        #pragma surface surf NoLighting keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nofog

        sampler2D _StarfieldTex;
        sampler2D _ConstellationTex;
        float _ConstellationWeight;
        float _Latitude;
        float _RotPolar;
        float _RotHorizontal;

        struct Input
        {
            float3 worldPos;
        };

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
    	    fixed4 c;
    	    c.rgb = s.Albedo; 
    	    c.a = s.Alpha;
    	    return c;
        }

        float2 calcStarSphereUV(float3 worldPos, float latitude, float rotPolar, float rotHorizontal){
            float3 dir = normalize(worldPos);
            float rotationX = (-latitude + 90.0) * UNITY_PI / 180.0;
            float rotationY = rotHorizontal * UNITY_PI / 180.0;
            float uvScrole = -rotPolar * UNITY_PI / 180.0;

            float cosRotX;
            float sinRotX;
            sincos(rotationX, sinRotX, cosRotX);
            float3x3 rotationMatrixXaxis = {1, 0, 0, 0, cosRotX, sinRotX, 0, -sinRotX, cosRotX};

            float cosRotY;
            float sinRotY;
            sincos(rotationY, sinRotY, cosRotY);
            float3x3 rotationMatrixYaxis = {cosRotY, 0, -sinRotY, 0, 1, 0, sinRotY, 0, cosRotY};
            
            float3 rotatedDir = mul(rotationMatrixXaxis, mul(rotationMatrixYaxis, dir));
            float2 uv = float2((atan2(rotatedDir.x, rotatedDir.z) + uvScrole) / (2 * UNITY_PI), asin(rotatedDir.y) / UNITY_PI + 0.5);
            return uv;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 uv = calcStarSphereUV(IN.worldPos, _Latitude, _RotPolar, _RotHorizontal);
            fixed4 starSphere = tex2D (_StarfieldTex, uv);
            fixed4 constellation = tex2D (_ConstellationTex, uv);
            fixed4 output = starSphere + constellation * _ConstellationWeight;
            o.Albedo = output.rgb;
        }
        ENDCG
    }
}
