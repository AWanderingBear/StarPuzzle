﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Player enum
public enum Player
{
    Player1,
    Player2
};

public class GameManager : MonoBehaviour {

    //Stars
    //The StarPrefab
    public GameObject StarOnePt;
    public GameObject StarTwoPt;
    public GameObject StarThreePt;

    //Empty Gameobject to keep all of the stars in
    public GameObject StarParent;
    //Sprites for each star
    public Sprite StarFirst;


    //Move Cards
    //Empty game object to store them in
    public GameObject CardParent;
    //Prefab
    public GameObject CardPrefab;
    public GameObject[] Cards;

    //Link Management
    //The star that was last clicked
    StarBehaviour LinkingStar;
    //The first star in the sequence
    StarBehaviour FirstStar;
    CardBehaviour SelectedCard;

    public Player Turn; // Tracks whose turn it is

    //Player Scoring
    int CurrentScoreTotal = 0;
    int P1Score = 0;
    int P2Score = 0;

    public Text P1Display;
    public Text P2Display;

    public int StarNumber = 40;    //Number of stars
    public int CardNumber = 7;     //Number of cards

    //public int movesAvailable = 10;
    public int movesAvailable = 0;

    //have to centre that

    // Use this for initialization
    void Start () {

        LinkingStar = null;

        int StartingCardX = CardNumber * -1 + 1;
        Vector3 StartingCardPos = new Vector3(StartingCardX, -4.0f, 0); //The starting position of cards



        //Spawning stars
        for (int i = 0; i < StarNumber; i++) {

            float XCoord = Random.Range(-6.0f, 6.0f);
            float YCoord = Random.Range(-4.0f, 4.0f);

            Vector3 Pos = new Vector3(XCoord, YCoord, 0);

            Quaternion Zero = new Quaternion(0, 0, 0, 0);

            int Score = Random.Range(1, 4);
            Debug.Log(Score);
            GameObject StarClone;

            switch (Score) {

                case 1: {

                        StarClone = (GameObject)Instantiate(StarOnePt, Pos, Zero);
                    }
                    break;

                case 2: {
                        StarClone = (GameObject)Instantiate(StarTwoPt, Pos, Zero);
                    }
                    break;

                case 3:
                    {
                        StarClone = (GameObject)Instantiate(StarThreePt, Pos, Zero);
                    }
                    break;

                default:
                    StarClone = null;
                    break;
            }

            if (StarClone != null)
            {
                StarClone.GetComponent<StarBehaviour>().SetScore(Score);
                StarClone.transform.parent = StarParent.transform;
                StarClone.name = "Star " + i;
            }

        }


        //Spawning cards

        for (int i = 0; i < CardNumber; i++)
        {
            int randCardNumber = Random.Range(3, 10);

            GameObject CardClone = (GameObject)Instantiate(CardPrefab, StartingCardPos, transform.rotation);
            CardClone.transform.parent = CardParent.transform;
            CardClone.name = "Card " + i;

            CardClone.GetComponent<CardBehaviour>().initialize(randCardNumber);

            StartingCardPos.x += 2;
        }
	}

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            LinkingStar = null;
            switch (FirstStar.GetScore())
            {

                case 1:
                    FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarOnePt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                case 2:
                    FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarTwoPt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                case 3:
                    FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarThreePt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                default:
                    break;
            }
            
            FirstStar = null;

        }

        P1Display.text = "Score: " + P1Score;
        P2Display.text = "Score: " + P2Score;
    }

    public bool SetLinkingStar(StarBehaviour StarToLink)
    {

        if (StarToLink == FirstStar)
        {

            LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>(), Turn);
            LinkingStar = null;
            switch (FirstStar.GetScore())
            {

                case 1:
                    FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarOnePt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                case 2:
                    FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarTwoPt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                case 3:
                    FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarThreePt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                default:
                    break;
            }
            FirstStar = null;
            
            if (Turn == Player.Player2)
            {
                P2Score += CurrentScoreTotal;
                CurrentScoreTotal = 0; 
                Turn = Player.Player1;
            }
            else
            {
                P1Score += CurrentScoreTotal;
                CurrentScoreTotal = 0;
                Turn = Player.Player2;
            }

            return true;
        }
        else if (LinkingStar != null && StarToLink != LinkingStar && (movesAvailable > 0))
        {

            RaycastHit2D Hit = Physics2D.Raycast(LinkingStar.transform.position, StarToLink.transform.position - LinkingStar.transform.position);

            if (Hit)
            {

                if (Hit.transform.name == StarToLink.transform.name)
                {

                    LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>(), Turn);
                    LinkingStar = StarToLink;
                    movesAvailable--;
                    return true;
                }
            }

            return false;
        }
        else if (FirstStar == null && (movesAvailable > 0))
        {

            LinkingStar = StarToLink;
            FirstStar = StarToLink;
            FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarFirst;
            CurrentScoreTotal = StarToLink.GetScore(); ;
            return false;
        }
        else
        {
            if (movesAvailable <= 0)
            {
                Debug.Log("No moves available"); //We need to display this to the user somehow.
            }
            else
            {
                Debug.Log("Unknown error in linking stars. ");
            }
        }
        return false;
    }

    public bool SetMovesAvailable(int CardMovesNumber)
    {
       movesAvailable = CardMovesNumber;

        return true;
    }
}
