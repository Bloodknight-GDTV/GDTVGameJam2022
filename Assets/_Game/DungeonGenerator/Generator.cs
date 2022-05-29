using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class Generator : MonoBehaviour
{

    [Header("Basic Rooms")]
    public GameObject[] startPrefabs;
    public GameObject[] tilePrefabs;
    public GameObject[] roomPrefabs;
    public GameObject[] passagePrefabs;
    public GameObject[] endPrefabs;

    [Header("Special Rooms")]
    public GameObject[] BossPrefabs;

    [Header("Debugging")]
    public KeyCode reloadKey = KeyCode.Backspace;


    [Header("Dungeon Limits")]

    [Range(2, 50)] public int dungeonSize = 10;
    [Range(0, 1f)] public float constructionDelay = 0.1f;
    public List<Tile> generatedTiles = new List<Tile>();
    Transform tileFrom, tileTo, tileRoot;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DungeonBuilder());
    }

    IEnumerator DungeonBuilder()
    {
        tileRoot = CreateStartTile();
        tileTo = tileRoot;
        for (int i = 0; i < dungeonSize - 1; i++)
        {
            yield return new WaitForSeconds(constructionDelay);

            if ((i % 2) == 0)
            {
                Debug.Log($"{i} Room");
            }
            else
            {
                Debug.Log($"{i} Passage");
            }
            if (i == dungeonSize - 2)
            {
                Debug.Log("Place End Room here");
            }
            else
            {
                tileFrom = tileTo;
                tileTo = CreateTile();
                ConnectTiles();
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

    // perform black magic to ... connect the two rooms
    void ConnectTiles()
    {
        Transform connectFrom = GetRandomConnector(tileFrom);
        if (connectFrom == null) { return; }
        Transform connectTo = GetRandomConnector(tileTo);
        if (connectTo == null) { return; }

        connectTo.SetParent(connectFrom);                   // set parent of connector to connector of tileFrom
        tileTo.SetParent(connectTo);                         // set parent of tileTo to connector of connectTo

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
        connectorList = tile.GetComponentsInChildren<Connector>().ToList().FindAll(item => item.isConnected == false);     // .ToList() is used to convert the Array/IEnumerable to a List

        if (connectorList.Count == 0) { return null; }
        int connectorIndex = Random.Range(0, connectorList.Count);
        connectorList[connectorIndex].isConnected = true;

        return connectorList[connectorIndex].transform;
    }

    Transform CreateTile()
    {
        int randomIndex = Random.Range(0, tilePrefabs.Length);
        GameObject goTile = Instantiate(tilePrefabs[randomIndex], Vector3.zero, Quaternion.identity, transform) as GameObject;
        //Instantiate()
        goTile.name = tilePrefabs[randomIndex].name;
        //generatedTiles.Add(goTile.GetComponent<Tile>());
        // 
        Transform origin = generatedTiles[generatedTiles.FindIndex(item => item.tile == tileFrom)].tile;
        generatedTiles.Add(new Tile(goTile.transform, origin));

        return goTile.transform;
    }

    // Randomly select a starting room from a list of starting rooms
    Transform CreateStartTile()
    {
        int index = Random.Range(0, startPrefabs.Length);
        GameObject goTile = Instantiate(startPrefabs[index], Vector3.zero, Quaternion.identity, transform) as GameObject;
        goTile.name = "StartRoom";
        float yrot = Random.Range(0, 4) * 90;
        goTile.transform.Rotate(0, yrot, 0);

        generatedTiles.Add(new Tile(goTile.transform, null));

        return goTile.transform;
    }

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

