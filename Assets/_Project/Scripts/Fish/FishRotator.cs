using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		var rb = GetComponent<Rigidbody2D>();

		//var tergetDir = new Vector2(0, 180);

		//var targetRot = Quaternion.Euler(0, 180, 0);

		//GetComponent<Rigidbody2D>().MoveRotation(/*GetComponent<Rigidbody2D>().rotation**/targetRot);

	//var	_targetRotateDir = transform.position;
	// 	_targetRotateDir.y = 0f;
	//var	_targetRotation = Quaternion.LookRotation(_targetRotateDir, Vector3.up);

		//var _newRotation = Quaternion.FromToRotation(rb.rotation, _targetRotation, 2f * Time.deltaTime);
		//rb.MoveRotation(_newRotation);

		Debug.Log("rotated");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
