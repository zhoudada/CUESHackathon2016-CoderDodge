using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using Utility;

public class GameController : MonoBehaviour
{
    public static string ObstacleTag = "Obstacle";
    public static GameController Instance;

    [SerializeField]
    private float _initialMovingSpeed = 7.5f;
    [SerializeField]
    private AvatarController _avatarController;
    [SerializeField]
    private bool _testInput;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private GameObject _gameOverTexts;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _gameFinish;
    [SerializeField]
    private GameObject _highScoresGameObject;
    [SerializeField]
    private Text _highScoresText;
    //[SerializeField]
    //private GameObject _highScoreSubmit;
    [SerializeField]
    private GameObject _retryHighScoreServer;

    private MicrobitReceiverClient _receiver;
    private HighScoreClient _highScoreClient;
    private int _port = 9999;
    private bool _gameOver;
    private int _score;
    private int _failedRetryCount = 0;
    private int _failedRetryMax = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }


        Miscellaneous.CheckNullAndLogError(_avatarController);
        Miscellaneous.CheckNullAndLogError(_scoreText);
        Miscellaneous.CheckNullAndLogError(_gameOverTexts);

        _receiver = new MicrobitReceiverClient(_port);
        _highScoreClient = GetComponent<HighScoreClient>();
        Miscellaneous.CheckNullAndLogError(_highScoreClient);
    }

    void Start()
    {
        _avatarController.UpdateForwardSpeed(_initialMovingSpeed);
    }

    private void UpdateFromInput(MicrobitData data)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _avatarController.MoveHorizontal(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _avatarController.MoveHorizontal(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _avatarController.Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
        }
        if (data != null)
        {
            if (data.Left)
            {
                _avatarController.MoveHorizontal(-1);
            }
            else if (data.Right)
            {
                _avatarController.MoveHorizontal(1);
            }
            else if (data.Front)
            {
                _avatarController.Jump();
            }
        }
    }

    void FixedUpdate()
    {
        if (_testInput)
        {
            UpdateFromInput(null);
        }
        else
        {
            if (_receiver.Connected)
            {
                MicrobitData data = _receiver.GetRxData();
                if (data == null)
                {
                    Debug.Log("No data yet.");
                }
                else
                {
                    //Debug.LogWarning(data);
                    UpdateFromInput(data);
                }
            }
        }
    }

    void Update()
    {
        UpdateScore();
    }

    private void ReleaseCameraAndHideAvatar()
    {
        Camera.main.transform.SetParent(null);
        _avatarController.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        _gameOver = true;
        Debug.Log("GameOver");
        Explode(_avatarController.transform.position);
        ReleaseCameraAndHideAvatar();
        _gameOverTexts.SetActive(true);
        SoundManager.Instance.PlayFailedSound();
        ManageNewScore();
    }

    private void UpdateScore()
    {
        if (!_gameOver)
        {
            _score = (int)(Time.time / 5);
            _scoreText.text = string.Format("Score: {0}", _score);
        }
    }

    private void Explode(Vector3 position)
    {
        Instantiate(_explosion, position, Quaternion.identity);
    }

    public void FinishGame()
    {
        ReleaseCameraAndHideAvatar();
        _gameOver = true;
        _gameFinish.SetActive(true);
        SoundManager.Instance.PlayLevelClearSound();
        ManageNewScore();
    }

    private void UpdateHighScores(HighScores highScores)
    {
        _highScoresText.text = highScores.ToString();
    }

    //private void ShowSubmitHighScore()
    //{
    //    _highScoreSubmit.SetActive(true);
    //}

    private void ShowRetryButton()
    {
        _retryHighScoreServer.SetActive(true);
    }

    private void HideRetryButton()
    {
        _retryHighScoreServer.SetActive(false);
    }

    public void ManageNewScore()
    {
        _highScoresGameObject.SetActive(true);
        StartCoroutine(_highScoreClient.GetScores(OnGetHighScoreSuccess, OnGetHighScoreFailed));
        HideRetryButton();
    }

    private void OnUploadScoreSuccess(HighScores highScores)
    {
        Debug.Log("Upload score success.");
        _failedRetryCount = 0;
        UpdateHighScores(highScores);
    }

    private void OnUploadScoreFailed()
    {
        _failedRetryCount++;
        Debug.Log("Upload score failed.");
        if (_failedRetryCount <= _failedRetryMax)
        {
            StartCoroutine(_highScoreClient.UploadNewScore(
                _score, OnUploadScoreSuccess, OnUploadScoreFailed));
        }
        else
        {
            ShowRetryButton();
        }
    }

    private void OnGetHighScoreSuccess(HighScores highScores)
    {
        Debug.Log("Get high score success.");
        _failedRetryCount = 0;
        if (_score > highScores.third)
        {
            StartCoroutine(_highScoreClient.UploadNewScore(
                _score, OnUploadScoreSuccess, OnUploadScoreFailed));
        }
        else
        {
            UpdateHighScores(highScores);
        }
    }


    private void OnGetHighScoreFailed()
    {
        Debug.Log("Get high score failed.");
        _failedRetryCount++;
        if (_failedRetryCount <= _failedRetryMax)
        {
            StartCoroutine(_highScoreClient.GetScores(OnGetHighScoreSuccess, OnGetHighScoreFailed));
        }
        else
        {
            ShowRetryButton();
        }
    }

    void OnDestroy()
    {
        if (_receiver != null)
        {
            _receiver.Dispose();
        }
    }
}
