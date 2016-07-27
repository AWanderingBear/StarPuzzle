using UnityEngine;
using System.Collections;

public class StarBehaviour : MonoBehaviour {

    public Material PlayerOneMat;
    public Material PlayerTwoMat;

    public ParticleSystem ParticleEmitter;

    GameManager Manager;
    LineRenderer LinkRenderer;

    bool SpinFlag = false;
    int SpinCount = 0;

    bool Used = false;

    int Score;

	// Use this for initialization
	void Start () {

        LinkRenderer = GetComponent<LineRenderer>();
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        if (SpinFlag)
        {
            if (SpinCount <= 30)
            {
                Vector3 Lerping = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 360), 0.0333f);
                transform.Rotate(Lerping);
                SpinCount++;
            }
            else
            {
                SpinCount = 0;
                SpinFlag = false;
            }

        }
	}

    void OnMouseDown()
    {
        Used = Manager.SetLinkingStar(this);
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

    public void Particle()
    {

        ParticleEmitter.Emit(60);
    }

    public bool IsUsed()
    {

        return Used;
    }

    public void Spin()
    {
        SpinFlag = true;
    }
}
