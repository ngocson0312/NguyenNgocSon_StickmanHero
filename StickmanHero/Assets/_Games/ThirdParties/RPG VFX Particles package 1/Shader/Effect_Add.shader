Shader "AppsTools/FastShader/Effect/Add" {
	Properties{
		_Main("Main", 2D) = "white" {}
		[HDR]_Main_Color("Main_Color", Color) = (0.5,0.5,0.5,1)
		_Rongjie("Rongjie", 2D) = "white" {}
		_souf("souf", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		_Mask_u("Mask_u", Float) = 0
		_Mask_v("Mask_v", Float) = 0
		_Main_u("Main_u", Float) = 0
		_Main_v("Main_v", Float) = 0
		_Rongjie_u("Rongjie_u", Float) = 0
		_Rongjie_v("Rongjie_v", Float) = 0
		[MaterialToggle] _UV_open("UV_open", Float) = 0
	}
		SubShader{
			Tags {
				"IgnoreProjector" = "True"
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}
			LOD 100
			Pass {
				Name "FORWARD"
				Tags {
					"LightMode" = "ForwardBase"
				}
				Blend One One
				Cull Off
				ZWrite Off

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#pragma fragmentoption ARB_precision_hint_fastest

				#pragma target 3.0
				uniform sampler2D _Main; uniform float4 _Main_ST;
				uniform fixed4 _Main_Color;
				uniform sampler2D _Rongjie; uniform float4 _Rongjie_ST;
				uniform fixed _souf;
				uniform sampler2D _Mask; uniform float4 _Mask_ST;
				uniform fixed _Mask_u;
				uniform fixed _Mask_v;
				uniform fixed _Main_u;
				uniform fixed _Main_v;
				uniform fixed _Rongjie_u;
				uniform fixed _Rongjie_v;
				uniform fixed _UV_open;
				struct VertexInput {
					fixed4 vertex : POSITION;
					fixed2 texcoord0 : TEXCOORD0;
					fixed4 texcoord1 : TEXCOORD1;
					fixed4 vertexColor : COLOR;
				};
				struct VertexOutput {
					fixed4 pos : SV_POSITION;
					fixed2 uv0 : TEXCOORD0;
					fixed4 uv1 : TEXCOORD1;
					fixed4 vertexColor : COLOR;
				};

				VertexOutput vert(VertexInput v)
				{
					VertexOutput o = (VertexOutput)0;
					o.uv0 = v.texcoord0;
					o.uv1 = v.texcoord1;
					o.vertexColor = v.vertexColor;
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}
				float4 frag(VertexOutput i, float facing : VFACE) : COLOR
				{
					fixed2 _UV_open_var = lerp((i.uv0 + (_Time.g*fixed2(_Main_u,_Main_v))), (i.uv0 + fixed2(i.uv1.b,i.uv1.a)), _UV_open);
					fixed4 _Main_var = tex2D(_Main,TRANSFORM_TEX(_UV_open_var, _Main));
					fixed4 _Rongjie_var = tex2D(_Rongjie,TRANSFORM_TEX((i.uv0 + (_Time.g*fixed2(_Rongjie_u, _Rongjie_v))), _Rongjie));
					fixed4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX((i.uv0 + (_Time.g*fixed2(_Mask_u, _Mask_v))), _Mask));
					fixed3 emissive = ((_Main_Color.rgb*_Main_var.rgb*i.vertexColor.rgb*saturate(((_Rongjie_var.r*_souf) - lerp(_souf,(-1.5),i.uv1.r)))*_Mask_var.rgb)*i.vertexColor.a*_Main_var.a*_Mask_var.a);
					return fixed4(emissive, 1);
				}
				ENDCG
			}
		}
		FallBack "Legacy Shaders/VertexLit"
}
