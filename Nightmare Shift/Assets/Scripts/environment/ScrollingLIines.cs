using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScrollingLinesEffect : MonoBehaviour
{
    public Shader scrollingLinesShader;
    private Material material;

    public Texture2D lineTexture;
    public float scrollSpeed = 1.0f;
    public float blendAmount = 0.5f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (scrollingLinesShader != null)
        {
            if (material == null)
            {
                material = new Material(scrollingLinesShader);
            }

            material.SetTexture("_LineTex", lineTexture);
            material.SetFloat("_ScrollSpeed", scrollSpeed);
            material.SetFloat("_BlendAmount", blendAmount);

            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest); // No effect
        }
    }
}
