using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Author: Nathan Hales
/// Objective data class used by the objecttive system.
/// </summary>
[System.Serializable]
public class ObjectiveData {

	public string description =""; 
	public string name  ="";

	public int currentCount = 0;
	public int total = 0;

	public bool bIsCompleted = false;

}
