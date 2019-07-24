using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
	public Collider2D myCollider;

	ContactFilter2D contactFilter;

	int _contactLayer;
	int _ignoreLayer;

	Collider2D[] _results = new Collider2D[2];

	private void Awake()
	{
		Physics2D.alwaysShowColliders = true;
		//Physics2D.showColliderAABB = true;

		_contactLayer = 1 << LayerMask.NameToLayer("Contact");
		_ignoreLayer = 1 << LayerMask.NameToLayer("Draggable");

		contactFilter = new ContactFilter2D() { layerMask = _contactLayer, useLayerMask = true };

		//Physics2D.IgnoreLayerCollision(_contactLayer, _ignoreLayer, true);
	}

	private void Update()
	{
		Physics2D.OverlapCollider(myCollider, contactFilter, _results);

		//if (_results.Length > 0)
		//{
		foreach (var item in _results)
		{
			if (item != null)
			{
				/*Array.ForEach(_results, x => x = null);*/ Array.Clear(_results, 0, _results.Length);
				Debug.Log("Contact!");
			}
		}


		//}
	}
}
