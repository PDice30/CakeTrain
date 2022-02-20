using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // TODO : To be changed to follow train, or possibly player if they are closer?
    private float speed = 5f;
    // Update is called once per frame
    public override void EnemyUpdate()
    {
        // TODO : Possibly start with Vector2 and normalize then convert back to Vector3
        // Vector3 directionNormal = (transform.position - worldManager.player.transform.position).normalized;
        // transform.position += directionNormal * Time.deltaTime * speed;

        Vector3 directionNormal = (worldManager.player.transform.position - transform.position).normalized;
        transform.position += directionNormal * Time.deltaTime * speed;

        // transform.position = Vector3.MoveTowards(transform.position, worldManager.player.transform.position, Time.deltaTime * speed);

    }
}
