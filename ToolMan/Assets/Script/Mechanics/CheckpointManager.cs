using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CheckpointManager
{
    static Checkpoint ckpt;
    static CheckpointInfo ckptInfo;
    static string DataDirectory = Application.streamingAssetsPath + "/GameData";
    static string ckptPath = Application.streamingAssetsPath + "/GameData" + "/checkpoint.json";
    static int[] player1ToolNums = { 1, 1, 1, 2, 2, 2 };
    static int[] player2ToolNums = { 1, 1, 2, 2, 3, 3 };
    static int[] lastUnlockedHIntList = { 1, 1, 12, 14, 17, 18 };
    public static void LoadCheckpoint()
    {
        if (!Directory.Exists(DataDirectory) || !File.Exists(ckptPath))
        {
            UpdateCheckpoint(1);
        }
        else
        {
            StreamReader r = new StreamReader(ckptPath);
            string jsonString = r.ReadToEnd();
            ckpt = JsonUtility.FromJson<Checkpoint>(jsonString);
            ckptInfo = new CheckpointInfo(ckpt);
        }
    }

    public static void NextLevel()
    {
        UpdateCheckpoint(ckptInfo.level + 1);
    }
    public static void resetLevel()
    {
        UpdateCheckpoint(1);
    }
    public static void UpdateCheckpoint(int level)
    {
        if (level < 0 || level > 5)
        {
            Debug.Log("Error: Level is not in range of (0, 5).");
            return;
        }
        ckptInfo = new CheckpointInfo(level);
        ckpt = ckptInfo.ToCheckpoint();
        SaveCkeckpoint();
    }

    public static void SaveCkeckpoint()
    {
        if (!Directory.Exists(DataDirectory))
            Directory.CreateDirectory(DataDirectory);
        File.WriteAllText(ckptPath, JsonUtility.ToJson(ckpt));
        //if (!File.Exists(ckptPath))
        //{
        //    File.Create(ckptPath).Close();
        //}
        //else
        //{
        //    Debug.Log("in save, exist!");
            
        //}
    }
    public static CheckpointInfo GetCheckpointInfo()
    {
        return ckptInfo;
    }

    public class CheckpointInfo
    {
        public int level;
        public int player1ToolNum;
        public int player2ToolNum;
        public int lastUnlockedHint;
        public CheckpointInfo(Checkpoint newCkpt)
        {
            setByLevel(int.Parse(newCkpt.level));
        }
        public CheckpointInfo(string level)
        {
            setByLevel(int.Parse(level));
        }
        public CheckpointInfo(int level)
        {
            setByLevel(level);
        }
        void setByLevel(int level)
        {
            this.level = level;
            this.player1ToolNum = player1ToolNums[level];
            this.player2ToolNum = player2ToolNums[level];
            this.lastUnlockedHint = lastUnlockedHIntList[level];
        }
        public Checkpoint ToCheckpoint()
        {
            return new Checkpoint(this.level.ToString());
        }
    }
}
