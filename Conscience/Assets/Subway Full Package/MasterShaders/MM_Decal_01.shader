// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MM_Decal_01"
{
	Properties
	{
		_ScratchesopacityPower("Scratches opacity Power", Range( 0 , 1)) = 1
		_TilingU("TilingU", Float) = 1
		_TilingV("TilingV", Float) = 1
		_AlbedoTexture("Albedo Texture", 2D) = "white" {}
		_OpacityMulti("Opacity Multi", Range( 0 , 2)) = 1
		_RoughMultiply("RoughMultiply", Range( 0 , 1)) = 0.35
		[Toggle]_UseAlbedoRGBforAlpha("Use Albedo RGB for Alpha?", Float) = 1
		_ColorTint("Color Tint", Color) = (1,1,1,1)
		_microdetail("microdetail", 2D) = "white" {}
		[Toggle]_Opacity("Opacity ?", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _ColorTint;
		uniform sampler2D _microdetail;
		uniform float _TilingU;
		uniform float _TilingV;
		uniform sampler2D _AlbedoTexture;
		uniform float4 _AlbedoTexture_ST;
		uniform float _RoughMultiply;
		uniform float _ScratchesopacityPower;
		uniform float _Opacity;
		uniform float _UseAlbedoRGBforAlpha;
		uniform float _OpacityMulti;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_TexCoord11 = i.uv_texcoord * float2( 0.5,0 );
			float2 uv_TexCoord14 = i.uv_texcoord * float2( 0,0.5 );
			float4 tex2DNode16 = tex2D( _microdetail, ( ( _TilingU * uv_TexCoord11 ) + ( _TilingV * uv_TexCoord14 ) ) );
			float temp_output_2_0 = ( 1.0 * tex2DNode16.g );
			float2 uv_AlbedoTexture = i.uv_texcoord * _AlbedoTexture_ST.xy + _AlbedoTexture_ST.zw;
			float4 tex2DNode3 = tex2D( _AlbedoTexture, uv_AlbedoTexture );
			o.Albedo = ( _ColorTint * ( temp_output_2_0 * tex2DNode3 ) ).rgb;
			o.Metallic = 0.0;
			o.Smoothness = ( 1.0 - ( ( 1.0 - tex2DNode16.g ) + _RoughMultiply ) );
			float4 temp_cast_1 = (( 1.0 - pow( tex2DNode16.g , _ScratchesopacityPower ) )).xxxx;
			float4 temp_cast_2 = (1.0).xxxx;
			float4 temp_cast_3 = (tex2DNode3.a).xxxx;
			float4 blendOpSrc28 = temp_cast_1;
			float4 blendOpDest28 = lerp(temp_cast_2,lerp(temp_cast_3,( tex2DNode3 * temp_output_2_0 ),_UseAlbedoRGBforAlpha),_Opacity);
			o.Alpha = ( ( saturate( ( blendOpDest28 - blendOpSrc28 ) )) * _OpacityMulti ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
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
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
1;1;1918;1016;1959.344;-159.3466;1.145414;True;True
Node;AmplifyShaderEditor.RangedFloatNode;13;-1599.338,842.5514;Float;False;Property;_TilingV;TilingV;2;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-1640.923,1024.434;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-1602.463,603.2325;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-1560.878,421.3497;Float;False;Property;_TilingU;TilingU;1;0;Create;True;0;0;False;0;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1297.463,442.2325;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1335.923,863.4342;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-1054.463,621.0325;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-865.1898,-201.6318;Float;False;Constant;_AlbedoScratchesValue;Albedo Scratches Value;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-839.0829,609.8915;Float;True;Property;_microdetail;microdetail;7;0;Create;True;0;0;False;0;a3f19706d0f5d9e48a79b72909f72cb3;a3f19706d0f5d9e48a79b72909f72cb3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-876.6787,63.90091;Float;True;Property;_AlbedoTexture;Albedo Texture;3;0;Create;True;0;0;True;0;None;7750ce1a482df634b883dcb676d43034;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-570.1638,-191.6438;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-811.2642,999.3957;Float;False;Property;_ScratchesopacityPower;Scratches opacity Power;0;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-315.5746,77.25341;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;5;-76.22087,108.9722;Float;True;Property;_UseAlbedoRGBforAlpha;Use Albedo RGB for Alpha?;5;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-349.5632,1123.712;Float;True;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;23;-409.1776,928.6165;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-391.8516,776.2126;Float;False;Property;_RoughMultiply;RoughMultiply;4;0;Create;True;0;0;False;0;0.35;0.35;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-80.88184,1213.768;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;26;-150.5759,860.0482;Float;True;Property;_Opacity;Opacity ?;8;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;17;-330.4531,536.6181;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-290.7341,-339.7055;Float;False;Property;_ColorTint;Color Tint;6;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;148.9157,1372.146;Float;False;Property;_OpacityMulti;Opacity Multi;3;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-265.8337,-106.85;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-47.82371,611.7053;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;28;175.4368,1035.712;Float;True;Subtract;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;22;360.9338,482.9621;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;538.8141,1018.026;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;381.0065,330.8093;Float;False;Constant;_Metalic;Metalic;0;0;Create;True;0;0;False;0;0;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;21;293.3344,763.762;Float;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;66.26591,-102.2055;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;634.0433,274.6407;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MM_Decal_01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;15;0;13;0
WireConnection;15;1;14;0
WireConnection;12;0;10;0
WireConnection;12;1;15;0
WireConnection;16;1;12;0
WireConnection;2;0;1;0
WireConnection;2;1;16;2
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;5;0;3;4
WireConnection;5;1;4;0
WireConnection;23;0;16;2
WireConnection;23;1;24;0
WireConnection;29;0;23;0
WireConnection;26;0;27;0
WireConnection;26;1;5;0
WireConnection;17;0;16;2
WireConnection;6;0;2;0
WireConnection;6;1;3;0
WireConnection;18;0;17;0
WireConnection;18;1;19;0
WireConnection;28;0;29;0
WireConnection;28;1;26;0
WireConnection;22;0;18;0
WireConnection;30;0;28;0
WireConnection;30;1;31;0
WireConnection;7;0;8;0
WireConnection;7;1;6;0
WireConnection;0;0;7;0
WireConnection;0;1;21;0
WireConnection;0;3;20;0
WireConnection;0;4;22;0
WireConnection;0;9;30;0
ASEEND*/
//CHKSM=E2E7681ACCE7F6BCD8D1BB86A1F5BB25A28C88BE