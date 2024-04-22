using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{

    [SerializeField] Tile tilePrefab;
    Tile[,] board;
    [SerializeField] int column, row;
    [SerializeField] int bombNumber;
    [SerializeField] List<Vector3> bombPositions = new();
    public List<Vector3> BombPositions => bombPositions;
    Vector3[] directions = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    Vector3[] fulDirections = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0),
                                new Vector3 (1,1,0),new Vector3(1, -1, 0),new Vector3(-1, 1, 0),new Vector3(-1, -1, 0)};
    int count;
    public bool isWin;
    public bool isLose;
    private void Start()
    {
        Generate();
        SpawnBomb();
        SaveTrace();// Dem so luong bom  xung quanh o do neu khong co bom setText tile rong neu o do la bomb cung setext rong neu so 
        //luong bom >0 ghi so bom vao setText

    }
    void Generate()
    {
        board = new Tile[row, column];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {


                board[i, j] = Instantiate(tilePrefab);
                board[i, j].transform.localPosition = new Vector3(i, j, 0);
                board[i, j].transform.SetParent(transform);

            }
        }
    }

    void SpawnBomb()
    {


        while (bombPositions.Count < bombNumber)
        {
            Vector3 randomPosition = Vector3.zero;
            randomPosition.x = Random.Range(0, row);
            randomPosition.y = Random.Range(0, column);
            if (bombPositions.Contains(randomPosition))
            {
                continue;

            }
            else bombPositions.Add(randomPosition);
            board[(int)randomPosition.x, (int)randomPosition.y].SetActiveBomb(true);
        }


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < bombPositions.Count; i++)
        {
            Gizmos.DrawSphere(bombPositions[i] - Vector3.forward, 0.5f);
        }
    }

    public void OpenEmptyTile(Vector3 position)
    {

        if (!IsInsideBoard(position))
        {
            return;
        }
        var tile = board[(int)position.x, (int)position.y];

        if (BombPositions.Contains(position))
        {
            return;
        }
        if (!tile.isOpen)
        {
            tile.SetActiveMask(false);
            tile.isOpen = true;
            count++;
        }
        else return;
        foreach (var direction in directions)
        {
            var checkpos = position + direction;
            OpenEmptyTile(checkpos);
        }

    }
    private bool IsInsideBoard(Vector3 position)
    {
        if (position.x < 0)
        {
            return false;
        }
        if (position.x >= row)
        {
            return false;
        }
        if (position.y < 0)
        {
            return false;
        }
        if (position.y >= column)
        {
            return false;
        }
        return true;
    }

    public bool CheckWin()
    {
        if (count == row * column - bombNumber)
        {
            return true;
        }
        else return false;
    }
    public void NewGame()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                board[i, j].Reset();
            }
        }
        bombPositions.Clear();
        isLose = false;
        isWin = false;
        count = 0;
        SpawnBomb();
        SaveTrace();
    }
    private void SaveTrace()
    {
        for (int i = 0;i<row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                if (bombPositions.Contains(board[i,j].transform.position))
                {
                    board[i, j].SetText("");
                    continue;
                }
                int countBomb = 0;
                foreach (var direction in fulDirections)
                {
                    if (bombPositions.Contains(board[i, j].transform.position + direction))
                    {
                        countBomb++;
                    }

                }
                if(countBomb <= 0) 
                {
                    board[i, j].SetText("");

                }
                else 
                {
                    board[i, j].SetText($"{countBomb}");
                }
                
            }
        }
    }
}
