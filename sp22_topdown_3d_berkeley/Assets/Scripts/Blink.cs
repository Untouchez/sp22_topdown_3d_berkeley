using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private float blinkTimer;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    float blinkDuration;
    float blinkIntensity;
    Color startColor;
    Color blinkColor;
    // Start is called before the first frame update
    void Start()
    {
        startColor = skinnedMeshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        if (intensity <= 1)
            skinnedMeshRenderer.material.color = startColor * intensity;
        else
            skinnedMeshRenderer.material.color = blinkColor * intensity;
    }

    public void BlinkME(float _blinkDuration, float _blinkIntensity, Color color)
    {
        blinkTimer = blinkDuration;
        blinkDuration = _blinkDuration;
        blinkIntensity = _blinkIntensity;
        blinkColor = color;
    }
}
