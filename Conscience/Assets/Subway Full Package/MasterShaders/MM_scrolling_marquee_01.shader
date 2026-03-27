// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MM_scrolling_marquee_01"
{
	Properties
	{
		_scrollingtexture("scrolling texture", 2D) = "white" {}
		_Tile_mask("Tile_mask", 2D) = "white" {}
		_Mainmask("Main mask", 2D) = "white" {}
		[HDR]_ColorEmissive("Color Emissive", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Mainmask;
		uniform sampler2D _Tile_mask;
		uniform float4 _ColorEmissive;
		uniform sampler2D _scrollingtexture;
		uniform float4 _scrollingtexture_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float4 color8 = IsGammaSpace() ? float4(0.1509434,0.1035956,0.05624777,0) : float4(0.0198239,0.01058992,0.004521611,0);
			o.Albedo = color8.rgb;
			float4 tex2DNode1 = tex2D( _Mainmask, i.uv_texcoord );
			float2 uv_TexCoord5 = i.uv_texcoord * float2( 5,0.5 ) + float2( 0,0.5 );
			float4 tex2DNode4 = tex2D( _Tile_mask, uv_TexCoord5 );
			float mulTime20 = _Time.y * 0.3;
			float2 uv_scrollingtexture = i.uv_texcoord * _scrollingtexture_ST.xy + _scrollingtexture_ST.zw;
			float4 color28 = IsGammaSpace() ? float4(0.4788959,0.9716981,0.3070933,1) : float4(0.1950248,0.9368213,0.07680037,1);
			float2 panner19 = ( mulTime20 * float2( 0.45,0 ) + ( uv_scrollingtexture * (color28).rg ));
			o.Emission = ( tex2DNode1.r * ( ( ( tex2DNode4.g * _ColorEmissive ) * 0.01 ) + ( ( 0.01 * _ColorEmissive ) + ( ( tex2DNode4.g * tex2D( _scrollingtexture, panner19 ) ) * _ColorEmissive ) ) ) ).rgb;
			o.Metallic = 0.0;
			o.Smoothness = ( 1.0 - ( tex2DNode1.b * 2.0 ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
-1919;1;1918;1016;1700.516;33.05548;1;True;True
Node;AmplifyShaderEditor.ColorNode;28;-2257.992,920.4818;Float;False;Constant;_TextureCoord;Texture Coord;3;0;Create;True;0;0;False;0;0.4788959,0.9716981,0.3070933,1;0.25,1,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2163.108,691.3479;Float;False;0;18;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;23;-1983.115,899.6699;Float;False;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;20;-1822.234,1030.858;Float;True;1;0;FLOAT;0.3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1784.651,772.0561;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1557.647,292.7504;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,0.5;False;1;FLOAT2;0,0.5;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;19;-1569.224,803.8308;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.45,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-1315.563,263.4507;Float;True;Property;_Tile_mask;Tile_mask;1;0;Create;True;0;0;False;0;20e8d520128ebb0489dc3261cb04843d;20e8d520128ebb0489dc3261cb04843d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;18;-1359.633,792.7849;Float;True;Property;_scrollingtexture;scrolling texture;0;0;Create;True;0;0;False;0;fdb1165b8ba9e6645a3fc5a616bfa241;fdb1165b8ba9e6645a3fc5a616bfa241;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1246.964,520.8415;Float;False;Constant;_AdditiveEmissiveMulti;Additive Emissive Multi;2;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-1056.091,940.1313;Float;False;Property;_ColorEmissive;Color Emissive;3;1;[HDR];Create;True;0;0;False;0;1,1,1,1;28.4094,5.800871,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-969.3181,691.9442;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-862.0054,262.5506;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-793.5419,495.0883;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-604.6918,699.0153;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-919.5053,131.9506;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1497.938,-119.0745;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-569.6825,420.9968;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-617.0057,189.5506;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1056.208,-120.3517;Float;True;Property;_Mainmask;Main mask;2;0;Create;True;0;0;False;0;20e8d520128ebb0489dc3261cb04843d;20e8d520128ebb0489dc3261cb04843d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-387.7207,165.3987;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-490.091,-18.0519;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-254.2556,329.8481;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;35;-376.2531,601.5566;Float;True;Constant;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;7;-266.3909,2.7481;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-336.6909,-220.9519;Float;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;0.1509434,0.1035956,0.05624777,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-268.2795,107.8928;Float;False;Constant;_Metalic;Metalic;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;291.448,101.4198;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MM_scrolling_marquee_01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;28;0
WireConnection;21;0;22;0
WireConnection;21;1;23;0
WireConnection;19;0;21;0
WireConnection;19;1;20;0
WireConnection;4;1;5;0
WireConnection;18;1;19;0
WireConnection;32;0;4;2
WireConnection;32;1;18;0
WireConnection;11;0;4;2
WireConnection;11;1;34;0
WireConnection;16;0;17;0
WireConnection;16;1;34;0
WireConnection;33;0;32;0
WireConnection;33;1;34;0
WireConnection;15;0;16;0
WireConnection;15;1;33;0
WireConnection;10;0;11;0
WireConnection;10;1;9;0
WireConnection;1;1;3;0
WireConnection;12;0;10;0
WireConnection;12;1;15;0
WireConnection;6;0;1;3
WireConnection;13;0;1;1
WireConnection;13;1;12;0
WireConnection;7;0;6;0
WireConnection;0;0;8;0
WireConnection;0;1;35;0
WireConnection;0;2;13;0
WireConnection;0;3;14;0
WireConnection;0;4;7;0
ASEEND*/
//CHKSM=5C0BD2EB661283F787983E586F37C1248AF7E4F9