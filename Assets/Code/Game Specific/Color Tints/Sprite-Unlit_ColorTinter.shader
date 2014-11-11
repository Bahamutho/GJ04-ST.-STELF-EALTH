Shader "Custom/Sprite-Unlit_ColorTinter" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_ColorTintTex ("ColorTintLookupTex", 2D) = "white" {}
		_SpriteColorToIndex ("RColorToIndex", 2D) = "white" {}
		_TintTexWidth("ColorTintLookupWidth",int) = 0
	}
	SubShader 
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha
		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON // WHADISDIS
			#include "UnityCG.cginc" // WHADISDIS
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR; // Vertex color is index
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float4 color    : COLOR; // Vertex color is index
				half2 texcoord  : TEXCOORD0;
			};
			
			
			// Vertex shader
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _ColorTintTex;
			
			// Pixel shader (without lighting/unity hax style)
			fixed4 frag(v2f IN) : SV_Target
			{
				// Get the sprite color
				// Put it in inside a 
				
				fixed4 c = tex2D(_MainTex, IN.texcoord); // contains index
				float r = IN.color.r;
				int test = round(c.r * 255);
				
				if(test%2 == 0)
					return float4(1,0,0,1);
				else
					return float4(0,1,0,1);
					
				
				//float ind = 2/(fixed)8.0f;
				//float2 index = (0.5,ind);
				//fixed4 colortTint = tex2D(_ColorTintTex, index);
				//colortTint.rgb *= c.a;
				//return colortTint;
				//return float4(1,1,1,1);
				//c *= 100;
				//return c;
				
				float i = 4;
				float mx = 256;
				float b = IN.color.r*mx;
				if(b == i)
					return float4(1,1,1,1);
				else
				{		
					IN.color.rgb *= c.a;
					IN.color.a = c.a;
					
					return IN.color;
				}
				//if(c.r*256 == 1)
				//	return float4(0,0,0,1);
				//else
				//{
					//if(c.r == 1)
					//	return float4(1,1,1,1);
				//	float2 index = (c.r*256,1);
					
				//	fixed4 colortTint = tex2D(_ColorTintTex, index); // RGB is color // Alpha is value to grab from the 
					//c.rgb *= c.a;
				//	colortTint.rgb *= c.a;
					
				//	return colortTint;
				//}
			}
		ENDCG
		}
	} 
	CustomEditor "ColorTinterMaterialEditor"
	Fallback "Sprites/Default"
}