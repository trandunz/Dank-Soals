using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TerrainGeneration : MonoBehaviour
{
    [SerializeField] int m_Depth = 40;
    [SerializeField] int m_Width = 2000;
    [SerializeField] int m_Height = 2000;
    [SerializeField] float m_Scale = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData _terrainData)
    {
        _terrainData.heightmapResolution = m_Width + 1;
        _terrainData.size = new Vector3(m_Width, m_Depth, m_Height);

        _terrainData.SetHeights(0, 0, GenerateHeights());

        return _terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[m_Width, m_Height];
        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float) x / m_Width * m_Scale;
        float yCoord = (float) y / m_Height * m_Scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
