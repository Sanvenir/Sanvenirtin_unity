using System;
using System.Collections.Generic;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
	public class DynamicSubstanceSpriteController : MonoBehaviour, ISubstanceSpriteController
	{

		public Sprite[] Sprites;
		public Sprite DisabledSprite;

		public int Speed = 10;

		public Direction TargetDirection = Direction.Down;

		private Direction _movingDirection;

		public bool NextMoving = false;
		public bool CurrentMoving = false;
		public bool isDisabled = false;

		private SpriteRenderer _spriteRenderer;
		
		private static readonly Dictionary<Direction, int[]> AnimationsSequences = 
			new Dictionary<Direction, int[]>
			{
				{Direction.Down, new []{1, 0, 1, 2}},
				{Direction.Left, new []{4, 3, 4, 5}},
				{Direction.Right, new []{7, 6, 7, 8}}, 
				{Direction.Up, new []{10, 9, 10, 11}},
				{Direction.None, new []{1, 4, 7, 10}}
			};
	
		// Use this for initialization
		private void Start()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_spriteRenderer.sprite = Sprites[0];
			_movingDirection = TargetDirection;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isDisabled)
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
			var timeIndex = (int) (Time.time * Speed);
			var index = timeIndex % AnimationsSequences[_movingDirection].Length;
			if (Sprites[AnimationsSequences[_movingDirection][index]] ==
			    Sprites[AnimationsSequences[_movingDirection][0]])
			{
				CurrentMoving = NextMoving;
			}
			
			_spriteRenderer.sprite = CurrentMoving ? Sprites[AnimationsSequences[_movingDirection][index]] : Sprites[AnimationsSequences[_movingDirection][0]];
		}

		public void StartMoving()
		{
			NextMoving = true;
			CurrentMoving = true;
		}

		public void StopMoving()
		{
			NextMoving = false;
		}

		public void SetDisable(bool disabled)
		{
			isDisabled = disabled;
		}


		public void SetDirection(Direction direction)
		{
			TargetDirection = direction;
		}

		public bool IsMoving()
		{
			return NextMoving;
		}
	}
}
