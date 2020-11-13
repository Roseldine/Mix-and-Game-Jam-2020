// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Stardop / Matcap with Light"
{
	Properties
	{
		[Toggle(_USETEXTURE_ON)] _Usetexture("Use texture", Float) = 1
		_LightcapTexture("Lightcap Texture", 2D) = "white" {}
		_Texture("Texture", 2D) = "white" {}
		_LightcapAngle("Lightcap Angle", Range( 0 , 360)) = 0.5
		_ColorTint("Color Tint", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_local __ _USETEXTURE_ON
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
		};

		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform sampler2D _LightcapTexture;
		uniform float _LightcapAngle;
		uniform float4 _ColorTint;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color35 = IsGammaSpace() ? float4(0.4235294,0.4235294,0.4235294,0) : float4(0.1499598,0.1499598,0.1499598,0);
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			#ifdef _USETEXTURE_ON
				float4 staticSwitch37 = tex2D( _Texture, uv_Texture );
			#else
				float4 staticSwitch37 = color35;
			#endif
			float3 ase_worldNormal = i.worldNormal;
			float cos30 = cos( ( _LightcapAngle / 57.50799 ) );
			float sin30 = sin( ( _LightcapAngle / 57.50799 ) );
			float2 rotator30 = mul( ( ( mul( UNITY_MATRIX_V, float4( ase_worldNormal , 0.0 ) ).xyz * 0.5 ) + 0.5 ).xy - float2( 0.5,0.5 ) , float2x2( cos30 , -sin30 , sin30 , cos30 )) + float2( 0.5,0.5 );
			float grayscale33 = Luminance(tex2D( _LightcapTexture, rotator30 ).rgb);
			o.Emission = ( staticSwitch37 * ( grayscale33 * _ColorTint ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18703
0;195;1680;834;2474.64;1351.059;2.901742;True;False
Node;AmplifyShaderEditor.CommentaryNode;20;-1362.555,-667.6934;Inherit;False;2291.328;1158.503;;12;37;35;33;32;31;30;28;27;25;23;22;21;Matcap;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;21;-1075.48,-246.4923;Inherit;False;549.8301;190.5432;;3;29;26;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ViewMatrixNode;22;-1206.899,-197.2933;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.WorldNormalVector;23;-1269.536,-98.9861;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1046.451,-194.5782;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1228.634,45.91283;Float;False;Constant;_Float0;Float 0;-1;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-891.0118,-192.7991;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-499.918,-69.31912;Float;False;Property;_LightcapAngle;Lightcap Angle;3;0;Create;True;0;0;False;0;False;0.5;0.5;0;360;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;28;-199.213,-33.65432;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;57.50799;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-673.3848,-197.8353;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RotatorNode;30;-48.81896,-58.48512;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;32;-1273.804,-586.7723;Inherit;False;368.5;280;Comment;1;34;Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;31;147.0301,-78.4351;Inherit;True;Property;_LightcapTexture;Lightcap Texture;1;0;Create;True;0;0;False;0;False;-1;None;1a9f806c577ad1045b2a8decb4a86946;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;39;637.2479,741.7814;Inherit;False;Property;_ColorTint;Color Tint;4;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;35;-814.6454,-576.0381;Inherit;False;Constant;_Color1;Color 1;27;0;Create;True;0;0;False;0;False;0.4235294,0.4235294,0.4235294,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCGrayscale;33;633.3934,-91.4133;Inherit;True;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-1238.705,-509.9503;Inherit;True;Property;_Texture;Texture;2;0;Create;True;0;0;False;0;False;-1;None;d463676968781d04ca54a7be4aa20884;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;918.3218,709.9078;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;37;-466.9886,-421.8032;Inherit;False;Property;_Usetexture;Use texture;0;0;Create;True;0;0;False;0;False;1;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;996.6804,-359.1582;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;19;1293.45,19.98717;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Stardop / Matcap with Light;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;26;0;24;0
WireConnection;26;1;25;0
WireConnection;28;0;27;0
WireConnection;29;0;26;0
WireConnection;29;1;25;0
WireConnection;30;0;29;0
WireConnection;30;2;28;0
WireConnection;31;1;30;0
WireConnection;33;0;31;0
WireConnection;38;0;33;0
WireConnection;38;1;39;0
WireConnection;37;1;35;0
WireConnection;37;0;34;0
WireConnection;40;0;37;0
WireConnection;40;1;38;0
WireConnection;19;2;40;0
ASEEND*/
//CHKSM=A017566CF4FCE30C747DFD09789F86A3902E8E4F