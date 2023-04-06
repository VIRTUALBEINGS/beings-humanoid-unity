// ======================================================================
// This file contains proprietary technology owned by Virtual Beings SAS.
// Copyright 2011-2023 Virtual Beings SAS.
// ======================================================================

using System;
using UnityEngine;

namespace VirtualBeings.Beings.Humanoid.Sample.Look
{
    public class CameraFade : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Choose the color, which will fill the screen.")]
        private Color fadeColor = new(255.0f, 255.0f, 255.0f, 1.0f);

        private float _alpha;
        private Texture2D _texture;

        private float _fadeHalfTime;
        private float _timeStartFade = 0;
        private bool _isFading, _isFadingIn;

        public event Action OnFadeInFinished;

        public void DoFade(float fadeTime)
        {
            _fadeHalfTime = fadeTime * .5f;
            _timeStartFade = Time.time;
            _alpha = 0f;
            _isFading = _isFadingIn = true;
        }

        private void Start()
        {
            _texture = new Texture2D(1, 1);
            _texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, _alpha));
            _texture.Apply();
        }

        public void OnGUI()
        {
            if (_isFading)
            {
                ShowBlackScreen();
            }
        }

        private void ShowBlackScreen()
        {
            CalculateAlpha();

            if (_alpha < 0f)
            {
                // termination condition fulfilled -> terminate
                _isFading = false;
                _alpha = 0f;
            }
            else if (_alpha > 1f)
            {
                // we've reached full fade in -> fade out and send event
                _isFadingIn = false;
                _timeStartFade = Time.time;
                _alpha = 1f;
                OnFadeInFinished?.Invoke();
            }

            _texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, _alpha));
            _texture.Apply();
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
        }

        private void CalculateAlpha()
        {
            if (_isFadingIn)
            {
                _alpha = (Time.time - _timeStartFade) / _fadeHalfTime; // intentionally unclamped
            }
            else
            {
                _alpha = 1f - (Time.time - _timeStartFade) / _fadeHalfTime; // intentionally unclamped
            }
        }
    }
}
