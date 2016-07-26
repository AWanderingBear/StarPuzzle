using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameManager : MonoBehaviour {

    public Text F1Display;
    public Text F2Display;

    public GameObject Score1;
    public GameObject Score2;
    public GameManager manager1;
    public GameManager manager2;

    // Use this for initialization
    void Start () {

        //Score1 = GameObject.Find("P1Score");
        //Score2 = GameObject.Find("P2Score");

        manager1 = Score1.GetComponent<GameManager>();
        manager2 = Score2.GetComponent<GameManager>();
        F1Display.text = "Final Score: " + manager1.P1Score;
        F2Display.text = "Final Score: " + manager2.P2Score;
    }

    // Update is called once per frame
    void Update () {
       
    }
}
