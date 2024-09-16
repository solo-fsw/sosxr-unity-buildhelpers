using UnityEngine;


namespace SOSXR.SimpleHelpers
{
    public class DestroyInBuild : MonoBehaviour
    {
        [SerializeField] private bool m_destroyInBuild = true;


        private void Awake()
        {
            if (!NeedsDestroying())
            {
                return;
            }

            Destroy(gameObject);
        }


        private bool NeedsDestroying()
        {
            return m_destroyInBuild && !Application.isEditor;
        }
    }
}