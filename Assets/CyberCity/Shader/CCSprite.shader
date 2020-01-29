Shader "CyberCity/CCSprite" 
{
   Properties {
      [HDR]
	  _Color ("Color", Color) = (1,1,1,1)
      _MainTex ("Texture Image", 2D) = "white" {}
   }

   SubShader {
		
	  Tags {"Queue" = "Transparent"}

      Pass {

         ZWrite Off

         Blend SrcAlpha One

         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         // User-specified uniforms            
         uniform sampler2D _MainTex;  
 
         struct vertexInput {
            float4 vertex : POSITION;
            float4 tex : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 tex : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
 
			output.tex = input.tex;
            output.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0.0, 0.0, 0.0, 1.0)) - float4(input.vertex.x, input.vertex.z, 0.0, 0.0));
            return output;
         }
		 
		 fixed4 _Color;
         
		 float4 frag(vertexOutput input) : COLOR
         {
            return tex2D(_MainTex, float2(input.tex.xy)) * _Color;    
         }
 
         ENDCG
      }
   }
}
