using UnityEngine;
using System.Collections;

public class CardBehaviour : MonoBehaviour
{

    GameManager Manager;
    public int CardMovesNumber;
    public int CardUsed = 0; // 0 = not used, 1 = 1 player used, 2 = 2 player used, 3 = both player used.

    public Sprite[] Cards;

    // Use this for initialization
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void initialize(int randCardNumber)
    {
        //FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarBase;
        GetComponent<SpriteRenderer>().sprite = Cards[randCardNumber - 3];
        CardMovesNumber = randCardNumber;
    }

    void OnMouseDown()
    {

        if (Manager.Turn == Player.Player1 && CardUsed != 1 && CardUsed != 3)
        {
            Manager.SetMovesAvailable(CardMovesNumber, gameObject);
            CardUsed += 1;
        }
        else if (Manager.Turn == Player.Player2 && CardUsed != 2 && CardUsed != 3)
        {
            Manager.SetMovesAvailable(CardMovesNumber, gameObject);
            CardUsed += 2;
        }
            Debug.Log("The " + name + " was clicked");
            //Debug.Log("The " + this.GameObject.name + " was clicked");
    }        

}

