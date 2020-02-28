using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    // map size
    public int n;
    // start and end nodes
    public int[] StartNode;
    public int[] EndNode;
    // Obstacles
    public float[] ObstalcePosX;
    public float[] ObstalcePosY;
    public int[] ObstalceType;

    public Save(GridMap map)
    {
        n = map.MapSize;

        StartNode = new int[2];
        StartNode[0] = (int)map.StartNode.transform.position.x;
        StartNode[1] = (int)map.StartNode.transform.position.y;

        EndNode = new int[2];
        EndNode[0] = (int)map.EndNode.transform.position.x;
        EndNode[1] = (int)map.EndNode.transform.position.y;

        int k = map.Obstacles.Count;
        ObstalcePosX = new float[k];
        ObstalcePosY = new float[k];
        ObstalceType = new int[k];

        for (int i = 0; i < k; i++)
        {
            ObstalcePosX[i] = map.Obstacles[i].transform.position.x;
            ObstalcePosY[i] = map.Obstacles[i].transform.position.y;

            if (map.Obstacles[i].name == "tile1x1(Clone)")
            {
                ObstalceType[i] = 0;
            }
            else if (map.Obstacles[i].name == "tile2x1(Clone)")
            {
                ObstalceType[i] = 1;
            }
            else if (map.Obstacles[i].name == "tile1x2(Clone)")
            {
                ObstalceType[i] = 2;
            }
            else if (map.Obstacles[i].name == "tile2x2(Clone)")
            {
                ObstalceType[i] = 3;
            }
        }
    }
}
