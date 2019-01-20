using System;
using System.Collections.Generic;
using System.Linq;
using ExceptionScripts;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts.SpriteController
{
    public class DynamicSpriteController : SpriteController
    {
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

        public List<DirectedChildPos> ChildrenPos;

        [HideInInspector] public Direction TargetDirection = Direction.Down;

        [HideInInspector] public bool Moving;

        public bool _isDisabled;

        public Dictionary<Direction, List<Sprite>> MoveSprites;
        public Dictionary<Direction, List<Sprite>> StopSprites;

        private Dictionary<string, int> _childrenIndex;

        private readonly Dictionary<string, ChildSpriteItem> _childrenSprites =
            new Dictionary<string, ChildSpriteItem>();

        private List<Sprite> _currentSprites;
        private Direction _currentDirection;

        private struct ChildSpriteItem
        {
            public readonly SpriteRenderer SpriteRenderer;
            public readonly BaseObject BaseObject;

            public ChildSpriteItem(BaseObject baseObject, Transform transform)
            {
                BaseObject = baseObject;
                SpriteRenderer = Instantiate(baseObject.SpriteRenderer, transform);
            }
        }

        [Serializable]
        public struct DirectedChildPos
        {
            public string Name;
            public Vector2 UpPos;
            public int UpDepth;
            public Vector2 DownPos;
            public int DownDepth;
            public Vector2 LeftPos;
            public int LeftDepth;
            public Vector2 RightPos;
            public int RightDepth;
            public bool Reverse;

            public Vector2 GetPos(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Down:
                        return DownPos;
                    case Direction.Left:
                        return LeftPos;
                    case Direction.Up:
                        return UpPos;
                    case Direction.Right:
                        return RightPos;
                    case Direction.None:
                        return DownPos;
                    default:
                        throw new ArgumentOutOfRangeException("direction", direction, null);
                }
            }

            public int GetDepth(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Down:
                        return DownDepth;
                    case Direction.Left:
                        return LeftDepth;
                    case Direction.Up:
                        return UpDepth;
                    case Direction.Right:
                        return RightDepth;
                    case Direction.None:
                        return DownDepth;
                    default:
                        throw new ArgumentOutOfRangeException("direction", direction, null);
                }
            }

            public bool GetReverse(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Down:
                        return Reverse;
                    case Direction.Left:
                        return false;
                    case Direction.Up:
                        return !Reverse;
                    case Direction.Right:
                        return true;
                    case Direction.None:
                        return Reverse;
                    default:
                        throw new ArgumentOutOfRangeException("direction", direction, null);
                }
            }
        }

        private DirectedChildPos GetChildPos(string index)
        {
            if (!_childrenIndex.ContainsKey(index)) throw new ArgumentOutOfRangeException();
            return ChildrenPos[_childrenIndex[index]];
        }

        private void UpdateChild(string index)
        {
            if (!_childrenSprites.ContainsKey(index)) return;
            var sprite = _childrenSprites[index];
            var pos = GetChildPos(index);
            if (sprite.BaseObject == null) RemoveChildSprite(index);
            sprite.SpriteRenderer.transform.localPosition = pos.GetPos(_currentDirection);
            sprite.SpriteRenderer.sortingLayerName = SpriteRenderer.sortingLayerName;
            sprite.SpriteRenderer.sortingOrder = SpriteRenderer.sortingOrder + pos.GetDepth(_currentDirection);
            sprite.SpriteRenderer.flipX = pos.GetReverse(_currentDirection);
        }

        public override void AddNewChildSprite(string index, BaseObject baseObject)
        {
            if (_childrenSprites.ContainsKey(index) &&
                _childrenSprites[index].BaseObject != null) throw new ObjectNotNullException();
            if (!_childrenIndex.ContainsKey(index)) return;
            _childrenSprites[index] = new ChildSpriteItem(baseObject, transform);
            UpdateChild(index);
        }

        public override void RemoveChildSprite(string index)
        {
            if (!_childrenSprites.ContainsKey(index)) return;
            Destroy(_childrenSprites[index].SpriteRenderer.gameObject);
            _childrenSprites.Remove(index);
        }

        // Use this for initialization
        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            _currentDirection = TargetDirection;
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

            _childrenIndex = new Dictionary<string, int>();
            for (var index = 0; index < ChildrenPos.Count(); ++index) _childrenIndex[ChildrenPos[index].Name] = index;

            if (_isDisabled)
            {
                SpriteRenderer.sprite = DisabledSprite;
                return;
            };
            SpriteRenderer.sprite = StopSprites[_currentDirection][0];
        }

        // Update is called once per frame
        private void Update()
        {
            if (_isDisabled)
            {
                SpriteRenderer.sprite = DisabledSprite;
                return;
            };

            if (TargetDirection != _currentDirection)
            {
                if (TargetDirection == Direction.None)
                    _currentDirection = Direction.None;
                else
                    switch (_currentDirection)
                    {
                        case Direction.Down:
                            _currentDirection = TargetDirection == Direction.Right ? Direction.Right : Direction.Left;
                            break;
                        case Direction.Left:
                            _currentDirection = TargetDirection == Direction.Down ? Direction.Down : Direction.Up;
                            break;
                        case Direction.Up:
                            _currentDirection = TargetDirection == Direction.Left ? Direction.Left : Direction.Right;
                            break;
                        case Direction.Right:
                            _currentDirection = TargetDirection == Direction.Up ? Direction.Up : Direction.Down;
                            break;
                        case Direction.None:
                            _currentDirection = TargetDirection;
                            break;
                        default:
                            _currentDirection = Direction.None;
                            break;
                    }
            }

            _currentSprites = Moving ? MoveSprites[_currentDirection] : StopSprites[_currentDirection];

            var timeIndex = (int) (Time.time * Speed);
            var index = timeIndex % _currentSprites.Count;

            SpriteRenderer.sprite = _currentSprites[index];
            foreach (var spriteIndex in _childrenSprites.Keys) UpdateChild(spriteIndex);
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
            SpriteRenderer.sortingLayerName = "Abstract";
            foreach (var spriteIndex in _childrenSprites.Keys) RemoveChildSprite(spriteIndex);
            _isDisabled = disabled;
        }


        public override void SetDirection(Direction direction)
        {
            TargetDirection = direction;
        }
    }
}