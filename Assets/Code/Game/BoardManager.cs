using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

// Creado siguiendo el manual https://unity3d.com/es/learn/tutorials/projects/2d-roguelike-tutorial/writing-board-manager?playlist=17150
public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int minimun, int maximum)
        {
            this.minimum = minimun;
            this.maximum = maximum;
        }
    }

    public int columns = 50;
    public int rows = 50;
    public Count wallCount = new Count(5, 9);
    public Count objectCount = new Count(5, 9);

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] objectTiles;
    public GameObject[] outerWallTiles;

    public Transform boardHolder;
    public List<Vector3> gridPositions = new List<Vector3>();

    // Inicializa la lista de celdas que servirá para no rellenar una celda dos veces
    void InitialiseGridPositions()
    {
        gridPositions.Clear();

        // Eje x
        for (int x = 1; x < columns - x; x++)
        {
            // Eje y
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        // Eje x
        for (int x = -1; x < columns + 1; x++)
        {
            // Eje y
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // Cierra el mapa con bordes exteriores
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];

        //Evita coger meter dos objetos en la misma celda eliminandola de la colección
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tiles, int minObjectCount, int maxObjectCount)
    {
        int objectCount = Random.Range(minObjectCount, maxObjectCount + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tile = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseGridPositions();

        // Coloca los objectos
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(objectTiles, objectCount.minimum, objectCount.maximum);

    }
}
