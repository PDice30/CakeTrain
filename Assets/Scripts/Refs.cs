using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileId : int
{
    TILE_ID_DIRT = 0,
    TILE_ID_GRASS,
    TILE_ID_WATER,
    TILE_ID_COUNT,
}

enum StructureId: int
{
    STRUCTURE_ID_TREE = 0,
    STRUCTURE_ID_IRONDEPOSIT,
    STRUCTURE_ID_BEDROCK,
    STRUCTURE_ID_TRACK,
    STRUCTURE_ID_COUNT,
}

enum EnemyId : int
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
    public GameObject[] structures;

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
        tiles = new GameObject[(int)TileId.TILE_ID_COUNT];
        for(int i = 0; i < (int)TileId.TILE_ID_COUNT; i++)
        {
            switch(i)
            {
                case (int)TileId.TILE_ID_DIRT:  tiles[i] = tileDirt; break;
                case (int)TileId.TILE_ID_WATER: tiles[i] = tileWater; break;
                case (int)TileId.TILE_ID_GRASS: tiles[i] = tileGrass; break;
                default: Debug.Log("failed to place tile!"); break;
            }
        }

        structures = new GameObject[(int)StructureId.STRUCTURE_ID_COUNT];
        for(int i = 0; i < (int)StructureId.STRUCTURE_ID_COUNT; i++)
        {
            switch(i)
            {
                case (int)StructureId.STRUCTURE_ID_TREE:        structures[i] = structureTree; break;
                case (int)StructureId.STRUCTURE_ID_IRONDEPOSIT: structures[i] = structureIronDeposit; break;
                case (int)StructureId.STRUCTURE_ID_BEDROCK:     structures[i] = structureBedrock; break;
                case (int)StructureId.STRUCTURE_ID_TRACK:       structures[i] = structureTrack; break;
                default: Debug.Log("failed to place structure!"); break;
            }
        }

        enemies = new GameObject[(int)EnemyId.ENEMY_ID_COUNT];
        for(int i = 0; i < (int)EnemyId.ENEMY_ID_COUNT; i++)
        {
            switch(i)
            {
                case (int)EnemyId.ENEMY_ID_BAT:  enemies[i] = enemyBat; break;
                case (int)EnemyId.ENEMY_ID_GOBLIN: enemies[i] = enemyGoblin; break;
                default: Debug.Log("failed to place enemy!"); break;
            }
        }
        
    }

}
