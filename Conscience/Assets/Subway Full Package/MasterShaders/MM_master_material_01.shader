// Upgrade NOTE: upgraded instancing buffer 'MM_master_material_01' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MM_master_material_01"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_AlbedoColorTint("Albedo Color Tint", Color) = (1,1,1,1)
		[Toggle]_AlbedoMap("Albedo Map ?", Float) = 1
		_AlbedoColorVertexTint("Albedo Color Vertex Tint", Color) = (1,1,1,1)
		[Toggle]_UseVertexPaintedTint("Use Vertex Painted Tint ?", Float) = 0
		[Toggle]_AmbientOcclusion("Ambient Occlusion ?", Float) = 1
		[Toggle]_CustomEmissiveTexture("Custom Emissive Texture ?", Float) = 0
		[HDR]_EmissiveColorMulti("Emissive Color Multi", Color) = (1,1,1,1)
		[Toggle]_Emissive_("Emissive_ ?", Float) = 0
		_Emissive("Emissive", 2D) = "white" {}
		[HDR]_RMA("RMA", 2D) = "white" {}
		_RMATextureCoord("RMA Texture Coord", Range( 0 , 1)) = 0
		[Toggle]_MetalnessMap("Metalness Map ?", Float) = 1
		_MetalnessValue("Metalness Value", Range( 0 , 1)) = 0
		_MetalnessValueMulti("Metalness Value Multi", Range( 0 , 1)) = 1
		[Normal]_Normal("Normal", 2D) = "bump" {}
		[Toggle]_NormalMap("Normal Map ?", Float) = 1
		_NormalMulti("Normal Multi", Vector) = (1,1,1,1)
		[Toggle]_NoOpacity("No Opacity ?", Float) = 1
		[Toggle]_OpacityinAlbedoAlpha("Opacity in Albedo Alpha ?", Float) = 0
		_OpacityMap("Opacity Map", 2D) = "white" {}
		[Toggle]_TextureCoordMicro("Texture Coord Micro", Float) = 1
		_MicroRoughnessMulti("Micro Roughness Multi", Range( 0 , 10)) = 5
		_MicroRoughnessPower("Micro Roughness Power", Range( 0 , 5)) = 1
		_RoughnessmicroDetail("Roughness micro Detail", 2D) = "white" {}
		Tiling_micro_rough("Tiling_micro_rough", Range( 0 , 5)) = 1
		[Toggle]_MicroRoughnessDetail("Micro RoughnessDetail  ?", Float) = 1
		[Toggle]_RoughnessMap("Roughness Map ?", Float) = 1
		_RoughnessMulti("Roughness Multi", Range( 0 , 2)) = 1
		_RougnhnessPower("Rougnhness Power", Range( 0 , 2)) = 0.1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _NormalMap;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _NormalMulti;
		uniform float _AlbedoMap;
		uniform float4 _AlbedoColorTint;
		uniform float _UseVertexPaintedTint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _AlbedoColorVertexTint;
		uniform float _Emissive_;
		uniform float _CustomEmissiveTexture;
		uniform sampler2D _RMA;
		uniform float _RMATextureCoord;
		uniform sampler2D _Emissive;
		uniform float4 _Emissive_ST;
		uniform float4 _EmissiveColorMulti;
		uniform float _MetalnessMap;
		uniform float _MetalnessValue;
		uniform float _MetalnessValueMulti;
		uniform float _MicroRoughnessDetail;
		uniform float _RoughnessMap;
		uniform float _RougnhnessPower;
		uniform float _RoughnessMulti;
		uniform sampler2D _RoughnessmicroDetail;
		uniform float Tiling_micro_rough;
		uniform float _TextureCoordMicro;
		uniform float _AmbientOcclusion;
		uniform float _NoOpacity;
		uniform float _OpacityinAlbedoAlpha;
		uniform sampler2D _OpacityMap;
		uniform float4 _OpacityMap_ST;

		UNITY_INSTANCING_BUFFER_START(MM_master_material_01)
			UNITY_DEFINE_INSTANCED_PROP(float, _MicroRoughnessMulti)
#define _MicroRoughnessMulti_arr MM_master_material_01
			UNITY_DEFINE_INSTANCED_PROP(float, _MicroRoughnessPower)
#define _MicroRoughnessPower_arr MM_master_material_01
		UNITY_INSTANCING_BUFFER_END(MM_master_material_01)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = lerp(float4( float3(0,0,1) , 0.0 ),( float4( UnpackNormal( tex2D( _Normal, uv_Normal ) ) , 0.0 ) * _NormalMulti ),_NormalMap).xyz;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode45 = tex2D( _Albedo, uv_Albedo );
			float4 temp_output_47_0 = ( tex2DNode45 * _AlbedoColorTint );
			float4 lerpResult50 = lerp( ( tex2DNode45 * _AlbedoColorVertexTint ) , temp_output_47_0 , i.vertexColor.g);
			o.Albedo = lerp(_AlbedoColorTint,lerp(temp_output_47_0,lerpResult50,_UseVertexPaintedTint),_AlbedoMap).rgb;
			float4 temp_cast_4 = (0.0).xxxx;
			float2 temp_output_3_0 = ( i.uv_texcoord * _RMATextureCoord );
			float4 tex2DNode4 = tex2D( _RMA, temp_output_3_0 );
			float4 temp_cast_5 = (tex2DNode4.a).xxxx;
			float2 uv_Emissive = i.uv_texcoord * _Emissive_ST.xy + _Emissive_ST.zw;
			o.Emission = lerp(temp_cast_4,( lerp(temp_cast_5,tex2D( _Emissive, uv_Emissive ),_CustomEmissiveTexture) * _EmissiveColorMulti ),_Emissive_).rgb;
			o.Metallic = lerp(_MetalnessValue,( tex2DNode4.g * _MetalnessValueMulti ),_MetalnessMap);
			float temp_output_37_0 = ( pow( lerp(0.1,tex2DNode4.r,_RoughnessMap) , _RougnhnessPower ) * _RoughnessMulti );
			float2 temp_cast_7 = (Tiling_micro_rough).xx;
			float2 uv_TexCoord2_g11 = i.uv_texcoord * temp_cast_7;
			float _MicroRoughnessPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_MicroRoughnessPower_arr, _MicroRoughnessPower);
			float _MicroRoughnessMulti_Instance = UNITY_ACCESS_INSTANCED_PROP(_MicroRoughnessMulti_arr, _MicroRoughnessMulti);
			float clampResult14_g11 = clamp( ( pow( (tex2D( _RoughnessmicroDetail, ( uv_TexCoord2_g11 * _TextureCoordMicro ) )).r , _MicroRoughnessPower_Instance ) * _MicroRoughnessMulti_Instance ) , 0.0 , 1.0 );
			float blendOpSrc30 = temp_output_37_0;
			float blendOpDest30 = clampResult14_g11;
			float clampResult39 = clamp( lerp(temp_output_37_0,( saturate( ( blendOpSrc30 + blendOpDest30 ) )),_MicroRoughnessDetail) , 0.0 , 1.0 );
			o.Smoothness = ( 1.0 - clampResult39 );
			o.Occlusion = lerp(1.0,tex2DNode4.b,_AmbientOcclusion);
			float4 temp_cast_8 = (tex2DNode45.a).xxxx;
			float2 uv_OpacityMap = i.uv_texcoord * _OpacityMap_ST.xy + _OpacityMap_ST.zw;
			float4 temp_cast_9 = (1.0).xxxx;
			o.Alpha = lerp(lerp(temp_cast_8,tex2D( _OpacityMap, uv_OpacityMap ),_OpacityinAlbedoAlpha),temp_cast_9,_NoOpacity).r;
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				half4 color : COLOR0;
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
				o.color = v.color;
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
				surfIN.vertexColor = IN.color;
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
0;25;1906;1001;547.1237;1580.685;1.246376;True;True
Node;AmplifyShaderEditor.CommentaryNode;8;-2118.575,238.7855;Float;False;570;320;Texture Coords;3;3;1;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-2068.574,288.7855;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-1991.575,443.7855;Float;False;Property;_RMATextureCoord;RMA Texture Coord;12;0;Create;True;0;0;False;0;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-1717.575,358.7855;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;9;-1891.087,-509.7554;Float;False;968.9343;379.9145;Bump Offset Using RMA Alpha;4;4;7;6;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;41;212.7119,-912.9191;Float;False;1461.548;453;Roughness;9;32;34;31;33;35;37;30;38;39;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;4;-1864.614,-352.8391;Float;True;Property;_RMA;RMA;11;1;[HDR];Create;True;0;0;False;0;None;86a079dad844f9b489daf908c4ec019c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;32;255.563,-582.9192;Float;False;Constant;_Roughness;Roughness;1;0;Create;True;0;0;False;0;0.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;31;418.6091,-670.1998;Float;False;Property;_RoughnessMap;Roughness Map ?;30;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;470.2601,-550.9192;Float;False;Property;_RougnhnessPower;Rougnhness Power;32;0;Create;True;0;0;False;0;0.1;0.67;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;55;-406.8452,-1676.677;Float;False;971.6406;583.6095;Albedo;3;45;46;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PowerNode;33;778.2601,-660.9192;Float;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;825.8602,-556.3192;Float;False;Property;_RoughnessMulti;Roughness Multi;31;0;Create;True;0;0;False;0;1;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;54;-398.7244,-2381.853;Float;False;1130.152;500.6549;Vertex Paint Tint Color;5;50;51;53;49;52;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;45;-356.8452,-1626.677;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;1d4639ebdad45fc4abff38b407b81273;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;53;-348.7244,-2331.853;Float;False;Property;_AlbedoColorVertexTint;Albedo Color Vertex Tint;4;0;Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;89;245.7119,-859.0782;Float;True;MF_micro_roughness;23;;11;421390835b905fd4ab674f40634f8c31;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-1318.728,408.6544;Float;False;1823.458;540.1173;Emissive;9;13;11;12;16;15;14;17;10;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1098.26,-639.9193;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;46;-295.9524,-1300.067;Float;False;Property;_AlbedoColorTint;Albedo Color Tint;1;0;Create;True;0;0;False;0;1,1,1,1;0.7169812,0.7169812,0.7169812,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;77.1503,-2294.491;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;24;-46.34643,-120.4647;Float;False;656.4701;336.4393;Metalness;4;20;22;23;21;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;43.57379,-1600.843;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;52;-338.8267,-2090.154;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;30;866.2048,-898.0056;Float;True;LinearDodge;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-582.7998,486.6544;Float;True;Property;_Emissive;Emissive;10;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;63;885.6329,691.6213;Float;False;1044.359;622.873;Normal;5;57;56;62;59;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;64;783.2623,-1520.512;Float;False;683.6403;369.9161;Opacity;2;43;44;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;27;1549.174,108.4834;Float;False;486.5009;239.1205;Ambient Occlusion;2;26;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;56;935.6329,741.6213;Float;True;Property;_Normal;Normal;16;1;[Normal];Create;True;0;0;False;0;None;252db3b823a16fc45892f39a258caeae;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;250.6513,119.4036;Float;False;Property;_MetalnessValueMulti;Metalness Value Multi;15;0;Create;True;0;0;False;0;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;38;1233.726,-883.4218;Float;True;Property;_MicroRoughnessDetail;Micro RoughnessDetail  ?;29;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;43;823.3965,-1470.512;Float;True;Property;_OpacityMap;Opacity Map;22;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;59;975.6832,978.6218;Float;True;Property;_NormalMulti;Normal Multi;18;0;Create;True;0;0;False;0;1,1,1,1;0.5,0.5,0.5,0.5;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;14;-112.4957,456.3521;Float;False;Property;_CustomEmissiveTexture;Custom Emissive Texture ?;7;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;50;278.8809,-2265.849;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;15;-481.5847,741.7709;Float;False;Property;_EmissiveColorMulti;Emissive Color Multi;8;1;[HDR];Create;True;0;0;False;0;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;1449.638,844.0629;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;165.7496,-82.54523;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;1599.174,232.604;Float;False;Constant;_1;1;1;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;39;1536.814,-836.624;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;183.8344,533.4216;Float;False;Constant;_0;0;1;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-126.6845,780.6709;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;49;421.4275,-2019.198;Float;False;Property;_UseVertexPaintedTint;Use Vertex Painted Tint ?;5;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;66;1262.413,-1124.927;Float;False;Constant;_Float0;Float 0;28;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-47.01813,117.0178;Float;False;Property;_MetalnessValue;Metalness Value;14;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;61;1561.828,987.4943;Float;True;Constant;_Vector0;Vector 0;18;0;Create;True;0;0;False;0;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ToggleSwitchNode;44;1140.903,-1294.596;Float;False;Property;_OpacityinAlbedoAlpha;Opacity in Albedo Alpha ?;21;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1287.397,578.5214;Float;False;Constant;_EmissiveHeight;Emissive Height;1;0;Create;True;0;0;False;0;0.05;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;6;-1420.076,-245.25;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;48;744.4847,-1636.484;Float;False;Property;_AlbedoMap;Albedo Map ?;2;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;62;1732.613,826.7824;Float;False;Property;_NormalMap;Normal Map ?;17;0;Create;True;0;0;False;0;1;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ToggleSwitchNode;12;-847.5044,496.0706;Float;False;Property;_UseBumpOffset;Use Bump Offset ?;33;0;Create;True;0;0;False;0;0;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1831.674,-461.7348;Float;False;Constant;_heightratiooffset;height ratio offset;1;0;Create;True;0;0;False;0;0.05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;25;1807.675,158.4834;Float;False;Property;_AmbientOcclusion;Ambient Occlusion ?;6;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;76;1722.244,-793.2336;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;65;1532.413,-1295.927;Float;False;Property;_NoOpacity;No Opacity ?;20;0;Create;True;0;0;False;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;21;414.4104,-69.45918;Float;False;Property;_MetalnessMap;Metalness Map ?;13;0;Create;True;0;0;False;0;1;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;11;-963.5439,646.473;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;7;-1140.096,-275.8287;Float;True;Property;_RMAAlphabumpOffset;RMA Alpha bump Offset ?;19;0;Create;True;0;0;False;0;0;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;17;418.9953,593.6027;Float;False;Property;_Emissive_;Emissive_ ?;9;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2562.889,-581.0762;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MM_master_material_01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;4;1;3;0
WireConnection;31;0;32;0
WireConnection;31;1;4;1
WireConnection;33;0;31;0
WireConnection;33;1;34;0
WireConnection;37;0;33;0
WireConnection;37;1;35;0
WireConnection;51;0;45;0
WireConnection;51;1;53;0
WireConnection;47;0;45;0
WireConnection;47;1;46;0
WireConnection;30;0;37;0
WireConnection;30;1;89;0
WireConnection;38;0;37;0
WireConnection;38;1;30;0
WireConnection;14;0;4;4
WireConnection;14;1;13;0
WireConnection;50;0;51;0
WireConnection;50;1;47;0
WireConnection;50;2;52;2
WireConnection;57;0;56;0
WireConnection;57;1;59;0
WireConnection;20;0;4;2
WireConnection;20;1;23;0
WireConnection;39;0;38;0
WireConnection;16;0;14;0
WireConnection;16;1;15;0
WireConnection;49;0;47;0
WireConnection;49;1;50;0
WireConnection;44;0;45;4
WireConnection;44;1;43;0
WireConnection;6;0;4;4
WireConnection;6;1;3;0
WireConnection;6;2;5;0
WireConnection;48;0;46;0
WireConnection;48;1;49;0
WireConnection;62;0;61;0
WireConnection;62;1;57;0
WireConnection;12;0;7;0
WireConnection;12;1;11;0
WireConnection;25;0;26;0
WireConnection;25;1;4;3
WireConnection;76;0;39;0
WireConnection;65;0;44;0
WireConnection;65;1;66;0
WireConnection;21;0;22;0
WireConnection;21;1;20;0
WireConnection;11;0;7;0
WireConnection;11;1;10;0
WireConnection;7;0;3;0
WireConnection;7;1;6;0
WireConnection;17;0;18;0
WireConnection;17;1;16;0
WireConnection;0;0;48;0
WireConnection;0;1;62;0
WireConnection;0;2;17;0
WireConnection;0;3;21;0
WireConnection;0;4;76;0
WireConnection;0;5;25;0
WireConnection;0;9;65;0
ASEEND*/
//CHKSM=F9041EC3F7F9DB5C3970DAE3A8FE493929D6CE43