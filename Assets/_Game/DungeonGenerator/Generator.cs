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
    public KeyCode reloadKey = KeyCode.Backspace;


    [Header("Dungeon Limits")]
    [Range(2, 50)] public int dungeonSize = 6;
    [Range(0, 1f)] public float constructionDelay = 0.01f;
    public List<Tile> generatedTiles = new List<Tile>();
    Transform tileFrom, tileTo, tileRoot;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DungeonBuilder());
    }

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
                Debug.Log($"{i} Room");
            }
            else
            {
                roomType = 2;
                Debug.Log($"{i} Passage");
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
                Debug.Log("Place End Room here");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            SceneManager.LoadScene("ProtoLevel");
        }
    }

    // Game Functions

    // perform black magic to connect the two rooms
    void ConnectTiles()
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
    Transform GetRandomConnector(Transform tile)
    {
        // TODO
        if (tile == null) { return null; }                                       // no tile, whaaaaa
        List<Connector> connectorList = new List<Connector>();

        // .ToList() is used to convert the Array/IEnumerable to a List
        connectorList = tile.GetComponentsInChildren<Connector>().ToList().FindAll(item => item.isConnected == false);

        if (connectorList.Count == 0) { return null; }
        int connectorIndex = Random.Range(0, connectorList.Count);
        connectorList[connectorIndex].isConnected = true;

        return connectorList[connectorIndex].transform;
    }

    Transform CreateTile(int roomType)
    {
        int randomIndex;
        GameObject goTile = null;//new GameObject();
        Transform origin;

        if (roomType == 1)
        {
            randomIndex = Random.Range(0, roomPrefabs.Length);
            goTile = Instantiate(roomPrefabs[randomIndex], Vector3.zero, Quaternion.identity, transform);
            goTile.name = roomPrefabs[randomIndex].name;
            origin = generatedTiles[generatedTiles.FindIndex(item => item.tile == tileFrom)].tile;
            generatedTiles.Add(new Tile(goTile.transform, origin));
            return goTile.transform;
        }

        if (roomType == 2)
        {
            randomIndex = Random.Range(0, passagePrefabs.Length);
            goTile = Instantiate(passagePrefabs[randomIndex], Vector3.zero, Quaternion.identity, transform);
            goTile.name = passagePrefabs[randomIndex].name;
            origin = generatedTiles[generatedTiles.FindIndex(item => item.tile == tileFrom)].tile;
            generatedTiles.Add(new Tile(goTile.transform, origin));
            return goTile.transform;
        }

        return goTile.transform;

        // // THis isnt the code you are looking for, no really its not, stop reading
        // int randomIndex = Random.Range(0, placePrefabs.Length);
        // GameObject goTile = Instantiate(placePrefabs[randomIndex], Vector3.zero, Quaternion.identity, transform);
        // //Instantiate()
        // goTile.name = placePrefabs[randomIndex].name;
        // //generatedTiles.Add(goTile.GetComponent<Tile>());
        // // 
        // Transform origin = generatedTiles[generatedTiles.FindIndex(item => item.tile == tileFrom)].tile;
        // generatedTiles.Add(new Tile(goTile.transform, origin));
    }

    // Randomly select a Starting room from a list of starting rooms
    Transform CreateStartRoom()
    {
        int index = Random.Range(0, startPrefabs.Length);
        GameObject goTile = Instantiate(startPrefabs[index], Vector3.zero, Quaternion.identity, transform) as GameObject;
        goTile.name = "StartRoom";
        float yrot = Random.Range(0, 4) * 90;
        yrot = 0;     // test value, delete later
        goTile.transform.Rotate(0, yrot, 0);

        generatedTiles.Add(new Tile(goTile.transform, null));

        return goTile.transform;
    }

    // Randomly select a Ending room from a list of starting rooms
    Transform CreateEndingRoom()
    {
        int index = Random.Range(0, endPrefabs.Length);
        GameObject goTile = Instantiate(endPrefabs[index], Vector3.zero, Quaternion.identity, transform) as GameObject;
        goTile.name = "EndRoom";
        float yrot = Random.Range(0, 4) * 90;
        goTile.transform.Rotate(0, yrot, 0);

        generatedTiles.Add(new Tile(goTile.transform, null));

        return goTile.transform;
    }


}

