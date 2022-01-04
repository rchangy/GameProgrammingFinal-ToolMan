using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CheckpointManager
{
    static Checkpoint ckpt;
    static CheckpointInfo ckptInfo;
    static string ckptPath = "Assets/Script/Mechanics/checkpoint.json";
    static int[] player1ToolNums = { 1, 1, 2, 2, 2 };
    static int[] player2ToolNums = { 1, 2, 2, 3, 3 };
    static int[] lastUnlockedHIntList = { 1, 15, 17, 20, 21 };
    public static void LoadCheckpoint()
    {
        //StreamReader r = new StreamReader(ckptPath);
        //string jsonString = r.ReadToEnd();
        //ckpt = JsonUtility.FromJson<Checkpoint>(jsonString);
        ckptInfo = new CheckpointInfo(5);

    }

    public static void NextLevel()
    {
        UpdateCheckpoint(ckptInfo.level + 1);
    }
    public static void resetLevel()
    {
        UpdateCheckpoint(1);
    }
    private static void UpdateCheckpoint(int level)
    {
        if (level <= 0 || level > 5)
        {
            Debug.Log("Error: Level is not in range of (1, 5).");
            return;
        }
        ckptInfo = new CheckpointInfo(level);
        ckpt = ckptInfo.ToCheckpoint();
        SaveCkeckpoint();
    }

    public static void SaveCkeckpoint()
    {
        //File.WriteAllText(ckptPath, JsonUtility.ToJson(ckpt));
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
            this.player1ToolNum = player1ToolNums[level - 1];
            this.player2ToolNum = player2ToolNums[level - 1];
            this.lastUnlockedHint = lastUnlockedHIntList[level - 1]; 
        }
        public Checkpoint ToCheckpoint()
        {
            return new Checkpoint(this.level.ToString());
        }
    }
}
