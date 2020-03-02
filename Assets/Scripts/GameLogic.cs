using System;
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
    [SerializeField] private Toggle ScreenToggle;
    List<Node> path;
    [SerializeField] private GameObject LoadPanel;
    [SerializeField] private GameObject MenuPanel;

    void Start()
    {
        if (PlayerPrefs.GetInt("Fullscreen",1) == 1)
        {
            ScreenToggle.isOn = true;
            Screen.fullScreen = true;
        }
        else if (PlayerPrefs.GetInt("Fullscreen") == 0)
        {
            ScreenToggle.isOn = false;
            Screen.fullScreen = false;
        }
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
        path = null;
        if (Grid.MapReadyToWork())
        {
            if (SelectedAlgorithm.value == 0)
            {
                //SetInfo("You've selected A Star algorithm");
                path = Algorithms.AStar(Grid);
            }
            else if (SelectedAlgorithm.value == 1)
            {
                //SetInfo("You've selected Dijkstra's algorithm");
                path = Algorithms.Dijkstra(Grid);
            }
            else
            {
                path = null;
            }


            if (path == null)
            {
                SetInfo("There is no path!");
            }
            else
            {
                foreach (Node node in path)
                {
                    if (node != path[0] && node != path[path.Count - 1])
                    {
                        node.GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                }
                SetInfo("Path Finded!");
            }
        }
        else
        {
            SetInfo("There is no map!");
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
        if (Grid.MapReadyToWork())
        {
            SaveManager.SaveMap(txt, Grid);
            SetInfo("Map saved!");
        }
        else
        {
            SetInfo("You doesn't have a map!");
        }
        
    }
    public void LoadButton(Text txt)
    {
        Save data = SaveManager.Load(txt);
        if (data != null)
            Grid.RecoveryMapFromSaveFile(data);
        else
        {
            SetInfo("There is a problem with your map file!");
        }
    }

    public void FilesList(ScrollRect rect)
    {
        if (rect != null)
        {
            string Path = Application.persistentDataPath + "/";
            foreach (string file in System.IO.Directory.GetFiles(Path, "*.map"))
            {
                Button but = Instantiate(SaveLoadButton,rect.content.transform,false) as Button;
                but.GetComponentInChildren<Text>().text = file.Substring(file.LastIndexOf('/') + 1);
                but.onClick.AddListener( ()=> {
                    LoadButton(but.GetComponentInChildren<Text>());
                    ClosePanel(LoadPanel);
                    OpenPanel(MenuPanel);
                    PauseCamMovement(false);
                    ClearFileList(rect);
                } );
            }
            rect.content.ForceUpdateRectTransforms();
        }

    }

    public void ClearFileList(ScrollRect rect)
    {
        foreach (Button item in rect.content.GetComponentsInChildren<Button>())
        {
            Destroy(item.gameObject);
        }
        rect.content.ForceUpdateRectTransforms();
    }

    public void ClearGridButton()
    {
        if (Grid.GetGridMap() != null)
        {
            SetInfo("Grid removed!");
        }
        else
        {
            SetInfo("There is no grid to remove!");
        }

        Grid.RemoveGrid();

    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void FullscreenUpdate()
    {
        if (ScreenToggle.isOn == true)
        {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("Fullscreen", 1);

        }
        else
        {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }

    public void ClearPath()
    {
        if (path != null)
        {
            foreach (Node node in path)
            {
                if (node != path[0] && node != path[path.Count - 1])
                {
                    if(node != null)
                    node.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
        else
        {
            SetInfo("There is no path!");
        }
    }
}
