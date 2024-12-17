using System;
using System.Collections;
using System.Collections.Generic;
using Tests;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private List<GameObject> maps;
    [SerializeField] private List<GameObject> Cameras;
    [SerializeField] private Transform ceilingTransform;

    [SerializeField] private AStarRunner aStarRunner;
    [SerializeField] private Program aStarProgram;

    private UIGameplayManager uiGameplayManager;

    Coord3D sizeMapMinBound = new Coord3D(-10, 0, -10);
    Coord3D sizeMapMaxBound = new Coord3D(9, 19, 9);
    public Coord3D sizeMapOffset = new Coord3D(10, 0, 10);
    public Coord3D sizeMap { get; private set; }
    public int Score { get; private set; }
    
    public int CurrMapIndx { get; private set; }

    private bool kostilGenerateEatRetry = false;

    public void IncScore()
    {
        Score++;
        uiGameplayManager.SetTextScore(Score);
    }
    private void Awake()
    {
        Score = 0;
        CurrMapIndx = 1; 

        PrepareGameplay();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (kostilGenerateEatRetry)
        {
            kostilGenerateEatRetry = false;
            GenerateNewFood(snakeController.partsSnakeList);
        }
    }

    public class Coord3D
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public Coord3D(int _x, int _y, int _z)
        {
            X = _x; Y = _y; Z = _z;
        }

        public Coord3D(Vector3 v3)
        {
            X = (int)v3.x; Y = (int)v3.y; Z = (int)v3.z;
        }

        public Coord3D()
        {
            X = 0; Y = 0; Z = 0;
        }
    }

    public void GenerateNewFood(List<PartSnakeLogic> snake)
    {
        List<Coord3D> emptyCell = new List<Coord3D>();
        for (int i = sizeMapMinBound.X; i <= sizeMapMaxBound.X; i++)
        {
            for (int j = sizeMapMinBound.Y; j <= sizeMapMaxBound.Y; j++)
            {
                for (int k = sizeMapMinBound.Z; k <= sizeMapMaxBound.Z; k++)
                {
                    emptyCell.Add(new Coord3D(i, j, k));
                }
            }
        }

        int x=0, y=0, z=0, index=0;
        List<int> indexes = new List<int>();
        try
        {
            foreach (PartSnakeLogic part in snake)
            {
                x = (int)part.transform.position.x + sizeMapOffset.X;
                y = (int)part.transform.position.y + sizeMapOffset.Y;
                z = (int)part.transform.position.z + sizeMapOffset.Z;

                index = (sizeMap.Z * sizeMap.Y * x) + (sizeMap.Z * y) + z;
                indexes.Add(index);
            }

            //indexes.Sort((a,b) => b > a ? a : b);
            indexes.Sort();
            indexes.Reverse();

            foreach (var indx in indexes)
            {               
                emptyCell.RemoveAt(indx);
            }

        } catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log($"x={x}, y={y}, z={z}, sizeMap={sizeMap.X * sizeMap.Y * sizeMap.Z}");
            Debug.Log($"index = {index}");
            Debug.Log($"count of emptyCell = {emptyCell.Count}");
            Debug.Log($"size of snake = {snake.Count}");
            kostilGenerateEatRetry = true;
            return;
        }

        int rand = UnityEngine.Random.Range(0, emptyCell.Count);
        Coord3D cellToSpawn = emptyCell[rand];
        GameObject food = Instantiate(foodPrefab);
        food.transform.position = new Vector3(cellToSpawn.X + 0.5f, cellToSpawn.Y + 0.5f, cellToSpawn.Z + 0.5f);
        //aStarProgram.target = new Vector3(food.transform.position.x + sizeMapOffset.X, food.transform.position.y + sizeMapOffset.Y, food.transform.position.z + sizeMapOffset.Z);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiGameplayManager.OpenGameoverPanel(Score);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Play()
    {
        Time.timeScale = 1;
    }

    public GameObject GetCurrentCamera()
    {
        return Cameras[CurrMapIndx];
    }

    public AStarRunner GetAStarRunner()
    {
        return aStarRunner;
    }

    public void PrepareGameplay()
    {
        uiGameplayManager = gameObject.GetComponent<UIGameplayManager>();
        uiGameplayManager.SetTextScore(Score);

        foreach (var m in maps)
        {
            m.SetActive(false);
        } 
        maps[CurrMapIndx].SetActive(true);

        foreach (var c in Cameras)
        {
            c.SetActive(false);
        }
        Cameras[CurrMapIndx].SetActive(true);

        float scaleMap = maps[CurrMapIndx].transform.localScale.x;

        Vector3 mapPos = maps[CurrMapIndx].transform.position;
        float halfSizeOfMap = (ceilingTransform.localScale.x * scaleMap) / 2;
        Vector3 halfSizeOfMapVector3 = new Vector3(halfSizeOfMap, halfSizeOfMap, halfSizeOfMap);

        Vector3 minBound = (mapPos - halfSizeOfMapVector3);
        minBound.y = mapPos.y;
        Vector3 maxBound = mapPos + halfSizeOfMapVector3 - Vector3.one;
        maxBound.y = mapPos.y + halfSizeOfMap * 2 - 1;
        Vector3 offsetBound = (mapPos - halfSizeOfMapVector3) * -1;
        offsetBound.y = mapPos.y * -1;
        sizeMapMinBound = new Coord3D(minBound);
        sizeMapMaxBound = new Coord3D(maxBound);
        sizeMapOffset = new Coord3D(offsetBound);

        sizeMap = new Coord3D(Mathf.Abs(sizeMapMaxBound.X - sizeMapMinBound.X) + 1,
            Mathf.Abs(sizeMapMaxBound.Y - sizeMapMinBound.Y) + 1,
            Mathf.Abs(sizeMapMaxBound.Z - sizeMapMinBound.Z) + 1);
    }
}
