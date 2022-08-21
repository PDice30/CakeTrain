using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int x;
    public int z;
    public float dx;
    public float dz;

    public float life = 0.0f;

    public void BulletUpdate()
    {
        float newx = transform.position.x + dx*Time.deltaTime*Consts.bullet_speed;
        float newz = transform.position.z + dz*Time.deltaTime*Consts.bullet_speed;
        transform.position = new Vector3(newx, Consts.bullet_y, newz);
        life += Time.deltaTime;
    }
}
