using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Player enum
public enum Player
{
    Player1,
    Player2
};

//points per turn
//extyra point for second player turn
// grpahics that

public class GameManager : MonoBehaviour {

    //Stars
    //The StarPrefab
    public GameObject StarOnePt;
    public GameObject StarTwoPt;
    public GameObject StarThreePt;

    public Sprite StarFirstOnePt;
    public Sprite StarFirstTwoPt;
    public Sprite StarFirstThreePt;

    //Empty Gameobject to keep all of the stars in
    public GameObject StarParent;
    //Sprites for each star
    public Sprite StarFirst;

    public GameObject PlanetWithPlayers;
    //Move Cards
    //Empty game object to store them in
    public GameObject CardParent;
    //Prefab
    public GameObject CardPrefab;
    public GameObject[] Cards;

    public Material glowBlue;
    public Material glowGold;
    public Material glowRed;
    public Material glowRedBlue;

    public GameObject LastChosenCard;

    //Link Management
    //The star that was last clicked
    StarBehaviour LinkingStar;
    //The first star in the sequence
    List<StarBehaviour> StarList;
    CardBehaviour SelectedCard;

    public Player Turn; // Tracks whose turn it is

    //Player Scoring
    int CurrentScoreTotal = 0;
    public int P1Score = 0;
    public int P2Score = 0;
    public int movesAvailable = 0;

    public Text P1Display;
    public Text P2Display;
    public Text currentMoves;

    public int StarNumber = 50;    //Number of stars
    public int CardNumber = 7;     //Number of cards


    //int movesAvailable = 0;

    //Audio
    int Clicks = 0;
    public AudioSource AudioPlayer;
    public AudioClip[] ClickSounds;
    public AudioClip ConstellationSound;

    private bool CardAlreadyChosen = false;
    public int CardCurrentChosen = -1;

    private bool isRotatingPlanet;
    private int planetLerpCounter = 60;

    // Use this for initialization
    void Start () {

        StarList = new List<StarBehaviour>();

        LinkingStar = null;



        //Spawning stars
        for (int i = 0; i < StarNumber; i++) {

            float XCoord = Random.Range(-10.0f, 9.0f);
            float YCoord = Random.Range(-3.0f, 3.5f);

            Vector3 Pos = new Vector3(XCoord, YCoord, 0);

            Quaternion Zero = new Quaternion(0, 0, 0, 0);

            int Score = Random.Range(1, 4);
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

        int StartingCardY = CardNumber * -1 + 2;
        Vector3 StartingCardPos = new Vector3(7.0f, StartingCardY, 0); //The starting position of cards

        for (int i = 0; i < CardNumber; i++)
        {
            int randCardNumber = Random.Range(3, 10);

            GameObject CardClone = (GameObject)Instantiate(CardPrefab, StartingCardPos, transform.rotation);
            CardClone.transform.parent = CardParent.transform;
            CardClone.name = "Card " + i;

            CardClone.GetComponent<CardBehaviour>().initialize(randCardNumber);

            StartingCardPos.y += 1.5f;
        }
	}

	// Update is called once per frame
	void Update () {

        P1Display.text = "Score: " + P1Score;
        P2Display.text = "Score: " + P2Score;
        currentMoves.text = "Current Moves Left: " + movesAvailable;

        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene("Finish");
        }

        if (planetLerpCounter < 60)
        {
            turnPlanet();
            planetLerpCounter++;
        }
    }

    public bool SetLinkingStar(StarBehaviour StarToLink)
    {

        if (StarList.Capacity > 0 && StarToLink == StarList[0])
        {

            AudioPlayer.clip = ConstellationSound;
            AudioPlayer.Play();
            Clicks = 0;

            StarToLink.Particle();

            LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>(), Turn);
            LinkingStar = null;
            switch (StarList[0].GetScore())
            {

                case 1:
                    StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarOnePt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                case 2:
                    StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarTwoPt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                case 3:
                    StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarThreePt.GetComponentInChildren<SpriteRenderer>().sprite;
                    break;

                default:
                    break;
            }

            ChangeTurns();

            foreach (StarBehaviour iStar in StarList)
            {
                iStar.Spin();
                iStar.Particle();
            }

            StarList.Clear();
            StarList.Capacity = 0; 

            return true;
        }
        else if (LinkingStar != null && StarToLink != LinkingStar && (movesAvailable > 0))
        {

            if (!StarToLink.IsUsed()) { 
            RaycastHit2D Hit = Physics2D.Raycast(LinkingStar.transform.position, StarToLink.transform.position - LinkingStar.transform.position);

            if (Hit)
            {

                if (Hit.transform.name == StarToLink.transform.name)
                {

                    LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>(), Turn);
                    CurrentScoreTotal += StarToLink.GetScore();
                    LinkingStar = StarToLink;

                    StarList.Add(StarToLink);

                    StarToLink.Particle();

                    Clicks += 1;
                    AudioPlayer.clip = ClickSounds[Clicks - 1];
                    AudioPlayer.Play();

                    movesAvailable--;
                    return true;
                }
            }
        }

            return false;
        }
        else if (StarList.Capacity == 0 && (movesAvailable > 0))
        {

            LinkingStar = StarToLink;
            StarList.Add(StarToLink);

            StarToLink.Particle();

            StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarFirst;
            CurrentScoreTotal = StarToLink.GetScore();

            Clicks = 1;
            AudioPlayer.clip = ClickSounds[Clicks - 1];
            AudioPlayer.Play();

            switch (StarList[0].GetScore())
            {

                case 1:
                    StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarFirstOnePt;
                    break;

                case 2:
                    StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarFirstTwoPt;
                    break;

                case 3:
                    StarList[0].GetComponentInChildren<SpriteRenderer>().sprite = StarFirstThreePt;
                    break;

                default:
                    break;
            }


            return true;
        }
        else
        {
            if (movesAvailable <= 0 && CardAlreadyChosen)
            {
                ChangeTurns();
                Debug.Log("No moves available"); //We need to display this to the user somehow.

            }
            else if (movesAvailable <= 0 && !CardAlreadyChosen)
            {
                Debug.Log("Card not chosen yet");
            }
            else
            {
                Debug.Log("Unknown error in linking stars. ");
            }            

        }
        return false;
    }

    public bool SetMovesAvailable(int CardMovesNumber, GameObject card)
    {
        if (CardAlreadyChosen == false)
        {
            LastChosenCard = card;

            if (Turn == Player.Player1)
            {
                if (CardMovesNumber > 5 && CardMovesNumber <= 7)
                {
                    P1Score -= CardMovesNumber;

                }
                if (CardMovesNumber > 7)
                {
                    P1Score -= 2 * CardMovesNumber;
                }
            }
            else if (Turn == Player.Player2)
            {
                if (CardMovesNumber > 5 && CardMovesNumber <= 7)
                {
                    P2Score -= CardMovesNumber;

                }
                if (CardMovesNumber > 7)
                {
                    P2Score -= 2 * CardMovesNumber;
                }
            }

            MeshRenderer CardRenderer = card.GetComponentInChildren<MeshRenderer>();
            CardRenderer.enabled = true;
            CardRenderer.material = glowGold;

            movesAvailable = CardMovesNumber;
            CardAlreadyChosen = true;
        }
        return true;
    }

    void ChangeTurns()
    {
        movesAvailable = 0;

        MeshRenderer currentCardRenderer = LastChosenCard.GetComponentInChildren<MeshRenderer>();
        if (Turn == Player.Player1)
        {
            Debug.Log("Changing turns to " + Player.Player1);
            P1Score += CurrentScoreTotal;
            CurrentScoreTotal = 0;
            Turn = Player.Player2;

            if (LastChosenCard.GetComponentInChildren<CardBehaviour>().CardUsed == 3)
            {
                currentCardRenderer.material = glowRedBlue;
            }
            else
            {
                currentCardRenderer.material = glowBlue;
            }
        }
       else if (Turn == Player.Player2)
        {
            Debug.Log("Changing turns to " + Player.Player2);
            P2Score += CurrentScoreTotal;
            CurrentScoreTotal = 0;
            Turn = Player.Player1;

            if (LastChosenCard.GetComponentInChildren<CardBehaviour>().CardUsed == 3)
            {
                currentCardRenderer.material = glowRedBlue;
            }
            else
            {
                currentCardRenderer.material = glowRed;
            }
        }
        CardAlreadyChosen = false;

        planetLerpCounter = 0;
    }

    void turnPlanet()
    {

        Vector3 Lerping = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 180), 0.0167f);
        PlanetWithPlayers.transform.Rotate(Lerping);
    }
}

