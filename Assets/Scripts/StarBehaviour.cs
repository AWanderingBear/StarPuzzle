using UnityEngine;
using System.Collections;

public class StarBehaviour : MonoBehaviour {

    GameManager Manager;
    LineRenderer LinkRenderer;

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

    public void SetLineTarget(Transform Target)
    {
        LinkRenderer.SetPosition(0, GetComponent<Transform>().position);
        LinkRenderer.SetPosition(1, Target.position);
    }

    public void SetLayer(LayerMask Layer)
    {

        gameObject.layer = Layer;
    }
}
