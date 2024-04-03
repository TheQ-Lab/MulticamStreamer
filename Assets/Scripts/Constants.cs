using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Constants I;
    [SerializeField] public Color red, green, blue;

    private void Awake()
    {
        TooManyFuncts.Singletonize(ref I, this, false);
    }
}
