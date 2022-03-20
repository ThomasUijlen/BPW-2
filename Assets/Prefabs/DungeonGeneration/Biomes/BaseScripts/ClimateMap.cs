using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Noise;

public class ClimateMap : MonoBehaviour
{
    private float[,] temperatureMap;
    private float[,] humidityMap;

    [Header("Temperature Settings")]

    [SerializeField]
    private int temperatureSeed = 0;
    [SerializeField]
    private float temperatureNoiseFrequency = 1f;

    [SerializeField]
    private float minTemperature = -10f;
    [SerializeField]
    private float maxTemperature = 10f;


    [Header("Humidity Settings")]

    [SerializeField]
    private int humiditySeed = 0;
    [SerializeField]
    private float humidityNoiseFrequency = 1f;

    [SerializeField]
    private float minHumidity = 0f;
    [SerializeField]
    private float maxHumidity = 10f;

    private FastNoise fastNoise = new FastNoise();

    public void GenerateClimateMap(int mapSize)
    {
        fastNoise.SetNoiseType(FastNoise.NoiseType.Simplex);

        temperatureMap = new float[mapSize, mapSize];
        humidityMap = new float[mapSize, mapSize];

        GenerateTemperatureMap();
        GenerateHumidityMap();
    }

    //Temperature -----------------------------------------------------------------------------------
    private void GenerateTemperatureMap()
    {
        fastNoise.SetSeed(temperatureSeed);
        fastNoise.SetFrequency(temperatureNoiseFrequency);

        for (int x = 0; x < temperatureMap.GetLength(0); x++)
        {
            for (int y = 0; y < temperatureMap.GetLength(1); y++)
            {
                float temperature = GenerateTemperatureForCoordinate(x,y);
                temperatureMap[x,y] = temperature;
            }
        }
    }

    private float GenerateTemperatureForCoordinate(int x, int y) {
        float noise = fastNoise.GetNoise2D(x,y);
        float temperature = map(noise,0.0f,1.0f,minTemperature,maxTemperature);
        return temperature;
    }

    public float GetTemperature(Vector2 coord) {
        return temperatureMap[Mathf.RoundToInt(coord.x),Mathf.RoundToInt(coord.y)];
    }



    //Humidity -----------------------------------------------------------------------------------

    private void GenerateHumidityMap()
    {
        fastNoise.SetSeed(humiditySeed);
        fastNoise.SetFrequency(humidityNoiseFrequency);

        for (int x = 0; x < humidityMap.GetLength(0); x++)
        {
            for (int y = 0; y < humidityMap.GetLength(1); y++)
            {
                float humidity = GenerateHumidityForCoordinate(x,y);
                humidityMap[x,y] = humidity;
            }
        }
    }

    private float GenerateHumidityForCoordinate(int x, int y) {
        float noise = fastNoise.GetNoise2D(x,y);
        float humidity = map(noise,0,1,minHumidity,maxHumidity);
        return humidity;
    }

    float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public float GetHumidity(Vector2 coord) {
        return humidityMap[Mathf.RoundToInt(coord.x),Mathf.RoundToInt(coord.y)];
    }
}
