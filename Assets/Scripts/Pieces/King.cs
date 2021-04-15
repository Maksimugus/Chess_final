using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);

        foreach (Vector2Int dir in directions)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + dir.x, gridPoint.y + dir.y);
            /*List<GameObject> list = GameManager.instance.otherPlayer.pieces;
            for (int i = 0; i < list.Count; i++)
            {
                GameObject fig = list[i];
                List<Vector2Int> moves = GameManager.instance.MovesForPiece(fig);
                if (!moves.Contains(nextGridPoint))
                {
                    
                }
            }*/
            locations.Add(nextGridPoint);
        }
        
        if(gridPoint.y == 0)
        {
            if (GameManager.instance.flagA1)
            {
                locations.Add(new Vector2Int(gridPoint.x-2, gridPoint.y));
            }
            if (GameManager.instance.flagH1)
            {
                locations.Add(new Vector2Int(gridPoint.x + 2, gridPoint.y));
            }
        }

        if(gridPoint.y == 7)
        {
            if (GameManager.instance.flagA8)
            {
                locations.Add(new Vector2Int(gridPoint.x - 2, gridPoint.y));
            }
            if (GameManager.instance.flagH8)
            {
                locations.Add(new Vector2Int(gridPoint.x + 2, gridPoint.y));
            }
        }

        return locations;
    }
}
