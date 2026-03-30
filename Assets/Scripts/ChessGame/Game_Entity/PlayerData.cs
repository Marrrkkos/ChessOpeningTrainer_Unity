using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public string name;
    public int dwz;
    public bool color;

    public PlayerData(string name, int dwz, bool color)
    {
        this.name = name;
        this.dwz = dwz;
        this.color = color;
    }
    

}
