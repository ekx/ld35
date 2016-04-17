using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
    public void Start()
    {
        var renderer = GetComponentInChildren<Renderer>();

        if (renderer != null)
        {
            material = renderer.material;
            offset = material.GetTextureOffset("_MainTex").y;
        }
    }

    public void FixedUpdate()
    {
        offset += (ObstacleController.Instance.Speed * 0.45f) * GameController.Instance.TimeScale;
        material.SetTextureOffset("_MainTex", new Vector2(0f, offset));
    }

    private float offset;
    private Material material;
}