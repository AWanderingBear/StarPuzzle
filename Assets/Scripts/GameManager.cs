using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Star;
    public GameObject StarParent;

    public GameObject CardPrefab;
    public GameObject CardParent;

    StarBehaviour LinkingStar;
    StarBehaviour FirstStar;
    CardBehaviour SelectedCard;

    public Sprite StarBase;
    public Sprite StarFirst;

    public GameObject[] Cards;

    public LayerMask Default;
    public LayerMask Ignore;
    public LayerMask RayMask;

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

            Vector3 Pos;
            Pos.x = XCoord;
            Pos.y = YCoord;
            Pos.z = 0;

            Quaternion Zero;
            Zero.x = 0;
            Zero.y = 0;
            Zero.z = 0;
            Zero.w = 0;

            GameObject StarClone = (GameObject) Instantiate(Star, Pos, Zero);
            StarClone.transform.parent = StarParent.transform;
            StarClone.name = "Star " + i;
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
            FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarBase;
            FirstStar = null;

        }
    }

    public bool SetLinkingStar(StarBehaviour StarToLink)
    {


        if (StarToLink == FirstStar)
        {

            LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>());
            LinkingStar = null;
            FirstStar.GetComponentInChildren<SpriteRenderer>().sprite = StarBase;
            FirstStar = null;

            return true;
        }
        else if (LinkingStar != null && StarToLink != LinkingStar && (movesAvailable > 0))
        {

            RaycastHit2D Hit = Physics2D.Raycast(LinkingStar.transform.position, StarToLink.transform.position - LinkingStar.transform.position);

            if (Hit)
            {

                if (Hit.transform.name == StarToLink.transform.name)
                {
                    LinkingStar.SetLineTarget(StarToLink.GetComponent<Transform>());
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
