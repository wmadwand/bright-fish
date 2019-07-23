using DG.Tweening;
using System;
using System.Collections;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

public class Fish : MonoBehaviour/*, IDragHandler, IBeginDragHandler, IEndDragHandler*/
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
	//private bool _isCollided;
	private Color _color;
	private SpriteRenderer[] _spriteRenderers;
	//private Vector2 _originPosition;
	//private bool _isDraggable;
	private FishHealth _fishHealth;
	private GameSettings _gameSettings;

	private int _bubbleLayer;

	//----------------------------------------------------------------

	public void Setup(BubbleType bubbleType)
	{
		_type = bubbleType;

		switch (bubbleType)
		{
			case BubbleType.A: _color = _gameSettings.ColorA; break;
			case BubbleType.B: _color = _gameSettings.ColorB; break;
			case BubbleType.C: _color = _gameSettings.ColorC; break;
			case BubbleType.D: _color = _gameSettings.ColorD; break;
			case BubbleType.E: _color = _gameSettings.ColorE; break;
			default: _color = _gameSettings.ColorA; break;
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

		_bubbleLayer = 1 << LayerMask.NameToLayer("PhysicsObject")/* | 1 << LayerMask.NameToLayer("Player")*/;



		Physics2D.alwaysShowColliders = true;
		//Physics2D.showColliderAABB = true;

		_contactLayer = 1 << LayerMask.NameToLayer("Contact");
		_ignoreLayer = 1 << LayerMask.NameToLayer("Draggable");

		contactFilter = new ContactFilter2D() { layerMask = _contactLayer, useLayerMask = true };

		//Physics2D.IgnoreLayerCollision(_contactLayer, _ignoreLayer, true);
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

		CheckForCollide();

	}

	private void OnDrawGizmos()
	{
		//Gizmos.color = Color.red;

		//Gizmos.DrawWireSphere(transform.position, 1);
	}

	public Collider2D myCollider;

	ContactFilter2D contactFilter;

	int _contactLayer;
	int _ignoreLayer;

	Collider2D[] _results = new Collider2D[2];

	private void CheckForCollide()
	{
		//var vec = new Vector2(3, 2);
		////var result = Physics2D.OverlapCapsule(transform.position, vec, CapsuleDirection2D.Horizontal, 0, _bubbleLayer);
		//var result = Physics2D.OverlapCircle(transform.position, 1, _bubbleLayer);

		//if (result && ((result is CircleCollider2D && result.GetComponent<Bubble>()) || (result is BoxCollider2D && result.GetComponent<Fish>())))
		//{
		//	Debug.Log("Bubble has just collided with fish");

		//	OnCollideCircleEnter(result);
		//}

		Physics2D.OverlapCollider(myCollider, contactFilter, _results);

		//if (_results.Length > 0)
		//{
		foreach (var item in _results)
		{
			if (item != null)
			{
				/*Array.ForEach(_results, x => x = null);*/

				OnCollideCircleEnter(_results[0]);

				Array.Clear(_results, 0, _results.Length);
			}
		}
	}

	private void OnCollideCircleEnter(Collider2D other)
	{
		if (other.GetComponentInParent<Bubble>() && other is BoxCollider2D)
		{
			var bubble = other.GetComponentInParent<Bubble>();


			if (bubble.Type == _type)
			{
				MessageBus.OnBubbleColorMatch.Send(bubble.ScoreCount);

				_fishHealth.ChangeHealth(30);
				UpdateHealthBar(_fishHealth.value);

				SpawnCoinScroreText(bubble.ScoreCount);

				GameController.Instance.sound.PlaySound(feedFishGood);

				bubble.SelfDestroy(isRequiredBadSound: false);
			}
			else if (bubble.Type != _type)
			{
				MessageBus.OnBubbleColorMatch.Send(-bubble.ScoreCount);

				SpawnCoinScroreText(bubble.ScoreCount, true);

				_fishHealth.ChangeHealth(-30);
				UpdateHealthBar(_fishHealth.value);

				GameController.Instance.sound.PlaySound(feedFishBad);

				bubble.SelfDestroy(isRequiredBadSound: false);
			}


		}
		else if (other.GetComponentInParent<Food>() && other is BoxCollider2D)
		{
			var food = other.GetComponentInParent<Food>();

			if (food.Type == _type)
			{
				MessageBus.OnBubbleColorMatch.Send(food.ScoreCount);

				_fishHealth.ChangeHealth(30);
				UpdateHealthBar(_fishHealth.value);

				SpawnCoinScroreText(food.ScoreCount);

				GameController.Instance.sound.PlaySound(feedFishGood);

				food.SelfDestroy(isRequiredBadSound: false);
			}
			else if (food.Type != _type)
			{
				MessageBus.OnBubbleColorMatch.Send(-food.ScoreCount);

				SpawnCoinScroreText(food.ScoreCount, true);

				_fishHealth.ChangeHealth(-30);
				UpdateHealthBar(_fishHealth.value);

				GameController.Instance.sound.PlaySound(feedFishBad);

				food.SelfDestroy(isRequiredBadSound: false);
			}
		}
		else if (other.GetComponentInParent<Fish>() && other is BoxCollider2D)
		{
			var movement = GetComponentInChildren<FishMovement>();

			if (!movement._isDraggable)
			{
				return;
			}

			movement._isCollided = true;

			transform.position = other.GetComponentInParent<Fish>().transform.position;
			other.GetComponentInParent<Fish>().transform.position = movement._originPosition;
			movement._originPosition = transform.position;


			if (!movement._isCollided)
			{
				transform.position = movement._originPosition;
			}

			movement._isDraggable = false;
		}

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
		//if (other.GetComponent<Bubble>() && other is CircleCollider2D)
		//{
		//	if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().Type == _type)
		//	{
		//		MessageBus.OnBubbleColorMatch.Send(other.GetComponent<Bubble>().ScoreCount);

		//		_fishHealth.ChangeHealth(30);
		//		UpdateHealthBar(_fishHealth.value);

		//		SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount);

		//		GameController.Instance.sound.PlaySound(feedFishGood);

		//		other.GetComponent<Bubble>().SelfDestroy(isRequiredBadSound: false);
		//	}
		//	else if (other.GetComponent<Bubble>() && other.GetComponent<Bubble>().Type != _type)
		//	{
		//		MessageBus.OnBubbleColorMatch.Send(-other.GetComponent<Bubble>().ScoreCount);

		//		SpawnCoinScroreText(other.GetComponent<Bubble>().ScoreCount, true);

		//		_fishHealth.ChangeHealth(-30);
		//		UpdateHealthBar(_fishHealth.value);

		//		GameController.Instance.sound.PlaySound(feedFishBad);

		//		other.GetComponent<Bubble>().SelfDestroy(isRequiredBadSound: false);
		//	}


		//}
		//else if (other.GetComponent<Food>() && other is CircleCollider2D)
		//{
		//	if (other.GetComponent<Food>() && other.GetComponent<Food>().Type == _type)
		//	{
		//		MessageBus.OnBubbleColorMatch.Send(other.GetComponent<Bubble>().ScoreCount);

		//		_fishHealth.ChangeHealth(30);
		//		UpdateHealthBar(_fishHealth.value);

		//		SpawnCoinScroreText(other.GetComponent<Food>().ScoreCount);

		//		GameController.Instance.sound.PlaySound(feedFishGood);

		//		other.GetComponent<Food>().SelfDestroy(isRequiredBadSound: false);
		//	}
		//	else if (other.GetComponent<Food>() && other.GetComponent<Food>().Type != _type)
		//	{
		//		MessageBus.OnBubbleColorMatch.Send(-other.GetComponent<Bubble>().ScoreCount);

		//		SpawnCoinScroreText(other.GetComponent<Food>().ScoreCount, true);

		//		_fishHealth.ChangeHealth(-30);
		//		UpdateHealthBar(_fishHealth.value);

		//		GameController.Instance.sound.PlaySound(feedFishBad);

		//		other.GetComponent<Food>().SelfDestroy(isRequiredBadSound: false);
		//	}
		//}
		///*else*/ if (other.GetComponent<Fish>() && other is BoxCollider2D)
		//{
		//	var movement = GetComponentInChildren<FishMovement>();

		//	if (!movement._isDraggable)
		//	{
		//		return;
		//	}

		//	movement._isCollided = true;

		//	transform.position = other.GetComponent<Fish>().transform.position;
		//	other.GetComponent<Fish>().transform.position = movement._originPosition;
		//	movement._originPosition = transform.position;


		//	if (!movement._isCollided)
		//	{
		//		transform.position = movement._originPosition;
		//	}

		//	movement._isDraggable = false;
		//}
	}

	private void SpawnCoinScroreText(int scoreCount, bool wrongCoin = false)
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(_scoreTextSpawnPoint.position);

		var scoreGO = Instantiate(GameController.Instance.coinScoreTextPref, GameController.Instance.canvas.transform);
		scoreGO.transform.position = pos;

		scoreGO.GetComponent<CoinScoreText>().SetScore(scoreCount, wrongCoin);
	}

	//----------------------------------------------------------------

	public class FishDIFactory : PlaceholderFactory<Fish> { }
}
