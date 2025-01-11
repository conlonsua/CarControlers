using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject leaderboardCanvas;
    public GameObject playerUI;

    [Header("Leaderboard Elements")]
    public Transform leaderboardContent;
    public GameObject leaderboardEntryPrefab;

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string carName;
        public float finishTime;
        public bool isPlayer;
    }

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private int totalCars = 5; // 4 AI + 1 player
    private int finishedCars = 0;
    private bool hasShownLeaderboard = false;

    private void Start()
    {
        leaderboardCanvas.SetActive(false);
        playerUI.SetActive(true);
    }

    public void AddEntry(string carName, float time, bool isPlayer)
    {
        Debug.Log($"Adding entry: {carName} with time {time}");
        LeaderboardEntry entry = new LeaderboardEntry
        {
            carName = carName,
            finishTime = time,
            isPlayer = isPlayer
        };

        leaderboardEntries.Add(entry);
        finishedCars++;
        
        Debug.Log($"Total finished cars: {finishedCars}/{totalCars}");
        UpdateLeaderboard();
        
        if (finishedCars >= totalCars && !hasShownLeaderboard)
        {
            hasShownLeaderboard = true;
            ShowLeaderboard();
            StartCoroutine(StopGameAfterDelay());
        }
    }

    private void UpdateLeaderboard()
    {
        // Sắp xếp entries theo thời gian (thấp đến cao)
        var sortedEntries = leaderboardEntries.OrderBy(e => e.finishTime).ToList();

        // Xóa các entries cũ trong UI
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Tạo entries mới
        for (int i = 0; i < sortedEntries.Count; i++)
        {
            GameObject entryGO = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            var entry = sortedEntries[i];

            // Lấy các Text components từ prefab
            Text[] texts = entryGO.GetComponentsInChildren<Text>();
            if (texts.Length >= 3)
            {
                texts[0].text = (i + 1).ToString(); // Place
                texts[1].text = entry.carName; // Name
                texts[2].text = FormatTime(entry.finishTime); // Time

                // Highlight nếu là player
                if (entry.isPlayer)
                {
                    foreach (Text text in texts)
                    {
                        text.color = Color.green;
                    }
                }
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60);
        int seconds = (int)(timeInSeconds % 60);
        int milliseconds = (int)((timeInSeconds * 100) % 100);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void ShowLeaderboard()
    {
        Debug.Log("Showing leaderboard");
        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(true);
            playerUI.SetActive(false);
            
            // Chỉ dừng game khi tất cả xe đã về đích
            if (finishedCars >= totalCars)
            {
                StartCoroutine(StopGameAfterDelay());
            }
        }
        else
        {
            Debug.LogError("LeaderboardCanvas is null!");
        }
    }

    private IEnumerator StopGameAfterDelay()
    {
        yield return new WaitForSeconds(2f); // Đợi 2 giây để người chơi thấy được leaderboard
        Time.timeScale = 0f;
    }

    public void HideLeaderboard()
    {
        leaderboardCanvas.SetActive(false);
        playerUI.SetActive(true);
    }

    public void ToggleLeaderboard()
    {
        if (leaderboardCanvas.activeSelf)
        {
            HideLeaderboard();
        }
        else
        {
            ShowLeaderboard();
        }
    }
}
