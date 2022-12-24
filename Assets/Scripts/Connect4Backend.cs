using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connect4Backend : MonoBehaviour {
    
    [Header("Board")]
    public int width = 7;
    public int height = 6;
    public int[,] board;
    //0 = EMPTY    1 = RED    2 = YELLOW

    [Header("Debug")]
    public int column;
    public int team;
    public bool ENABLEDEBUG;

    [Header("Alignment Data")]
    private float xOff = 92f;
    private float yOff = 92f;

    [Header("Objects")]
    public GameObject tileRed;
    public GameObject tileYellow;
    public GameObject backBoard;

    [Header("UI")]
    public UIHandler UIH;

    [Header("Delay")]
    float timeSince;
    public float delay = 0.5f;

    struct dir {
        public int x, y;

        public dir(int newX, int newY) {
            x = newX;
            y = newY;
        }

        public static dir operator + (dir d1, dir d2) {
            return new dir(d1.x + d2.x, d1.y + d2.y);
        }

    };

    List<dir> directions;

    private void Start() {

        board = new int[width,height];
        directions = new List<dir>();

        directions.Add(new dir(0,1));
        directions.Add(new dir(1,1));
        directions.Add(new dir(1,0));
        directions.Add(new dir(1,-1));

        directions.Add(new dir(0,-1));
        directions.Add(new dir(-1,-1));
        directions.Add(new dir(-1,0));
        directions.Add(new dir(-1,1));
        
        ClearBoard();

        DebugBoard();

        UIH.SwitchTo(1);

        timeSince = delay;
    }

    private void Update() {
        if ( ENABLEDEBUG ) {
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                AddTile(column);

                DebugBoard();
            }
        }

        timeSince += Time.deltaTime;
    }

    private void ClearBoard() {
        for ( int i = 0; i < width; i++ ) {
            for ( int j = 0; j < height; j++ ) {
                board[i,j] = 0;
            }
        }
    }

    private void DebugBoard() {
        Debug.Log("=== PRINTING BOARD ===");
        for ( int i = 0; i < width; i++ ) {
            string s = "Line " + (i+1).ToString() + ": ";
            for ( int j = 0; j < height; j++ ) {
                s = s + board[i,j].ToString() + " ";
            }
            s += '\n';
            Debug.Log(s);
        }
    }

    public void AddTile(int col) {
        if ( timeSince < delay ) {
            return;
        }
        bool placed = false;
        int i;
        for ( i = 0; i < height && !placed; i++ ) {
            if ( board[col, i] == 0 ) {
                board[col, i] = team;
                placed = true;
            }
        }

        if ( !placed ) {
            Debug.LogError("Placing failed! Column is full!");
        } else {
                DebugBoard();
            AddVisualTile(col, i - 1, team);
            CheckForVictory(col, i - 1);
        }

        if ( team == 1 ) {
            team = 2;
        } else {
            team = 1;
        }
        UIH.SwitchTo(team);

        timeSince = 0;
    }

    public void AddVisualTile(int l, int c, int team) {

        //Debug.Log("visually placing tile from team " + team.ToString() + " at position " + l.ToString() + ", " + c.ToString());

        GameObject tileGO = team == 1 ? tileRed : tileYellow;

        tileGO = Instantiate(tileGO, backBoard.transform.position, Quaternion.identity ) as GameObject;
        tileGO.transform.SetParent(backBoard.transform);

        Tile tile = tileGO.GetComponent<Tile>();

        tile.destination = backBoard.transform.position - new Vector3(xOff * 3, yOff * 2.5f, 0) + new Vector3(xOff * l, yOff * c, 0);
        tile.startingLocation = backBoard.transform.position - new Vector3(xOff * 3, yOff * 2.5f, 0) + new Vector3(xOff * l, yOff * 9f, 0);
        tile.StartMovement();
    }

    private void CheckForVictory(int col, int lin) {
        
        for ( int dirIndex = 0; dirIndex < 4; dirIndex++ ) {
            int total = 1;
            int checkingTeam = board[col,lin];

            dir pos = new dir(col, lin);
            dir check = pos;
            dir add = directions[dirIndex];
            bool stillValid = true;

            for ( int i = 0; i < 2; i++ ) {
                
                check += add;
                while (IsInBoardBounds(check.x, check.y) && stillValid) {

                    //Debug.Log("checking " + check.x.ToString() + " " + check.y.ToString());

                    if ( board[check.x, check.y] == checkingTeam && stillValid ) {
                        total++;
                    } else {
                        stillValid = false;
                    }

                    check += add;
                }
                
                check = pos;
                add = directions[dirIndex + 4];
                stillValid = true;
            }

            if ( total >= 4 ) {
                Debug.Log(checkingTeam.ToString() + " has won! (1 is red, 2 is yellow)");
                UIH.TriggerVictoryDelay(checkingTeam, 0.8f);
            }
        }
    }

    private bool IsInBoardBounds(int x, int y) {
        return x >= 0 && x < width && y >= 0 && y < height ? true : false;
    }

}
