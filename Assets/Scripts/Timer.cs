using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text countDownText;
    public float countDownTimer = 5f;
    public PlayerCarController[] playerCarControllers;
    public OpponentCar[] opponentCars;
    public OppenentCarWaypoints[] waypoints;

    void Awake(){
        playerCarControllers = FindObjectsOfType<PlayerCarController>();
        opponentCars = FindObjectsOfType<OpponentCar>();
        waypoints = FindObjectsOfType<OppenentCarWaypoints>();

        StartCoroutine(TimeCount());
    }

    IEnumerator TimeCount(){
        DisableScripts();

        while(countDownTimer > 0){
            UpdateCountdownText(countDownTimer.ToString("0"));
            yield return new WaitForSeconds(1f);
            countDownTimer --;
        }

        EnableScripts();

        UpdateCountdownText("GO");
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(false);

    }

    void DisableScripts()
    {
        foreach (OpponentCar opponentCar in opponentCars)
        {
            opponentCar.enabled = false;
        }

        foreach (OppenentCarWaypoints waypoint in waypoints)
        {
            waypoint.enabled = false;
        }

        foreach (PlayerCarController playerCarController in playerCarControllers)
        {
            playerCarController.enabled = false;
        }
    }

    void EnableScripts()
    {
        foreach (OpponentCar opponentCar in opponentCars)
        {
            opponentCar.enabled = true;
        }

        foreach (OppenentCarWaypoints waypoint in waypoints)
        {
            waypoint.enabled = true;
        }

        foreach (PlayerCarController playerCarController in playerCarControllers)
        {
            playerCarController.enabled = true; 

        }
    }

    void UpdateCountdownText(string text){
        countDownText.text = text;
    }
}
