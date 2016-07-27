using UnityEngine;
using System.Collections;

public class MouseChase : MonoBehaviour {

    public LayerMask backLayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, backLayer))
        {
            transform.position = hit.point;
        }

    }
}
