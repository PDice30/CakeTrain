using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    // TODO : To be changed to follow train, or possibly player if they are closer?
    public float size = 1f;
    private float speed = 1f;
    // Update is called once per frame

    public override void EnemyUpdate()
    {
        // TODO : Possibly start with Vector2 and normalize then convert back to Vector3
        // Vector3 directionNormal = (worldManager.player.transform.position - transform.position).normalized;
        // transform.position += directionNormal * Time.deltaTime * speed;
        // transform.position = transform.position -
        //  new Vector3(worldManager.player.transform.position.x, 
        //              0,
        //              worldManager.player.transform.position.z) * Time.deltaTime * speed;
        pulse += Time.deltaTime;
        bias += Time.deltaTime*1.4f;
        float frameSpeed = speed* 0.2f+(0.8f*(Mathf.Sin(pulse*3.0f)+1.0f)/2.0f);
        Vector2 biasOff = new Vector2(Mathf.Cos(bias),Mathf.Sin(bias))*0.9f;

        Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerSizeOffset = new Vector2(size / 2 - Consts.player_s / 2, size / 2 - Consts.player_s / 2);
        Vector2 playerPos = new Vector2(worldManager.player.transform.position.x - playerSizeOffset.x, worldManager.player.transform.position.z - playerSizeOffset.y);
        Vector2 offset = (playerPos - enemyPos).normalized;
        enemyPos += (offset+biasOff) * Time.deltaTime * frameSpeed;

        transform.position = new Vector3(enemyPos.x, Consts.enemy_y, enemyPos.y);
    }
}
