using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    //we create a new object of GameData type
    GameData gameData = new GameData();

    public void  Awake()
    {
        if (instance == null) instance = this;
        LoadDataFromFile();
    }

    /// <summary>
    /// Method to clear all the save data
    /// </summary>
    public void ClearData()
    {
        DebugLog("GameData Cleared");
        gameData.saveData.Clear();//we 1st clear the dictionary
        CreateData();//and create the new data
    }

    /// <summary>
    /// Method called to load the data from the save
    /// </summary>
    /// <typeparam name="T">Make it generic type</typeparam>
    /// <param name="key">Key of the dictionary to get specific value</param>
    /// <returns></returns>
    public T LoadData<T>(string key)
    {
        return (T)gameData.saveData[key];
    }

    /// <summary>
    /// Mthod to check if the given key has any value
    /// </summary>
    /// <typeparam name="T">Make it generic type</typeparam>
    /// <param name="key">Key of the dictionary to get specific value</param>
    /// <returns></returns>
    public bool HasKey<T>(string key)
    {
        if (gameData.saveData.ContainsKey(key))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Method to store the data to the save
    /// </summary>
    /// <typeparam name="T">Make it generic type</typeparam>
    /// <param name="key">Key of the dictionary to get specific value</param>
    /// <param name="value">Value to stored in the save for the given key</param>
    public void SaveData<T>(string key, T value)
    {
        //check if dictionary has the key
        if (gameData.saveData.ContainsKey(key))
        {
            //if yes then override the value
            gameData.saveData[key] = value;
        }
        else
        {
            //else create the new key,value pair and add to dictionary
            gameData.saveData.Add(key, value);
        }
    }

    /// <summary>
    /// Methodused to create the save data file
    /// </summary>
    public void CreateData()
    {
        //*NOTE*: Make sure the name of save file is specific to your game to avoid any errors//
        //create the file at required path
        FileStream file = File.Create(Application.persistentDataPath + "/saveManager.data");
        DebugLog("Created file");
        //try catch s used to make sure the game do not crash if Creating goes wrong
        try
        {
            //create new BinaryFormatter object
            BinaryFormatter bf = new BinaryFormatter();
            DebugLog("Created BinaryFormatter");
            DebugLog("Data set");
            //serialise the info to the file
            bf.Serialize(file, gameData);
            DebugLog("Serialize gameData");
            DebugLog("GameData saved");
        }
        catch (System.Exception e)
        {
            //if e get any exception the we debug the erro and return
            DebugLog(e.ToString());
            throw;
        }
        finally
        {
            //if file is created
            if (file != null)
            {
                //close it
                file.Close();
            }
        }
    }

    /// <summary>
    /// Load the data from the file we saved
    /// </summary>
    private void LoadDataFromFile()
    {
        //*NOTE*: Make sure the name of save file is specific to your game to avoid any errors//
        //1st we check if save file exist at the given path
        if (File.Exists(Application.persistentDataPath + "/saveManager.data"))
        {
            //create FileStream object
            FileStream file = null;

            try
            {
                //create BinaryFormatter object
                BinaryFormatter bf = new BinaryFormatter();
                //*NOTE*: Make sure the name of save file is specific to your game to avoid any errors//
                //open file from the given path and set file object data to it
                file = File.Open(Application.persistentDataPath + "/saveManager.data", FileMode.Open);
                
                gameData = (GameData)bf.Deserialize(file);//deserialize data and cast it as GameData

                DebugLog("GameData Loaded");
            }
            catch (System.Exception e)
            {
                DebugLog(e.ToString());
                throw;
            }
            finally
            {
                //if file exits
                if (file != null)
                {
                    //close it
                    file.Close();
                }
            }
        }
        else
        {
            DebugLog("No GameData Loaded");
        }
    }

    void DebugLog(string val)
    {
        Debug.Log("<color=green>[SaveManager]</color> " + val);
    }

}

[System.Serializable]
class GameData
{
    public Dictionary<string, object> saveData;
    public GameData()
    {
        saveData = new Dictionary<string, object>();
    }
}