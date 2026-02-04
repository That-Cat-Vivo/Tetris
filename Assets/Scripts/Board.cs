using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public TetrisManager tetrisManager;
    public TetronimoData[] tetronimos;

    Pieces activePiece;

    public Tilemap tilemap;

    public Vector2Int boardSize;
    public Pieces prefabPiece;
    public Vector2Int startPosition;

    public float dropInterval = 0.5f;

    private int gameOverAmount;

    private int pieceStep;

    float dropTime = 0.0f;
    

    int left
    {
        get { return -boardSize.x / 2; }
    }

    int right
    {
        get { return boardSize.x / 2; }
    }
    int top
    {
        get { return boardSize.y / 2; }
    }
    int bottom
    {
        get { return -boardSize.y / 2; }
    }

    private void Start()
    {
        gameOverAmount = 0;
        SpawnPiece();
    }

    private void Update()
    {
        if (tetrisManager.gameOver) return;

        dropTime += Time.deltaTime;

        if(dropTime >= dropInterval)
        {
            dropTime = 0.0f;

            Clear(activePiece);
            bool moveResult = activePiece.Move(Vector2Int.down);
            Set(activePiece);

            if(!moveResult)
            {
                activePiece.freeze = true;
                CheckBoard();
                SpawnPiece();
            }
        }
    }

    void CheckEndGame()
    {
        if (!IsPositionValid(activePiece, activePiece.position))
        {
            tetrisManager.SetGameOver(true);
        }
    }

    public void UpdateGameOver()
    {
        if(!tetrisManager.gameOver && gameOverAmount != 1)
        {
            gameOverAmount++;
        }

        else
        {
            gameOverAmount = 0;
            pieceStep = 0;
            ResetBoard();
        }
        
    }

    void ResetBoard()
    {
        Pieces[] foundPieces = FindObjectsByType<Pieces>(FindObjectsSortMode.None);

        foreach (Pieces piece in foundPieces) Destroy(piece.gameObject);

        activePiece = null;

        tilemap.ClearAllTiles();

        dropTime = 0.0f;

        pieceStep = 0;

        SpawnPiece();
    }

    public void SpawnPiece()
    {
        if (pieceStep == 0)
        {
            activePiece = Instantiate(prefabPiece);


            activePiece.Initialize(this, Tetronimo.lowt);

            CheckEndGame();

            Set(activePiece);

            pieceStep = 1;
        }
        else if (pieceStep == 1) 
        {
            activePiece = Instantiate(prefabPiece);


            activePiece.Initialize(this, Tetronimo.I);

            CheckEndGame();

            Set(activePiece);

            pieceStep = 2;
        }
        else if (pieceStep == 2)
        {
            activePiece = Instantiate(prefabPiece);


            activePiece.Initialize(this, Tetronimo.T);

            CheckEndGame();

            Set(activePiece);

            pieceStep = 3;
        }
        else if (pieceStep == 3)
        {
            activePiece = Instantiate(prefabPiece);


            activePiece.Initialize(this, Tetronimo.Z);

            CheckEndGame();

            Set(activePiece);

            pieceStep = 4;
        }
        else if (pieceStep == 4)
        {
            activePiece = Instantiate(prefabPiece);


            activePiece.Initialize(this, Tetronimo.J);

            CheckEndGame();

            Set(activePiece);

            pieceStep = 5;
        }
        else if (pieceStep == 5)
        {
            activePiece = Instantiate(prefabPiece);


            activePiece.Initialize(this, Tetronimo.BIGJ);

            CheckEndGame();

            Set(activePiece);

            pieceStep = 6;
        }


        //OG code
        else if (pieceStep == 6)
        {
            activePiece = Instantiate(prefabPiece);
            Tetronimo randomTetronimo = (Tetronimo)Random.Range(0, tetronimos.Length);

            activePiece.Initialize(this, randomTetronimo);

            CheckEndGame();

            Set(activePiece);
        }

            
    }

    
    public void Set(Pieces piece)
    {
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int cellPosition = (Vector3Int)(piece.cells[i] + piece.position);
            tilemap.SetTile(cellPosition, piece.data.tile);
        }
    }
    public void Clear(Pieces piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int cellPosition = (Vector3Int)(piece.cells[i] + piece.position);
            tilemap.SetTile(cellPosition, null);
        }
    }

    public bool IsPositionValid(Pieces piece, Vector2Int position)
    {
        int left = -boardSize.x / 2;
        int right = boardSize.x / 2;
        int bottom = -boardSize.y / 2;
        int top = boardSize.y / 2;
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int cellPosition = (Vector3Int)(piece.cells[i] + position);
            
            //Bounds Check
            if (cellPosition.x < left || cellPosition.x >= right || cellPosition.y < bottom || cellPosition.y >= top) return false;

            //Position Occupation Check
            if (tilemap.HasTile(cellPosition)) return false;
        }
        return true;
    }

    bool IsLineFull(int y)
    {
        for (int x = left; x < right; x++)
        {
            Vector3Int cellPosition = new Vector3Int(x, y);
            if (!tilemap.HasTile(cellPosition)) return false;
        }

        return true;
    }

    void DestroyLine(int y)
    {
        for (int x = left; x < right; x++)
        {
            Vector3Int cellPosition = new Vector3Int(x, y);
            tilemap.SetTile(cellPosition, null);
        }
    }

    void ShiftRowsDown(int clearedRow)
    {
        for (int y = clearedRow + 1; y < top; y++)
        {
            for (int x = left; x < right; x++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y);

                TileBase currentTile = tilemap.GetTile(cellPosition);

                tilemap.SetTile(cellPosition, null);

                cellPosition.y -= 1;
                tilemap.SetTile(cellPosition, currentTile);
            }
        }
    }

    public void CheckBoard()
    {
        List<int> destroyedLines = new List<int>();
        for (int y = bottom; y < top; y++)
        {
            if (IsLineFull(y))
            {
                DestroyLine(y);
                destroyedLines.Add(y);
            }
        }


        int rowsShiftedDown = 0;
        foreach (int y in destroyedLines)
        {
            ShiftRowsDown(y - rowsShiftedDown);
            rowsShiftedDown++;
        }

        int score = tetrisManager.CalculateScore(destroyedLines.Count);
        tetrisManager.ChangeScore(score);
    }
}
