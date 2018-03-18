﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RelicPedestal : MonoBehaviour {

	public GameObject[] targetObjects;

	public Vector3 finalPosition;
	public Vector3 beginningPosition;

	public bool ButterflyRelic;
	public bool StatueRelic;

	public bool active = true;

	// Use this for initialization
	void Start () {
		finalPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
		beginningPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 0.457f, transform.localPosition.z);

		transform.localPosition = beginningPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman && active) 
		{
			if(Juanito.ins.CheckFacingObjects(targetObjects))
			{

				UIManager.instance.TooltipInteract();

				if (Input.GetKeyDown (KeyCode.E) || CrossPlatformInputManager.GetButtonDown ("Action")) 
				{
					UIManager.instance.TooltipDisable();
					transform.GetChild (0).gameObject.SetActive (false);
					if(ButterflyRelic) Juanito.ins.butterflyRelic = true;
					if(StatueRelic) Juanito.ins.statueRelic = true;
					StartCoroutine(LowerPedestal());
					//UIManager.instance.dialogueSystem.addDialogue("You picked up a relic.");
					active = false;
				}		
			}
			else
			{
				UIManager.instance.TooltipDisable();
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject == Juanito.ins.JuanitoHuman)
		{
			UIManager.instance.TooltipDisable();
		}
	}

	public IEnumerator RaisePedestal()
	{
		float k = 0;

		while (k < 1) 
		{
			transform.localPosition = Vector3.Lerp (beginningPosition, finalPosition, k);

			transform.Rotate(Vector3.up * Time.deltaTime * 100f);

			k += Time.deltaTime * Random.Range(0.2f,0.5f);
			yield return null;
		}
	}

	IEnumerator LowerPedestal()
	{
		float k = 0;

		while (k < 1) 
		{
			transform.localPosition = Vector3.Lerp (finalPosition, beginningPosition, k);

			transform.Rotate(Vector3.up * Time.deltaTime * 100f);

			k += Time.deltaTime * Random.Range(0.2f,0.5f);
			yield return null;
		}
	}

}
