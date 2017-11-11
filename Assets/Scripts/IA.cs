using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class IA : MonoBehaviour {
	private List<Node> boundary;
	private List<Node> visited ;
    private Thread solvethread;
    private List<Node> path;
    public GameObject logPanel;
    private int states =1;
    private float time;
	// Use this for initialization
	void Start () {
        logPanel.SetActive(false);
		boundary = new List<Node> ();
		visited = new List<Node> ();
        solvethread = new Thread(solveCoroutine);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
	static public int manhattan(int[][] board){
		int value = 0;
		for(int i = 0; i< 3; i++){
			for (int j = 0; j < 3; j++) {
				
				if (board [i] [j] != 0) {
					value += Mathf.Abs (i - Board.winState [board [i] [j]].x);
					value += Mathf.Abs (j - Board.winState [board [i] [j]].y);
				}
			}
		}
		return value;
	}

	public int heuristicFunction(Node board){
		return board.getSteps()+manhattan(board.getState());
	}

    public void solve()
    {
        Node root = new Node(Board.copy(Board.state));
        root.setSteps(0);
        boundary.Add(root);
        visited.Add(root);
        //StartCoroutine(solveCoroutine());
        if (!solvethread.IsAlive)
        {
            time = Time.time;
            logPanel.SetActive(true);
            solvethread.Start();
            StartCoroutine(waitEndOfThread());
        }
    }

    IEnumerator waitEndOfThread()
    {
        while (solvethread.IsAlive)
        {
            yield return new WaitForEndOfFrame();
        }
        time = Time.time - time;
        //Debug.Log("Time: "+time);
        solvethread = null;
        solvethread = new Thread(solveCoroutine);
        StartCoroutine(executeSolution());
    }

	private void solveCoroutine(){
        //Debug.Log("STARTING");
		
		while (manhattan (boundary [0].getState ()) != 0) {
			List<Node> children = boundary [0].getNextPossibleStates ();
            bool tira = false;
            
            ////Debug.Log("----------------------");
			for(int i = 0; i< children.Count-1; i++){
                if (tira)
                {
                    i--;
                    tira = false;
                }
				Node child = children [i];
               // Board.showBoard(child.getState());
                for (int j = 0; j< visited.Count-1; j++) {
					Node visit = visited [j];
					if (Board.compare (visit.getState (), child.getState ())) {
                        children.Remove(child);
              //          //Debug.Log("OPA");
                        tira = true;
						break;
					}
				}
                if (tira) continue;
                visited.Add(child);
				for(int j = 0; j<boundary.Count-1;j++) {
					Node node = boundary [j];
					if (Board.compare (node.getState (), child.getState ())) {
						if ((heuristicFunction(node)) < (heuristicFunction(child))) {
							children.Remove (child);
						} else {
							boundary.Remove (node);
							boundary.Add (child);
							children.Remove (child);
						}
                        tira = true;
						break;
					}
				}
			}
            //Board.showBoard(boundary[0].getState());
            states += children.Count;
            boundary.AddRange (children);
            boundary.Remove(boundary[0]);
            ////Debug.Log(children.Count+" --- "+boundary.Count);
			boundary.Sort (delegate(Node x, Node y) {
				return heuristicFunction(x).CompareTo (heuristicFunction(y));
			});
            //Board.showBoard(boundary[0].getState());
           // yield return null;
        }
        //Debug.Log("YaAAAY " + boundary[0].getSteps() + " Steps");
        Node winner = boundary[0];
        path = new List<Node>();
        path.Insert(0, winner);
        for(int i = 0; i<winner.getSteps()-1; i++)
        {
            path.Insert(0, path[0].getParent());
        }
        //StartCoroutine(executeSolution(path));

    }
    IEnumerator executeSolution()
    {
        Text logText = logPanel.transform.Find("Text").GetComponent<Text>();
        logText.text = "This solution took " + path.Count + " steps.\n"+states+" states were discovered.\nList of moves: \n";
        foreach (Node node in path)
        {
            logText.text += "=> "+node.getPieceChanged()+"\n";
            PieceController controller = Board.pieces[node.getPieceChanged() - 1].transform.Find("Piece").GetComponent<PieceController>();
            controller.tryToMove();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
