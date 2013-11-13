Shader "Puffy_Smoke/FakedVolumetric" {

Properties {

	_ShadowColor ("Shadow Color", Color) = (0.0,0.5,0.5,1)
	
	_MainTex ("Particle Texture", 2D) = "white" {}
	_DetailTex ("Particle Details Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	
	_Opacity ("Opacity", Range(0.0,1)) = 0.5
	
	_Scattering ("Scattering", Range(0.0,1.0)) = 1
	
	_Density ("Density", Range(0.0,1.0)) = 0
	_Sharpness ("Sharpness", Range(0.0,5.0)) = 0
	_DetailsSpeed ("Details Speed", Range(0.0,5.0)) = 0.2
	
}


Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "texcoord", texcoord0
		Bind "texcoord1", texcoord1
	}
	
	// ---- Fragment program cards
	SubShader {
		Pass {
		
			CGPROGRAM
			// Upgrade NOTE: excluded shader from OpenGL ES 2.0 because it does not contain a surface program or both vertex and fragment programs.
			#pragma exclude_renderers gles
			// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct appdata_t members worldPos)
			#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _DetailTex;
			sampler2D _CameraDepthTexture;
			
	      	fixed4 _ShadowColor;
	      	fixed4 _AmbientColor;
	      	fixed4 _LightColor;
	      	float4 _MainTex_ST;
			float4 _DetailTex_ST;
			
	      	float _Density;
        	float _DetailsSpeed;
        	float _Sharpness;
        	float _Scattering;
        	float _Opacity;
        	float _details;
        	float _LightIntensity;
        	float _InvFade;
        	
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
				#endif
			};
			
			
			
			v2f vert (appdata_t v)
			{
				//float3 worldPos = mul(_Object2World, v.vertex).xyz;
				v2f o; 
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord1 = TRANSFORM_TEX(v.texcoord1,_DetailTex);
				return o;
			}

			
			fixed4 frag (v2f i) : COLOR
			{
					
				float _old = i.color.a; // particle age is stored in the color alpha channel
				float _young = 1 - _old;
				
				// get main texture color and alpha
				float4 _sampled = tex2D(_MainTex, i.texcoord);

				float4 _finalcolor = (i.color * _LightColor) * _LightIntensity;
				
				// mix light color and shadow color
				_finalcolor = lerp(_ShadowColor , _finalcolor , _sampled.r) + _AmbientColor;
				
				// older particles receive more light color
				if(_Scattering > 0) _finalcolor = lerp(_finalcolor , lerp(_finalcolor , _LightColor * i.color * _LightIntensity + _AmbientColor , _old), _Scattering);
								
				if(_Density < 1){
					// animated noise
					_details = tex2D(_DetailTex, i.texcoord1 + (_young * _DetailsSpeed)).r;
					_details = max(0,min(1 , lerp((0.5+_details) * 0.5, _details , _old*_Sharpness) + _Density));
					_sampled.a *= _details;
				}
				
				// main alpha
				_finalcolor.a = _Opacity * _sampled.a * _young;
								
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				_finalcolor.a *= fade;
				#endif
													
				return _finalcolor;
				
			}
			
			
			ENDCG 
		}	
	} 	
	
	// ---- Dual texture cards
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				constantColor [_TintColor]
				Combine texture * constant DOUBLE, texture * primary
			}
			SetTexture [_DetailTex] {
				Combine previous,previous * texture
			}
		}
	}
	
	// ---- Single texture cards (does not do color tint)
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}

}

}
