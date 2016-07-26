using UnityEngine;
using System.Collections;

public class MouseChase : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = Input.mousePosition;
	}
}
