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
        string tmp = txt.text;
        BinaryFormatter formatter = new BinaryFormatter();
        if (tmp == "")
        { tmp = "Unknown"; }

        string path = Application.persistentDataPath + "/" + tmp + ".map";
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
        string path = Application.persistentDataPath + "/" + txt.text;
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
            return null;
        }
    }


}
