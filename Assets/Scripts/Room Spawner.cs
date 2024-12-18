using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door


    [SerializeField] private GameObject grid;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    public float waitTime = 4f;

    void Start()
    {
        grid = GameObject.Find("Grid");
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

//templates.bottomRooms[rand].transform.rotation,
    void Spawn(){
        if(spawned == false){
            if(openingDirection == 1){
                // Need to spawn a room with a TOP door that connects to a BOTTOM door.
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity, grid.transform);
            } else if(openingDirection == 2){
                // Need to spawn a room with a BOTTOM door that connects to a TOP door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity, grid.transform);
            } else if(openingDirection == 3){
                // Need to spawn a room with a RIGHT door that connects to a LEFT door.
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity, grid.transform);
            } else if(openingDirection == 4){
                // Need to spawn a room with a LEFT door that connects to a RIGHT door.
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity, grid.transform);
            }
            spawned = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpawner otherRoomSpawner = other.GetComponent<RoomSpawner>();
            if (otherRoomSpawner != null && otherRoomSpawner.spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, grid.transform);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}