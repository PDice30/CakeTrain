using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileIds : int
{
    TILE_ID_DIRT = 0,
    TILE_ID_GRASS,
    TILE_ID_WATER,
    TILE_ID_COUNT,
}

enum EnemyIds : int
{
    ENEMY_ID_BAT = 0,
    ENEMY_ID_GOBLIN,
    ENEMY_ID_COUNT,
}

public class Refs : MonoBehaviour
{
    public GameObject player;
    public GameObject cartEngine;
    public GameObject cartCraft;

    public GameObject tileGrass;
    public GameObject tileDirt;
    public GameObject tileWater;
    [HideInInspector]
    public GameObject[] tiles;

    public GameObject structureTree;
    public GameObject structureIronDeposit;
    public GameObject structureBedrock;
    public GameObject structureTrack;

    public GameObject objectWood;
    public GameObject objectIron;
    public GameObject objectTrack;
    public GameObject objectWoodenAxe;
    public GameObject objectIronAxe;
    public GameObject objectWoodenPick;
    public GameObject objectIronPick;

    public GameObject enemyBat;
    public GameObject enemyGoblin;
    public GameObject[] enemies;

    public void Initialize()
    {
        tiles = new GameObject[(int)TileIds.TILE_ID_COUNT];
        for(int i = 0; i < (int)TileIds.TILE_ID_COUNT; i++)
        {
            switch(i)
            {
                case (int)TileIds.TILE_ID_DIRT:  tiles[i] = tileDirt; break;
                case (int)TileIds.TILE_ID_WATER: tiles[i] = tileWater; break;
                case (int)TileIds.TILE_ID_GRASS: tiles[i] = tileGrass; break;
                default: Debug.Log("failed to place tile!"); break;
            }
        }

        enemies = new GameObject[(int)EnemyIds.ENEMY_ID_COUNT];
        for(int i = 0; i < (int)EnemyIds.ENEMY_ID_COUNT; i++)
        {
            switch(i)
            {
                case (int)EnemyIds.ENEMY_ID_BAT:  enemies[i] = enemyBat; break;
                case (int)EnemyIds.ENEMY_ID_GOBLIN: enemies[i] = enemyGoblin; break;
                default: Debug.Log("failed to place enemy!"); break;
            }
        }
        
    }

}
