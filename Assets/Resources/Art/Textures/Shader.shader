// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader"
{
	Properties
	{
		[HDR]_Color0("Color 0", Color) = (0.4429957,0.4612382,0.5188679,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner5 = ( _Time.y * float2( 0,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 10,10 ) + panner5;
			float temp_output_8_0 = frac( uv_TexCoord1.y );
			float2 temp_cast_0 = (0.4).xx;
			float2 temp_cast_1 = (step( temp_output_8_0 , temp_cast_0 )).xx;
			float2 temp_cast_2 = (step( temp_cast_1 , temp_output_8_0 )).xx;
			o.Emission = ( _Color0 * float4( temp_cast_2, 0.0 , 0.0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18707
466;111;480;546;1756.515;-428.274;1.3;False;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1931.002,763.9193;Inherit;True;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1665.784,731.2476;Inherit;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;4;-1442.292,330.9733;Inherit;True;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;False;10,10;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;5;-1412.19,744.5057;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1181.498,267.9614;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;8;-891.8551,280.8963;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-825.3784,708.3907;Inherit;True;Constant;_Float1;Float 1;0;0;Create;True;0;0;False;0;False;0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;14;-647.8247,550.2617;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;12;-475.6,-37.26855;Inherit;False;Property;_Color0;Color 0;0;1;[HDR];Create;True;0;0;False;0;False;0.4429957,0.4612382,0.5188679,0;0.1647059,1.811765,1.890196,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;9;-519.0858,278.3055;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-297.7324,173.9343;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;48,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;7;0
WireConnection;5;1;6;0
WireConnection;1;0;4;0
WireConnection;1;1;5;0
WireConnection;8;0;1;2
WireConnection;14;0;8;0
WireConnection;14;1;10;0
WireConnection;9;0;14;0
WireConnection;9;1;8;0
WireConnection;13;0;12;0
WireConnection;13;1;9;0
WireConnection;0;2;13;0
ASEEND*/
//CHKSM=E83D97277E39E0C49646F2B04821B54E7EFD7314