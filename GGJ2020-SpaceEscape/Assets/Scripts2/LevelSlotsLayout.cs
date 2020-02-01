using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSlotsLayout : MonoBehaviour
{
	[SerializeField]
	private RepairSlots[] slots;
	
	public RepairSlots[] Slots
	{
		get
		{
			return slots;
		}
	}
}
