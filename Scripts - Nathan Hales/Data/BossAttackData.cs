/// Author: Nate Hales
/// <summary>
/// Boss attack data. This is a class that can be instansianed by other scripts as a bass template to build a bosses attack arround.
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossAttackData  {

	public string attackName;
	public int attackIndex;

	public float damageValue; 
	public float attackRange;
	public AudioClip attackSound;
	public AnimationClip attackAnim;

	public bool bCanInterrupt;

	public enum targetType{
		AreaOfEffect,
		SingleTarget,
		Defualt
	}

	public targetType attackType;

}
