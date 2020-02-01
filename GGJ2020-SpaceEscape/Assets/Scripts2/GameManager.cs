using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	[SerializeField]
	private Transform AnswerFrameArea;
	[SerializeField]
	private Transform ObjectsFrameArea;
	[SerializeField]
	private LevelData[] levels;
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
		
	}
}


[System.Serializable]
public struct LevelData
{
	public AudioClip soundTrack;
	public int correctAnswers;
	public int allAnswers;
}