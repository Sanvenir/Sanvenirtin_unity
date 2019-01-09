using System.Collections.Generic;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public class DynamicSpriteController : SpriteController
    {

        [HideInInspector] public Direction TargetDirection = Direction.Down;
        private Direction _movingDirection;

        [HideInInspector] public bool Moving;

        [HideInInspector] public bool IsDisabled;

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
            SpriteRenderer = GetComponent<SpriteRenderer>();
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
            SpriteRenderer.sprite = StopSprites[_movingDirection][0];
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsDisabled)
            {
                SpriteRenderer.sprite = DisabledSprite;
                SpriteRenderer.sortingLayerName = "Abstract";
                return;
            }

            if (TargetDirection != _movingDirection)
            {
                if (TargetDirection == Direction.None)
                    _movingDirection = Direction.None;
                else
                    switch (_movingDirection)
                    {
                        case Direction.Down:
                            _movingDirection = TargetDirection == Direction.Right ? Direction.Right : Direction.Left;
                            break;
                        case Direction.Left:
                            _movingDirection = TargetDirection == Direction.Down ? Direction.Down : Direction.Up;
                            break;
                        case Direction.Up:
                            _movingDirection = TargetDirection == Direction.Left ? Direction.Left : Direction.Right;
                            break;
                        case Direction.Right:
                            _movingDirection = TargetDirection == Direction.Up ? Direction.Up : Direction.Down;
                            break;
                        case Direction.None:
                            _movingDirection = TargetDirection;
                            break;
                        default:
                            _movingDirection = Direction.None;
                            break;
                    }
            }

            _currentSprites = Moving ? MoveSprites[_movingDirection] : StopSprites[_movingDirection];

            var timeIndex = (int) (Time.time * Speed);
            var index = timeIndex % _currentSprites.Count;

            SpriteRenderer.sprite = _currentSprites[index];
        }

        public override void StartMoving()
        {
            Moving = true;
        }

        public override void StopMoving()
        {
            Moving = false;
        }

        public override void SetDisable(bool disabled)
        {
            IsDisabled = disabled;
        }


        public override void SetDirection(Direction direction)
        {
            TargetDirection = direction;
        }

        public override bool IsMoving()
        {
            return Moving;
        }
    }
}