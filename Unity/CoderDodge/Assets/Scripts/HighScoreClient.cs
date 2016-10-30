using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreClient: MonoBehaviour
{
    [SerializeField]
    private string _url;
    [SerializeField]
    private int _port = -1;

    private string UrlWithPort { get { return _port == -1 ? _url : _url + ':' + _port; } }

    public IEnumerator GetScores(Action<HighScores> onSuccess, Action onFailed = null)
    {
        WWW www = new WWW(UrlWithPort + "/highestScores");
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            HighScores highScores = JsonUtility.FromJson<HighScores>(www.text);
            onSuccess(highScores);
        }
        else
        {
            if (onFailed != null)
            {
                onFailed();
            }
        }
    }

    public IEnumerator UploadNewScore(int score, Action<HighScores> onSuccess, Action onFailed = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("newScore", score);
        WWW www = new WWW(UrlWithPort + "/updateScores", form);
        yield return www;
        Debug.Log("Uploading new score finished");
        if (string.IsNullOrEmpty(www.error))
        {
            HighScores highScores = JsonUtility.FromJson<HighScores>(www.text);
            onSuccess(highScores);
        }
        else
        {
            if (onFailed != null)
            {
                onFailed();
            }
        }
    }
}
