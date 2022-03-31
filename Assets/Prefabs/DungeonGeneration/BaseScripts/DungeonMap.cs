using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noise;
public class DungeonMap : MonoBehaviour
{
    [SerializeField]
    public int seed = 0;

    [SerializeField]
    private float roomSpawnChance = 10f;

    [SerializeField]
    private int minRoomSize = 2;
    [SerializeField]
    private int maxRoomSize = 5;

    [SerializeField]
    private float enemySpawnChance = 10f;
    [SerializeField]
    private float itemSpawnChance = 10f;


    private FastNoise fastNoise = new FastNoise();

    private bool[,] dungeonMap;

    public void GenerateDungeonMap(int mapSize)
    {
        fastNoise.SetNoiseType(FastNoise.NoiseType.Simplex);
        fastNoise.SetSeed(seed);
        fastNoise.SetFrequency(1000);

        dungeonMap = new bool[mapSize, mapSize];
        for(int x = 0; x < mapSize; x++) {
            for(int y = 0; y < mapSize; y++) {
                dungeonMap[x,y] = false;
            }
        }

        Room[] rooms = GenerateRooms();
        GeneratePaths(rooms);
    }

    private Room[] GenerateRooms() {
        List<Room> rooms = new List<Room>();

        for (int x = 0; x < dungeonMap.GetLength(0); x++)
        {
            for (int y = 0; y < dungeonMap.GetLength(1); y++)
            {
                if(fastNoise.GetNoise2D(x,y)*100f < roomSpawnChance) {
                    Room room = new Room();
                    room.center = new Vector2(x,y);
                    room.size = new Vector2(
                        map(fastNoise.GetNoise2D(x,y),0f,1f,minRoomSize,maxRoomSize),
                        map(fastNoise.GetNoise2D(x,y),0f,1f,minRoomSize,maxRoomSize)
                    );

                    rooms.Add(room);
                    PlaceRoom(room);
                }
            }
        }

        return rooms.ToArray();
    }

    private void PlaceRoom(Room room) {
        for(int x = 0; x < room.size.x; x++) {
            for(int y = 0; y < room.size.y; y++) {
                Vector2 coord = new Vector2(x+room.center.x-room.size.x/2f,y+room.center.y-room.size.y/2f);
                activateTile(coord);
            }
        }
    }

    private void GeneratePaths(Room[] rooms) {
        if(rooms.Length < 2) return;

        List<Room> unconnectedRooms = new List<Room>(rooms);

        Vector2 startingCoord = unconnectedRooms[0].center;
        unconnectedRooms.RemoveAt(0);

        while(unconnectedRooms.Count > 0) {
            //Find closest room
            Room closestRoom = null;
            float closestDistance = 0f;
            foreach(Room room in unconnectedRooms) {
                float distance = Vector2.Distance(startingCoord, room.center);
                if(distance < closestDistance || closestRoom == null) {
                    closestRoom = room;
                    closestDistance = distance;
                }
            }

            PlacePath(startingCoord, closestRoom.center);
            unconnectedRooms.Remove(closestRoom);
            startingCoord = closestRoom.center;
        }
    }

    private void PlacePath(Vector2 a, Vector2 b) {
        int x = (int) a.x;
        int y = (int) a.y;

        int targetX = (int) b.x;
        int targetY = (int) b.y;

        while (!(x == targetX && y == targetY)) {
            int[] direction = getRandomDirection(targetX-x,targetY-y, (x+y)*100);

            x += direction[0];
            y += direction[1];

            activateTile(new Vector2(x,y));
        }
    }

    private int[] getRandomDirection(int xDifference, int yDifference, int seed) {
        Random.InitState(seed);

        int xDirection;
        if(xDifference == 0) xDirection = 0; else if (xDifference >= 0) xDirection = 1; else xDirection = -1;
        int yDirection;
        if(yDifference == 0) yDirection = 0; else if(yDifference >= 0) yDirection = 1; else yDirection = -1;

        if(xDifference == 0) return new int[]{0,yDirection};
        if(yDifference == 0) return new int[]{xDirection,0};
        if(Random.Range(0f,1f) >= 0.5) return new int[]{xDirection,0}; else return new int[]{0,yDirection};
    }

    private void activateTile(Vector2 coord) {
        if(coord.x < 0 || coord.y < 0 || coord.x >= dungeonMap.GetLength(0) || coord.y >= dungeonMap.GetLength(1)) return;
        dungeonMap[(int) Mathf.Floor(coord.x), (int) Mathf.Floor(coord.y)] = true;
    }

    public bool IsActive(Vector2 coord) {
        if(coord.x < 0 || coord.y < 0 || coord.x >= dungeonMap.GetLength(0) || coord.y >= dungeonMap.GetLength(1)) return false;
        return dungeonMap[(int) Mathf.Floor(coord.x), (int) Mathf.Floor(coord.y)];
    }

    float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    private class Room {
        public Vector2 center;
        public Vector2 size;
    }









    //Object spawning
    public void PlaceObjects(DungeonGenerator dungeonGenerator) {
        fastNoise.SetNoiseType(FastNoise.NoiseType.Simplex);
        fastNoise.SetSeed(seed);
        fastNoise.SetFrequency(100000);

        for(int x = 0; x < dungeonGenerator.mapSize; x++) {
            for(int y = 0; y < dungeonGenerator.mapSize; y++) {
                DungeonGenerator.TileCell tileCell = dungeonGenerator.tiles[x,y];

                spawnEnemy(tileCell);
            }
        }

        fastNoise.SetSeed(seed+10000);
        for(int x = 0; x < dungeonGenerator.mapSize; x++) {
            for(int y = 0; y < dungeonGenerator.mapSize; y++) {
                DungeonGenerator.TileCell tileCell = dungeonGenerator.tiles[x,y];

                spawnItem(tileCell);
            }
        }
    }

    private void spawnEnemy(DungeonGenerator.TileCell tileCell) {
        if(!tileCell.active || tileCell.biome.enemySet.Length == 0 || fastNoise.GetNoise2D(tileCell.coordinates.x,tileCell.coordinates.y)*100f > enemySpawnChance) return;
        GridCharacter character = tileCell.biome.enemySet[Random.Range(0,tileCell.biome.enemySet.Length)];
        tileCell.occupiedBy = Instantiate(character,new Vector3(tileCell.coordinates.x,0,tileCell.coordinates.y)*DungeonGenerator.TILE_WIDTH + Vector3.up, Quaternion.Euler(0,0,0));
    }

    private void spawnItem(DungeonGenerator.TileCell tileCell) {
        if(!tileCell.active || tileCell.biome.itemSet.Length == 0 || fastNoise.GetNoise2D(tileCell.coordinates.x,tileCell.coordinates.y)*100f > itemSpawnChance) return;
        Item item = tileCell.biome.itemSet[Random.Range(0,tileCell.biome.itemSet.Length)];
        tileCell.item = Instantiate(item,new Vector3(tileCell.coordinates.x,0,tileCell.coordinates.y)*DungeonGenerator.TILE_WIDTH + Vector3.up, Quaternion.Euler(0,0,0));
        tileCell.item.canBeGrabbed = false;
    }
}