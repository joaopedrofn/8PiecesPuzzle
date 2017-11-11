using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    int lastMove;
    static public int[][] state;
	//static public int[][] winState;
	static public Transform[] pieces;
	static public Dictionary<int, Position> winState;
    static public GameObject victoryPanel;
	void Start(){
        victoryPanel = transform.Find("VictoryPanel").gameObject;
        victoryPanel.SetActive(false);
		Board.state = new int[][]{ new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { 7, 8, 0 } };
		//winState = new int[][]{new int[]{0, 1, 2},new int[]{3, 4, 5},new int[]{6, 7, 8}};
		winState = new Dictionary<int, Position> ();
		winState.Add (1, new Position (0, 1));
		winState.Add (2, new Position (0, 2));
		winState.Add (3, new Position (1, 0));
		winState.Add (4, new Position (1, 1));
		winState.Add (5, new Position (1, 2));
		winState.Add (6, new Position (2, 0));
		winState.Add (7, new Position (2, 1));
		winState.Add (8, new Position (2, 2));
		generateState ();

	}

	void generateState(){
        pieces = new Transform[8];
        for (int i = 1; i <= 8; i++)
        {
            pieces[i - 1] = transform.Find("Piece (" + i + ")");
        }
        lastMove = 0;
        StartCoroutine(generateCoroutine());

    }

    IEnumerator generateCoroutine()
    {
        
        
        for (int i = 0; i < 50; i++)
        {
            int[][] tempBoard = Board.state;
            for(int z = 0; z <5 || !compare(tempBoard, Board.state); z++)
            {

                int x = 0;
                int y = 0;
                do
                {
                    x = Random.Range(0, 3);
                    y = Random.Range(0, 3);
                } while (Board.state[x][y] == 0);
                Transform piece = transform.Find("Piece (" + Board.state[x][y] + ")");
                PieceController controller = piece.Find("Piece").GetComponent<PieceController>();
                if (lastMove != controller.piece)
                {
                    controller.tryToMove();
                    if (!compare(tempBoard, Board.state))
                    {
                        lastMove = controller.piece;
                    }
                }
                
            }
            
            yield return new WaitForSeconds(0.05f);
            
        }
    }

	static public void showBoard(int[][] board){
		string debug = "";
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				debug += board [i] [j] + " | ";
			}
			debug += "\n";
		}
		Debug.Log (debug);
		Debug.Log ("Manhatan: " + IA.manhattan (Board.state));
	}
	static public void verifyVictory(){

		
		if(IA.manhattan(Board.state) == 0)
        {
            victoryPanel.SetActive(true);
            foreach(Transform piece in pieces)
            {
                piece.Find("Piece").gameObject.GetComponent<Button>().interactable = false;
            }
        }
			
	}
	static public bool compare(int[][] board1, int[][] board2){
		bool equity = true;
        for (int i = 0; i < 3; i++)
        {
            equity = board1[i].Equals(board2[i]);
            if (!equity) break;
        }
		return equity;
	}

    static public int[][] copy(int[][] array)
    {
        int[][] temp = new int[][] { new int[3], new int[3], new int[3]};
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                temp[i][j] = array[i][j];
        return temp;
    }
}
