using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [Header("Finish UI Var")]
    public GameObject finishUI;
    public GameObject playerUI;
    public GameObject playerCar;

    [Header("Win/Lose status")]
    public Text status;

    private float startTime;
    private float finishTime;

    private void Start(){
        StartCoroutine(waitforthefinishUI());
        startTime = Time.time;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            finishTime = Time.time - startTime;
            StartCoroutine(finishZoneTimer());
            gameObject.GetComponent<BoxCollider>().enabled = false;

            status.text = $"You Win\nTime: {finishTime:F2} seconds";
            status.color = Color.black;
        }
        else if(other.gameObject.tag == "OpponentCar"){
            StartCoroutine(finishZoneTimer());

            status.text = "You Lose";
            status.color = Color.red;
        }
    }

    IEnumerator waitforthefinishUI(){
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(10f);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    IEnumerator finishZoneTimer(){
        finishUI.SetActive(true);
        playerUI.SetActive(false);
        playerCar.SetActive(false);

        yield return new WaitForSeconds(5f);
        Time.timeScale = 0f;
    }
}