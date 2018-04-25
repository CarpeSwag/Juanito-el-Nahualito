﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulley_manager : MonoBehaviour {

	public pulley_platform plat1, plat2;

	private Vector3 topPos, bottomPos;

	// Use this for initialization
	void Start () {
		Vector3 p1 = plat1.GetPosition ();
		Vector3 p2 = plat2.GetPosition ();
		if (p1.y < p2.y) {
			topPos = p2;
			bottomPos = p1;
		} else {
			topPos = p1;
			bottomPos = p2;
		}
	}
	
	public void MovePlatforms(){
		float w1 = plat1.GetWeight ();
		float w2 = plat2.GetWeight ();
		if (w1 < w2) {
			plat1.MoveTo (topPos);
			plat2.MoveTo (bottomPos);
		} else if (w1 > w2) {
			plat1.MoveTo (bottomPos);
			plat2.MoveTo (topPos);
		}
	}
}
