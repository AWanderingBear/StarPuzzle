using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class StartUpManager : MonoBehaviour
{

    // Use this for initialization
    //void Start () {

    //}

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Menu");

    }
}
