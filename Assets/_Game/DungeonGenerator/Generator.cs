using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Generator : MonoBehaviour
{

    [Header("Basic Rooms")]

    public GameObject[] roomPrefabs;
    public GameObject[] passagePrefabs;


    [Header("Special Rooms")]
    public GameObject[] startPrefabs;
    public GameObject[] endPrefabs;
    public GameObject[] BossPrefabs;

    [Header("Debugging")]
    public KeyCode RegenKey = KeyCode.Backspace;


    [Header("Dungeon Limits")]
    [Range(2, 50)] public int dungeonSize = 6;
    [Range(0, 1f)] public float constructionDelay = 0.01f;
    public List<Tile> generatedTiles = new List<Tile>();
    private List<GameObject> roomList = new List<GameObject>();
    Transform tileFrom, tileTo, tileRoot;

    // Unity Functions
    private void Start()
    {
        StartCoroutine(DungeonBuilder());
    }

    private void Update()
    {
        if (Input.GetKeyDown(RegenKey))
        {
            SceneManager.LoadScene("ProtoLevel");
        }
    }

    // Dungeon Generation Functions
    IEnumerator DungeonBuilder()
    {
        int roomType = 2;   // variable **name** is reused but it is always locally scoped

        // This is where you spawn, each dungeon has a start room, 
        // this is the only safe zone in the dungeon
        tileRoot = CreateStartRoom();
        tileTo = tileRoot;
        tileFrom = tileTo;
        tileTo = CreateTile(roomType);
        ConnectTiles();

        // This loop creates room/passage pairs, evenyually this will create
        // a sprawling dungeons, but as god knows creating characters is a long 
        // and painful process, so you get a short run through a straight dungeon
        for (int i = 0; i < dungeonSize; i++)
        {
            yield return new WaitForSeconds(constructionDelay);

            if ((i % 2) == 0)
            {
                roomType = 1;
                //Debug.Log($"{i} Room");
            }
            else
            {
                roomType = 2;
                //Debug.Log($"{i} Passage");
            }

            tileFrom = tileTo;
            tileTo = CreateTile(roomType);
            ConnectTiles();

            if (i == dungeonSize - 1)
            {
                // TODO: Find and defeats the boss to get the key to this room
                tileFrom = tileTo;
                tileTo = CreateEndingRoom();
                ConnectTiles();
                roomType = 3;
                //Debug.Log("Place End Room here");
            }
        }

        // Dungeon layout Generation is complete
        // set all flags needed to continue generating extra features

        ListRooms();

    }// IEnumerator DungeonBuilder()

    // perform black magic to connect the two rooms
    private void ConnectTiles()
    {
        Transform connectFrom = GetRandomConnector(tileFrom);
        if (connectFrom == null) { return; }
        Transform connectTo = GetRandomConnector(tileTo);
        if (connectTo == null) { return; }

        connectTo.SetParent(connectFrom);                   // set parent of connector to connector of tileFrom
        tileTo.SetParent(connectTo);                        // set parent of tileTo to connector of connectTo

        connectTo.localPosition = Vector3.zero;             // set position of connector to 0,0,0 (connector)
        connectTo.localRotation = Quaternion.identity;      // set the rotation of tile to 0,0,0,0
        connectTo.Rotate(0, 180f, 0);                       // rotate the connectTo tile by 180 degrees

        tileTo.SetParent(transform);                        // set the parent of tileTo to the transform
        connectTo.SetParent(tileTo.Find("Connectors"));     // set the parent of the connector back to the connectors node of tileTo

        generatedTiles.Last().connector = connectFrom.GetComponent<Connector>();
    }

    // Return a random connector from the given tile 
    // after checking for unconnected points.
    private Transform GetRandomConnector(Transform tile)
    {
        // TODO
        if (tile == null) { return null; }                                       // no tile, whaaaaa
        List<Connector> connectorList = new List<Connector>();

        // .ToList() is used to convert the Array/IEnumerable to a List
        connectorList = tile.GetComponentsInChildren<Connector>().ToList().FindAll(item => item.isConnected == false);

        if (connectorList.Count > 0)
        {
            int connectorIndex = Random.Range(0, connectorList.Count);
            connectorList[connectorIndex].isConnected = true;

            return connectorList[connectorIndex].transform;
        }

        return null;
    }

    private Transform CreateTile(int roomType)
    {
        int randomIndex;
        GameObject tileGO = null;//new GameObject();
        Transform origin;

        if (roomType == 1)
        {
            randomIndex = Random.Range(0, roomPrefabs.Length);
            tileGO = Instantiate(roomPrefabs[randomIndex], Vector3.zero, Quaternion.identity, transform);
            tileGO.name = roomPrefabs[randomIndex].name;
            origin = generatedTiles[generatedTiles.FindIndex(item => item.tile == tileFrom)].tile;
            //generatedTiles.Add(new Tile(tileGO.transform, origin));
            CreateLists(tileGO);
            return tileGO.transform;
        }

        if (roomType == 2)
        {
            randomIndex = Random.Range(0, passagePrefabs.Length);
            tileGO = Instantiate(passagePrefabs[randomIndex], Vector3.zero, Quaternion.identity, transform);
            tileGO.name = passagePrefabs[randomIndex].name;
            origin = generatedTiles[generatedTiles.FindIndex(item => item.tile == tileFrom)].tile;
            //generatedTiles.Add(new Tile(tileGO.transform, origin));
            CreateLists(tileGO);
            return tileGO.transform;
        }

        return tileGO.transform;
    }



    // Randomly select a Starting room from a list of starting rooms
    private Transform CreateStartRoom()
    {
        int index = Random.Range(0, startPrefabs.Length);
        GameObject tileGO = Instantiate(startPrefabs[index], Vector3.zero, Quaternion.identity, transform) as GameObject;
        tileGO.name = "StartRoom";
        float yrot = Random.Range(0, 4) * 90;
        yrot = 0;     // test value, delete later
        tileGO.transform.Rotate(0, yrot, 0);

        CreateLists(tileGO); ;

        return tileGO.transform;
    }

    // Randomly select a Ending room from a list of starting rooms
    private Transform CreateEndingRoom()
    {
        int index = Random.Range(0, endPrefabs.Length);
        GameObject tileGO = Instantiate(endPrefabs[index], Vector3.zero, Quaternion.identity, transform) as GameObject;
        tileGO.name = "EndRoom";
        float yrot = Random.Range(0, 4) * 90;
        tileGO.transform.Rotate(0, yrot, 0);

        CreateLists(tileGO);

        return tileGO.transform;
    }

    private void collisionCheck()
    {
        BoxCollider roomBox = tileTo.GetComponent<BoxCollider>();
        //  if any other room intersects then we need to backtrack
        //  TODO: figure out how in the heck to do that

    }

    private void CreateLists(GameObject tileGO)
    {
        generatedTiles.Add(new Tile(tileGO.transform, null));
        if (tileGO.name.Contains("Room"))
        {
            roomList.Add(tileGO);
        }
        //roomList.Add(tileGO);

        Debug.Log($"Added Room {tileGO.name};");
    }

    public void ListRooms()
    {
        //  need to connect the listed rooms to game objects in the unity scene
        //  TODO: figure out how in the heck to do that
        foreach (GameObject room in roomList)
        {

        }
    }


}

