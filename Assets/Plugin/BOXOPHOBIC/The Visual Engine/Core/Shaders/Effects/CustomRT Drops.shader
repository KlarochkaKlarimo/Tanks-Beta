// Made with Amplify Shader Editor v1.9.4.4
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Visual Engine/Effects/CustomRT Drops"
{
    Properties
    {
		[StyledBanner(CustomRT Drops)]_BANNER("[ BANNER ]", Float) = 0
		[NoScaleOffset][StyledTextureSingleLine]_DropsTex("Drops Texture", 2D) = "white" {}
		[Space(10)]_RingsSpeedValue("Rings Speed", Range( 0 , 2)) = 1
		_RingsNormalValue("Rings Normal", Range( -8 , 8)) = 1
		[IntRange]_RingsSinMinValue("Rings Sin Min", Range( 1 , 20)) = 1
		[IntRange]_RingsSinMaxValue("Rings Sin Max", Range( 1 , 20)) = 2
		[Space(10)]_DropsSpeedValue("Drops Speed", Range( 0 , 2)) = 1
		_DropsNormalValue("Drops Normal", Range( -8 , 8)) = 1
		[IntRange]_DropsSinMinValue("Drops Sin Min", Range( 1 , 20)) = 2
		[IntRange]_DropsSinMaxValue("Drops Sin Max", Range( 1 , 20)) = 1
		[IntRange]_DropsTillingValue("Drops Tilling", Range( 1 , 10)) = 6
		[Space(10)][StyledTextureSingleLine]_DropsMaskTex("Drops Mask", 2D) = "white" {}
		[Space(10)]_DropsMaskSpeedValue("Drops Mask Speed", Range( 0 , 2)) = 1
		[IntRange]_DropsMaskTillingValue("Drops Mask Tilling", Range( 1 , 10)) = 1

    }

	SubShader
	{
		LOD 0

		
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
        Pass
        {
			Name "Custom RT Update"
            CGPROGRAM
            
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex ASECustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
			#include "UnityShaderVariables.cginc"


			struct ase_appdata_customrendertexture
			{
				uint vertexID : SV_VertexID;
				
			};

			struct ase_v2f_customrendertexture
			{
				float4 vertex           : SV_POSITION;
				float3 localTexcoord    : TEXCOORD0;    // Texcoord local to the update zone (== globalTexcoord if no partial update zone is specified)
				float3 globalTexcoord   : TEXCOORD1;    // Texcoord relative to the complete custom texture
				uint primitiveID        : TEXCOORD2;    // Index of the update zone (correspond to the index in the updateZones of the Custom Texture)
				float3 direction        : TEXCOORD3;    // For cube textures, direction of the pixel being rendered in the cubemap
				
			};

			uniform float _BANNER;
			uniform sampler2D _DropsTex;
			uniform half4 TVE_TimeParams;
			uniform half _RingsSpeedValue;
			uniform half _RingsSinMinValue;
			uniform half _RingsSinMaxValue;
			uniform half _RingsNormalValue;
			uniform half _DropsTillingValue;
			uniform half _DropsSpeedValue;
			uniform half _DropsSinMinValue;
			uniform half _DropsSinMaxValue;
			uniform half _DropsNormalValue;
			uniform sampler2D _DropsMaskTex;
			uniform half _DropsMaskTillingValue;
			uniform half _DropsMaskSpeedValue;


			ase_v2f_customrendertexture ASECustomRenderTextureVertexShader(ase_appdata_customrendertexture IN  )
			{
				ase_v2f_customrendertexture OUT;
				
			#if UNITY_UV_STARTS_AT_TOP
				const float2 vertexPositions[6] =
				{
					{ -1.0f,  1.0f },
					{ -1.0f, -1.0f },
					{  1.0f, -1.0f },
					{  1.0f,  1.0f },
					{ -1.0f,  1.0f },
					{  1.0f, -1.0f }
				};

				const float2 texCoords[6] =
				{
					{ 0.0f, 0.0f },
					{ 0.0f, 1.0f },
					{ 1.0f, 1.0f },
					{ 1.0f, 0.0f },
					{ 0.0f, 0.0f },
					{ 1.0f, 1.0f }
				};
			#else
				const float2 vertexPositions[6] =
				{
					{  1.0f,  1.0f },
					{ -1.0f, -1.0f },
					{ -1.0f,  1.0f },
					{ -1.0f, -1.0f },
					{  1.0f,  1.0f },
					{  1.0f, -1.0f }
				};

				const float2 texCoords[6] =
				{
					{ 1.0f, 1.0f },
					{ 0.0f, 0.0f },
					{ 0.0f, 1.0f },
					{ 0.0f, 0.0f },
					{ 1.0f, 1.0f },
					{ 1.0f, 0.0f }
				};
			#endif

				uint primitiveID = IN.vertexID / 6;
				uint vertexID = IN.vertexID % 6;
				float3 updateZoneCenter = CustomRenderTextureCenters[primitiveID].xyz;
				float3 updateZoneSize = CustomRenderTextureSizesAndRotations[primitiveID].xyz;
				float rotation = CustomRenderTextureSizesAndRotations[primitiveID].w * UNITY_PI / 180.0f;

			#if !UNITY_UV_STARTS_AT_TOP
				rotation = -rotation;
			#endif

				// Normalize rect if needed
				if (CustomRenderTextureUpdateSpace > 0.0) // Pixel space
				{
					// Normalize xy because we need it in clip space.
					updateZoneCenter.xy /= _CustomRenderTextureInfo.xy;
					updateZoneSize.xy /= _CustomRenderTextureInfo.xy;
				}
				else // normalized space
				{
					// Un-normalize depth because we need actual slice index for culling
					updateZoneCenter.z *= _CustomRenderTextureInfo.z;
					updateZoneSize.z *= _CustomRenderTextureInfo.z;
				}

				// Compute rotation

				// Compute quad vertex position
				float2 clipSpaceCenter = updateZoneCenter.xy * 2.0 - 1.0;
				float2 pos = vertexPositions[vertexID] * updateZoneSize.xy;
				pos = CustomRenderTextureRotate2D(pos, rotation);
				pos.x += clipSpaceCenter.x;
			#if UNITY_UV_STARTS_AT_TOP
				pos.y += clipSpaceCenter.y;
			#else
				pos.y -= clipSpaceCenter.y;
			#endif

				// For 3D texture, cull quads outside of the update zone
				// This is neeeded in additional to the preliminary minSlice/maxSlice done on the CPU because update zones can be disjointed.
				// ie: slices [1..5] and [10..15] for two differents zones so we need to cull out slices 0 and [6..9]
				if (CustomRenderTextureIs3D > 0.0)
				{
					int minSlice = (int)(updateZoneCenter.z - updateZoneSize.z * 0.5);
					int maxSlice = minSlice + (int)updateZoneSize.z;
					if (_CustomRenderTexture3DSlice < minSlice || _CustomRenderTexture3DSlice >= maxSlice)
					{
						pos.xy = float2(1000.0, 1000.0); // Vertex outside of ncs
					}
				}

				OUT.vertex = float4(pos, 0.0, 1.0);
				OUT.primitiveID = asuint(CustomRenderTexturePrimitiveIDs[primitiveID]);
				OUT.localTexcoord = float3(texCoords[vertexID], CustomRenderTexture3DTexcoordW);
				OUT.globalTexcoord = float3(pos.xy * 0.5 + 0.5, CustomRenderTexture3DTexcoordW);
			#if UNITY_UV_STARTS_AT_TOP
				OUT.globalTexcoord.y = 1.0 - OUT.globalTexcoord.y;
			#endif
				OUT.direction = CustomRenderTextureComputeCubeDirection(OUT.globalTexcoord.xy);

				return OUT;
			}

            float4 frag(ase_v2f_customrendertexture IN ) : COLOR
            {
				float4 finalColor;
				half2 Rings_CoordA184 = IN.localTexcoord.xy;
				float4 tex2DNode57_g76865 = tex2D( _DropsTex, Rings_CoordA184 );
				half Rain_RippleHeight68_g76865 = (tex2DNode57_g76865).b;
				half Rain_RippleVariation59_g76865 = (tex2DNode57_g76865).a;
				float lerpResult128_g76790 = lerp( _Time.y , ( ( _Time.y * TVE_TimeParams.x ) + TVE_TimeParams.y ) , TVE_TimeParams.w);
				half Rings_Speed190 = ( lerpResult128_g76790 * _RingsSpeedValue );
				half Rain_RippleFrac67_g76865 = frac( ( Rain_RippleVariation59_g76865 + Rings_Speed190 ) );
				half Rain_TimeFrac74_g76865 = ( ( Rain_RippleFrac67_g76865 - 1.0 ) + Rain_RippleHeight68_g76865 );
				float clampResult79_g76865 = clamp( ( Rain_TimeFrac74_g76865 * _RingsSinMinValue ) , 0.0 , _RingsSinMaxValue );
				half Rain_RingsFactor88_g76865 = ( ( Rain_RippleHeight68_g76865 * Rain_RippleHeight68_g76865 * Rain_RippleHeight68_g76865 ) * sin( ( clampResult79_g76865 * UNITY_PI ) ) );
				half2 Rain_RippleNormal87_g76865 = (tex2DNode57_g76865).rg;
				half2 Wetness_Normal102_g76865 = ( Rain_RingsFactor88_g76865 * ( (Rain_RippleNormal87_g76865*2.0 + -1.0) * _RingsNormalValue ) );
				half2 Rings_CoordB186 = ( IN.localTexcoord.xy + float2( 0.6,0.6 ) );
				float4 tex2DNode57_g76867 = tex2D( _DropsTex, Rings_CoordB186 );
				half Rain_RippleHeight68_g76867 = (tex2DNode57_g76867).b;
				half Rain_RippleVariation59_g76867 = (tex2DNode57_g76867).a;
				half Rain_RippleFrac67_g76867 = frac( ( Rain_RippleVariation59_g76867 + ( Rings_Speed190 + 0.4567 ) ) );
				half Rain_TimeFrac74_g76867 = ( ( Rain_RippleFrac67_g76867 - 1.0 ) + Rain_RippleHeight68_g76867 );
				float clampResult79_g76867 = clamp( ( Rain_TimeFrac74_g76867 * _RingsSinMinValue ) , 0.0 , _RingsSinMaxValue );
				half Rain_RingsFactor88_g76867 = ( ( Rain_RippleHeight68_g76867 * Rain_RippleHeight68_g76867 * Rain_RippleHeight68_g76867 ) * sin( ( clampResult79_g76867 * UNITY_PI ) ) );
				half2 Rain_RippleNormal87_g76867 = (tex2DNode57_g76867).rg;
				half2 Wetness_Normal102_g76867 = ( Rain_RingsFactor88_g76867 * ( (Rain_RippleNormal87_g76867*2.0 + -1.0) * _RingsNormalValue ) );
				half2 Rings_Final180 = (( Wetness_Normal102_g76865 + Wetness_Normal102_g76867 )*0.5 + 0.5);
				float4 tex2DNode57_g76869 = tex2D( _DropsTex, ( IN.localTexcoord.xy * _DropsTillingValue ) );
				half Rain_RippleHeight68_g76869 = (tex2DNode57_g76869).b;
				half Rain_RippleVariation59_g76869 = (tex2DNode57_g76869).a;
				float lerpResult128_g76827 = lerp( _Time.y , ( ( _Time.y * TVE_TimeParams.x ) + TVE_TimeParams.y ) , TVE_TimeParams.w);
				half Rain_RippleFrac67_g76869 = frac( ( Rain_RippleVariation59_g76869 + ( lerpResult128_g76827 * _DropsSpeedValue ) ) );
				half Rain_TimeFrac74_g76869 = ( ( Rain_RippleFrac67_g76869 - 1.0 ) + Rain_RippleHeight68_g76869 );
				float clampResult79_g76869 = clamp( ( Rain_TimeFrac74_g76869 * _DropsSinMinValue ) , 0.0 , _DropsSinMaxValue );
				half Rain_RingsFactor88_g76869 = ( ( Rain_RippleHeight68_g76869 * Rain_RippleHeight68_g76869 * Rain_RippleHeight68_g76869 ) * sin( ( clampResult79_g76869 * UNITY_PI ) ) );
				half2 Rain_RippleNormal87_g76869 = (tex2DNode57_g76869).rg;
				half2 Wetness_Normal102_g76869 = ( Rain_RingsFactor88_g76869 * ( (Rain_RippleNormal87_g76869*2.0 + -1.0) * _DropsNormalValue ) );
				float lerpResult128_g76871 = lerp( _Time.y , ( ( _Time.y * TVE_TimeParams.x ) + TVE_TimeParams.y ) , TVE_TimeParams.w);
				half2 Drops_Final205 = (( Wetness_Normal102_g76869 * tex2D( _DropsMaskTex, ( ( IN.localTexcoord.xy * _DropsMaskTillingValue ) + ( lerpResult128_g76871 * _DropsMaskSpeedValue ) ) ).r )*0.5 + 0.5);
				float4 appendResult139 = (float4(Rings_Final180 , Drops_Final205));
				
                finalColor = appendResult139;
				return finalColor;
            }
            ENDCG
		}
    }
	
	CustomEditor "TVEShaderGUIHelper"
	Fallback Off
}
/*ASEBEGIN
Version=19404
Node;AmplifyShaderEditor.RangedFloatNode;169;-512,960;Half;False;Property;_RingsSpeedValue;Rings Speed;3;0;Create;False;0;0;0;False;1;Space(10);False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;146;-512,896;Inherit;False;Get Global Time;-1;;76790;2b2f842f8071fb945821b595284b5848;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;185;-512,768;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-192,896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;187;-192,768;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0.6,0.6;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;190;64,896;Half;False;Rings_Speed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;106;-512,640;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;104;-512,384;Inherit;True;Property;_DropsTex;Drops Texture;2;1;[NoScaleOffset];Create;False;0;0;0;False;1;StyledTextureSingleLine;False;None;226a76398d819eb40b921c0023fa0af2;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexCoordVertexDataNode;235;-512,3200;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;238;-512,3328;Half;False;Property;_DropsMaskTillingValue;Drops Mask Tilling;14;1;[IntRange];Create;False;0;0;0;False;0;False;1;5;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;240;-512,3456;Inherit;False;Get Global Time;-1;;76871;2b2f842f8071fb945821b595284b5848;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;241;-512,3520;Half;False;Property;_DropsMaskSpeedValue;Drops Mask Speed;13;0;Create;False;0;0;0;False;1;Space(10);False;1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;199;-512,2304;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;162;-512,2432;Half;False;Property;_DropsTillingValue;Drops Tilling;11;1;[IntRange];Create;False;0;0;0;False;0;False;6;5;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;163;64,384;Half;False;DropsTex;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;193;-512,1664;Inherit;False;190;Rings_Speed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;64,640;Half;False;Rings_CoordA;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;186;64,768;Half;False;Rings_CoordB;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;202;-512,2560;Inherit;False;Get Global Time;-1;;76827;2b2f842f8071fb945821b595284b5848;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;-512,2624;Half;False;Property;_DropsSpeedValue;Drops Speed;7;0;Create;False;0;0;0;False;1;Space(10);False;1;0.7;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;243;-192,3200;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;-192,3456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;201;-192,2304;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;-512,1536;Inherit;False;163;DropsTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-512,1408;Inherit;False;190;Rings_Speed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;-512,1600;Inherit;False;186;Rings_CoordB;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;167;-512,1792;Half;False;Property;_RingsSinMinValue;Rings Sin Min;5;1;[IntRange];Create;False;0;0;0;False;0;False;1;10;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;168;-512,1856;Half;False;Property;_RingsSinMaxValue;Rings Sin Max;6;1;[IntRange];Create;False;0;0;0;False;0;False;2;2;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-512,1920;Half;False;Property;_RingsNormalValue;Rings Normal;4;0;Create;False;0;0;0;False;0;False;1;0.5;-8;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;-512,1344;Inherit;False;184;Rings_CoordA;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;164;-512,1280;Inherit;False;163;DropsTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;194;-256,1664;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.4567;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;-512,2176;Inherit;False;163;DropsTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-512,2816;Half;False;Property;_DropsSinMinValue;Drops Sin Min;9;1;[IntRange];Create;False;0;0;0;False;0;False;2;2;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-512,2880;Half;False;Property;_DropsSinMaxValue;Drops Sin Max;10;1;[IntRange];Create;False;0;0;0;False;0;False;1;1;1;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-512,2944;Half;False;Property;_DropsNormalValue;Drops Normal;8;0;Create;False;0;0;0;False;0;False;1;1;-8;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-93.79144,2686.88;Half;False;Property;_DropsIntensityValue;Drops Intensity;1;0;Create;False;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;-192,2560;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;245;0,3200;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;230;256,1280;Inherit;False;Compute Rain Drops;-1;;76865;ab995bbff019b914ea1ee54bf23794b6;0;7;113;SAMPLER2D;0;False;114;FLOAT2;0,0;False;121;FLOAT;0;False;132;FLOAT;0;False;123;FLOAT;0;False;124;FLOAT;0;False;125;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;231;256,1536;Inherit;False;Compute Rain Drops;-1;;76867;ab995bbff019b914ea1ee54bf23794b6;0;7;113;SAMPLER2D;0;False;114;FLOAT2;0,0;False;121;FLOAT;0;False;132;FLOAT;0;False;123;FLOAT;0;False;124;FLOAT;0;False;125;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;232;256,2176;Inherit;False;Compute Rain Drops;-1;;76869;ab995bbff019b914ea1ee54bf23794b6;0;7;113;SAMPLER2D;0;False;114;FLOAT2;0,0;False;121;FLOAT;0;False;132;FLOAT;0;False;123;FLOAT;0;False;124;FLOAT;0;False;125;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;234;256,3200;Inherit;True;Property;_DropsMaskTex;Drops Mask;12;0;Create;False;0;0;0;False;2;Space(10);StyledTextureSingleLine;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;135;640,1280;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;233;640,2176;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;138;832,1280;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;204;896,2176;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;180;1088,1280;Half;False;Rings_Final;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;1344,2176;Half;False;Drops_Final;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;196;1792,1280;Inherit;False;180;Rings_Final;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;206;1792,1344;Inherit;False;205;Drops_Final;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;108;-512,256;Inherit;False;Property;_BANNER;[ BANNER ];0;0;Create;True;0;0;0;True;1;StyledBanner(CustomRT Drops);False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;139;2048,1280;Inherit;False;FLOAT4;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT2;0,0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2304,1280;Float;False;True;-1;2;TVEShaderGUIHelper;0;2;BOXOPHOBIC/The Visual Engine/Effects/CustomRT Drops;32120270d1b3a8746af2aca8bc749736;True;Custom RT Update;0;0;Custom RT Update;1;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;171;0;146;0
WireConnection;171;1;169;0
WireConnection;187;0;185;0
WireConnection;190;0;171;0
WireConnection;163;0;104;0
WireConnection;184;0;106;0
WireConnection;186;0;187;0
WireConnection;243;0;235;0
WireConnection;243;1;238;0
WireConnection;242;0;240;0
WireConnection;242;1;241;0
WireConnection;201;0;199;0
WireConnection;201;1;162;0
WireConnection;194;0;193;0
WireConnection;203;0;202;0
WireConnection;203;1;161;0
WireConnection;245;0;243;0
WireConnection;245;1;242;0
WireConnection;230;113;164;0
WireConnection;230;114;188;0
WireConnection;230;121;192;0
WireConnection;230;123;167;0
WireConnection;230;124;168;0
WireConnection;230;125;179;0
WireConnection;231;113;191;0
WireConnection;231;114;195;0
WireConnection;231;121;194;0
WireConnection;231;123;167;0
WireConnection;231;124;168;0
WireConnection;231;125;179;0
WireConnection;232;113;198;0
WireConnection;232;114;201;0
WireConnection;232;121;203;0
WireConnection;232;132;220;0
WireConnection;232;123;158;0
WireConnection;232;124;159;0
WireConnection;232;125;160;0
WireConnection;234;1;245;0
WireConnection;135;0;230;0
WireConnection;135;1;231;0
WireConnection;233;0;232;0
WireConnection;233;1;234;1
WireConnection;138;0;135;0
WireConnection;204;0;233;0
WireConnection;180;0;138;0
WireConnection;205;0;204;0
WireConnection;139;0;196;0
WireConnection;139;2;206;0
WireConnection;0;0;139;0
ASEEND*/
//CHKSM=3A7E5C7BA3755D0B32FE4C040FECC57C27D07DC0