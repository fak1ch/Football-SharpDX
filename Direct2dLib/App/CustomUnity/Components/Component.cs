using SharpDX;
using System.ComponentModel;
using System.Linq;

namespace Direct2dLib.App.CustomUnity
{
    public class Component
    {
        public Transform transform => gameObject.transform;
        public GameObject gameObject { get; private set; }

        public Component(GameObject go)
        {
            gameObject = go;
        }

        public virtual Vector3 GetIndividualPosition()
        {
            return gameObject.transform.position;
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {
        }

        public virtual void OnCollision(Component component)
        {

        }
    }
}
