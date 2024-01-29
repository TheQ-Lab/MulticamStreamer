using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamGridAgent : MonoBehaviour
{
    public CamGridHandler.CamFrameTransform CamFrameTransform;

    public bool tester;
    private void Start()
    {
        /*if(tester) 
            InputManager.Instance.inSwapAnim.setEvent += LaunchAnim;*/
    }

    public IEnumerator LaunchAnim(CamGridHandler.CamFrameTransform frameTransform)
    {
        //StartCoroutine(PhaseBAnim(new Vector3(-2.98f, 0, 0)));
        //StartCoroutine(PhaseBAnim(new Vector3(5.94999981f, -3.20000005f, 0)));

        yield return StartCoroutine(PhaseBAnim(frameTransform.position));
        CamFrameTransform = frameTransform;
    }

    private IEnumerator PhaseBAnim(Vector3 tgtPos)
    {
        //* phase = 'B';
        Vector3 startPos = transform.position;
        Vector3 directPath = tgtPos - startPos;
        Vector3 normal = Vector3.down;
        Vector3.OrthoNormalize(ref directPath, ref normal);


        //Debug.Log(tgtPos + " " + startPos);
        //Debug.Log(directPath + " " + normal);
        /*
        if (swingTopMode == LootAnimator.SwingTopMode.On)
            swingTop.Set();
        else if (swingTopMode == LootAnimator.SwingTopMode.Auto)
        {
            float angleNewStart = Vector3.Angle(directPath, new Vector3(-1f, 0f, 0f));
            float angleOGStart = Vector3.Angle(Vector3.Normalize(tgtPos - originalStart), new Vector3(-1f, 0f, 0f));
            if (angleNewStart - angleOGStart < 0f)
                swingTop.Set();
        }
        */
        float timeElapsed = 0f;
        while (timeElapsed < 1f)
        {
            var t = Mathf.Pow(timeElapsed, 3f); //4th root
            var currentPos = Vector3.Lerp(startPos, tgtPos, t);

            var x = TooManyFuncts.Remap((t + timeElapsed) / 2, 0, 1, 0, Mathf.PI);
            var sin = Mathf.Sin(x);
            currentPos += 2.5f * sin * normal;

            //Debug.Log(timeElapsed + " " + sin);

            transform.position = currentPos;
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        transform.position = tgtPos;
        //* phase = 'a';
    }
}
