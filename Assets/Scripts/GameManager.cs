using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;

    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whiteRook;
    public GameObject whitePawn;

    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackPawn;
    
    public GameObject view;

    public GameObject[,] pieces;
    private List<GameObject> movedPawns;

    private Player white;
    private Player black;
    public Player currentPlayer;
    public Player otherPlayer;
    

    public bool flagA1;
    public bool flagH1;
    public bool flagA8;
    public bool flagH8;

    internal object MovesForPiece(Piece fig)
    {
        throw new NotImplementedException();
    }

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        pieces = new GameObject[8, 8];
        movedPawns = new List<GameObject>();

        white = new Player("white", true);
        black = new Player("black", false);

        currentPlayer = white;
        otherPlayer = black;

        InitialSetup();
    }

    private void InitialSetup()
    {
        AddPiece(whiteRook, white, 0, 0);
        AddPiece(whiteKnight, white, 1, 0);
        AddPiece(whiteBishop, white, 2, 0);
        AddPiece(whiteQueen, white, 3, 0);
        AddPiece(whiteKing, white, 4, 0);
        AddPiece(whiteBishop, white, 5, 0);
        AddPiece(whiteKnight, white, 6, 0);
        AddPiece(whiteRook, white, 7, 0);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
        }

        AddPiece(blackRook, black, 0, 7);
        AddPiece(blackKnight, black, 1, 7);
        AddPiece(blackBishop, black, 2, 7);
        AddPiece(blackQueen, black, 3, 7);
        AddPiece(blackKing, black, 4, 7);
        AddPiece(blackBishop, black, 5, 7);
        AddPiece(blackKnight, black, 6, 7);
        AddPiece(blackRook, black, 7, 7);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(blackPawn, black, i, 6);
        }

        flagA1 = true;
        flagH1 = true;
        flagA8 = true;
        flagH8 = true;
    }

    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }

        if(pieceComponent.type == PieceType.King)
        {
            if(flagA1 && gridPoint.x == 2 && gridPoint.y == 0)
            {
                GameObject castle = PieceAtGrid(new Vector2Int(0, 0));
                Vector2Int gp = new Vector2Int(3, 0);
                board.MovePiece(castle, gp);
                pieces[0, 0] = null;
                pieces[3, 0] = castle;
            }
            if (flagH1 && gridPoint.x == 6 && gridPoint.y == 0)
            {
                GameObject castle = PieceAtGrid(new Vector2Int(7, 0));
                Vector2Int gp = new Vector2Int(5, 0);
                board.MovePiece(castle, gp);
                pieces[7, 0] = null;
                pieces[5, 0] = castle;
            }
            if (flagA8 && gridPoint.x == 2 && gridPoint.y == 7)
            {
                GameObject castle = PieceAtGrid(new Vector2Int(0, 7));
                Vector2Int gp = new Vector2Int(3, 7);
                board.MovePiece(castle, gp);
                pieces[0, 7] = null;
                pieces[3, 7] = castle;
            }
            if (flagH8 && gridPoint.x == 6 && gridPoint.y == 7)
            {
                GameObject castle = PieceAtGrid(new Vector2Int(7, 7));
                Vector2Int gp = new Vector2Int(5, 7);
                board.MovePiece(castle, gp);
                pieces[7, 7] = null;
                pieces[5, 7] = castle;
            }

        }

        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePiece(piece, gridPoint);
    }

    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }

    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            Debug.Log(currentPlayer.name + " wins!");
            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
        }
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;
        Destroy(pieceToCapture);
    }

    

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null) {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
        //System.Threading.Thread.Sleep(1000);
        if(currentPlayer == black)
        {
            view.transform.position = new Vector3(0.02f, 6f, 6.5f);
            view.transform.rotation = new Quaternion(0, 180, -76.5f, 0);

        }
        else
        {
            view.transform.position = new Vector3(0.02f, 6f, -6.5f);
            view.transform.rotation = new Quaternion(76.5f, 0, 0, 180);
        }
        
    }

    public void CanWhiteKingRokA1()
    {
        if (pieces[4, 0] == null || pieces[0, 0] == null)
            flagA1 = false;
    }

    public void CanWhiteKingRokH1()
    {
        if (pieces[4, 0] == null  || pieces[7, 0] == null)
            flagH1 = false;
    }

    public void CanBlackKingRokA8()
    {
        if ( pieces[4, 7] == null || pieces[0, 7] == null)
            flagA8 = false;
    }

    public void CanBlackKingRokH8()
    {
        if(pieces[4, 7] == null || pieces[7, 7] == null)
            flagH8 = false;
    }
}
