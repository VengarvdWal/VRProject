using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]

public class ArrowManager : MonoBehaviour
{
	
	public static ArrowManager Instance;

	//public SteamVR_TrackedObject trackedObj;
		
	public Rigidbody attachPoint;

	private GameObject currentArrow;

	public GameObject stringAttachPoint;
	public GameObject arrowStartPoint;
	public GameObject stringStartPoint;
	public GameObject arrowPrefab;

	public SteamVR_Action_Boolean spawn = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
	private bool isAttached = false;

	SteamVR_Behaviour_Pose trackedObj;
	FixedJoint joint;

	private void Awake()
	{

		trackedObj = GetComponent<SteamVR_Behaviour_Pose>();

		if (Instance == null)
			Instance = this;
	}

	private void OnDestroy()
	{
		if (Instance == this)
			Instance = null;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//AttachArrow();
		PullString();
    }

	private void FixedUpdate()
	{
		if (joint == null && spawn.GetStateDown(trackedObj.inputSource) && currentArrow == null)
		{
			currentArrow = GameObject.Instantiate(arrowPrefab);
			currentArrow.transform.position = attachPoint.transform.position;
			currentArrow.transform.parent = trackedObj.transform;
			currentArrow.transform.localPosition = new Vector3(0f, 0f, .342f);
			currentArrow.transform.localRotation = Quaternion.identity;

			joint = currentArrow.AddComponent<FixedJoint>();
			joint.connectedBody = attachPoint;
		}
		else if (joint != null && spawn.GetStateUp(trackedObj.inputSource))
		{
			GameObject go = joint.gameObject;
			Rigidbody rigidbody = go.GetComponent<Rigidbody>();
			Object.DestroyImmediate(joint);
			joint = null;
			//Object.Destroy(go, 180.0f);
		}
	}

	private void PullString()
	{
		if (isAttached)
		{
			float distance = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
			stringAttachPoint.transform.localPosition = stringStartPoint.transform.localPosition + new Vector3(distance, 0f, 0f);

			if (spawn.GetStateUp(trackedObj.inputSource))
			{
				Fire();
			}
		}
	}

	private void Fire()
	{
		float distance = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;

		currentArrow.transform.parent = null;
		currentArrow.GetComponent<Arrow>().Fired();
		Rigidbody r = currentArrow.GetComponent<Rigidbody>();
		r.velocity = currentArrow.transform.forward * 20f * distance;
		r.useGravity = true;

		stringAttachPoint.transform.position = stringStartPoint.transform.position;

		currentArrow = null;
		isAttached = false;
	}

	//private void AttachArrow()
	//{
		//if(currentArrow != null)
		//{
			//currentArrow = Instantiate(arrowPrefab);
			//currentArrow.transform.parent = trackedObj.transform;
			//currentArrow.transform.localPosition = new Vector3(0f, 0f, .342f);
			//currentArrow.transform.localRotation = Quaternion.identity;
		//}
	//}
	public void AttachBowToArrow()
	{
		currentArrow.transform.parent = stringAttachPoint.transform;
		currentArrow.transform.position = arrowStartPoint.transform.position;
		currentArrow.transform.rotation = arrowStartPoint.transform.rotation;

		isAttached = true;
	}
}
