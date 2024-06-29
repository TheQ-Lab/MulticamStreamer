using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(gameObject, transform);
        Destroy(obj.GetComponent<Instantiator>());
        obj.AddComponent<EasInInator>();
        obj.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
