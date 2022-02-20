using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public WorldManager worldManager;
  int x;
  int z;
  float z_maxBounds, z_minBounds, x_minBounds, x_maxBounds;
  public GameObject collidingStructure;
  float collidingDamage;

  void Start()
  {
    x_minBounds = 0;
    x_maxBounds = Consts.world_w;
    z_minBounds = 0;
    z_maxBounds = Consts.world_h;
  }

  // Update is called once per frame
  public void PlayerUpdate()
  {
    Vector2 quadSize = new Vector2(1.0f,1.0f);

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
      off = off*Consts.player_speed*Time.deltaTime;

      Vector2 proposed = p+off;

      //bounds
      if (proposed.x            < x_minBounds) proposed.x = x_minBounds;
      if (proposed.x+quadSize.x > x_maxBounds) proposed.x = x_maxBounds-quadSize.x;
      if (proposed.y            < z_minBounds) proposed.y = z_minBounds;
      if (proposed.y+quadSize.y > z_maxBounds) proposed.y = z_maxBounds-quadSize.y;

      Vector3 proposed3 = new Vector3(proposed.x,transform.position.y,proposed.y);
      collidingStructure = null;
      for(int i = 0; i < worldManager.structures.Count; i++)
      {
        if(Utils.quadCollideCorrect(proposed3,quadSize,worldManager.structures[i].GetComponent<Transform>().position,quadSize,ref proposed3))
        {
          collidingStructure = worldManager.structures[i];
        }
      }

      transform.position = proposed3;
    }

    if(collidingStructure && Input.GetKey(KeyCode.Space))
    {
      Structure s = collidingStructure.GetComponent<Structure>();
      if(s.type != StructureId.BEDROCK)
      {
        collidingDamage += Time.deltaTime;
        if(collidingDamage >= 1.0f)
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
            if(worldManager.structures[i] == collidingStructure)
            {
              worldManager.structures.RemoveAt(i);
              break;
            }
          }
          collidingStructure = null;
          collidingDamage = 0.0f;
        }
      }
    }
    else collidingDamage = 0.0f;
      
  }
}
