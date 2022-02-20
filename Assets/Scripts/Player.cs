using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public WorldManager worldManager;
  int x;
  int z;
  float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;
  public Vector2 interactionOffset;
  public GameObject collidingStructure;
  public GameObject interactingStructure;
  float interactingDamage;
  public GameObject interactingObject;
  public GameObject holdingObject;

  void Start()
  {
    x_minBounds = 0;
    x_maxBounds = Consts.world_w;
    z_minBounds = 0;
    z_maxBounds = Consts.world_h;
    interactionOffset = new Vector2(Consts.player_reach,0.0f);
  }

  // Update is called once per frame
  public void PlayerUpdate()
  {
    Vector2 p = new Vector2(transform.position.x,transform.position.z);
    Vector2 off = new Vector2(0.0f,0.0f);
    bool moving = false;

         if(Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) { off.y += 1.0f; moving = true; }
    else if(Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) { off.y -= 1.0f; moving = true; }
         if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { off.x -= 1.0f; moving = true; }
    else if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) { off.x += 1.0f; moving = true; }

    if(moving)
    {
      off.Normalize();
      interactionOffset = off*Consts.player_reach;
      off = off*Consts.player_speed*Time.deltaTime;

      Vector2 proposed = p+off;

      //bounds
      if (proposed.x                      < x_minBounds) proposed.x = x_minBounds;
      if (proposed.x+Consts.player_size.x > x_maxBounds) proposed.x = x_maxBounds-Consts.player_size.x;
      if (proposed.y                      < z_minBounds) proposed.y = z_minBounds;
      if (proposed.y+Consts.player_size.y > z_maxBounds) proposed.y = z_maxBounds-Consts.player_size.y;

      Vector3 proposed3 = new Vector3(proposed.x,transform.position.y,proposed.y);
      collidingStructure = null;
      for(int i = 0; i < worldManager.structures.Count; i++)
      {
        if(Utils.quadCollideCorrect(proposed3,Consts.player_size,worldManager.structures[i].GetComponent<Transform>().position,Consts.unit_size,ref proposed3))
        {
          collidingStructure = worldManager.structures[i];
        }
      }
      Vector2 interactingPt = new Vector2(proposed3.x+Consts.player_half_size.x+interactionOffset.x,proposed3.z+Consts.player_half_size.y+interactionOffset.y);
      interactingStructure = null;
      for(int i = 0; i < worldManager.structures.Count; i++)
      {
        if(Utils.quadCollidePt(worldManager.structures[i].GetComponent<Transform>().position,Consts.unit_size,interactingPt))
          interactingStructure = worldManager.structures[i];
      }
      interactingObject = null;
      for(int i = 0; i < worldManager.objects.Count; i++)
      {
        if(Utils.quadCollidePt(worldManager.objects[i].GetComponent<Transform>().position,Consts.unit_size,interactingPt))
          interactingObject = worldManager.objects[i];
      }

      transform.position = proposed3;
    }

    if(interactingStructure) {
      if (interactingStructure.GetComponent<Structure>().type != StructureId.BEDROCK) {
        worldManager.tileHighlight_Passive.transform.position = new Vector3(interactingStructure.transform.position.x, Consts.object_y + 1f, interactingStructure.transform.position.z);
      }
    } else {
      worldManager.tileHighlight_Passive.transform.position = Consts.hiddenTilePosition;
    }

    if(interactingStructure && Input.GetKey(KeyCode.Space))
    {
      Structure s = interactingStructure.GetComponent<Structure>();
      if(s.type != StructureId.BEDROCK)
      {
        worldManager.tileHighlight_Active.transform.position = new Vector3(s.transform.position.x, Consts.object_y + 2f, s.transform.position.z);
        interactingDamage += Time.deltaTime;
        if(interactingDamage >= 1.0f)
        {
          GameObject newObject;
          Object o;
          switch(s.type)
          {
            case StructureId.BEDROCK: break; //impossible!
            case StructureId.TREE:
              newObject = GameObject.Instantiate(worldManager.refs.objects[(int)ObjectId.WOOD], new Vector3(s.x,Consts.object_y,s.z), Quaternion.identity);
              o = newObject.GetComponent<Object>();
              o.type = ObjectId.WOOD;
              o.x = s.x;
              o.z = s.z;
              worldManager.objects.Add(newObject);
              break;
            case StructureId.IRONDEPOSIT:
            {
              newObject = GameObject.Instantiate(worldManager.refs.objects[(int)ObjectId.IRON], new Vector3(s.x,Consts.object_y,s.z), Quaternion.identity);
              o = newObject.GetComponent<Object>();
              o.type = ObjectId.IRON;
              o.x = s.x;
              o.z = s.z;
              worldManager.objects.Add(newObject);
              break;
            }
          }
          for(int i = 0; i < worldManager.structures.Count; i++)
          {
            if(worldManager.structures[i] == interactingStructure)
            {
              worldManager.structures.RemoveAt(i);
              break;
            }
          }
          interactingStructure = null;
          interactingDamage = 0.0f;
        }
      }
    }
    else {
      interactingDamage = 0.0f;
      worldManager.tileHighlight_Active.transform.position = Consts.hiddenTilePosition;
    }
      
  }
}
