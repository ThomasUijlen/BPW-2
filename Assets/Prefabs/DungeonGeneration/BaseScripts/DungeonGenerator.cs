using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DungeonGenerator : MonoBehaviour
{
    public const int TILE_WIDTH = 2;

    [SerializeField]
    private Biome[] biomes;

    [SerializeField]
    public int mapSize = 20;

    public GridCharacter player;

    [SerializeField]
    private bool fillEmpty = true;

    private ClimateMap climateMap;
    private DungeonMap dungeonMap;

    public TileCell[,] tiles;

    private void Awake() {
        climateMap = GetComponent<ClimateMap>();
        dungeonMap = GetComponent<DungeonMap>();

        SetSeeds();
        climateMap.GenerateClimateMap(mapSize);
        dungeonMap.GenerateDungeonMap(mapSize);
        GenerateDungeon();
        FillDungeon();

        int mapCenter = Mathf.CeilToInt(mapSize/2f);
        Vector2 playerCoord = new Vector2(mapCenter,mapCenter);
        OccupyTile(playerCoord,player);
        player.SetCoord(playerCoord);
    }

    private void GenerateDungeon() {
        tiles = new TileCell[mapSize,mapSize];

        Random.InitState(dungeonMap.seed);

        for(int x = 0; x < mapSize; x++) {
            for(int y = 0; y < mapSize; y++) {
                TileCell tileCell = new TileCell();
                tiles[x,y] = tileCell;

                tileCell.coordinates = new Vector2(x,y);
                AssignBiome(tileCell);
                CreateTile(tileCell);
            }
        }
    }

    private void FillDungeon() {
        dungeonMap.PlaceObjects(this);
    }

    private void AssignBiome(TileCell tileCell) {
        foreach (Biome biome in biomes)
        {
            float newScore = biome.GetBiomeScore(tileCell.coordinates,climateMap);
            if(newScore > tileCell.biomeScore) {
                tileCell.biomeScore = newScore;
                tileCell.biome = biome;
            }
        }
    }

    private void CreateTile(TileCell tileCell) {
        tileCell.active = dungeonMap.IsActive(tileCell.coordinates);

        if(tileCell.active) {
            Tile randomTile = tileCell.biome.tileSet[Random.Range(0,tileCell.biome.tileSet.Length)];
            tileCell.tile = Instantiate(randomTile,new Vector3(tileCell.coordinates.x,0,tileCell.coordinates.y)*TILE_WIDTH, Quaternion.Euler(0,0,0));
        } else {
            if(!fillEmpty) return;
            Tile randomTile = tileCell.biome.blockedTileSet[Random.Range(0,tileCell.biome.blockedTileSet.Length)];
            tileCell.tile = Instantiate(randomTile,new Vector3(tileCell.coordinates.x,0,tileCell.coordinates.y)*TILE_WIDTH, Quaternion.Euler(0,0,0));
        }

        tileCell.tile.gameObject.transform.eulerAngles = new Vector3(0,((int) Mathf.Floor(Random.Range(0,5)))*90,0);
    }

    public bool IsActive(Vector2 coord) {
        if(coord.x < 0 || coord.y < 0 || coord.x >= tiles.GetLength(0) || coord.y >= tiles.GetLength(1)) return false;
        return tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].active;
    }

    public void OccupyTile(Vector2 coord, GridCharacter character) {
        tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].occupiedBy = character;
    }

    public GridCharacter GetOccupyingCharacter(Vector2 coord) {
        return tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].occupiedBy;
    }

    public bool IsEmpty(Vector2 coord) {
        return IsActive(coord) && tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].occupiedBy == null;
    }

    public void PlaceItem(Vector2 coord, Item item) {
        TileCell tileCell = tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)];
        item.gameObject.SetActive(true);
        item.canBeGrabbed = false;
        tileCell.item = item;
        item.transform.position = tileCell.tile.transform.position + Vector3.up;
    }

    public Item GetItem(Vector2 coord) {
        return tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].item;
    }

    public bool HasItem(Vector2 coord) {
        return IsActive(coord) && tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].item != null;
    }

    public void ClearItem(Vector2 coord) {
        tiles[Mathf.RoundToInt(coord.x), Mathf.RoundToInt(coord.y)].item = null;
    }

    public class TileCell {
        public Tile tile;
        public Vector2 coordinates;
        public Biome biome;
        public Item item;
        public float biomeScore = -1000000f;

        public bool active = false;
        public GridCharacter occupiedBy = null;
    }










    //Saving and loading
    private string SAVE_PATH = "";
    private DungeonSettings dungeonSettings = null;
    public void SetSeeds() {
        SetFilePath();

        if(File.Exists(SAVE_PATH)) {
            Debug.Log("Load old");
            StreamReader reader = new StreamReader(SAVE_PATH);
            dungeonSettings = JsonUtility.FromJson<DungeonSettings>(reader.ReadToEnd());
        } else {
            Debug.Log("generate new");
            dungeonSettings = new DungeonSettings();
            dungeonSettings.Randomize();
        }

        dungeonMap.seed = dungeonSettings.dungeonSeed;
        climateMap.temperatureSeed = dungeonSettings.temperatureSeed;
        climateMap.humiditySeed = dungeonSettings.humiditySeed;
    }

    private void SetFilePath() {
        if(Application.isEditor) {
            SAVE_PATH = Application.dataPath + "/dungeonSave.text";
        } else {
            SAVE_PATH = Application.persistentDataPath + "/dungeonSave.text";
        }
    }

    private void OnApplicationQuit() {
        StreamWriter writer = new StreamWriter(SAVE_PATH, false);
        writer.WriteLine(JsonUtility.ToJson(dungeonSettings));
        writer.Close();
        writer.Dispose();
    }

    private class DungeonSettings {
        public int dungeonSeed = 0;
        public int temperatureSeed = 0;
        public int humiditySeed = 0;

        public void Randomize() {
            Random.InitState((int)System.DateTime.Now.Ticks);
            dungeonSeed = Random.Range(0,100000000);
            temperatureSeed = Random.Range(0,100000000);
            humiditySeed = Random.Range(0,100000000);
        }
    }
}
