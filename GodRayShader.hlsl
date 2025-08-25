Texture2D InputTexture : register(t0);
SamplerState InputSampler : register(s0);

cbuffer constants : register(b0)
{
    float x : packoffset(c0.x);
    float y : packoffset(c0.y);
    float intensity : packoffset(c0.z);
    float decay : packoffset(c0.w);
    
    float density : packoffset(c1.x);
    float weight : packoffset(c1.y);
    float colorR : packoffset(c1.z);
    float colorG : packoffset(c1.w);
    
    float colorB : packoffset(c2.x);
    float time : packoffset(c2.y);
    float fluctuationFrequency : packoffset(c2.z);
    float fluctuationAmount : packoffset(c2.w);
};

static const int NUM_SAMPLES = 64;

float rand(float2 co)
{
    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float4 main(
    float4 pos : SV_POSITION,
    float4 posScene : SCENE_POSITION,
    float4 uv0 : TEXCOORD0
) : SV_Target
{
    float4 originalColor = InputTexture.Sample(InputSampler, uv0.xy);
    float2 lightPos = float2(x, y);
    float2 currentPos = posScene.xy;
    
    float2 lightDirection = lightPos - currentPos;
    float distanceToLight = length(lightDirection);
    
    if (distanceToLight > 0.001f)
    {
        lightDirection /= distanceToLight;
    }
    
    float stepLength = density;
    float2 step = lightDirection * stepLength;
    float2 stepUV = step * uv0.zw;
    float2 marchUV = uv0.xy;
    
    float3 godRayColor = float3(0, 0, 0);
    float illuminationDecay = 1.0;
    
    for (int i = 0; i < NUM_SAMPLES; i++)
    {
        marchUV += stepUV;
        
        float2 distortedUV = marchUV;
        if (fluctuationAmount > 0)
        {
            float noise = (rand(marchUV * fluctuationFrequency + time * 0.01) - 0.5) * 2.0;
            distortedUV += noise * fluctuationAmount * 0.001;
        }

        if (distortedUV.x >= 0.0f && distortedUV.x <= 1.0f && distortedUV.y >= 0.0f && distortedUV.y <= 1.0f)
        {
            float4 sampleColor = InputTexture.Sample(InputSampler, distortedUV);
            float brightness = dot(sampleColor.rgb, float3(0.299f, 0.587f, 0.114f));
            godRayColor += sampleColor.rgb * brightness * illuminationDecay;
        }
        
        illuminationDecay *= decay;
        if (illuminationDecay < 0.001f)
        {
            break;
        }
    }
    
    float3 lightColorRGB = float3(colorR, colorG, colorB);
    godRayColor *= lightColorRGB * intensity * weight;
    
    float3 finalColor = originalColor.rgb + godRayColor;
    
    return float4(finalColor, originalColor.a);
}