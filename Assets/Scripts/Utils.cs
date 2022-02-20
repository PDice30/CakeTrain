using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static bool quadCollidePt(Vector2 a, Vector2 asize, Vector2 pt)
    {
        if(a.x+asize.x < pt.x || a.x > pt.x) return false;
        if(a.y+asize.y < pt.y || a.y > pt.y) return false;
        return true;
    }
    public static bool quadCollidePt3(Vector3 a, Vector2 asize, Vector2 pt)
    {
        if(a.x+asize.x < pt.x || a.x > pt.x) return false;
        if(a.z+asize.y < pt.y || a.z > pt.y) return false;
        return true;
    }

    public static bool quadCollide(Vector2 a, Vector2 asize, Vector2 b, Vector2 bsize)
    {
        if(a.x+asize.x < b.x || a.x > b.x+bsize.x) return false;
        if(a.y+asize.y < b.y || a.y > b.y+bsize.y) return false;
        return true;
    }

    public static bool quadCollide3(Vector3 a, Vector2 asize, Vector3 b, Vector2 bsize)
    {
        if(a.x+asize.x < b.x || a.x > b.x+bsize.x) return false;
        if(a.z+asize.y < b.z || a.z > b.z+bsize.y) return false;
        return true;
    }

    public static bool quadCollide3Correct(Vector3 a, Vector2 asize, Vector3 b, Vector2 bsize, ref Vector3 newa)
    {
        newa = a;
        float aright = (a.x+asize.x)-b.x;
        float aleft  = (b.x+bsize.x)-a.x;
        if(aleft < 0.0f || aright < 0.0f) return false;
        float atop    = (a.z+asize.y)-b.z;
        float abottom = (b.z+bsize.y)-a.z;
        if(atop < 0.0f || abottom < 0.0f) return false;

        if(aleft < aright)
        {
          if(aleft < atop)
          {
            if(aleft < abottom)
                newa.x += aleft;
            else
                newa.z += abottom;
          }
          else
          {
            if(atop < abottom)
                newa.z -= atop;
            else
                newa.z += abottom;
          }
        }
        else
        {
          if(aright < atop)
          {
            if(aright < abottom)
                newa.x -= aright;
            else
                newa.z += abottom;
          }
          else
          {
            if(atop < abottom)
                newa.z -= atop;
            else
                newa.z += abottom;
          }
        }

        return true;
    }

    public static void resizePrefab(GameObject go, float s)
    {
        GameObject spriteobj = go.transform.GetChild(0).gameObject;
        spriteobj.transform.localScale    = new Vector3(s,        s,  1.0f);
        spriteobj.transform.localPosition = new Vector3(s/2.0f,1.0f,s/2.0f);
    }
}
