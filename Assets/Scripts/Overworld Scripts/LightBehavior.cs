using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehavior : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D light2;
    public float variance = .02f;

    public float baseOuter;
    float minOuterRadius;
    float maxOuterRadius;
    float choosenOuterRadius;

    public float baseInner;
    float minInnerRadius;
    float maxInnerRadius;
    float choosenInnerRadius;

    public float minTime = .1f;
    public float maxTime = .2f;

    float elapsedTime = 0f;
    float choosenTime;

    // Start is called before the first frame update
    void Start()
    {
        minOuterRadius = baseOuter - variance;
        maxOuterRadius = baseOuter + variance;

        minInnerRadius = baseInner - variance;
        maxInnerRadius = baseInner + variance;
        ResetValues();
        light2 = gameObject.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= choosenTime)
        {
            ResetValues();
            light2.pointLightInnerRadius = choosenInnerRadius;
            light2.pointLightOuterRadius = choosenOuterRadius;
        }
    }

    void ResetValues()
    {
        elapsedTime = 0f;
        choosenTime = Random.Range(minTime, maxTime);
        choosenInnerRadius = Random.Range(minInnerRadius, maxInnerRadius);
        choosenOuterRadius = Random.Range(minOuterRadius, maxOuterRadius);
    }
}
