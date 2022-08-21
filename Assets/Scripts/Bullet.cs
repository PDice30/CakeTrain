using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int x;
    public int z;
    public float dx;
    public float dz;

    public float size = 1f;
    private float speed = 2f;

    public void BulletUpdate()
    {
        float newx = transform.position.x + dx*Time.deltaTime*speed;
        float newz = transform.position.z + dz*Time.deltaTime*speed;
        transform.position = new Vector3(newx, Consts.bullet_y, newz);
    }
}
