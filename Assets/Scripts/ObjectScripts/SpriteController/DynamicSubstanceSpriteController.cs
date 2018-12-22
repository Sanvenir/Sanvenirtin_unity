using System;
using System.Collections.Generic;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
	public class DynamicSubstanceSpriteController : MonoBehaviour, ISubstanceSpriteController
	{
		public Sprite DisabledSprite;

		public int Speed = 10;

		[HideInInspector]
		public Direction TargetDirection = Direction.Down;
		private Direction _movingDirection;
		
		[HideInInspector]
		public bool Moving = false;
		
		[HideInInspector]
		public bool IsDisabled = false;

		private SpriteRenderer _spriteRenderer;

		public Dictionary<Direction, List<Sprite>> MoveSprites;
		public Dictionary<Direction, List<Sprite>> StopSprites;

		public List<Sprite> MoveUpSprites;
		public List<Sprite> MoveDownSprites;
		public List<Sprite> MoveLeftSprites;
		public List<Sprite> MoveRightSprites;
		public List<Sprite> MoveNoneSprites;
		public List<Sprite> StopUpSprites;
		public List<Sprite> StopDownSprites;
		public List<Sprite> StopLeftSprites;
		public List<Sprite> StopRightSprites;
		public List<Sprite> StopNoneSprites;

		private List<Sprite> _currentSprites;
	
		// Use this for initialization
		private void Start()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_movingDirection = TargetDirection;
			MoveSprites = new Dictionary<Direction, List<Sprite>>
			{
				{Direction.Up, MoveUpSprites}, 
				{Direction.Down, MoveDownSprites},
				{Direction.Left, MoveLeftSprites},
				{Direction.Right, MoveRightSprites},
				{Direction.None, MoveNoneSprites}
			};
			StopSprites = new Dictionary<Direction, List<Sprite>>
			{
				{Direction.Up, StopUpSprites}, 
				{Direction.Down, StopDownSprites},
				{Direction.Left, StopLeftSprites},
				{Direction.Right, StopRightSprites},
				{Direction.None, StopNoneSprites}
			};
			_spriteRenderer.sprite = StopSprites[_movingDirection][0];
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (IsDisabled)
			{
				_spriteRenderer.sprite = DisabledSprite;
				return;
			}
			if (TargetDirection != _movingDirection)
			{
				if (TargetDirection == Direction.None)
				{
					_movingDirection = Direction.None;
				}
				else
				{
					switch (_movingDirection)
					{
						case Direction.Down:
							_movingDirection = TargetDirection == Direction.Right ? 
								Direction.Right : Direction.Left;
							break;
						case Direction.Left:
							_movingDirection = TargetDirection == Direction.Down ? 
								Direction.Down : Direction.Up;
							break;
						case Direction.Up:
							_movingDirection = TargetDirection == Direction.Left ? 
								Direction.Left : Direction.Right;
							break;
						case Direction.Right:
							_movingDirection = TargetDirection == Direction.Up ? 
								Direction.Up : Direction.Down;
							break;
						case Direction.None:
							_movingDirection = TargetDirection;
							break;
						default:
							_movingDirection = Direction.None;
							break;
					}
					
				}
			}

			_currentSprites = Moving ? MoveSprites[_movingDirection] : StopSprites[_movingDirection];
			
			var timeIndex = (int) (Time.time * Speed);
			var index = timeIndex % _currentSprites.Count;

			_spriteRenderer.sprite = _currentSprites[index];
		}

		public void StartMoving()
		{
			Moving = true;
		}

		public void StopMoving()
		{
			Moving = false;
		}

		public void SetDisable(bool disabled)
		{
			IsDisabled = disabled;
		}


		public void SetDirection(Direction direction)
		{
			TargetDirection = direction;
		}

		public bool IsMoving()
		{
			return Moving;
		}
	}
}
