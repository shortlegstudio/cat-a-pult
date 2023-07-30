using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class ServerComs : MonoBehaviour
{
    public int NumSendErrors = 0;
    public static ServerComs Current { get; private set; }

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
    }

    public HighScores GetHighScores()
    {
        if (GamePreferences.Current.UseServerForScores)
        {
            StartCoroutine(RetrieveHighScoresFromServer());
        }
        return GamePreferences.Current.HighScoreTable;
    }

    IEnumerator RetrieveHighScoresFromServer()
    {
        if (NumSendErrors > 20)
            yield break;

        UnityWebRequest www = UnityWebRequest.Get(GamePreferences.Current.GetServerUrl());
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            NumSendErrors++;
        }
        else
        {
            HighScores res = JsonUtility.FromJson<HighScores>(www.downloadHandler.text);
            GamePreferences.Current.HighScoreTable = res;
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }

    public void AddGameScore(GameScore gameScore)
    {
        if (!GamePreferences.Current.UseServerForScores)
            return;

        var nextPayload = JsonUtility.ToJson(gameScore, false);
        if (nextPayload != null)
        {
            StartCoroutine(PostRequest(GamePreferences.Current.GetServerUrl(), nextPayload));
        }
    }

    IEnumerator PostRequest(string serverUrl, string payload)
    {
        if (NumSendErrors > 20)
            yield break;

        if (!string.IsNullOrEmpty(serverUrl))
        {
            var uwr = new UnityWebRequest(serverUrl, "POST");
            byte[] asBytes = new System.Text.UTF8Encoding().GetBytes(payload);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(asBytes);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.responseCode != 200)
            {
                NumSendErrors++;
            }
        }
    }
}
