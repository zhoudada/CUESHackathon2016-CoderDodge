using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class RestfulAPITest : MonoBehaviour
{
    
    IEnumerator Start()
    {
        string url = "http://localhost:5000/updateScores";
        // Start a download of the given URL
        WWWForm form = new WWWForm();
        form.AddField("newScore", 100);
        WWW www = new WWW(url, form);

        // Wait for download to complete
        yield return www;

        Debug.Log(www.text);
        HighScores highScores = JsonUtility.FromJson<HighScores>(www.text);
        Debug.Log(highScores);
    }
}
