using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
    public static void SaveMap(Text txt,GridMap Map)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/" + txt.text + ".map";
        if (File.Exists(path))
        {
            Debug.Log("file already exists");
        }
        FileStream stream = new FileStream(path, FileMode.Create);
        //---------------------------
        Save data = new Save(Map);
        //------------------------
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static Save Load(Text txt)
    {
        string path = Application.persistentDataPath + "/" + txt.text + ".map";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            Save data = formatter.Deserialize(stream) as Save;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File doesn't exists !");
            return null;
        }
    }


}
