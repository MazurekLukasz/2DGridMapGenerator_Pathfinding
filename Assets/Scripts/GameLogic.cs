﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private Text NoOfBlocks;
    [SerializeField] private Text NoOfObstacles;
    [SerializeField] private Text Info;
    [SerializeField] private Dropdown SelectedAlgorithm;

    [SerializeField] private GridMap Grid;

    [SerializeField] private Button SaveLoadButton;

    [SerializeField] private CameraMovement Cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public int CheckInput(Text t, int min)
    {
        int num;

        if (t.text != "")
        {
            num = int.Parse(t.text);
            if (num < min)
            {
                num = min;
            }
        }
        else
        {
            num = min;
        }
        return num;
    }

    public void GenerateButton()
    {
        int n = CheckInput(NoOfBlocks, 10);
        int m = CheckInput(NoOfObstacles, 0);

        Grid.CreateNewGrid(n,m);
        SetInfo("Map Generated");
    }

    void SetInfo(string txt)
    {
        Info.text = txt;
    }

    public void RunAlgorithmButton()
    {
        List<Node> path;
        if (SelectedAlgorithm.value == 0)
        {
            SetInfo("You've selected A Star algorithm");
            path = Algorithms.AStar(Grid);
        }
        else if (SelectedAlgorithm.value == 1)
        {
            SetInfo("Second algorithm");
            path = Algorithms.Dijkstra(Grid);
        }
        else
        {
            path = null;
        } 


        if (path == null)
        {
            SetInfo("There is no path");
        }
        else
        {
            foreach (Node node in path)
            {
                if (node != path[0] && node != path[path.Count - 1])
                {
                    node.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void PauseCamMovement(bool pause = false)
    {
        Cam.Pause = pause;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void SaveButton(Text txt)
    {
        SaveManager.SaveMap(txt,Grid);
    }
    public void LoadButton(Text txt)
    {
        Save data = SaveManager.Load(txt);
        Grid.RecoveryMapFromSaveFile(data);
    }

    public void FilesList(ScrollRect rect)
    {
        //Vector2 pos = new Vector2(rect.transform.position.x, rect.transform.position.x);
        string path = Application.persistentDataPath + "/";
        Vector3 pos = new Vector3(100,-14,0); 
        foreach (string file in System.IO.Directory.GetFiles(path, "*.map"))
        {
            
            Button but = Instantiate(SaveLoadButton,rect.content);
            but.GetComponentInChildren<Text>().text = file.Substring(file.LastIndexOf('/') + 1);
            but.transform.localPosition = pos;
            pos = pos - new Vector3(0,28,0); 
            //but.transform.localPosition = 
        }
    }

    public void ClearFileList(ScrollRect rect)
    {
        foreach (Button item in rect.content.GetComponentsInChildren<Button>())
        {
            Destroy(item.gameObject);
        }
    }

    public void ClearGridButton()
    {
        Grid.RemoveGrid();
    }

}