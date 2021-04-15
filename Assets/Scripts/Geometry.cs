using UnityEngine;

public class Geometry
{
    static public Vector3 PointFromGrid(Vector2Int gridPoint)
    {
        float x = -3.48f + 1.0001f*gridPoint.x;
        float z = -3.56f + 1.0001f*gridPoint.y;
        return new Vector3(x, 0, z);
    }

    static public Vector2Int GridPoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

    static public Vector2Int GridFromPoint(Vector3 point)
    {
        int col = Mathf.FloorToInt(4.2f + point.x);
        int row = Mathf.FloorToInt(4.2f + point.z);
        return new Vector2Int(col, row);
    }
}
