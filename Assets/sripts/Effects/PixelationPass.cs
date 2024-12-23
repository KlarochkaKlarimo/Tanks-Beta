using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PixelationPass : CustomPass
{
    public int pixelWidth = 320;
    public int pixelHeight = 180;

    protected override void Execute(CustomPassContext ctx)
    {
        var cameraWidth = ctx.cameraColorBuffer.rt.width;
        var cameraHeight = ctx.cameraColorBuffer.rt.height;

        // Создаем RenderTexture с низким разрешением
        var lowResRT = RenderTexture.GetTemporary(pixelWidth, pixelHeight, 0, RenderTextureFormat.DefaultHDR);
        lowResRT.filterMode = FilterMode.Point; // Для эффекта пикселизации

        // Копируем текущий буфер в низкое разрешение
        CoreUtils.SetRenderTarget(ctx.cmd, lowResRT);
        ctx.cmd.Blit(ctx.cameraColorBuffer, lowResRT);

        // Копируем обратно в оригинальный буфер
        CoreUtils.SetRenderTarget(ctx.cmd, ctx.cameraColorBuffer);
        ctx.cmd.Blit(lowResRT, ctx.cameraColorBuffer);

        RenderTexture.ReleaseTemporary(lowResRT);
    }
}