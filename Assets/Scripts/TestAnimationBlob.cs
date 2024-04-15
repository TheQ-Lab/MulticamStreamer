using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationBlob : MonoBehaviour
{
    public Transform tgt;
    public float swerveFactor = 0.2f;
    public float startLingerFactor = 2f;
    public float duration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(TriggerAnim), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerAnim()
    {
        StartCoroutine(ArcAnimation(transform, tgt, swerveFactor, startLingerFactor, duration));
    }

    public IEnumerator ArcAnimation(Transform subject, Transform tgtObj, float swerveFactor, float startLingerFactor, float duration)
    {
        Vector2 startPos = subject.position;
        Vector2 tgtPos = tgtObj.position;
        Vector2 directPathVector = tgtPos - startPos;
        Vector2 normal = Vector2.Perpendicular(directPathVector);
        
        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            var tNormalized = timeElapsed / duration;
            var tNormalizedEase = Mathf.Pow(tNormalized, startLingerFactor); //3rd root
            var currentPos = Vector3.Lerp(startPos, tgtPos, tNormalizedEase);

            var x = TooManyFuncts.Remap((tNormalizedEase + tNormalized) /2, 0, 1, 0, Mathf.PI);
            var sin = Mathf.Sin(x);

            currentPos += (Vector3) (swerveFactor * sin * normal);
            // Debug.Log(timeElapsed + " " + x);
            subject.position = currentPos;

            yield return null;
            timeElapsed += Time.deltaTime;
        }

        subject.position = tgtPos;
    }

    public IEnumerator ArcAnimationOld3D(Transform subject, Transform tgtObj, float duration)
    {
        Vector3 startPos = subject.position;
        Vector3 tgtPos = tgtObj.position;
        Vector3 directPathVector = tgtPos - startPos;
        Vector3 normal = Vector3.down;
        Vector3.OrthoNormalize(ref directPathVector, ref normal);


        float timeElapsed = 0.0f;
        while (timeElapsed < duration)
        {
            var t = Mathf.Pow(timeElapsed, 3f); //3rd root
            var currentPos = Vector3.Lerp(startPos, tgtPos, t);

            var x = TooManyFuncts.Remap((t + timeElapsed) / 2, 0, duration, 0, Mathf.PI);
            var sin = Mathf.Sin(x);

            currentPos += 222.5f * sin * normal;
            //Debug.Log(timeElapsed + " " + sin);
            subject.position = currentPos;

            yield return null;
            timeElapsed += Time.deltaTime;
        }

        subject.position = tgtPos;
    }
}
