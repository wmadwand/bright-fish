using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

using Terminus.Extensions;
using Terminus.Game.Messages;

public class Fish : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[SerializeField] private Sprite[] _sprites;
	[SerializeField] private SpriteRenderer _headSpriteRenderer;
	[SerializeField] private SpriteRenderer _bodySpriteRenderer;
	[SerializeField] private Sounds feedFishGood, feedFishBad, fishDead, fishHappy;
	[SerializeField] private GameObject _fishHealthBarTemplate;
	[SerializeField] private GameObject _particleTemplate;
	[SerializeField] private Transform _healthbarPoint;
	[SerializeField] private Transform _scoreTextSpawnPoint;
	[SerializeField] private Transform _particleSpawnPoint;

	private FishHealthBar _healthBar;
	private BubbleType _type;
	private bool _isDead;
	private bool _isCollided;
	private Color _color;
	private SpriteRenderer[] _spriteRenderers;
	private Vector2 _originPosition;
	private bool _isDraggable;
	private FishHealth _fishHealth;
	private GameSettings _gameSettings;

	//----------------------------------------------------------------

	public void Setup(BubbleType bubbleType)
	{
		_type = bubbleType;

		switch (bubbleType)
		{
			case BubbleType.A: _color = _gameSettings.ColorA; break;
			case BubbleType.B: _color = _gameSettings.ColorB; break;
			case BubbleType.C: _color = _gameSettings.ColorC; break;
		}

		_headSpriteRenderer.material.color = _color;
		_bodySpriteRenderer.material.color = _color;
	}

	public void ChangePosition(Vector3 value)
	{
		gameObject.transform.position = value;
	}

	public void UpdateHealthBar(float value)
	{
		_bodySpriteRenderer.material.SetFloat("_Progress", value * .01f);
	}

	public void Destroy()
	{
		try
		{
			//Destroy(_healthBar.gameObject);
			Destroy(gameObject);
		}
		catch (Exception)
		{

			//throw;
		}
	}

	//----------------------------------------------------------------

	[Inject]
	private void Construct(GameSettings gameSettings)
	{
		_gameSettings = gameSettings;
	}

	private void Awake()
	{
		_spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		_fishHealth = GetComponent<FishHealth>();
	}

	private void Start()
	{
		//_healthBar = Instantiate(_fishHealthBarTemplate, GameController.Instance.canvas.transform).GetComponent<FishHealthBar>();
		//_healthBar.Init(_healthbarPoint);

		UpdateHealthBar(_fishHealth.value);
	}

	private void Update()
	{
		if (_isDead)
		{
			return;
		}

		if (_fishHealth.IsDead)
		{
			GameController.Instance.sound.PlaySound(fishDead);
			_isDead = true;

			MessageBus.OnFishDying.Send();

			StartCoroutine(DelayBeforeHide(() =>
			{
				MessageBus.OnFishDead.Send(this, _type, transform.position);
				Destroy();
			}));
		}
		else if (_fishHealth.IsFedUp)
		{
			_isDead = true;

			MessageBus.OnFishRescued.Send(this, _type, transform.position);

			ShowPaintSplash(_color);

			StartCoroutine(DelayBeforeHide(() =>
			{
				MessageBus.OnFishFinishedSmiling.Send(this, _type, transform.position);
				Destroy();
			}));
		}

		UpdateSprite();

	}

	private void ShowPaintSplash(Color color)
	{
		var obj = Instantiate(_particleTemplate, _particleSpawnPoint.position, Quaternion.identity);
		var script = obj.GetComponent<FishPaint>();

		script.SetColor(color);
	}

	private IEnumerator DelayBeforeHide(Action callback)
	{
		yield return new WaitForSeconds(1);

		Array.ForEach(_spriteRenderers, x => x.DOFade(0, 1));
		_bodySpriteRenderer.material.DOFade(0, 1);

		yield return new WaitForSeconds(1);

		callback();
	}

	private void UpdateSprite()
	{
		if (_fishHealth.IsFedUp)
		{
			_spriteRenderers[0].sprite = _sprites[1];
		}
		else if (_fishHealth.IsDead)
		{
			_spriteRenderers[0].sprite = _sprites[2];
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Bubble>())
		{
			if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().Type == _type)
			{
				MessageBus.OnBubbleColorMatch.Send(other.GetComponent<Bubble>().ScoreCount);

				_fishHealth.ChangeHealth(30);
				UpdateHealthBar(_fishHealth.value);

				SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount);

				GameController.Instance.sound.PlaySound(feedFishGood);

				other.GetComponent<Bubble>().SelfDestroy(isRequiredBadSound: false);
			}
			else if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().Type != _type)
			{
				MessageBus.OnBubbleColorMatch.Send(-other.GetComponent<Bubble>().ScoreCount);

				SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount, true);

				_fishHealth.ChangeHealth(-30);
				UpdateHealthBar(_fishHealth.value);

				GameController.Instance.sound.PlaySound(feedFishBad);

				other.GetComponent<Bubble>().SelfDestroy(isRequiredBadSound: false);
			}


		}
		else if (other.GetComponent<Food>())
		{
			if (other.GetComponent<Food>() && other.GetComponent<Food>().Type == _type)
			{
				MessageBus.OnBubbleColorMatch.Send(other.GetComponent<Bubble>().ScoreCount);

				_fishHealth.ChangeHealth(30);
				UpdateHealthBar(_fishHealth.value);

				SpawnCoinScroreText(other.GetComponent<Food>().ScoreCount);

				GameController.Instance.sound.PlaySound(feedFishGood);

				other.GetComponent<Food>().SelfDestroy(isRequiredBadSound: false);
			}
			else if (other.GetComponent<Food>() && other.GetComponent<Food>().Type != _type)
			{
				MessageBus.OnBubbleColorMatch.Send(-other.GetComponent<Bubble>().ScoreCount);

				SpawnCoinScroreText(other.GetComponent<Food>().ScoreCount, true);

				_fishHealth.ChangeHealth(-30);
				UpdateHealthBar(_fishHealth.value);

				GameController.Instance.sound.PlaySound(feedFishBad);

				other.GetComponent<Food>().SelfDestroy(isRequiredBadSound: false);
			}
		}
		else if (other.GetComponent<Fish>())
		{
			if (!_isDraggable)
			{
				return;
			}

			_isCollided = true;

			//isDraggable = false;
			transform.position = other.GetComponent<Fish>().transform.position;
			other.GetComponent<Fish>().transform.position = _originPosition;
			_originPosition = transform.position;




			if (!_isCollided)
			{
				transform.position = _originPosition;
			}

			_isDraggable = false;


		}
	}

	private void SpawnCoinScroreText(int scoreCount, bool wrongCoin = false)
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(_scoreTextSpawnPoint.position);

		var scoreGO = Instantiate(GameController.Instance.coinScoreTextPref, GameController.Instance.canvas.transform);
		scoreGO.transform.position = pos;

		scoreGO.GetComponent<CoinScoreText>().SetScore(scoreCount, wrongCoin);
	}

	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
	{
		_isCollided = false;
		_isDraggable = true;
		_originPosition = transform.position;
	}

	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		if (!_isDraggable)
		{
			return;
		}

		if (_isCollided)
		{
			_isCollided = false;
		}

		////Very nice approach for 2D objects dragging
		//transform.position = eventData.position;


		// Solution #01
		Plane plane = new Plane(Vector3.forward, transform.position);
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);

		if (plane.Raycast(ray, out float distance))
		{
			var vec = ray.origin + ray.direction * distance;

			var xMin = GameController.Instance.fishSpawner.SpawnPoint[0].transform.position.x;
			var xMax = GameController.Instance.fishSpawner.SpawnPoint[2].transform.position.x;
			transform.position = new Vector2(Mathf.Clamp(vec.x, xMin, xMax), transform.position.y);
		}

		// Solution #02
		//Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); // Get the ray from mouse position
		//Vector3 PO = transform.position; // Take current position of this draggable object as Plane's Origin
		//Vector3 PN = -Camera.main.transform.forward; // Take current negative camera's forward as Plane's Normal
		//float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN); // plane vs. line intersection in algebric form. It find t as distance from the camera of the new point in the ray's direction.
		//Vector3 P = R.origin + R.direction * t; // Find the new point.

		//transform.position = P;
	}

	void IEndDragHandler.OnEndDrag(PointerEventData eventData)
	{
		if (!_isCollided)
		{
			transform.position = _originPosition;
		}

		_isDraggable = false;
	}

	//----------------------------------------------------------------

	public class FishDIFactory : PlaceholderFactory<Fish> { }
}
