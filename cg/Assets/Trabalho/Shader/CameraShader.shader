Shader "Custom/CameraShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OldTVNoise("Texture", 2D) = "white" {}
		_NoiseAttenuation("NoiseAttenuation", Range(0.0,1.0)) = 0.5
		_GrainScale("GrainScale", Range(0.0,1.0)) = 0.5
		_VignetteBlinkvelocity("VignetteBlinkVelocity", Range(0.0,1.0)) = 0.5
		_VignetteDarkAmount("VignetteDarkAmount", Range(0.0,1.0)) = 0.5
		_VigneteDistanceFormCenter("VignetteDistanceFormCenter", Range(0.0,1.0)) = 0.5
		_RandomNumber("RandomNumber", Range(-1.0,1.0)) = 0.5
		_ScreenPartitionWidth("ScreenPartitionWidth", Range(0.0,1.0)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			sampler2D _OldTVNoise;
			float4 _MainTex_ST;
			float _ScreenPartitionWidth;
			float _NoiseAttenuation;
			float _GrainScale;
			float _RandomNumber;
			float _VignetteBlinkvelocity;
			float _VignetteDarkAmount;
			float _VigneteDistanceFormCenter;
			float _Theta = 0.0;

		
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float4 blur(sampler2D tex, float2 uv, float4 size)
			{
				float4 c = tex2D(tex, uv + float2(-size.x, size.y)) + tex2D(tex, uv + float2(0, size.y)) + tex2D(tex, uv + float2(size.x, size.y)) +
							tex2D(tex, uv + float2(-size.x, 0)) + tex2D(tex, uv + float2(0, 0)) + tex2D(tex, uv + float2(size.x, 0)) +
							tex2D(tex, uv + float2(-size.x, -size.y)) + tex2D(tex, uv + float2(0, -size.y)) + tex2D(tex, uv + float2(size.x, -size.y));
				return c / 9;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
 
				fixed4 col = tex2D(_MainTex, i.uv);
 
				//If the uv x cordenate is highter than _ScreenPartitionWidth we apply the b&w effect, if not, we apply the image render how it is.
			     if(i.uv.x >_ScreenPartitionWidth)
			     {
			      //This condition is done in order to draw a vertical line which is the frontier between the image processed and the normal image
			        if(abs(i.uv.x -_ScreenPartitionWidth) < 0.005)
			        return fixed4(0.0,0.0,0.0,1.0);
 
			        //Apply the perception brightness proportion for each color chanel
				float luminosity = col.x * 0.3 + col.y * 0.59 + col.z *  0.11;
 
				_RandomNumber = cos((_Time));
				fixed4 noise = clamp(fixed4(_NoiseAttenuation,_NoiseAttenuation,_NoiseAttenuation,1.0) + tex2D(_OldTVNoise, i.uv*_GrainScale + float2(_RandomNumber,_RandomNumber)), 0.0, 1.0);
				float fadeInBlack = pow(clamp(_VigneteDistanceFormCenter -distance(i.uv, float2(0.5,0.5)) +  abs(cos( _RandomNumber/10 +  _Time*10*_VignetteBlinkvelocity))/4, 0.0, 1.0),_VignetteDarkAmount);
				float4 blurCol = blur(_MainTex, i.uv, float4(1.0,1.0,1.0,1.0));
				float blurValue = (blurCol.x * 0.3 + blurCol.y * 0.59 + blurCol.z *  0.11);
			     	return fixed4(luminosity,luminosity,luminosity,1.0)/blurValue * noise * fadeInBlack*fadeInBlack * blurValue;
 
			     }
			     else{
			      	return col;
			     }
			}
			ENDCG
		}
	}
}