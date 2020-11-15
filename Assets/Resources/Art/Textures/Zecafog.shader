// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Zecafog"
{
	Properties
	{
		_FogIntensity("FogIntensity", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPosition25;
		};

		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _FogIntensity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float3 vertexPos25 = ase_screenPosNorm.xyz;
			float4 ase_screenPos25 = ComputeScreenPos( UnityObjectToClipPos( vertexPos25 ) );
			o.screenPosition25 = ase_screenPos25;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos25 = i.screenPosition25;
			float4 ase_screenPosNorm25 = ase_screenPos25 / ase_screenPos25.w;
			ase_screenPosNorm25.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm25.z : ase_screenPosNorm25.z * 0.5 + 0.5;
			float screenDepth25 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm25.xy ));
			float distanceDepth25 = ( screenDepth25 - LinearEyeDepth( ase_screenPosNorm25.z ) ) / ( 1.0 );
			o.Alpha = ( distanceDepth25 * _FogIntensity );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18707
1368;73;551;936;1585.892;474.6462;1.425807;False;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;10;-1897.521,-292.3787;Float;True;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-1468.789,27.84593;Inherit;False;Property;_FogIntensity;FogIntensity;0;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;25;-1463.275,-199.5116;Inherit;False;True;False;False;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;16;-1667.618,-88.01302;Inherit;False;0;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1172.576,-43.37282;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;26;-1080.699,-253.3377;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1012.334,125.4821;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;27;-1276.842,443.4705;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.01;False;4;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;14;-890.5298,-331.4105;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Zecafog;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;1;10;0
WireConnection;16;0;10;0
WireConnection;19;0;25;0
WireConnection;19;1;22;0
WireConnection;14;9;19;0
ASEEND*/
//CHKSM=A65FE090E40098459EF7A639E32BBC6606F38379