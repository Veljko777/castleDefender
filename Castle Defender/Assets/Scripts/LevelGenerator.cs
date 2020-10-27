using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //prefabs
    public GameObject dirtTilePrefab;
    public GameObject stoneTilePrefab;
    public GameObject woodTilePrefab;
    public GameObject castlePrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    //settings
    public Tile[,] grid;
    public int rows { get; private set; } = 12;
    public int columns { get; private set; } = 25;
    private float nextActionTime;
    public float TileSize
    {
        get { return dirtTilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    int numOfEnemies;
    int spawnedEnemies;

    void Awake()
    {
        grid = new Tile[columns, rows];
        SetCameraBasedOnRatio();
        GenerateTiles();
        GenerateBase();
    }

    void Start()
    {
        spawnedEnemies = 0;
        nextActionTime = UnityEngine.Random.Range(4, 8);
        numOfEnemies = GameLogic.numOfEnemies;
        GeneratePlayer();
        GenerateEnemies();
    }
    void Update()
    {
        if (numOfEnemies > spawnedEnemies)
        {
            if (Time.timeSinceLevelLoad > nextActionTime)
            {
                nextActionTime += UnityEngine.Random.Range(4, 8); ;
                GenerateEnemies();
            }
        }
    }

    private void GenerateTiles()
    {
        for (int i = 0; i < columns; i++)
        {
            int safePassage = UnityEngine.Random.Range(1, rows - 1);//make at least one safe passage
            for (int j = 0; j < rows; j++)
            {
                CreateTile(i, j,safePassage);
            }
        }
    }

    private void CreateTile(int width, int height,int safePassage)
    {
        GameObject newTile;
        if(width<1|| width>=columns-1|| height == 0 || height == rows - 1) //create map border
        {
            newTile = Instantiate(stoneTilePrefab);
            newTile.name = "stone";
        }
       else if (width < 4|| width%2==1)//create every second row and base lines without obsticles
        {
            newTile =Instantiate(dirtTilePrefab);
            newTile.name = "dirt";
        }
        else //create obsticles
        {
            if (height == safePassage) //safe passage
            {
                newTile = Instantiate(dirtTilePrefab);
                newTile.name = "dirt";
            }
            else //random generated obsticles or passages
            {
                int x = UnityEngine.Random.Range(0, 3);
                newTile = x == 0 ? Instantiate(dirtTilePrefab) : x == 1 ? Instantiate(woodTilePrefab) : Instantiate(stoneTilePrefab);
                newTile.name = x == 0 ? "dirt" : x == 1 ? "wood" : "stone";
            }
        }

        newTile.transform.position = new Vector3( TileSize* width+0.5f , TileSize* height+TileSize-0.5f, 1);

        //add tile to grid
        Tile tile= new Tile(newTile.transform.position, new Vector2Int(width, height));
        tile.isWall = newTile.name=="dirt"?false:true;
        grid[width, height] = tile;
    }

    private void GenerateBase()
    {
        GameObject newBase = Instantiate(castlePrefab);
        newBase.transform.position = new Vector3(TileSize*2, TileSize*9,0); //set base position at 2nd column and 9th row
        newBase.name = "Base";
    }

    private void GeneratePlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        newPlayer.transform.position = new Vector3(TileSize*3-0.5f,TileSize*6+0.5f, 0); //set player to the center of tile at column 3 and row 6
        newPlayer.name = "Player";
    }

    private void GenerateEnemies()
    {
        int enemiesPerWave = UnityEngine.Random.Range(3,6);
        int remainingEnemiesToSpawn = numOfEnemies - spawnedEnemies;
        int enemiesToSpawnPerWave = enemiesPerWave <= remainingEnemiesToSpawn ? enemiesPerWave : remainingEnemiesToSpawn;
        for (int i = 0; i <enemiesToSpawnPerWave; i++)
        {
            int spawningArea = UnityEngine.Random.Range(1,rows-1);
            Vector3 pos = grid[23,spawningArea].worldPos;
            GameObject newEnemy = Instantiate(enemyPrefab);
            newEnemy.transform.position =new Vector3(pos.x,pos.y, 0);
            spawnedEnemies++;
            newEnemy.name = "Enemy " + spawnedEnemies;
        }
    }

    private void SetCameraBasedOnRatio()
    {
        float size = Camera.main.orthographicSize;
        float ratio = Camera.main.aspect;
        Camera.main.transform.position = new Vector3(size * ratio, size, -10);
    }
}
