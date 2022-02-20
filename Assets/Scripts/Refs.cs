using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refs : MonoBehaviour
{
    public GameObject player;
    public GameObject cartEngine;
    public GameObject cartCraft;
    public GameObject track;

    public GameObject tileGrass;
    public GameObject tileDirt;
    public GameObject tileWater;
    [HideInInspector]
    public GameObject[] tiles;

    public GameObject structureTree;
    public GameObject structureIronDeposit;
    public GameObject structureBedrock;
    public GameObject[] structures;

    public GameObject objectWood;
    public GameObject objectIron;
    public GameObject objectTrack;
    public GameObject objectWoodenAxe;
    public GameObject objectIronAxe;
    public GameObject objectWoodenPick;
    public GameObject objectIronPick;
    public GameObject[] objects;

    public GameObject enemyBat;
    public GameObject enemyGoblin;
    public GameObject[] enemies;

    public GameObject tileHighlight_Passive;
    public GameObject tileHighlight_Active;

    public void Initialize()
    {
        tiles = new GameObject[(int)TileId.COUNT];
        for(int i = 0; i < (int)TileId.COUNT; i++)
        {
            switch(i)
            {
                case (int)TileId.DIRT:  tiles[i] = tileDirt; break;
                case (int)TileId.WATER: tiles[i] = tileWater; break;
                case (int)TileId.GRASS: tiles[i] = tileGrass; break;
                default: Debug.Log("failed to place tile!"); break;
            }
        }

        structures = new GameObject[(int)StructureId.COUNT];
        for(int i = 0; i < (int)StructureId.COUNT; i++)
        {
            switch(i)
            {
                case (int)StructureId.TREE:        structures[i] = structureTree; break;
                case (int)StructureId.IRONDEPOSIT: structures[i] = structureIronDeposit; break;
                case (int)StructureId.BEDROCK:     structures[i] = structureBedrock; break;
                default: Debug.Log("failed to place structure!"); break;
            }
        }

        objects = new GameObject[(int)ObjectId.COUNT];
        for(int i = 0; i < (int)ObjectId.COUNT; i++)
        {
            switch(i)
            {
                case (int)ObjectId.WOOD:  objects[i] = objectWood; break;
                case (int)ObjectId.IRON: objects[i] = objectIron; break;
                default: Debug.Log("failed to place object!"); break;
            }
        }

        enemies = new GameObject[(int)EnemyId.COUNT];
        for(int i = 0; i < (int)EnemyId.COUNT; i++)
        {
            switch(i)
            {
                case (int)EnemyId.BAT:    enemies[i] = enemyBat; break;
                case (int)EnemyId.GOBLIN: enemies[i] = enemyGoblin; break;
                default: Debug.Log("failed to place enemy!"); break;
            }
        }
        
    }

}
