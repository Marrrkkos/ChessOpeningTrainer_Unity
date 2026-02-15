using UnityEngine;
[System.Serializable]
public class Player
{
    public string name;
    public int dwz;
    public bool color;

    public Player(string name, int dwz, bool color)
    {
        this.name = name;
        this.dwz = dwz;
        this.color = color;
    }
    

}
