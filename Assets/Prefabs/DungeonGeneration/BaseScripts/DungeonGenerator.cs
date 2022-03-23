using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public const int TILE_WIDTH = 2;

    [SerializeField]
    private Biome[] biomes;

    [SerializeField]
    private int mapSize = 20;

    [SerializeField]
    private bool fillEmpty = true;

    private ClimateMap climateMap;
    private DungeonMap dungeonMap;

    private TileCell[,] tiles;

    private void Start() {
        climateMap = GetComponent<ClimateMap>();
        climateMap.GenerateClimateMap(mapSize);

        dungeonMap = GetComponent<DungeonMap>();
        dungeonMap.GenerateDungeonMap(mapSize);

        generateDungeon();
    }

    private void generateDungeon() {
        tiles = new TileCell[mapSize,mapSize];

        Random.InitState(dungeonMap.seed);

        for(int x = 0; x < mapSize; x++) {
            for(int y = 0; y < mapSize; y++) {
                TileCell tileCell = new TileCell();
                tiles[x,y] = tileCell;

                tileCell.coordinates = new Vector2(x,y);
                assignBiome(tileCell);
                createTile(tileCell);
            }
        }
    }

    private void assignBiome(TileCell tileCell) {
        foreach (Biome biome in biomes)
        {
            float newScore = biome.GetBiomeScore(tileCell.coordinates,climateMap);
            if(newScore > tileCell.biomeScore) {
                tileCell.biomeScore = newScore;
                tileCell.biome = biome;
            }
        }
    }

    private void createTile(TileCell tileCell) {
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
        return tiles[(int) coord.x, (int) coord.y].active;
    }

    public bool IsEmpty(Vector2 coord) {
        return IsActive(coord) && tiles[(int) coord.x, (int) coord.y].occupied;
    }

    private class TileCell {
        public Tile tile;
        public Vector2 coordinates;
        public Biome biome;
        public float biomeScore = -1000000f;

        public bool active = false;
        public bool occupied = false;
    }
}
