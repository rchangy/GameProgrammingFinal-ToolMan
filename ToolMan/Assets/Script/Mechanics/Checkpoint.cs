using System;

[Serializable]
public class Checkpoint
{
    public string level = "0";
    public Checkpoint(string level)
    {
        this.level = level;
    }
}
