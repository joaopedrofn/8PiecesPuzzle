using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PieceController : MonoBehaviour {

	public enum Direction{UP, RIGHT, DOWN, LEFT}

	public int piece;
	public Animator animator;
	public Position position;
	public GameObject parent;
	private bool moving;
	private Vector3 newPosition;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		var y = (int)((parent.transform.localPosition.y + 148.75f) / 147.5f);
		if (y == 0)
			y = 2;
		else if (y == 2)
			y = 0;
		position = new Position ((int)((parent.transform.localPosition.x+148.75f)/147.5f), y);
		moving = false;
		piece = System.Int32.Parse(Regex.Match (parent.name, @"\d+").Value);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = Vector3.zero;
		if (moving) {
			if (parent.transform.localPosition == newPosition)
				moving = false;
			else {
				parent.transform.localPosition = Vector3.SmoothDamp (parent.transform.localPosition, newPosition, ref velocity, 0.03f);
			}
		}
	}



	private void move(Direction dir){
		switch (dir) {
		case Direction.UP:
			--position.y;
			break;
		case Direction.RIGHT:
			++position.x;
			break;
		case Direction.DOWN:
			++position.y;
			break;
		case Direction.LEFT:
			--position.x;
			break;
		}
		moving = true;
		newPosition =  new Vector2 (position.x*147.5f-148.75f, -(position.y*147.5f-148.75f));
		updateBoard ();
		//showBoard ();
		Board.verifyVictory();
	}

	public void tryToMove(){
		if (position.y>0) if( Board.state [position.y-1][position.x] == 0) {
			move (Direction.UP);
			return;
		} 
		if (position.y< 2) if( Board.state [position.y+1][position.x] == 0) {
			move (Direction.DOWN);
			return;
		}
		if (position.x<2) if( Board.state [position.y][position.x+1] == 0) {
			move (Direction.RIGHT);
			return;
		}
		if (position.x > 0 ) if( Board.state [position.y][position.x-1] == 0) {
			move (Direction.LEFT);
			return;
		}
	}
	public void alocatePiece(int x, int y){
		position = new Position (x, y);
		parent.transform.localPosition =  new Vector2 (position.x*147.5f-148.75f, -(position.y*147.5f-148.75f));

	}

	void updateBoard(){
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				if (Board.state [j] [i] == piece)
					Board.state [j] [i] = 0;
			}
		}
		Board.state [position.y] [position.x] = piece;
	}

}
