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

public class Refs : MonoBehaviour
{
    public GameObject player;

    public GameObject tileGrass;
    public GameObject tileDirt;
    public GameObject tileWater;
    [HideInInspector]
    public GameObject[] tiles;

    public GameObject objectWood;
    public GameObject objectIron;
    public GameObject objectTrack;
    public GameObject objectWoodenAxe;
    public GameObject objectIronAxe;
    public GameObject objectWoodenPick;
    public GameObject objectIronPick;

    public GameObject structureTree;
    public GameObject structureIronDeposit;
    public GameObject structureBedrock;
    public GameObject structureTrack;

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
        
    }

}
