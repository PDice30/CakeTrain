using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swapper : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer c0;
    MeshRenderer c1;
    bool flipped = false;
    void Start()
    {
        c0 = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        c1 = gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();
        flip();
    }
    public void flip()
    {
        if(flipped)
        {
            c0.enabled = true;
            c1.enabled = false;
            flipped = false;
        }
        else
        {
            c0.enabled = false;
            c1.enabled = true;
            flipped = true;
        }
    }

}
