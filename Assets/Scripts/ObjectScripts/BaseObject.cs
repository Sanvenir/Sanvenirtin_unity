using System.Collections;
using System.Text;
using ExceptionScripts;
using ObjectScripts.SpriteController;
using UnityEngine;
using UtilScripts;

namespace ObjectScripts
{
    /// <inheritdoc />
    /// <summary>
    ///     Base type of Game Object, for every game entity which has a name, a image(SpriteRenderer) and checkable collider.
    /// </summary>
    public abstract class BaseObject : MonoBehaviour
    {
        public string TextName;
        public string Info;

        [HideInInspector] public bool Visible;

        public Vector2Int WorldCoord { get; private set; }

        public Vector2 WorldPos
        {
            get { return transform.position; }
            set
            {
                transform.position = value;
                WorldCoord = SceneManager.Instance.WorldPosToCoord(WorldPos);
            }
        }

        public virtual StringBuilder GetConditionDescribe()
        {
            var describe = new StringBuilder();
            describe.AppendLine(string.Format("Size: {0}", GetSize()));
            describe.AppendLine(string.Format("Weight: {0}", GetWeight()));
            return describe;
        }

        [HideInInspector] public SpriteRenderer SpriteRenderer;
        [HideInInspector] public SpriteController.SpriteController SpriteController;

        /// <summary>
        ///     The checkable collider of object, if it is a substance, the layer should be BlockLayer
        /// </summary>
        [HideInInspector] public Collider2D Collider2D;

        [HideInInspector] public Rigidbody2D Rigidbody2D;

        /// <summary>
        ///     Use the size of this object
        /// </summary>
        /// <returns></returns>
        public abstract float GetSize();

        /// <summary>
        ///     Use the weight of this object
        /// </summary>
        /// <returns></returns>
        public abstract float GetWeight();

        /// <summary>
        ///     After instantiate or enable an object, initialize function needs to be called
        /// </summary>
        public void Initialize(Vector2 worldPos)
        {
            Collider2D = GetComponent<Collider2D>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            SpriteController = GetComponentInChildren<SpriteController.SpriteController>();
            if (Collider2D == null || Rigidbody2D == null || SpriteRenderer == null || SpriteController == null)
            {
                Destroy(gameObject);
                throw new GameException("Lack of essential component!");
            }

            Visible = false;

            SetPosition(worldPos);
        }

        public void SetPosition(Vector2 worldPos)
        {
            WorldPos = worldPos;
            SpriteRenderer.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        ///     Use the position of SpriteRenderer
        /// </summary>
        /// <returns></returns>
        public Vector2 GetVisualPos()
        {
            return SpriteRenderer.transform.position;
        }

        /// <summary>
        ///     Set coord and transform position, then play a smooth move animation
        /// </summary>
        /// <param name="end">End position</param>
        /// <param name="moveTime">Total action time</param>
        protected void SmoothMoveTo(Vector2 end, int moveTime)
        {
            Vector2 start = SpriteController.transform.position;
            WorldPos = end;
            SpriteRenderer.transform.position = start;
            StartCoroutine(SmoothMovement(moveTime));
        }

        private IEnumerator SmoothMovement(int moveTime)
        {
            var moveSteps = moveTime / SceneManager.Instance.GetUpdateTime();
            var moveVector = -SpriteRenderer.transform.localPosition / moveSteps;
            for (; moveSteps != 0; moveSteps--)
            {
                SpriteController.StartMoving();
                SpriteRenderer.transform.localPosition += moveVector;
                SpriteRenderer.sortingOrder = -Utils.FloatToInt(
                    SpriteRenderer.transform.position.y);
                yield return null;
            }

            SpriteController.StopMoving();
        }

        /// <summary>
        ///     Simulate a hit animation
        /// </summary>
        /// <param name="direction">Direction of the hit action</param>
        /// <param name="moveTime">Total action time</param>
        /// <param name="hitTime">The time for object to return</param>
        /// <param name="intense">Range of hit</param>
        protected void HitTo(Direction direction, int moveTime, int hitTime = 1, float intense = 0.5f)
        {
            StartCoroutine(HitMovement(
                moveTime, (Vector2) Utils.DirectionToVector(direction) * intense, hitTime));
        }

        private IEnumerator HitMovement(int moveTime, Vector2 delta, int hitTime = 1)
        {
            var time = hitTime / SceneManager.Instance.GetUpdateTime();
            Vector3 moveVector;

            if (time == 0)
            {
                SpriteRenderer.transform.localPosition += Utils.Vector2To3(delta);
            }
            else
            {
                moveVector = delta / time;
                for (; time != 0; time--)
                {
                    SpriteRenderer.transform.localPosition += moveVector;
                    yield return null;
                }
            }

            time = (moveTime - hitTime) / SceneManager.Instance.GetUpdateTime();
            moveVector = -SpriteRenderer.transform.localPosition / time;
            for (; time != 0; time--)
            {
                SpriteRenderer.transform.localPosition += moveVector;
                yield return null;
            }
        }

        protected virtual void LateUpdate()
        {
            if ((SceneManager.Instance.PlayerObject.WorldPos -
                 WorldPos).sqrMagnitude > 5000)
                Destroy(gameObject);
            Visible = SceneManager.Instance.PlayerObject.IsVisible(this);
        }

        protected virtual void Update()
        {
            SpriteRenderer.enabled = Visible;
            SpriteRenderer.sortingOrder = -Utils.FloatToInt(
                SpriteRenderer.transform.position.y);
        }

        /// <summary>
        ///     Play the action effect animation
        /// </summary>
        /// <param name="effect"></param>
        public void PlayEffect(EffectController effect)
        {
            if (effect == null || !Visible) return;
            var instance = Instantiate(effect, transform);
            instance.Parent = this;
            instance.Initialize();
        }
    }
}