using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridMap : MonoBehaviour
{
    //private int n = 10;
    private GameObject[,] Grid;
    //private float BlockSize = 1;
    [SerializeField] private List<GameObject> ObstaclePrefabs;
    [SerializeField] private GameObject BlockPrefab;
    [SerializeField] private CameraMovement Cam;
    public Node StartNode { get;  private set; }
    public Node EndNode { get; private set; }
    public int MapSize { get;  set; }
    public int NumberOfObstacle { get; set; }
    public List<GameObject> Obstacles { get; private set; } = new List<GameObject>();

    public void CreateNewGrid(int n, int m)
    {
        RemoveGrid();
        MapSize = n;
        NumberOfObstacle = m;
        Grid = new GameObject[n, n];

        CreateGrid(n);

        ObstaclesGenerationAlgorithm();
        RandomStartEndNodes(n);

        if (StartNode != null)
        { Cam.ChangePosition(new Vector2(StartNode.gameObject.transform.position.x, StartNode.gameObject.transform.position.y));}    
    }
    void CreateGrid(int n)
    {
        for (int row = 0; row < n; row++)
        {
            for (int col = 0; col < n; col++)
            {
                Vector2 pos = new Vector2(col, row);

                Grid[col, row] = Instantiate(BlockPrefab, pos, new Quaternion(), transform);
                Grid[col, row].GetComponent<Node>().InitNode(col, row);
            }
        }
    }

    void RandomStartEndNodes(int n)
    {
        if (CountFreeBlocks() >= 2)
        {
            StartNode = RandomNode(n);
            EndNode = RandomNode(n);

            StartNode.GetComponent<SpriteRenderer>().color = Color.green;
            EndNode.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            Debug.Log("no free points");
        }
    }

    Node RandomNode(int n)
    {
        Vector2Int block;
        do
        {
            block = new Vector2Int(Random.Range(0, n), Random.Range(0, n));
        }
        while (Grid[block.x, block.y].GetComponent<Node>().Occupied || Grid[block.x, block.y] == StartNode);

        return Grid[block.x, block.y].GetComponent<Node>();
    }

    public void ObstaclesGenerationAlgorithm()
    {
        if (Grid == null)
        {
            Debug.Log("Create Grid First !");
        }
        else
        {
            int obstacles = 0;

            while (obstacles < NumberOfObstacle)
            {
                int freeBlocks = CountFreeBlocks();
                if (freeBlocks <= 0)
                {
                    return;
                }
                Vector2Int pos = new Vector2Int(Random.Range(0, MapSize), Random.Range(0, MapSize));
                int ObstaclePrefab = Random.Range(0, 4);

                if (CreateObstacle(ObstaclePrefab, pos))
                {
                    obstacles++;
                }

            }
        }

    }

    public bool CreateObstacle(int obstaclePref, Vector2Int pos)
    {

        switch (obstaclePref)
        {
            case 0:// block 1x1
                {
                    if (!ReturnNode(pos.x, pos.y).Occupied)
                    {
                        ReturnNode(pos.x, pos.y).Occupied = true;
                        Obstacles.Add(Instantiate(ObstaclePrefabs[0], new Vector2(pos.x, pos.y), new Quaternion(), transform));
                        return true;
                    }
                    break;
                }
            case 1:// block 2x1
                {
                    if (pos.x+1 < MapSize)
                    {
                        if (!ReturnNode(pos.x, pos.y).Occupied && !ReturnNode(pos.x+1, pos.y).Occupied)
                        {
                            ReturnNode(pos.x, pos.y).Occupied = true;
                            ReturnNode(pos.x+1, pos.y).Occupied = true;
                            Obstacles.Add(Instantiate(ObstaclePrefabs[1], new Vector2(pos.x + 0.5f, pos.y), new Quaternion(), transform));
                            return true;
                        }
                    }
                    break;
                }
            case 2:
                {
                    if (pos.y - 1 >= 0)
                    {
                        if (!ReturnNode(pos.x, pos.y).Occupied && !ReturnNode(pos.x, pos.y-1).Occupied)
                        {
                            ReturnNode(pos.x, pos.y).Occupied = true;
                            ReturnNode(pos.x, pos.y-1).Occupied = true;
                            Obstacles.Add(Instantiate(ObstaclePrefabs[2], new Vector2(pos.x, pos.y -0.5f), new Quaternion(), transform));
                            return true;
                        }
                    }
                    break;
                }
            case 3: // block 2x2
                {
                    if (pos.x+1 < MapSize && pos.x-1 >= 0 && pos.y+1 < MapSize && pos.y-1 >= 0)
                    {
                        if (!ReturnNode(pos.x, pos.y).Occupied && !ReturnNode(pos.x + 1, pos.y).Occupied &&
                                !ReturnNode(pos.x, pos.y - 1).Occupied && !ReturnNode(pos.x + 1, pos.y - 1).Occupied)
                        {
                            ReturnNode(pos.x, pos.y).Occupied = true;
                            ReturnNode(pos.x + 1, pos.y).Occupied = true;
                            ReturnNode(pos.x, pos.y - 1).Occupied = true;
                            ReturnNode(pos.x + 1, pos.y - 1).Occupied = true;
                            Obstacles.Add(Instantiate(ObstaclePrefabs[3], new Vector2(pos.x + 0.5f, pos.y - 0.5f), new Quaternion(), transform));
                            return true;
                        }
                    }
                    break;
                }
        }
        return false;
    }

    int CountFreeBlocks()
    {
        int counter = 0;
        foreach (var item in Grid)
        {
            if (!item.GetComponent<Node>().Occupied)
            {
                counter++;
            }
        }
        return counter;
    }

    public void RemoveGrid()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        Obstacles.Clear();
        Grid = null;
        StartNode = null;
        EndNode = null;
    }

    public GameObject[,] GetGridMap()
    {
        return Grid;
    }

    public Node ReturnNode(int x,int y)
    {
        return Grid[x,y].GetComponent<Node>();
    }

    public void RecoveryMapFromSaveFile(Save data)
    {
        RemoveGrid();
        MapSize = data.n;
        NumberOfObstacle = data.ObstalceType.Length;

        Grid = new GameObject[data.n, data.n];
        CreateGrid(data.n);
        StartNode = ReturnNode(data.StartNode[0], data.StartNode[1]);
        EndNode = ReturnNode(data.EndNode[0], data.EndNode[1]);

        StartNode.GetComponent<SpriteRenderer>().color = Color.green;
        EndNode.GetComponent<SpriteRenderer>().color = Color.red;

        int k = data.ObstalceType.Length;

        for (int i = 0; i < k; i++)
        {
            if (data.ObstalceType[i] == 0)
            {
                Obstacles.Add(Instantiate(ObstaclePrefabs[0], new Vector2(data.ObstalcePosX[i], data.ObstalcePosY[i]), new Quaternion(), transform));
                ReturnNode((int)data.ObstalcePosX[i], (int)data.ObstalcePosY[i]).Occupied = true;
            }
            else if (data.ObstalceType[i] == 1)
            {
                Obstacles.Add(Instantiate(ObstaclePrefabs[1], new Vector2(data.ObstalcePosX[i], data.ObstalcePosY[i]), new Quaternion(), transform));
                ReturnNode((int)(data.ObstalcePosX[i] - 0.5f), (int)data.ObstalcePosY[i]).Occupied = true;
                ReturnNode((int)(data.ObstalcePosX[i] + 0.5f), (int)data.ObstalcePosY[i]).Occupied = true;
            }
            else if (data.ObstalceType[i] == 2)
            {
                Obstacles.Add(Instantiate(ObstaclePrefabs[2], new Vector2(data.ObstalcePosX[i], data.ObstalcePosY[i]), new Quaternion(), transform));
                ReturnNode((int)data.ObstalcePosX[i], (int)(data.ObstalcePosY[i] - 0.5f)).Occupied = true;
                ReturnNode((int)data.ObstalcePosX[i], (int)(data.ObstalcePosY[i] + 0.5f)).Occupied = true;
            }
            else if (data.ObstalceType[i] == 3)
            {
                Obstacles.Add(Instantiate(ObstaclePrefabs[3], new Vector2(data.ObstalcePosX[i], data.ObstalcePosY[i]), new Quaternion(), transform));
                ReturnNode((int)(data.ObstalcePosX[i]+0.5f), (int)(data.ObstalcePosY[i] + 0.5f)).Occupied = true;
                ReturnNode((int)(data.ObstalcePosX[i] - 0.5f), (int)(data.ObstalcePosY[i] + 0.5f)).Occupied = true;
                ReturnNode((int)(data.ObstalcePosX[i] + 0.5f), (int)(data.ObstalcePosY[i] - 0.5f)).Occupied = true;
                ReturnNode((int)(data.ObstalcePosX[i] - 0.5f), (int)(data.ObstalcePosY[i] - 0.5f)).Occupied = true;
            }

        }


        if (StartNode != null)
        { Cam.ChangePosition(new Vector2(StartNode.gameObject.transform.position.x, StartNode.gameObject.transform.position.y)); }

    }
}
