using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Npgsql;
using System.Linq;
using System;

public class LeaderboardScore
{
    public string Name { get; }
    public int Score { get; }
    
    public LeaderboardScore(string name, int score)
    {
        Name = name;
        Score = score;
    }
}

public class LeaderboardHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] PlayerController player;
    ScrollRect scrollRect;
    public Text scores;
    List<LeaderboardScore> everyScoreFromLeaderboard;
    [SerializeField] GameObject highScorePanel;

    private const string connString = "Host=ec2-52-7-228-45.compute-1.amazonaws.com;Username=lqxgwvjxwjtwqh;Password=4a0f9daf896f0f5aeb49e711183c96e50dabb063aa653afe4611885788beeb08;Database=daj2sfubcko33d;SSL Mode=Require;TrustServerCertificate = true";


    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        everyScoreFromLeaderboard = new List<LeaderboardScore>();
        ConnectToPostgres();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void ConnectToPostgres()
    {
        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        // Retrieve all rows
        var cmd = new NpgsqlCommand("SELECT * FROM Leaderboard ORDER BY Score DESC", conn);
        try
        {
            everyScoreFromLeaderboard = new List<LeaderboardScore>();
            //await cmd4.ExecuteNonQueryAsync();
            var reader = await cmd.ExecuteReaderAsync();
            int i = 0;
            while (await reader.ReadAsync())
            {
                string name = reader.GetString(0);

                int s = reader.GetInt32(1);
                //int.TryParse(reader.GetString(1), out s);

                //scores[i].text = name + ": " + s;
                Debug.Log(reader.GetString(0));
                i++;
                everyScoreFromLeaderboard.Add(new LeaderboardScore(name, s));
            }
            i = 0;
        }
        finally
        {
            cmd.Dispose();
            // cmd4.Dispose();
            MakeLeaderboard();
            conn.Close();
        }
    }

    void MakeLeaderboard()
    {
        scores.text = "\n";
        int rank = 1;
        foreach (LeaderboardScore score in everyScoreFromLeaderboard)
        {
            scores.text += rank + " " + score.Name + ": " + score.Score + "\n\n";
            rank++;
        }
    }

    /// <summary>
    /// Opens 3 connections
    /// Selects all rows and stores them in a list
    /// closes first connection
    /// Checks if the incoming name exists in that list
    ///     if it does - updates if score is lower
    ///     if it doesnt - inserts
    /// closes second connection
    /// selects all rows again and rewrites the leaderboard with updated table
    /// closes third connection
    /// </summary>
    /// <param name="leaderboardScore"></param>
    public async void AddHighScore(LeaderboardScore newScore)
    {
        var connString = "Host=ec2-52-7-228-45.compute-1.amazonaws.com;Username=lqxgwvjxwjtwqh;Password=4a0f9daf896f0f5aeb49e711183c96e50dabb063aa653afe4611885788beeb08;Database=daj2sfubcko33d;SSL Mode=Require;TrustServerCertificate = true";

        await using var conn = new NpgsqlConnection(connString);
        await using var writeConn = new NpgsqlConnection(connString);
        await using var readConn = new NpgsqlConnection(connString);

        await conn.OpenAsync();
        await writeConn.OpenAsync();
        await readConn.OpenAsync();

        var cmd = new NpgsqlCommand("SELECT * FROM Leaderboard ORDER BY Score DESC", conn);
        var updateCommand = new NpgsqlCommand($"UPDATE Leaderboard SET Score = {newScore.Score} WHERE Name = '{newScore.Name}'", writeConn);
        var insertCommand = new NpgsqlCommand($"INSERT INTO Leaderboard(Name, Score) VALUES('{newScore.Name}', {newScore.Score})", writeConn);

        try
        {
            everyScoreFromLeaderboard = new List<LeaderboardScore>();
            var reader = await cmd.ExecuteReaderAsync();
            int i = 0;
            while (await reader.ReadAsync())
            {
                string name = reader.GetString(0);

                int s = reader.GetInt32(1);
                //Debug.Log(reader.GetString(0));
                i++;
                everyScoreFromLeaderboard.Add(new LeaderboardScore(name, s));
            }
            i = 0;
        }
        finally
        {
            conn.Close();
            cmd.Dispose();
            try
            {
                var existingScore = everyScoreFromLeaderboard.FirstOrDefault(s => s.Name == newScore.Name);
                if (existingScore != null)
                {
                    //update if new score is higher
                    if (existingScore.Score < newScore.Score)
                        ShowHighScoreDialog();
                        await updateCommand.ExecuteNonQueryAsync();
                }
                else
                {
                    //insert
                    ShowHighScoreDialog();
                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                insertCommand.Dispose();
                updateCommand.Dispose();
                writeConn.Close();
                cmd = new NpgsqlCommand("SELECT * FROM Leaderboard ORDER BY Score DESC", readConn);
                try
                {
                    everyScoreFromLeaderboard = new List<LeaderboardScore>();
                    var reader = await cmd.ExecuteReaderAsync();
                    int i = 0;
                    while (await reader.ReadAsync())
                    {
                        string name = reader.GetString(0);

                        int s = reader.GetInt32(1);
                        Debug.Log(reader.GetString(0));
                        i++;
                        everyScoreFromLeaderboard.Add(new LeaderboardScore(name, s));
                    }
                    i = 0;
                }
                finally
                {
                    cmd.Dispose();
                    readConn.Close();
                    MakeLeaderboard();
                }
            }
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

