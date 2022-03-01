using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{
    [SerializeField]
    private float preferredTemperature = 0.0f;
    private float temperatureRange = 10.0f;

    [SerializeField]
    private float preferredHumidity = 0.0f;
    private float humidityRange = 10.0f;

    [SerializeField]
    private Object[] Tileset;

    public float GetBiomeScore(Vector2 coord, ClimateMap climateMap) {
        return (GetTemperatureScore(coord,climateMap) + GetHumidityScore(coord,climateMap))/2f;
    }

    public float GetTemperatureScore(Vector2 coord, ClimateMap climateMap) {
        float temperature = climateMap.GetTemperature(coord);
        float temperatureScore = temperatureRange - (Mathf.Clamp(Mathf.Abs(temperature-preferredTemperature),0,temperatureRange));
        return temperatureScore/temperatureRange;
    }

    public float GetHumidityScore(Vector2 coord, ClimateMap climateMap) {
        float humidity = climateMap.GetHumidity(coord);
        float humidityScore = humidityRange - (Mathf.Clamp(Mathf.Abs(humidity-preferredHumidity),0,humidityRange));
        return humidityScore/humidityRange;
    }
}
