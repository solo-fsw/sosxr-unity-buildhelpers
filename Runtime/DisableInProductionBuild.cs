using UnityEngine;


public class DisableInProductionBuild : MonoBehaviour
{
    [SerializeField] private GameObject m_objectToDisable;


    private void Awake()
    {
        if (m_objectToDisable == null)
        {
            m_objectToDisable = gameObject;
        }

        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        // Do nothing
        #else
        DisableComponents();
        #endif
    }


    [ContextMenu(nameof(DisableComponents))]
    private void DisableComponents()
    {
        m_objectToDisable.SetActive(false);
    }
}