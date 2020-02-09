using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

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
    private AudioSource endGameBGMPlayer;
    [SerializeField]
    private AudioClip winBGM;
    [SerializeField]
    private AudioClip loseBGM;
	[SerializeField]
	private AudioClip currentTrack = null;
    [SerializeField]
    private Button playButtonImage;
    private bool resultSoundPlaying;
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
                    if (answerCheckMode)
                    {
                        if (!answerResults[currNoiseIndex])
                        {
                            noisePlayer.PlayOneShot(currentAnswerSet[currNoiseIndex]);
                            DistortMainTrack();
                            CancelInvoke("RestoreMainTrack");
                            Invoke("RestoreMainTrack", currentAnswerSet[currNoiseIndex].length);
                        }
                        else
                        {

                            // If the answer was correct
                            //noisePlayer.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/Correct"));
                        }


                    }
                    else
                    {
                        noisePlayer.PlayOneShot(currentAnswerSet[currNoiseIndex]);
                        DistortMainTrack();
                        CancelInvoke("RestoreMainTrack");
                        Invoke("RestoreMainTrack", currentAnswerSet[currNoiseIndex].length);

                    }
                    currNoiseIndex++;
                }

            }

            if (mainTrackPlayer.clip.length - mainTrackPlayer.time < 2)
            {
                if (answerCheckMode && !resultSoundPlaying)
                {
                    if (!levelResult)
                    {
                        // If the answer was wrong
                        //Debug.Log("Wrong");
                        noisePlayer.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/Incorrect_3"));
                        // Penalty
                    }
                    else
                    {
                        noisePlayer.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/Correct"));
                    }

                    resultSoundPlaying = true;
                }
            }
		} else
		{
            if (answerCheckMode)
            {
                GameManager.Instance.ModifyTimer(levelResult);
            }
            resultSoundPlaying = false;
            answerCheckMode = false;
			currNoiseIndex = 0;
			if (levelResult)
			{
				levelResult = false;
				
				GameManager.Instance.LoadNewPuzzle();
			}
		}
    }

    public bool IsPlayingMusic()
    {
        return (mainTrackPlayer.isPlaying);
    }

    public void StopMusicPlay()
    {
        levelResult = false;
        ChangePlayButtonSpriteState(false);
        mainTrackPlayer.Stop();
    }

    public void DistortMainTrack()
    {
        float distortion = UnityEngine.Random.Range(0.9f, 1.1f);
        StopCoroutine("ModifyMainTrackPitch");
        StartCoroutine("ModifyMainTrackPitch", distortion);
    }
    IEnumerator ModifyMainTrackPitch(float target)
    {
        float rate = target - mainTrackPlayer.pitch;
        float origPitch = mainTrackPlayer.pitch;

        while (mainTrackPlayer.pitch != target)
        {
            mainTrackPlayer.pitch += rate * Time.deltaTime;
            float min = target;
            float max = target;
            if (rate > 0)
            {
                min = origPitch;
            }
            else
            {
                max = origPitch;
            }
            mainTrackPlayer.pitch = Mathf.Clamp(mainTrackPlayer.pitch, min, max);
            yield return null;
        }

    }
    public void RestoreMainTrack()
    {
        StopCoroutine("ModifyMainTrackPitch");
        StartCoroutine("ModifyMainTrackPitch", 1);
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
			currentAnswerTimeSet.Add(UnityEngine.Random.Range(0, currentTrack.length - 5));
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
		
		levelResult = result;

        StartPlayingTrack();
        Debug.Log("Level Result = "  + levelResult);
	}
	
	public void StartPlayingTrack()
	{
		currNoiseIndex = 0;
		mainTrackPlayer.clip  = currentTrack;
        if (!levelResult)
        {
            DistortMainTrack();
        }
        else
        {
            RestoreMainTrack();
        }

        if (!answerCheckMode)
        {
            //answerResults = new List<bool>(currentAnswerSet.Count);
            if (mainTrackPlayer.isPlaying)
                mainTrackPlayer.Stop();
            else
                mainTrackPlayer.Play();
        }
        else
        {
            mainTrackPlayer.Play();
        }

        ChangePlayButtonSpriteState(mainTrackPlayer.isPlaying);
    }
	
    public void ChangePlayButtonSpriteState(bool isPlaying)
    {
        if (isPlaying)
        {
            playButtonImage.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/stop");
            SpriteState newSpriteState = playButtonImage.spriteState;
            newSpriteState.highlightedSprite = Resources.Load<Sprite>("Sprites/stopHover");
            playButtonImage.spriteState = newSpriteState;
        }
        else
        {
            playButtonImage.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/play");
            SpriteState newSpriteState = playButtonImage.spriteState;
            newSpriteState.highlightedSprite = Resources.Load<Sprite>("Sprites/playHover");
            playButtonImage.spriteState = newSpriteState;
        }
    }
	public void SetCurrentSoundTrack(AudioClip newTrack)
	{
		currentTrack = newTrack;
	}
	
    public void PlayEndGameBGM(bool playerWon)
    {
        if (playerWon)
            endGameBGMPlayer.clip = winBGM;
        else
            endGameBGMPlayer.clip = loseBGM;

        endGameBGMPlayer.Play();
            
    }

    public void PlayRepairObjectInputSound()
    {
        noisePlayer.PlayOneShot(Resources.Load<AudioClip>("Sounds/SFX/Welder_Repair_Short_1"));
    }
	public GameObject GetGameObjectByAudio(AudioClip clip)
	{
		return dict[clip];
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

    public void playTheNoise(AudioClip clip)
    {
        noisePlayer.clip = clip;
        noisePlayer.Play();
    }
}
