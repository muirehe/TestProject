using UnityEngine;

namespace Presentation.Abilities
{
    public class AoeVfx : MonoBehaviour
    {
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Renderer rend;
        [SerializeField] private float duration = 0.4f;

        private MaterialPropertyBlock _block;
        private Color _color;
        private float _diameter;
        private float _time;

        public void Play(float radius)
        {
            _diameter = radius * 2f;
            _block = new MaterialPropertyBlock();
            _color = rend.sharedMaterial.GetColor(BaseColor);
            Destroy(gameObject, duration);
            UpdateAnimation();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            var t = Mathf.Clamp01(_time / duration);
            var size = Mathf.Lerp(0.2f, 1f, t) * _diameter;
            transform.localScale = new Vector3(size, 0.01f, size);
            _block.SetColor(BaseColor, new Color(_color.r, _color.g, _color.b, _color.a * (1f - t)));
            rend.SetPropertyBlock(_block);
        }
    }
}