
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        int forwardDirection = GameManager.instance.currentPlayer.forward;
        Vector2Int forwardOne = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
        if (!GameManager.instance.PieceAtGrid(forwardOne))
        {
            locations.Add(forwardOne);
        }

        Vector2Int forwardTwo = new Vector2Int(gridPoint.x, gridPoint.y + 2 * forwardDirection);
        if (!GameManager.instance.HasPawnMoved(gameObject) && !GameManager.instance.PieceAtGrid(forwardTwo))
        {
            locations.Add(forwardTwo);
        }

        Vector2Int forwardRL = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardRL))
        {
            locations.Add(forwardRL);
        }

        Vector2Int forwardLR = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (GameManager.instance.PieceAtGrid(forwardLR))
        {
            locations.Add(forwardLR);
        }

        Vector2Int forwardAisleRL = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        if (gridPoint.x < 7)
        {
            GameObject obj1 = GameManager.instance.pieces[gridPoint.x + 1, gridPoint.y];
            if (obj1 && !GameManager.instance.PieceAtGrid(forwardAisleRL))
            {
                Piece piece = obj1.GetComponent<Piece>();

                if (piece.type == PieceType.Pawn && piece.cancapture)
                {
                    if (GameManager.instance.currentPlayer.name == "white" && gridPoint.y == 4)
                    {
                        locations.Add(forwardAisleRL);
                    }

                    if (GameManager.instance.currentPlayer.name == "black" && gridPoint.y == 3)
                    {
                        locations.Add(forwardAisleRL);
                    }
                }
            }
        }

        Vector2Int forwardAisleLR = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        if (gridPoint.x > 0)
        {
            GameObject obj2 = GameManager.instance.pieces[gridPoint.x - 1, gridPoint.y];
            if (obj2 && !GameManager.instance.PieceAtGrid(forwardAisleLR))
            {
                Piece piece = obj2.GetComponent<Piece>();

                if (piece.type == PieceType.Pawn && piece.cancapture)
                {
                    if (GameManager.instance.currentPlayer.name == "white" && gridPoint.y == 4)
                    {
                        locations.Add(forwardAisleLR);
                    }

                    if (GameManager.instance.currentPlayer.name == "black" && gridPoint.y == 3)
                    {
                        locations.Add(forwardAisleLR);
                    }
                }
            }
        }

        return locations;
    }
}
