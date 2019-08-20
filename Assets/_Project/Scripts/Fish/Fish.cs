using DG.Tweening;
using System;
using System.Collections;
using Terminus.Game.Messages;
using UnityEngine;
using Zenject;

namespace BrightFish
{
	public sealed class Fish : MonoBehaviour
	{
		public int RescuedFishValue => _rescuedFishValue;

		[SerializeField] private Sprite[] _sprites;
		[SerializeField] private SpriteRenderer _headSpriteRenderer;
		[SerializeField] private SpriteRenderer _bodySpriteRenderer;
		[SerializeField] private Sounds feedFishGood, feedFishBad, fishDead, fishHappy;
		[SerializeField] private GameObject _fishHealthBarTemplate;
		//[SerializeField] private GameObject _particleTemplate;
		[SerializeField] private Transform _healthbarPoint;
		[SerializeField] private Transform _scoreTextSpawnPoint;
		//[SerializeField] private Transform _particleSpawnPoint;
		[SerializeField] private Collider2D _myCollider;
		[SerializeField] private int _rescuedFishValue = 1;

		private FishHealthBar _healthBar;
		private ColorType _type;
		private bool _isDead;
		private Color _color;
		private SpriteRenderer[] _spriteRenderers;

		private FishHealth _fishHealth;
		private GameSettings _gameSettings;
		private FishView _fishView;

		private int _bubbleLayer;

		private ContactFilter2D _contactFilter;

		private int _contactLayer;
		//private int _ignoreLayer;

		private Collider2D[] _results = new Collider2D[2];

		//----------------------------------------------------------------

		public void Setup(ColorType bubbleType)
		{
			_type = bubbleType;

			switch (bubbleType)
			{
				case ColorType.A: _color = _gameSettings.ColorA; break;
				case ColorType.B: _color = _gameSettings.ColorB; break;
				case ColorType.C: _color = _gameSettings.ColorC; break;
				case ColorType.D: _color = _gameSettings.ColorD; break;
				case ColorType.E: _color = _gameSettings.ColorE; break;
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
			//_bodySpriteRenderer.material.SetFloat("_Progress", value * .01f);
			_bodySpriteRenderer.material.SetFloat("_Fill", value * .01f);

			//_bodySpriteRenderer.material.color = new Color(_bodySpriteRenderer.material.color.r, _bodySpriteRenderer.material.color.g, _bodySpriteRenderer.material.color.b, value * .01f);
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
			_fishView = GetComponent<FishView>();

			//_bubbleLayer = 1 << LayerMask.NameToLayer("PhysicsObject")/* | 1 << LayerMask.NameToLayer("Player")*/;



			Physics2D.alwaysShowColliders = true;
			//Physics2D.showColliderAABB = true;

			_contactLayer = 1 << LayerMask.NameToLayer("Contact");
			//_ignoreLayer = 1 << LayerMask.NameToLayer("Draggable");

			_contactFilter = new ContactFilter2D(); /*{ layerMask = _contactLayer, useLayerMask = true }*/;

			//Physics2D.IgnoreLayerCollision(_contactLayer, _ignoreLayer, true);
		}

		private void Start()
		{
			//_healthBar = Instantiate(_fishHealthBarTemplate, GameController.Instance.canvas.transform).GetComponent<FishHealthBar>();
			//_healthBar.Init(_healthbarPoint);

			UpdateHealthBar(_fishHealth.Value);
		}

		private void Update()
		{
			CheckForContact();

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

				_fishView.ShowPaintSplash(_color);

				StartCoroutine(DelayBeforeHide(() =>
				{
					MessageBus.OnFishFinishedSmiling.Send(this, _type, transform.position);
					Destroy();
				}));
			}

			UpdateSprite();
		}

		private void CheckForContact()
		{
			Physics2D.OverlapCollider(_myCollider, _contactFilter, _results);

			foreach (var item in _results)
			{
				if (item != null)
				{
					OnContact(_results[0]);

					Array.Clear(_results, 0, _results.Length);
				}
			}
		}

		private void OnContact(Collider2D other)
		{
			if (other.GetComponentInParent<Bubble>() && other is BoxCollider2D)
			{
				var bubble = other.GetComponentInParent<Bubble>();

				if (_isDead)
				{
					bubble.SelfDestroy(isRequiredBadSound: false);
					return;
				}

				if (bubble.GetComponent<BubbleView>().IsProperColorType(_type))
				{
					MessageBus.OnBubbleColorMatch.Send(bubble.ScoreCount);

					_fishHealth.ChangeHealth(30);
					UpdateHealthBar(_fishHealth.Value);

					SpawnCoinScroreText(bubble.ScoreCount);

					GameController.Instance.sound.PlaySound(feedFishGood);

					bubble.SelfDestroy(isRequiredBadSound: false);
				}
				else if (!bubble.GetComponent<BubbleView>().IsProperColorType(_type))
				{
					MessageBus.OnBubbleColorMatch.Send(-bubble.ScoreCount);

					SpawnCoinScroreText(bubble.ScoreCount, true);

					_fishHealth.ChangeHealth(-30);
					UpdateHealthBar(_fishHealth.Value);

					GameController.Instance.sound.PlaySound(feedFishBad);

					bubble.SelfDestroy(isRequiredBadSound: false);
				}


			}
			else if (other.GetComponentInParent<Food>() && other is BoxCollider2D)
			{
				var food = other.GetComponentInParent<Food>();

				if (_isDead)
				{
					food.SelfDestroy(isRequiredBadSound: false);
					return;
				}

				if (food.Type == _type)
				{
					MessageBus.OnBubbleColorMatch.Send(food.ScoreCount);

					_fishHealth.ChangeHealth(30);
					UpdateHealthBar(_fishHealth.Value);

					SpawnCoinScroreText(food.ScoreCount);

					GameController.Instance.sound.PlaySound(feedFishGood);

					food.SelfDestroy(isRequiredBadSound: false);
				}
				else if (food.Type != _type)
				{
					MessageBus.OnBubbleColorMatch.Send(-food.ScoreCount);

					SpawnCoinScroreText(food.ScoreCount, true);

					_fishHealth.ChangeHealth(-30);
					UpdateHealthBar(_fishHealth.Value);

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

		private void SpawnCoinScroreText(int scoreCount, bool wrongCoin = false)
		{
			Vector3 pos = Camera.main.WorldToScreenPoint(_scoreTextSpawnPoint.position);

			var scoreGO = Instantiate(GameController.Instance.coinScoreTextPref, GameController.Instance.canvas.transform);
			scoreGO.transform.position = pos;

			scoreGO.GetComponent<CoinScoreText>().SetScore(scoreCount, wrongCoin);
		}

		//----------------------------------------------------------------

		public class FishDIFactory : PlaceholderFactory<Fish> { }

		public class FishPredatorDIFactory : PlaceholderFactory<Fish> { }
	}
}