using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	public static event Action<int> OnCoinMatch;
	public static event Action<BubbleType, Vector3> OnDeath;
	public static event Action<BubbleType, Vector3> OnHappy;

	public BubbleType type;

	[SerializeField] private GameObject _enemyHealthBarPref;
	[SerializeField] private Transform _healthbarPoint;

	public EnemyHealthBar healthBar;

	bool isDone;

	[SerializeField]
	Sounds feedFishGood,
	feedFishBad,
	fishDead,
	fishHappy;

	public Sprite[] sprites;


	bool isCollided;

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
		if (isDone)
		{
			return;
		}

		if (_fishHealth.IsDead)
		{
			GameController.Instance.sound.PlaySound(fishDead);

			//GameObject.Destroy(gameObject);
			isDone = true;

			//OnDeath?.Invoke(type, transform.position);

			SOmeDelayBeforeHide(() =>
			{
				OnDeath?.Invoke(type, transform.position);
				Destroy();
			});

			//Destroy();
		}
		else if (_fishHealth.IsFedup)
		{
			isDone = true;

			SOmeDelayBeforeHide(() =>
			{
				OnHappy?.Invoke(type, transform.position);
				Destroy();
			});
		}

		UpdateSprite();

	}

	async void SOmeDelayBeforeHide(Action callback)
	{

		await Task.Delay(TimeSpan.FromSeconds(1));

		GetComponent<SpriteRenderer>().DOFade(0, 1);
		healthBar.GetComponent<CanvasGroup>().DOFade(0, 1);

		await Task.Delay(TimeSpan.FromSeconds(1));

		callback();
	}

	public void Destroy()
	{
		Destroy(healthBar.gameObject);
		Destroy(gameObject);
	}

	private void Start()
	{
		healthBar = Instantiate(_enemyHealthBarPref, GameController.Instance.canvas.transform).GetComponent<EnemyHealthBar>();
		healthBar.Init(_healthbarPoint);

		UpdateHealthBar(_fishHealth.value);
	}

	public void UpdateHealthBar(int value)
	{
		healthBar.UpdateState(value);
	}

	void UpdateSprite()
	{
		if (_fishHealth.IsFedup)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[1];



		}
		else if (_fishHealth.IsDead)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[2];
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Bubble>())
		{
			if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().type == type)
			{
				OnCoinMatch?.Invoke(other.GetComponent<Bubble>().ScoreCount);

				_fishHealth.ChangeHealth(30);
				UpdateHealthBar(_fishHealth.value);

				SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount);

				GameController.Instance.sound.PlaySound(feedFishGood);
			}
			else if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().type != type)
			{
				OnCoinMatch?.Invoke(-other.GetComponent<Bubble>().ScoreCount);

				SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount, true);

				_fishHealth.ChangeHealth(-30);
				UpdateHealthBar(_fishHealth.value);

				GameController.Instance.sound.PlaySound(feedFishBad);
			}

			other.GetComponent<Bubble>().SelfDestroy();
		}
		else if (other.GetComponent<Fish>())
		{
			if (!isDraggable)
			{
				return;
			}

			isCollided = true;

			//isDraggable = false;
			transform.position = other.GetComponent<Fish>().transform.position;
			other.GetComponent<Fish>().transform.position = originPos;
			originPos = transform.position;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		//if
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

		if (isCollided)
		{
			isCollided = false;
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

	public void Setup(BubbleType type)
	{
		this.type = type;

		switch (this.type)
		{
			case BubbleType.A:
				_color = ColorA;
				break;
			case BubbleType.B:
				_color = ColorB;
				break;
			case BubbleType.C:
				_color = ColorC;
				break;
			default:
				break;
		}

		_renderer.material.color = _color;
	}

	void Generate()
	{
		type = (BubbleType)UnityEngine.Random.Range(0, 2);

		switch (type)
		{
			case BubbleType.A:
				_color = ColorA;
				break;
			case BubbleType.B:
				_color = ColorB;
				break;
			case BubbleType.C:
				_color = ColorC;
				break;
			default:
				break;
		}

		_renderer.material.color = _color;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isCollided = false;
		isDraggable = true;
		originPos = transform.position;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!isCollided)
		{
			transform.position = originPos;
		}

		isDraggable = false;
	}
}
