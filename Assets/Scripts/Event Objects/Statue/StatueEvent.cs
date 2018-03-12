﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;


public class StatueEvent : EventObject {

	public bool pushing = false;
	public int rotateFlag = 1;

	public StatueContainer container; 

	float push_strength = 0.5f;

	void OnTriggerStay(Collider other) {
		if(other.gameObject != Juanito.ins.JuanitoHuman)
			return;

		if(container.disabled)
		{
			if(pushing)
			{
				pushing = false;
				UIManager.instance.pushHelp.SetActive(false);
				UIManager.instance.pushMoveHelp.SetActive(false);
				Juanito.ins.HumanAnim.SetBool("Pushing", false);
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
 				UIManager.instance.dialogueSystem.addDialogue("The statue locks in place and refuses to budge.");
 				StatuePuzzleManager.ins.CheckStatueRotations();
			}

			return;
		}

		UIManager.instance.pushHelp.SetActive(true);

		if((Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButton("Action"))
			 && CheckPlayerDirection(Juanito.ins.JuanitoHuman))
		{
			UIManager.instance.pushHelp.SetActive(false);
			UIManager.instance.pushMoveHelp.SetActive(true);
			Juanito.ins.HumanAnim.SetBool("Pushing", true);
			pushing = true;
			Juanito.ins.transform.parent = transform.parent;
			Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = false;
			Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = false;
		}
		else
		{
			if(pushing)
			{
				Juanito.ins.HumanAnim.SetBool("Pushing", false);
				UIManager.instance.pushMoveHelp.SetActive(false);
				pushing = false;
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
			}
		}

		if(pushing)
		{
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
			bool keyboard = !(Mathf.Abs(h) < 0.1f && Mathf.Abs(v) < 0.1f);

			if (!keyboard)
			{
				h = CrossPlatformInputManager.GetAxis("Horizontal-Joystick");
				v = CrossPlatformInputManager.GetAxis("Vertical-Joystick");
			}

			Juanito.ins.HumanAnim.SetFloat("Forward", v);

            transform.parent.transform.Rotate(v * Vector3.up * rotateFlag * push_strength);
            container.currentRotation += v * rotateFlag * push_strength;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == Juanito.ins.JuanitoHuman)
		{
			if(pushing)
			{
				Juanito.ins.HumanAnim.SetBool("Pushing", false);
				pushing = false;
				Juanito.ins.transform.parent = null;
				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonUserControl>().enabled = true;
 				Juanito.ins.JuanitoHuman.GetComponent<ThirdPersonCharacter>().enabled = true;
			}

			UIManager.instance.pushHelp.SetActive(false);
			UIManager.instance.pushMoveHelp.SetActive(false);


		}
	}
}
