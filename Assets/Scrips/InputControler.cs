using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControler : MonoBehaviour
{
    [SerializeField] BoardGenerator boardGenerator;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (boardGenerator.isLose || boardGenerator.isWin)
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                var tile = hit.transform.GetComponent<Tile>();
                if (tile.isOpen)
                {
                    return;
                }
                //tile.isOpen = true;
                //tile.SetActiveMask(false);
                Debug.Log(hit.transform.name);
                Debug.Log("hit");
                if (boardGenerator.BombPositions.Contains(hit.transform.position))
                {

                    Debug.Log("Lose");
                    boardGenerator.isLose = true;
                    boardGenerator.NewGame();
                    return;
                }
                else
                {
                    boardGenerator.OpenEmptyTile(hit.transform.position);
                    if (boardGenerator.CheckWin())
                    {
                        Debug.Log("Win");
                        boardGenerator.isWin=true;
                        boardGenerator.NewGame();   
                    }


                }
            }

        }
    }
}
