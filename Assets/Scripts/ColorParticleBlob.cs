using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorParticleBlob : MonoBehaviour
{
    public Transform tgt;
    public float swerveFactor = 0.2f;
    public float startLingerFactor = 2f;
    public float duration = 1.0f;

    Vector2 startPos;
    InputSimulatorScript.GravitraxConnex.cmds colorCmd;

    // Start is called before the first frame update
    void Start()
    {
        ToInit();
        //Invoke(nameof(TriggerAnim), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToInit()
    {
        startPos = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    public void ToActivate(InputSimulatorScript.GravitraxConnex.cmds color)
    {
        gameObject.SetActive(true);
        colorCmd = color;
        tgt = HexButtons.Instance.GetCorrespondingButton(color).gameObj.transform;
        Invoke(nameof(TriggerAnim), 0.5f);
    }

    private void CompleteAnimation()
    {
        HexButtons.Instance.IterateBtnProg(colorCmd);
        ToDeactivate();
    }

    public void ToDeactivate()
    {
        transform.position = (Vector3)startPos;
        gameObject.SetActive(false);
    }

    public void TriggerAnim()
    {
        StartCoroutine(ArcAnimation(transform, tgt, swerveFactor, startLingerFactor, duration, delegate { }, CompleteAnimation)) ;
    }

    public IEnumerator ArcAnimation(Transform subject, Transform tgtObj, float swerveFactor, float startLingerFactor, float duration, Action OnEveryFrameAction, Action OnCompleteAction)
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

            OnEveryFrameAction();
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        subject.position = tgtPos;
        OnCompleteAction();
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
