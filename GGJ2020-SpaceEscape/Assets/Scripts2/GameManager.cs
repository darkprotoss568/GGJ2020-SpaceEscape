using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField]
	private Transform answerFrameArea;
	[SerializeField]
	private Transform repairObjsFrameArea;
	[SerializeField]
	private PuzzleData[] levels;
	private LevelSlotsLayout currentLevelLayout;
	private int currentPuzzleIndex = -1;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Text timeModText;
    [SerializeField]
    private float timer;
    private bool isInCritical;
    [SerializeField]
    private float bonusTimeOnCorrectAnswer;
    [SerializeField]
    private float penaltyTimeOnWrongAnswer;
    private float currentTimer;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject winScreen;

    [SerializeField]
    private Text levelText;
    [SerializeField]
    private GameObject replayButton;
    [SerializeField]
    private GameObject checkButton;
    // Start is called before the first frame update
    [SerializeField]
    private MouseHoverPrompt rightClickPrompt;

    [SerializeField]
    private int levelNumber = 1;
    void Awake()
    {
        if (GameManager.Instance != null)
			Destroy(gameObject);
		else
			Instance = this;
	}
	
	void Start()
	{
        currentTimer = timer;
		LoadNewPuzzle();
	}

    public void SwitchMode(bool checkAvailable)
    {
        if (checkAvailable)
        {
            if (replayButton.activeInHierarchy)
                replayButton.SetActive(false);
            if (!checkButton.activeInHierarchy)
                checkButton.SetActive(true);
        }
        else
        {
            bool answerInSlot = false;
            for (int i = 0; i < currentLevelLayout.Slots.Length; i++)
            {
                if (currentLevelLayout.Slots[i].CurrentAnswer != null)
                {
                    answerInSlot = true;
                    break;
                }
            }
            if (!answerInSlot)
            {
                if (!replayButton.activeInHierarchy)
                    replayButton.SetActive(true);
                if (checkButton.activeInHierarchy)
                    checkButton.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!MusicGameplayManager.Instance.IsPlayingMusic() && currentTimer > 0)
        {
            //Debug.Log(currentTimer);
            currentTimer = Mathf.Clamp(currentTimer - Time.deltaTime, 0, timer);
            timerText.text = System.Math.Round(currentTimer, 2).ToString();
        }

        if (currentTimer <= 10 && currentTimer > 0)
        {
            if (!isInCritical)
            {
                InvokeRepeating("TimerTextFlash", 0f, 0.2f);
                isInCritical = true;
            }
        } else
        {
            if (isInCritical)
            {
                CancelInvoke("TimerTextFlash");
                isInCritical = false;
            }
        }

        if (currentTimer == 0)
        {
            timerText.color = Color.red;
            if (!gameOverScreen.activeInHierarchy)
            {
                gameOverScreen.SetActive(true);
                MusicGameplayManager.Instance.PlayEndGameBGM(false);
            }
        }

        HandleGameScreenChangeInput();
    }

    private void HandleGameScreenChangeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.R))
                BackToMainMenu();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void SetMouseHoverPromptActive(bool value)
    {
        if (rightClickPrompt != null)
        {
            if (currentPuzzleIndex < 2 || !value)
                rightClickPrompt.SetActiveState(value);
        }
    }

    private void TimerTextFlash()
    {
        Color c = timerText.color;
        if (c == Color.white)
        {
            c = Color.red;
        }
        else if (c == Color.red)
        {
            c = Color.white;
        }

        timerText.color = c;
    }
	public void LoadNewPuzzle()
	{
		currentPuzzleIndex ++;
        if (currentPuzzleIndex >= levels.Length)
        {
            winScreen.SetActive(true);
            MusicGameplayManager.Instance.PlayEndGameBGM(true);
            return;
        }
		PuzzleData level = levels[currentPuzzleIndex];
		MusicGameplayManager.Instance.SetCurrentSoundTrack(level.soundTrack);
		List<AudioClip> allAnswers = MusicGameplayManager.Instance.CreateNewAnswerSet(level.correctAnswers, level.allAnswers);
		if (currentLevelLayout != null)
			Destroy(currentLevelLayout.gameObject);
		currentLevelLayout = Instantiate(level.puzzleLayout, answerFrameArea).GetComponent<LevelSlotsLayout>();
		
		List<Transform> repairPartsSpawnPoints = new List<Transform>(repairObjsFrameArea.gameObject.GetComponentsInChildren<Transform>());
        //Debug.Log("Count b = " + repairPartsSpawnPoints.Count);
        for (int i = 0; i < repairPartsSpawnPoints.Count; i++)
        {
            if (repairPartsSpawnPoints[i] == repairObjsFrameArea)
            {
                repairPartsSpawnPoints.RemoveAt(i);
                break;
            }
        }
        //Debug.Log("Count a = " + repairPartsSpawnPoints.Count);
        for (int i = 0; i < allAnswers.Count; i ++)
		{
			int index = UnityEngine.Random.Range(0, repairPartsSpawnPoints.Count);
			Transform parentTransform = repairPartsSpawnPoints[index];
			GameObject newPart = Instantiate(MusicGameplayManager.Instance.GetGameObjectByAudio(allAnswers[i]), parentTransform);
			newPart.GetComponent<RepairObjScript>().AnswerClip = allAnswers[i];
			//newPart.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			newPart.transform.SetParent(currentLevelLayout.gameObject.transform);
			newPart.GetComponent<DragDrop>().ResetStartingPos();
			repairPartsSpawnPoints.Remove(parentTransform);
		}

        //currentTimer = timer;
        //timerText.text = System.Math.Round(timer, 2).ToString();

        SwitchMode(false);
        MusicGameplayManager.Instance.ChangePlayButtonSpriteState(false);
        levelText.text = "Level " + levelNumber.ToString() + " - Part " + (currentPuzzleIndex + 1).ToString();
	}

    public void ModifyTimer(bool correctAnswer)
    {
        timeModText.gameObject.SetActive(true);
        if (correctAnswer)
        {
            currentTimer += bonusTimeOnCorrectAnswer;
            timeModText.text = "+" + bonusTimeOnCorrectAnswer.ToString() + "s";
            timeModText.color = Color.green;
        }
        else
        {
            currentTimer -= penaltyTimeOnWrongAnswer;
            timeModText.text = "-" + penaltyTimeOnWrongAnswer.ToString() + "s";
            timeModText.color = Color.red;
        }

        CancelInvoke("DeactivateTimeModText");
        Invoke("DeactivateTimeModText", 1f);
    }

    public void DeactivateTimeModText()
    {
        timeModText.gameObject.SetActive(false);

    }

    public void LoadNewLevel(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

	public void CheckAnswers()
	{
		List<AudioClip> answers = new List<AudioClip>();
		for (int i = 0; i < levels[currentPuzzleIndex].correctAnswers; i++)
		{
			answers.Add(currentLevelLayout.Slots[i].CurrentAnswer);
		}
		
		MusicGameplayManager.Instance.CheckAnswers(answers);
	}

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


[System.Serializable]
public struct PuzzleData
{
	public AudioClip soundTrack;
	public GameObject puzzleLayout;
	public int correctAnswers;
	public int allAnswers;
}