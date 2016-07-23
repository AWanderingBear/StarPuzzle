using UnityEngine;
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
    public GameObject Star;
    //Empty Gameobject to keep all of the stars in
    public GameObject StarParent;
    //Sprites for each star
    public Sprite StarBase;
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

    public Player Turn; // Tracks whose turn it is

    //Player Scoring
    int CurrentScoreTotal = 0;
    int P1Score = 0;
    int P2Score = 0;

    public Text P1Display;
    public Text P2Display;

	// Use this for initialization
	void Start () {

        Turn = Player.Player1;

        LinkingStar = null;
        int StarNumber = 40;    //Number of stars
        int CardNumber = 7;     //Number of cards
        Vector3 CurrentCardPos = new Vector3(-5.0f, -4.5f, 0.0f); //The starting position of cards

        //Spawning stars
        for (int i = 0; i < StarNumber; i++) {

            float XCoord = Random.Range(-6.0f, 6.0f);
            float YCoord = Random.Range(-4.0f, 4.0f);

            Vector3 Pos = new Vector3(XCoord, YCoord, 0); 

            Quaternion Zero = new Quaternion(0,0,0,0);


            GameObject StarClone = (GameObject) Instantiate(Star, Pos, Zero);
            StarClone.GetComponent<StarBehaviour>().SetScore(1);
            StarClone.transform.parent = StarParent.transform;
            StarClone.name = "Star " + i;
        }


        //Spawning cards
        for (int i = 0; i < CardNumber; i++)
        {
            int randCardNumber = Random.Range(3, 10);

            GameObject CardClone = (GameObject)Instantiate(CardPrefab, CurrentCardPos, transform.rotation);
            CardClone.transform.parent = CardParent.transform;
            CardClone.name = "Card " + i;

            CardClone.GetComponent<CardBehaviour>().initialize(randCardNumber);

            CurrentCardPos.x += 2;
        }
	}

	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            LinkingStar = null;
            FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarBase;
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
            FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarBase;
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
        else if (LinkingStar != null && StarToLink != LinkingStar)
        {

            RaycastHit2D Hit = Physics2D.Raycast(LinkingStar.transform.position, StarToLink.transform.position - LinkingStar.transform.position);

            if (Hit)
            {

                if (Hit.transform.name == StarToLink.transform.name)
                {

                    LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>(), Turn);
                    LinkingStar = StarToLink;
                    CurrentScoreTotal += StarToLink.GetScore();
                    return true;
                }
            }

            return false;
        }
        else if (FirstStar == null)
        {

            LinkingStar = StarToLink;
            FirstStar = StarToLink;
            FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarFirst;
            CurrentScoreTotal = StarToLink.GetScore(); ;
            return false;
        }

        return false;
    }
}
