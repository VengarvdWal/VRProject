using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


[RequireComponent(typeof(SteamVR_TrackedObject))]

public class Arrow : MonoBehaviour
{


	public SteamVR_Action_Boolean attach = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

	SteamVR_Behaviour_Pose trackedObj;

	bool isAttached = false;
	bool isFired = false;

	private void OnTriggerStay()
	{
		AttachArrow();
	}

	private void OnTriggerEnter()
	{
		AttachArrow();
	}

	private void Update()
	{
		if (isFired)
		{
			transform.LookAt(transform.position + transform.GetComponent <Rigidbody>().velocity);
		}
		
	}

	
	public void Fired()
	{
		isFired = true;
	}

	private void AttachArrow()
	{

		if (!isAttached)
		{
			ArrowManager.Instance.AttachBowToArrow();
			isAttached = true;
		}
		
		//{
		//	if (!isAttached && attach.GetStateUp(trackedObj.inputSource))
		//	{
		//		ArrowManager.Instance.AttachBowToArrow();
		//		isAttached = true;
		//	}
		//}
	}
}

