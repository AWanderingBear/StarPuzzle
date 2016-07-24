using UnityEngine;
using System.Collections;

public class StarBehaviour : MonoBehaviour {

    public Material PlayerOneMat;
    public Material PlayerTwoMat;

    GameManager Manager;
    LineRenderer LinkRenderer;

    int Score;

	// Use this for initialization
	void Start () {

        LinkRenderer = GetComponent<LineRenderer>();
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        
	}

    void OnMouseDown()
    {
        Manager.SetLinkingStar(this);
    }

    public void SetLineTarget(Transform Target, Player CurrentPlayer)
    {
        if (CurrentPlayer == Player.Player1)
        {
            LinkRenderer.material = PlayerOneMat;
        }
        else
        {
            LinkRenderer.material = PlayerTwoMat;
        }

        LinkRenderer.SetPosition(0, GetComponent<Transform>().position);
        LinkRenderer.SetPosition(1, Target.position);
    }

    public void SetLayer(LayerMask Layer)
    {

        gameObject.layer = Layer;
    }

    public void SetScore(int Value)
    {

        Score = Value;
    }

    public int GetScore()
    {
        return Score;
    }
}
