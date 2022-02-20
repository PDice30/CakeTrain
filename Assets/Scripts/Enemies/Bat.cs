using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    // TODO : To be changed to follow train, or possibly player if they are closer?
    public float size = 1f;
    private float speed = 2f;
    // Update is called once per frame
    public override void EnemyUpdate()
    {
        // TODO : Possibly start with Vector2 and normalize then convert back to Vector3
        // Vector3 directionNormal = (transform.position - worldManager.player.transform.position).normalized;
        // transform.position += directionNormal * Time.deltaTime * speed;

        // Vector3 directionNormal = (worldManager.player.transform.position - transform.position).normalized;
        // transform.position += directionNormal * Time.deltaTime * speed;

        // transform.position = Vector3.MoveTowards(transform.position, worldManager.player.transform.position, Time.deltaTime * speed);
        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerSizeOffset = new Vector2(size / 2 - Consts.player_s / 2, size / 2 - Consts.player_s / 2);
        Vector2 playerPos = new Vector2(worldManager.player.transform.position.x - playerSizeOffset.x, worldManager.player.transform.position.z - playerSizeOffset.y);
        Vector2 offset = (playerPos - enemyPos).normalized;
        enemyPos += offset * Time.deltaTime * speed;

        transform.position = new Vector3(enemyPos.x, Consts.enemy_y, enemyPos.y);
    }
}
