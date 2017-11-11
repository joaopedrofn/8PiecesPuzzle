using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
	private Node parent;
	private int steps;
	private int[][] state;
    private int pieceChanged;
    private Position zeroPosition;

    public void setPieceChanged(int piece)
    {
        this.pieceChanged = piece;
    }
	public void setParent(Node parent){
		this.parent = parent;
	}
	public void setSteps(int steps){
		this.steps = steps;
	}
    public int getPieceChanged()
    {
        return pieceChanged;
    }
	public Node getParent(){
		return parent;
	}
	public int getSteps(){
		return steps;
	}
	public int[][] getState(){
		return state;
	}
    private Node createChild(Position position)
    {
        int[][] newState = this.newState(Board.copy(this.getState()), zeroPosition, position);
        Node newNode = new Node(newState as int[][]);
        newNode.setParent(this);
        newNode.setSteps(this.steps + 1);
        newNode.setPieceChanged(state[position.y][position.x]);
        return newNode;
    }
	public List<Node> getNextPossibleStates(){
		List<Node> toReturn = new List<Node> ();
        if (zeroPosition.x > 0 && getPieceChanged() != getState()[zeroPosition.y][zeroPosition.x-1]) toReturn.Add(createChild(new Position(zeroPosition.x - 1, zeroPosition.y)));
        if (zeroPosition.x < 2 && getPieceChanged() != getState()[zeroPosition.y][zeroPosition.x + 1]) toReturn.Add(createChild(new Position(zeroPosition.x + 1, zeroPosition.y)));
        if (zeroPosition.y > 0 && getPieceChanged() != getState()[zeroPosition.y-1][zeroPosition.x]) toReturn.Add(createChild(new Position(zeroPosition.x, zeroPosition.y-1)));
        if (zeroPosition.y < 2 && getPieceChanged() != getState()[zeroPosition.y+1][zeroPosition.x]) toReturn.Add(createChild(new Position(zeroPosition.x, zeroPosition.y+1)));
  
        return toReturn;
	}
	
	int[][] newState(int[][] board,Position blank, Position position){
		int piece = board [position.y] [position.x];
		int[][] newBoard = board.Clone() as int [][];
        newBoard[position.y][position.x] = 0;
        newBoard[blank.y][blank.x] = piece;
        return newBoard;
	}
	public Node(int[][] state){
        this.state = state;
        int x = 0;
        int y;
        for(y = 0; y<3; y++)
        {
            x = System.Array.IndexOf(state[y], 0);
            if (x != -1) break;
        }
        zeroPosition = new Position(x, y);
    }
}
