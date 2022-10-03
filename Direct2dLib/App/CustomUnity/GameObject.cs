using Direct2dLib.App.CustomUnity;
using SharpDX;
using System.Collections.Generic;
using System.Diagnostics;

namespace Direct2dLib.App
{
    public class GameObject
    {
        public Transform transform;

        private List<Component> _components;
        private bool _isActive = true;

        public GameObject gameObject => this;

        public GameObject(Vector3 startPosition)
        {
            transform = new Transform(this);
            transform.position = startPosition;
            _components = new List<Component>();
        }

        public GameObject()
        {
            transform = new Transform(this);
            transform.position = DX2D.Instance.ScreenCenter;
            _components = new List<Component>();
        }

        public void Start()
        {
            if (!_isActive) return;

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Start();
            }
        }

        public void Update()
        {
            if (!_isActive) return;

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Update();
            }
        }

        public void OnCollision(Component component)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnCollision(component);
            }
        }

        public T AddComponent<T>(T newComponent) where T : Component
        {
            _components.Add(newComponent);

            return newComponent;
        }

        public void RemoveComponent<T>() where T : Component, new()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    _components.Remove(component);
                }
            }
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }

            return null;
        }

        public bool TryGetComponent<T>(out T outComponent) where T : Component
        {
            outComponent = null;

            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    outComponent = (T)component;
                    return true;
                }
            }

            return false;
        }

        public void SetActive(bool value)
        {
            _isActive = value;
        }
    }
}
