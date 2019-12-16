using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveGameManager
{
   public static void savePlayer(PlayerData playerData, int saveSlot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + saveSlot.ToString() + ".race";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerData loadPlayer(int saveSlot)
    {
        string path = Application.persistentDataPath + saveSlot.ToString() + ".race";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            throw new FileNotFoundException();
        }


    }

    public static void saveOptions(OptionsData optionsData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options.properties";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, optionsData);
        stream.Close();
    }

    public static OptionsData loadOptions()
    {

        string path = Application.persistentDataPath + "/options.properties";
        if (File.Exists(path))
        {
            Debug.Log("Options file found. Loading options.");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;
            OptionsData data = formatter.Deserialize(stream) as OptionsData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("no options found. Loading defaults.");
            OptionsData optionsData = new OptionsData();
            optionsData.masterVolume = 1;
            optionsData.sfxVolume = 1;
            optionsData.musicVolume = 1;
            return optionsData;
        }
    }
}

