using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairSlots : MonoBehaviour
{
	[SerializeField]
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
