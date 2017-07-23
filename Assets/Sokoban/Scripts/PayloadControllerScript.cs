using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadControllerScript : MonoBehaviour {

	private int num_goals = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		print ("enter " + other.tag);
		if (other.CompareTag("Goal")) {
			num_goals++;
			var pay1 = transform.FindChild ("payload1").gameObject;
			pay1.SetActive (false);
			var pay2 = transform.FindChild ("payload2").gameObject;
			pay2.SetActive (true);
			tag = "OnGoal";
		}
	}

	void OnTriggerExit(Collider other)
	{
		print ("exit " + other.tag);
		if (other.CompareTag("Goal")) {
			num_goals--;
			if (num_goals <= 0) {
				var pay1 = transform.FindChild ("payload1").gameObject;
				pay1.SetActive (true);
				var pay2 = transform.FindChild ("payload2").gameObject;
				pay2.SetActive (false);
				tag = "OffGoal";
			}
		}
	}
}
