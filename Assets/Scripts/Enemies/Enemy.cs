using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [HideInInspector]
    public WorldManager worldManager;


    public abstract void EnemyUpdate();
}
