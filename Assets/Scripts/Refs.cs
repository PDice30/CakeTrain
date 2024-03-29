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
    public GameObject structureBarricade;
    public GameObject structureTurret;
    public GameObject[] structures;

    public GameObject objectWood;
    public GameObject objectIron;
    public GameObject objectWoodenAxe;
    public GameObject objectIronAxe;
    public GameObject objectWoodenPick;
    public GameObject objectIronPick;
    public GameObject[] objects;

    public GameObject productTrack;
    public GameObject productBarricade;
    public GameObject productTurret;
    public GameObject[] products;

    public GameObject enemyBat;
    public GameObject enemyGoblin;
    public GameObject[] enemies;

    public GameObject bullet;

    public GameObject tileHighlight_Passive;
    public GameObject structureHighlight_Passive;
    public GameObject structureHighlight_Active;
    public GameObject objectHighlight_Passive;
    public GameObject productHighlight_Passive;
    public GameObject cartHighlight_Passive;
    public GameObject enemyHighlight_Passive;

    public GameObject craftPreviewBg;
    public GameObject craftPreviewEject;
    public GameObject craftPreviewSubmit;
    public GameObject craftPreviewTail;

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
                case (int)StructureId.BARRICADE:   structures[i] = structureBarricade; break;
                case (int)StructureId.TURRET:      structures[i] = structureTurret; break;
                default: Debug.Log("failed to place structure!"); break;
            }
        }

        objects = new GameObject[(int)ObjectId.COUNT];
        for(int i = 0; i < (int)ObjectId.COUNT; i++)
        {
            switch(i)
            {
                case (int)ObjectId.WOOD:  objects[i] = objectWood; break;
                case (int)ObjectId.IRON:  objects[i] = objectIron; break;
                default: Debug.Log("failed to place object!"); break;
            }
        }

        products = new GameObject[(int)ProductId.COUNT];
        for(int i = 0; i < (int)ProductId.COUNT; i++)
        {
            switch(i)
            {
                case (int)ProductId.TRACK:     products[i] = productTrack; break;
                case (int)ProductId.BARRICADE: products[i] = productBarricade; break;
                case (int)ProductId.TURRET:    products[i] = productTurret; break;
                default: Debug.Log("failed to place product!"); break;
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
