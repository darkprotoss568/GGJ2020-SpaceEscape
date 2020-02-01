using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairSlots : MonoBehaviour
{
	private AudioClip currentAnswer;
    
	public AudioClip CurrentAnswer
	{
		get 
		{
			return currentAnswer;
		}
		set
		{
			currentAnswer = value;
		}
		
	}
}
