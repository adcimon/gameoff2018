﻿Shader "Image Effects/Camera Transition"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" { }
		_TransitionTex("Transition Texture", 2D) = "white" { }
		_Color("Color", Color) = (0, 0, 0, 1)
		_Cutoff("Cutoff", Range(0, 1)) = 0
		_Fade("Fade", Range(0, 1)) = 1
	}

	SubShader
	{
		Cull Off
		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex Vertex
			#pragma fragment Fragment
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_TexelSize; // used to solve the issue of different vertical texture coordinates between Direct3D and OpenGL
			sampler2D _TransitionTex;
			fixed4 _Color;
			float _Cutoff;
			float _Fade;

			struct VertexData
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct FragmentData
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			FragmentData Vertex( VertexData v )
			{
				FragmentData f;
				f.position = UnityObjectToClipPos(v.position);
				f.uv = v.uv;
				f.uv1 = v.uv;

				// Vertical texture coordinate conventions differ between Direct3D-like and OpenGL-like platforms:
				// - In Direct3D, Metal and consoles, the coordinate is zero at the top, and increases downwards.
				// - In OpenGL and OpenGL ES, the coordinate is zero at the bottom, and increases upwards.
				// Most of the time this does not really matter, except when rendering into a Render Texture.
				// In that case, Unity internally flips rendering upside down when rendering into a texture on non-OpenGL,
				// so that the conventions match between the platforms.
				// Two common cases where this needs to be handled in the shaders are image effects and rendering things in UV space.
				// Link: http://docs.unity3d.com/Manual/SL-PlatformDifferences.html
					
				#if UNITY_UV_STARTS_AT_TOP
				if( _MainTex_TexelSize.y < 0 )
					f.uv1.y = 1 - f.uv1.y; // flip y coordinate
				#endif

				return f;
			}

			float4 Fragment( FragmentData f ) : COLOR
			{
				float4 transitionColor = tex2D(_TransitionTex, f.uv1);

				float4 color = tex2D(_MainTex, f.uv);

				if( transitionColor.r < _Cutoff )
				{
					return color = lerp(color, _Color, _Fade);
				}

				return color;
			}
			ENDCG
		}
	}
}