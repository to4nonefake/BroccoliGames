using System.Collections.Generic;

[System.Serializable]
public class MapData
{
    public List<Tile> List;
}

[System.Serializable]
public class Tile {

    public string Id;
    public string Type;
    public float X;
    public float Y;
    public float Width;
    public float Height;

    public Tile() {
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
    }

}