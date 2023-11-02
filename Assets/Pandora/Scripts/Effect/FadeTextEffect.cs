using System;
using TMPro;
using UnityEngine;

namespace Pandora.Scripts.Effect
{
    public class FadeTextEffect : MonoBehaviour
    {
        float speed = 1f;
        float fadeSpeed = 1f;
        float fadeTime = 0.5f;
        protected float fadeTimer = 0f;
        protected float moveTimer = 0f;
        protected float moveTime = 0.5f;
        float moveSpeed = 1f;
        Vector3 moveDirection = Vector3.up;
        Vector3 moveAmount = Vector3.zero;
        TextMeshPro textMesh;
        Color textColor;


        public void Init(string text, Color color, float speed, float fadeTime, float moveTime, Vector3 moveDirection)
        {
            this.speed = speed;
            this.fadeTime = fadeTime;
            this.moveTime = moveTime;
            this.moveDirection = moveDirection;
            textMesh = GetComponent<TextMeshPro>();
            textMesh.text = text;
            textColor = color;
            textMesh.color = textColor;
        }

        void Update()
        {
            fadeTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (fadeTimer < fadeTime)
            {
                textColor.a = Mathf.Lerp(1f, 0f, fadeTimer / fadeTime);
                textMesh.color = textColor;
            }
            else
            {
                Destroy(gameObject);
            }
            if (moveTimer < moveTime)
            {
                moveAmount += moveDirection * moveSpeed * Time.deltaTime;
                transform.position += moveAmount;
            }
        }
        
    }
}