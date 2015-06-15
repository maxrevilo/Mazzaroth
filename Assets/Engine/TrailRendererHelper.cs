using System.Collections;
using UnityEngine;

public class TrailRendererHelper : MonoBehaviour
{
    protected TrailRenderer TrailRenderer;
    protected float RendererTime = 0;

    void Awake()
    {
        TrailRenderer = gameObject.GetComponent<TrailRenderer>();
        if (null == TrailRenderer)
        {
            Debug.LogError("[TrailRendererHelper.Awake] invalid TrailRenderer.");
            return;
        }

        RendererTime = TrailRenderer.time;
    }

    void OnEnable()
    {
        if (null == TrailRenderer)
        {
            return;
        }

        StartCoroutine(ResetTrails());
    }

    IEnumerator ResetTrails()
    {
        TrailRenderer.time = 0;

        yield return new WaitForEndOfFrame();

        TrailRenderer.time = RendererTime;
    }
}