using UnityEngine;
using System.Collections;

public class CardBehaviour : MonoBehaviour
{

    GameManager Manager;
    public int CardMovesNumber;

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
        Manager.SetMovesAvailable(CardMovesNumber);
        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i].name == name)
            {
               // DestroyObject(Cards[i]);
                //Cards[i] = null;
            }
        }
        Debug.Log("The " + name + " was clicked");
        //Debug.Log("The " + this.GameObject.name + " was clicked");
    }
}
