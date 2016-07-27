using UnityEngine;
using System.Collections;

public class CardBehaviour : MonoBehaviour
{
    public SpriteRenderer cardBack;
    public SpriteRenderer cardFront;

    public Transform holder;

    GameManager Manager;
    public int CardMovesNumber;
    public int CardUsed = 0; // 0 = not used, 1 = 1 player used, 2 = 2 player used, 3 = both player used.

    public Sprite[] Cards;
    public Sprite noPoints;
    public Sprite minus_1;
    public Sprite minus_2;
    private bool isOver = false;
    public Vector3 frontRotation;
    public Vector3 backRotation;
    public float flipSpeed;
    public float flipHeight;

    // Use this for initialization
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();


    }

    // Update is called once per frame
    void Update()
    {
        if (isOver)
        {
            holder.localEulerAngles = Vector3.Lerp(holder.localEulerAngles, backRotation, flipSpeed * Time.deltaTime);
        }
        else
        {
            holder.localEulerAngles = Vector3.Lerp(holder.localEulerAngles, frontRotation, flipSpeed * Time.deltaTime);
        }
        holder.localPosition = new Vector3(0.0f, 0.0f, Mathf.Cos(holder.localEulerAngles.y / 180.0f * Mathf.PI * 2.0f) * flipHeight);
    }

    public void initialize(int randCardNumber)
    {
        //FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarBase;
        cardFront.sprite = Cards[randCardNumber - 3];
        CardMovesNumber = randCardNumber;
        if (CardMovesNumber < 6)
        {
            cardBack.sprite = noPoints;
        }
        else if (CardMovesNumber >= 6 && CardMovesNumber < 8)
        {
            cardBack.sprite = minus_1;
        }
        else if (CardMovesNumber >= 8)
        {
            cardBack.sprite = minus_2;
        }
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

    void OnMouseEnter()
    {
        isOver = true;
    }    

    void OnMouseExit()
    {
        isOver = false;
    }
}

