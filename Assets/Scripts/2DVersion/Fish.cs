using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public static event Action<int> OnCoinMatch;

	public CoinType type;

	public Transform scoreTextSpawnPoint;

	Color ColorA;
	Color ColorB;
	Color ColorC;

	Color _color;

	private Renderer _renderer;

	public Vector2 originPos;
	bool isDraggable;

	private FishHealth _fishHealth;

	void SetColors()
	{

		ColorA = GameController.Instance.gameSettings.colorA;
		ColorB = GameController.Instance.gameSettings.colorB;
		ColorC = GameController.Instance.gameSettings.colorC;
	}

	private void Awake()
	{
		_renderer = GetComponent<Renderer>();
		_fishHealth = GetComponent<FishHealth>();

		SetColors();
		//Generate();
	}

	void Update()
	{
		if (_fishHealth.IsFedup)
		{
			GameObject.Destroy(gameObject);
		}

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Bubble>())
		{
			if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().type == type)
			{
				OnCoinMatch?.Invoke(other.GetComponent<Bubble>().ScoreCount);

				_fishHealth.ChangeHealth(20);

				SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount);
			}
			else if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().type != type)
			{
				OnCoinMatch?.Invoke(-other.GetComponent<Bubble>().ScoreCount);

				SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount, true);

				_fishHealth.ChangeHealth(-20);
			}

			other.GetComponent<Bubble>().SelfDestroy();
		}
		else if (other.GetComponent<Fish>())
		{
			if (!isDraggable)
			{
				return;
			}

			isDraggable = false;
			transform.position = other.GetComponent<Fish>().transform.position;
			other.GetComponent<Fish>().transform.position = originPos;

		}
	}

	void SpawnCoinScroreText(int scoreCount, bool wrongCoin = false)
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(scoreTextSpawnPoint.position);

		var scoreGO = Instantiate(GameController.Instance.coinScoreTextPref, GameController.Instance.canvas.transform);
		scoreGO.transform.position = pos;

		scoreGO.GetComponent<CoinScoreText>().SetScore(scoreCount, wrongCoin);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!isDraggable)
		{
			return;
		}

		//return;

		////Very nice approach for 2D objects dragging
		//transform.position = eventData.position;


		// Solution #01
		Plane plane = new Plane(Vector3.forward, transform.position);
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

		if (plane.Raycast(ray, out float distamce))
		{
			var vec = ray.origin + ray.direction * distamce;
			transform.position = new Vector2(vec.x, transform.position.y);
		}

		// Solution #02
		//Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
		//Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
		//Vector3 PN = -Camera.main.transform.forward; // Take current negative camera's forward as Plane's Normal
		//float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
		//Vector3 P = R.origin + R.direction * t; // Find the new point.

		//transform.position = P;
	}

	public void ChangePosition(Vector3 value)
	{
		gameObject.transform.position = value;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.GetComponent<Bubble>())
		{
			return;
		}

		other.GetComponent<Bubble>().SelfDestroy();
	}

	public void Setup(CoinType type)
	{
		this.type = type;

		switch (this.type)
		{
			case CoinType.A:
				_color = ColorA;
				break;
			case CoinType.B:
				_color = ColorB;
				break;
			case CoinType.C:
				_color = ColorC;
				break;
			default:
				break;
		}

		_renderer.material.color = _color;
	}

	void Generate()
	{
		type = (CoinType)UnityEngine.Random.Range(0, 2);

		switch (type)
		{
			case CoinType.A:
				_color = ColorA;
				break;
			case CoinType.B:
				_color = ColorB;
				break;
			case CoinType.C:
				_color = ColorC;
				break;
			default:
				break;
		}

		_renderer.material.color = _color;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDraggable = true;
		originPos = transform.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{

	}
}
