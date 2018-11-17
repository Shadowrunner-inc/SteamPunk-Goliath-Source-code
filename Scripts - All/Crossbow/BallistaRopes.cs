using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaRopes : MonoBehaviour 
{

	public LineRenderer theString;

	public Transform point01;
	public Transform point02;


	void Start () 
	{
		
	}
	

	void Update () 
	{
		theString.SetPosition (0, point01.position);
		theString.SetPosition (1, point02.position);
	}


}
