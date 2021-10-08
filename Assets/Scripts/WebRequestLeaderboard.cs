using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.
public class WebRequestLeaderboard : MonoBehaviour
{
    string[] eachLine;
    public List<LeaderboardScore> allLeaderBoardData = new List<LeaderboardScore>();
    void Start()
    {
        // A correct website page.
        //StartCoroutine(GetRequest("http://localhost:3000/leaderboard.txt"));
        StartCoroutine(GetTest());
        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
        //StartCoroutine(Upload());
    }

    IEnumerator GetTest()
    {
        var uri = "https://david-portfolio-site.herokuapp.com/api";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        { 

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
                    Debug.Log(webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator Upload()
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/leaderboard.txt", myData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
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
                    eachLine = webRequest.downloadHandler.text.Split(',');
                    for(int i = 0; i < eachLine.Length; i++)
                    {
                        var nameAndScore = eachLine[i].Split(':'); // David:100,
                        var parsedScore = int.TryParse(nameAndScore[1], out int result);
                        allLeaderBoardData.Add(new LeaderboardScore(nameAndScore[0], result));
                    }
                    break;
            }
            foreach(LeaderboardScore score in allLeaderBoardData)
            {
               Debug.Log(score.Name + " : " + score.Score);
            }
        }
    }
}
