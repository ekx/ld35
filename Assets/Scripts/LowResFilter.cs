using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class LowResFilter : MonoBehaviour
{
    public int HorizontalResolution = 320;
    public int VerticalResolution = 240;

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture scaled = RenderTexture.GetTemporary(HorizontalResolution, VerticalResolution);
        scaled.filterMode = FilterMode.Point;
        Graphics.Blit(source, scaled);
        Graphics.Blit(scaled, destination);
        RenderTexture.ReleaseTemporary(scaled);
    }
}
