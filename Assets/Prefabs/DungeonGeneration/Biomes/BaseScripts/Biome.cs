using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    private const int TEMP_WEIGHT = 2;
    private const int HUMIDITY_WEIGHT = 1;

    [SerializeField]
    private float preferredTemperature = 0.0f;
    private float temperatureRange = 20.0f;

    [SerializeField]
    private float preferredHumidity = 0.0f;
    private float humidityRange = 40.0f;

    public Tile[] tileSet;

    public Tile[] blockedTileSet;

    public GridCharacter[] enemySet;
    public Item[] itemSet;

    public float GetBiomeScore(Vector2 coord, ClimateMap climateMap) {
        return (GetTemperatureScore(coord,climateMap)*TEMP_WEIGHT + GetHumidityScore(coord,climateMap)*HUMIDITY_WEIGHT)/2f;
    }

    public float GetTemperatureScore(Vector2 coord, ClimateMap climateMap) {
        float temperature = climateMap.GetTemperature(coord);
        float temperatureScore = temperatureRange - (Mathf.Abs(temperature-preferredTemperature));
        return temperatureScore/temperatureRange;
    }

    public float GetHumidityScore(Vector2 coord, ClimateMap climateMap) {
        float humidity = climateMap.GetHumidity(coord);
        float humidityScore = humidityRange - (Mathf.Abs(humidity-preferredHumidity));
        return humidityScore/humidityRange;
    }
}
