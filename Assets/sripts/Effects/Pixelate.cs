using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[System.Serializable, VolumeComponentMenu("Custom/Post-Processing/Pixelate")]
public class Pixelate : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    public ClampedIntParameter pixelSize = new ClampedIntParameter(8, 1, 512);

    private Material pixelateMaterial;
    private static readonly int PixelSizeID = Shader.PropertyToID("_PixelSize");

    public bool IsActive() => pixelSize.value > 1 && pixelateMaterial != null;

    public override void Setup()
    {
        if (pixelateMaterial == null)
        {
            pixelateMaterial = new Material(Shader.Find("Hidden/Custom/Pixelate"));
        }
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (pixelateMaterial != null)
        {
            pixelateMaterial.SetInt(PixelSizeID, pixelSize.value);
            cmd.Blit(source, destination, pixelateMaterial);
        }
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(pixelateMaterial);
    }
}