using System.Text;
using ObjectScripts.CharSubstance;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class CharacterTextDialog : MonoBehaviour
    {
        private Image _backGround;
        private Color _backOriginColor;
        private Canvas _canvas;
        private bool _clearFlag;

        private Color _decreaseColor;
        private int _originSortingLayer;

        private StringBuilder _stringBuilder = new StringBuilder();

        private Text _text;
        private Color _textOriginColor;
        private int _timeCount;
        [HideInInspector] public Character Character;
        public int DialogLastTime = 100;
        [Range(0f, 1f)] public float DisappearSpeed = 0.1f;

        private void Start()
        {
            _text = GetComponentInChildren<Text>();
            _backGround = GetComponent<Image>();
            _canvas = GetComponent<Canvas>();
            _originSortingLayer = _canvas.sortingLayerID;
            _decreaseColor = new Color(0, 0, 0, DisappearSpeed);
            _textOriginColor = _text.color;
            _backOriginColor = _backGround.color;
            _text.enabled = false;
            _backGround.enabled = false;
        }

        private void ResumeTransparent()
        {
            _timeCount = 0;
            _text.color = _textOriginColor;
            _backGround.color = _backOriginColor;
            if (_text.text == string.Empty) return;
            if (!Character.Visible) return;
            _text.enabled = true;
            _backGround.enabled = true;
        }

        private void DecreaseTransparent()
        {
            if (_text.color.a > 0)
            {
                _text.color -= _decreaseColor;
            }
            else
            {
                _text.enabled = false;
                _clearFlag = true;
            }

            if (_backGround.color.a > 0)
                _backGround.color -= _decreaseColor;
            else
                _backGround.enabled = false;
        }

        private void Update()
        {
            if (Character.Selected)
            {
                ResumeTransparent();
                _clearFlag = false;
                _canvas.sortingLayerName = "DialogOnTop";
            }
            else
            {
                _canvas.sortingLayerID = _originSortingLayer;
            }

            if (_timeCount == DialogLastTime)
            {
                DecreaseTransparent();
                return;
            }

            ++_timeCount;
        }

        public void PrintDialog(string message)
        {
            if (_clearFlag) Clear();
            _clearFlag = false;
            _stringBuilder.AppendLine(message);
            _text.text = _stringBuilder.ToString();
            ResumeTransparent();
        }

        public void Clear()
        {
            _stringBuilder = new StringBuilder();
            _text.text = _stringBuilder.ToString();
        }
    }
}