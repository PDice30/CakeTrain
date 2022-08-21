using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [HideInInspector]
    public WorldManager worldManager;

    public float pulse;
    public float bias;
    public void Start()
    {
        pulse = Random.Range(0.0f,100.0f);
        bias = Random.Range(0.0f,100.0f);
    }

    public abstract void EnemyUpdate();
}
