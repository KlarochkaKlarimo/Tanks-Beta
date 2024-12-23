using UnityEngine;

[ExecuteInEditMode]
public class PixelationEffect : MonoBehaviour
{
    public int targetWidth = 320;
    public int targetHeight = 180;

    private Camera _camera;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        RenderTexture lowRes = RenderTexture.GetTemporary(targetWidth, targetHeight);
        lowRes.filterMode = FilterMode.Point; // Для резких краев
        Graphics.Blit(src, lowRes);
        Graphics.Blit(lowRes, dest);
        RenderTexture.ReleaseTemporary(lowRes);
    }
}