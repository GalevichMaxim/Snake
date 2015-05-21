Shader "Custom/Snake" {
	Properties {
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex("Texture", 2D) = "white"{}
	}
	SubShader
	{
		Pass
		{
			Tags{"LightMode" = "ForwardBase"}
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			
			uniform float4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			
			uniform float4 _LightColor0;
			
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalDir : TEXCOORD2;
				
			};
			
			vertexOutput vert( vertexInput v )
			{
				vertexOutput o;
				
				o.posWorld = mul(_Object2World, v.vertex);
				o.normalDir = normalize( mul(float4(v.normal, 0.0), _World2Object).xyz);
				o.tex = v.texcoord;
				
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				float3 normalDirection = i.normalDir;
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				float3 lightDirection;
				float atten;
				
				if(_WorldSpaceLightPos0.w == 0.0)
				{
					atten = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					float3 fragmentToLightSource = _WorldSpaceCameraPos.xyz - i.posWorld.xyz;
					float distance = length(fragmentToLightSource);
					atten = 1.0/distance;
					lightDirection = normalize(fragmentToLightSource);
				}
				
				float3 diffuseReflection = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
				
				float3 lightFinal = diffuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;
				
				float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				
				return float4(tex.xyz * lightFinal * _Color.xyz, 1.0);
			}
			
			ENDCG
		}
	}
	FallBack "Diffuse"
}
