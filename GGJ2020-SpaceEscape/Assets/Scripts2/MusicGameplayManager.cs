using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MusicGameplayManager : MonoBehaviour
{
    public static MusicGameplayManager Instance;
    [SerializeField]
    private List<AudioClip> noises = new List<AudioClip>();
    [SerializeField]
    private List<GameObject> objectPrefabs = new List<GameObject>();
    private Dictionary<AudioClip, GameObject> dict = new Dictionary<AudioClip, GameObject>();
	
    private List<AudioClip> currentAnswerSet = new List<AudioClip>();
	private List<float> currentAnswerTimeSet = new List<float>();
    private List<AudioClip> currentAnswers = new List<AudioClip>();
	private List<bool> answerResults = new List<bool>();
	private bool levelResult = false;
	[SerializeField]
	private AudioSource mainTrackPlayer; 
	private bool answerCheckMode;
	[SerializeField]
	private AudioSource noisePlayer;
	private int currNoiseIndex = 0;
	[SerializeField]
	private AudioClip currentTrack = null;
	
	// Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        RandomizeObjectsAndSoundPair();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainTrackPlayer.isPlaying)
		{
			if (currNoiseIndex < currentAnswerSet.Count)
			{
				
				if (mainTrackPlayer.time >= currentAnswerTimeSet[currNoiseIndex])
				{
						
						Debug.Log("Played" + currentAnswerSet[currNoiseIndex]);
						noisePlayer.PlayOneShot(currentAnswerSet[currNoiseIndex]);
						
						if (answerCheckMode)
						{
							if (!answerResults[currNoiseIndex])
							{
								// If the answer was wrong
								// Penalty
							} else
							{
								// If the answer was correct
							}
						}
						currNoiseIndex ++;
				}

			}	
		} else
		{
			answerCheckMode = false;
			currNoiseIndex = 0;
			if (levelResult)
			{
				levelResult = false;
				
				GameManager.Instance.LoadNewLevel();
			}
		}
    }
	
    public List<AudioClip> CreateNewAnswerSet(int correctAnswers, int answers)
    {
        List<AudioClip> testList = new List<AudioClip>(noises);
        currentAnswerSet.Clear();
		currentAnswerTimeSet.Clear();
        for (int i = 0; i < correctAnswers; i++)
        {
            AudioClip answer = testList[UnityEngine.Random.Range(0, testList.Count)];
            currentAnswerSet.Add(answer);
			testList.Remove(answer);
        }
		
		List<AudioClip> result = new List<AudioClip>(currentAnswerSet);
		
        while (result.Count < answers)
        {
            AudioClip answer = testList[UnityEngine.Random.Range(0, testList.Count)];
            result.Add(answer);
            testList.Remove(answer);
        }
		
        for (var i = result.Count; i > 0; i--)
        {
            Swap(result, 0, UnityEngine.Random.Range(0, i));
        }
		
		for (int i = 0; i < currentAnswerSet.Count; i++)
		{
			currentAnswerTimeSet.Add(UnityEngine.Random.Range(0, currentTrack.length));
		}
		
		currentAnswerTimeSet.Sort(SortByFloatAscending);
		
        return result;
    }
	
	public int SortByFloatAscending(float a, float b)
	{
		return a.CompareTo(b);
	}
	
	public void CheckAnswers(List<AudioClip> answers)
	{
		bool result = true;
		answerResults.Clear();
		for (int i = 0; i < answers.Count; i ++)
		{
			if (answers[i] == null || answers[i] != currentAnswerSet[i])
			{
				result = false;
				answerResults.Add(false);
			} else
			{
				answerResults.Add(true);
			}
		}
		
		answerCheckMode = true;
		StartPlayingTrack();
		
		levelResult = result;
		Debug.Log("Level Result = "  + levelResult);
	}
	
	public void StartPlayingTrack()
	{
		currNoiseIndex = 0;
		mainTrackPlayer.clip  = currentTrack;
		mainTrackPlayer.Play();
	}
	
	public void SetCurrentSoundTrack(AudioClip newTrack)
	{
		currentTrack = newTrack;
	}
	
    public void RandomizeObjectsAndSoundPair()
    {
        
        for (var i = objectPrefabs.Count; i > 0; i--)
        {
            Swap(objectPrefabs, 0, UnityEngine.Random.Range(0, i));
        }
        dict = noises.Zip(objectPrefabs, (k, v) => new {Key = k, Value = v }).ToDictionary(x => x.Key, y => y.Value);
    }
	
	public void Swap<T>(List<T> list, int i, int j)
	{
		var temp = list[i];
		list[i] = list[j];
		list[j] = temp;
		
	}	
}
