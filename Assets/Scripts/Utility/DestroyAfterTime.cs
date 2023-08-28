using UnityEngine;

namespace ColourMatch
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float destroyAfterSeconds = 1.5f;

        private void Start()
        {
            Destroy(this.gameObject, destroyAfterSeconds);
        }
    }
}
