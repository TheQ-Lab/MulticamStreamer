using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Constants I;
    private void Awake()
    {
        TooManyFuncts.Singletonize(ref I, this, false);
    }

    [SerializeField] public Color red, green, blue;
    [SerializeField] public int commandPanelsPoolSize = 10;
    [SerializeField] public int votesNeededGtrxColor = 6;
    [SerializeField] public float votesDelay = 3f;

}
