using System.Collections;
using System.Collections.Generic;
using Terminus.Game.Messages;
using UnityEngine;

namespace BrightFish
{
	public class DestroyByBoundary : MonoBehaviour
	{
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.name == "View")
			{
				return;
			}

			if (collision.GetComponentInParent<Bubble>())
			{
				collision.GetComponentInParent<Bubble>().SelfDestroy();
			}
			else if (collision.GetComponentInParent<Fish>())
			{
				collision.GetComponentInParent<Fish>().Destroy();
			}
			else
			{
				Destroy(collision.gameObject);
			}
		}
	}
}
