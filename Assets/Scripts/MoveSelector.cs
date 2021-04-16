using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    void Start()
    {
        enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);

    }

    void Update()
    {
        GameManager.instance.CanWhiteKingRokA1();
        GameManager.instance.CanWhiteKingRokH1();
        GameManager.instance.CanBlackKingRokA8();
        GameManager.instance.CanBlackKingRokH8();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
            if (Input.GetMouseButtonDown(0))
            {
                GameObject piece = GameManager.instance.pieces[gridPoint.x, gridPoint.y];
                if(piece!=null)
                    if (piece.Equals(movingPiece))
                    {
                        CancelMove();
                    }

                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                Piece mP = movingPiece.GetComponent<Piece>();
                Vector2Int startGridPoint = GameManager.instance.GridForPiece(movingPiece);
                if (mP.type == PieceType.Pawn && piece == null)
                {
                    if (gridPoint.x - startGridPoint.x == 1)
                    {
                        GameManager.instance.CapturePieceAt(new Vector2Int(gridPoint.x, startGridPoint.y));
                    }
                    if (gridPoint.x - startGridPoint.x == -1)
                    {
                        GameManager.instance.CapturePieceAt(new Vector2Int(gridPoint.x, startGridPoint.y));
                    }
                }

                if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                {
                    GameManager.instance.Move(movingPiece, gridPoint);
                }
                else
                {
                    GameManager.instance.CapturePieceAt(gridPoint);
                    GameManager.instance.Move(movingPiece, gridPoint);
                }

                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    private void CancelMove()
    {
        enabled = false;

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        enabled = true;

        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        enabled = false;
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);

        movingPiece = null;
        GameManager.instance.NextPlayer();
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
