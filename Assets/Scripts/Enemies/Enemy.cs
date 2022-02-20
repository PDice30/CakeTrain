using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [HideInInspector]
    public WorldManager worldManager;
    // Update is called once per frame
    public abstract void EnemyUpdate();
}
