using UnityEngine;
using UnityEngine.Events;

namespace VENTUS.UnityHierarchyView
{
    public class HierarchyViewActivatable : MonoBehaviour
    {
        public UnityEvent OnActivate;

		private void Start()
		{
			OnActivate.AddListener(() => Debug.Log($"Activated: {transform.name}")); // for testing
		}
	}
}
