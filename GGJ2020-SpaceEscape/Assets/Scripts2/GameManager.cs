using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField]
	private Transform answerFrameArea;
	[SerializeField]
	private Transform repairObjsFrameArea;
	[SerializeField]
	private LevelData[] levels;
	private LevelSlotsLayout currentLevelLayout;
	private int currentLevel = -1;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance != null)
			Destroy(gameObject);
		else
			Instance = this;
	}
	
	void Start()
	{
		LoadNewLevel();
	}
    // Update is called once per frame
    void Update()
    {
        
    }

	
	public void LoadNewLevel()
	{
		currentLevel ++;
		LevelData level = levels[currentLevel];
		MusicGameplayManager.Instance.SetCurrentSoundTrack(level.soundTrack);
		List<AudioClip> allAnswers = MusicGameplayManager.Instance.CreateNewAnswerSet(level.correctAnswers, level.allAnswers);
		if (currentLevelLayout != null)
			Destroy(currentLevelLayout.gameObject);
		currentLevelLayout = Instantiate(level.levelLayout, answerFrameArea).GetComponent<LevelSlotsLayout>();
		
	}
	
	public void CheckAnswers()
	{
		List<AudioClip> answers = new List<AudioClip>();
		for (int i = 0; i < levels[currentLevel].correctAnswers; i++)
		{
			answers.Add(currentLevelLayout.Slots[i].CurrentAnswer);
		}
		
		MusicGameplayManager.Instance.CheckAnswers(answers);
	}
}


[System.Serializable]
public struct LevelData
{
	public AudioClip soundTrack;
	public GameObject levelLayout;
	public int correctAnswers;
	public int allAnswers;
}