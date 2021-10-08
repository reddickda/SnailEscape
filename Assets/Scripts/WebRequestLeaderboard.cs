using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.Linq;

public class Message
{
    public string name { get; set; }
    public int score { get; set; }
    public DateTime ts { get; set; }
}

public class ReturnedScores
{
    public List<Message> message { get; set; }
}

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.
public class WebRequestLeaderboard : MonoBehaviour
{
    const string uri = "https://david-portfolio-site.herokuapp.com/";
    public List<LeaderboardScore> allLeaderBoardData = new List<LeaderboardScore>();
    [SerializeField] GameObject highScorePanel;
    [SerializeField]  public Text scores;

    void Start()
    {
        StartCoroutine(GetScores());
    }

    //{"name": "Spinboi", "score": 24560}
    public IEnumerator InsertScore(LeaderboardScore score)
    {
        var existingScore = allLeaderBoardData.FirstOrDefault(s => s.Name == score.Name);
        if (existingScore == null)
        {
            string postBody = $"{{\"name\":\"{score.Name}\",\"score\":{score.Score}}}";
            Debug.Log(postBody);
            UnityWebRequest www = new UnityWebRequest(uri + "submitScore", "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postBody);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + "scores"))
            {
                allLeaderBoardData = new List<LeaderboardScore>();
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        //eachLine = webRequest.downloadHandler.text.Split(',');
                        ReturnedScores returnedScores = JsonSerializer.Deserialize<ReturnedScores>(webRequest.downloadHandler.text);
                        foreach (Message msg in returnedScores.message)
                        {
                            allLeaderBoardData.Add(new LeaderboardScore(msg.name, msg.score));
                        }
                        MakeLeaderboard();
                        break;
                }
            }
        }
        else if (existingScore.Score < score.Score)
        {
            string postBody = $"{{\"name\":\"{score.Name}\",\"score\":{score.Score}}}";
            Debug.Log(postBody);
            UnityWebRequest www = new UnityWebRequest(uri + "updateScore", "PUT");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(postBody);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
            ShowHighScoreDialog();
            //re request and make leaderboard
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri + "scores"))
            {
                allLeaderBoardData = new List<LeaderboardScore>();
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        //eachLine = webRequest.downloadHandler.text.Split(',');
                        ReturnedScores returnedScores = JsonSerializer.Deserialize<ReturnedScores>(webRequest.downloadHandler.text);
                        foreach (Message msg in returnedScores.message)
                        {
                            allLeaderBoardData.Add(new LeaderboardScore(msg.name, msg.score));
                        }
                        MakeLeaderboard();
                        break;
                }
            }
        }
    }

    public IEnumerator GetScores()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri+"scores"))
        {
            allLeaderBoardData = new List<LeaderboardScore>();
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //eachLine = webRequest.downloadHandler.text.Split(',');
                    ReturnedScores returnedScores = JsonSerializer.Deserialize<ReturnedScores>(webRequest.downloadHandler.text);
                    foreach(Message msg in returnedScores.message)
                    {
                        allLeaderBoardData.Add(new LeaderboardScore(msg.name, msg.score));
                    }
                    MakeLeaderboard();
                    break;
            }
            //foreach(LeaderboardScore score in allLeaderBoardData)
            //{
            //   Debug.Log(score.Name + " : " + score.Score);
            //}
        }
    }

    void MakeLeaderboard()
    {
        scores.text = "\n";
        int rank = 1;
        foreach (LeaderboardScore score in allLeaderBoardData)
        {
            scores.text += rank + " " + score.Name + ": " + score.Score + "\n\n";
            rank++;
        }
    }
    void ShowHighScoreDialog()
    {
        highScorePanel.SetActive(true);
        var closeButton = highScorePanel.GetComponentInChildren<Button>();
        closeButton.onClick.AddListener(close);
    }

    void close()
    {
        highScorePanel.SetActive(false);
    }
}
