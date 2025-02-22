using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text[] countDownTexts;
    public float countDownTimer = 5f;
    private PlayerCarController[] playerCarControllers;
    private OpponentCar[] opponentCars;
    private OppenentCarWaypoints[] waypoints;

    void Awake(){
        playerCarControllers = FindObjectsOfType<PlayerCarController>();
        opponentCars = FindObjectsOfType<OpponentCar>();
        waypoints = FindObjectsOfType<OppenentCarWaypoints>();

        StartCoroutine(TimeCount());
    }

    IEnumerator TimeCount(){
        DisableScripts();
        float currentTime = countDownTimer;

        while(currentTime > 0){
            UpdateCountdownText(currentTime);
            yield return new WaitForSeconds(1f);
            currentTime --;
        }

        EnableScripts();

        UpdateCountdownText("GO");
        yield return new WaitForSeconds(1f);
        setCountDownTextActive(false);

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
        // countDownText.text = text;
        foreach(Text countDownText in countDownTexts){
            countDownText.text = text;
        }
    }

    void UpdateCountdownText(float time){
        // countDownText.text = text;
        foreach(Text countDownText in countDownTexts){
            countDownText.text = time.ToString("0");
        }
    }

    void setCountDownTextActive(bool isActive){
        foreach(Text countDownText in countDownTexts){
            countDownText.gameObject.SetActive(isActive);
        }
    }
}
