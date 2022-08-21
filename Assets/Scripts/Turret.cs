using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [HideInInspector]
    public WorldManager worldManager;
    const float tts = 5.0f;
    public float time_remaining = tts;
    public void TurretUpdate()
    {
        time_remaining -= Time.deltaTime;
        if(time_remaining <= 0.0)
        {
            worldManager.spawnBulletTowardClosestEnemy(gameObject);
            time_remaining += tts;
        }
    }

}
