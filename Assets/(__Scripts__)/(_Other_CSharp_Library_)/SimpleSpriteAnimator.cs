using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSCL
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleSpriteAnimator : MonoBehaviour
    {
        [SerializeField] Sprite[] _sprites;
        [SerializeField] float[] _timeBetweenFrames;

        private SpriteRenderer _spriteRenderer;
        private float _lastTime;
        private int _currentFrame;
        private bool _cull;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if(_sprites == null || _timeBetweenFrames == null)
            {
                Debug.LogError($"SimpleSpriteAnimator is not initilazed.");
            }
        }

        private void OnEnable()
        {
            _currentFrame = 0;
            _lastTime = Time.time;
        }

        private void Update()
        {
            if(_cull) return;
            if(Time.time - _lastTime > _timeBetweenFrames[_currentFrame])
            {
                _currentFrame++;
                if(_currentFrame >= _sprites.Length) _currentFrame = 0;
                _lastTime = Time.time;

                _spriteRenderer.sprite = _sprites[_currentFrame];
            }
        }

        private void OnBecameVisible()
        {
            _cull = false;
        }

        private void OnBecameInvisible()
        {
            _cull = true;
        }
    }
}
