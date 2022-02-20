using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCart : MonoBehaviour
{
    public int x;
    public int z;

    [HideInInspector]
    public List<ObjectId> objectsInCrafter;

    public bool craftIsValid;

    public bool validateRecipe()
    {
        craftIsValid = false;
        for(int i = 0; i < Consts.recipes.Count; i++)
        {
            if(Consts.recipes[i].Count == objectsInCrafter.Count)
            {
                craftIsValid = true;
                for(int j = 0; j < objectsInCrafter.Count; j++)
                {
                    if(Consts.recipes[i][j] != objectsInCrafter[j])
                    {
                        craftIsValid = false;
                        break;
                    }
                }
            }
            if(craftIsValid) break;
        }
        return craftIsValid;
    }
}
