using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdpersocam : MonoBehaviour  {

    public GameObject target;
    public Vector3 offset = new Vector3(10,10,0);
		
	//we use lateupdate cause its called after all other update funtions so the camera follows just after the player movement.
	void LateUpdate ()
    {
        transform.position = target.transform.position + offset;
	}
}
