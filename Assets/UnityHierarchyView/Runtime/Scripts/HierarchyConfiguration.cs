using UnityEngine;

namespace CENTIS.UnityHierarchyView
{
	[System.Serializable]
    public class HierarchyConfiguration
    {
		public UINode NodePrefab => _nodePrefab;
		[SerializeField] private UINode _nodePrefab;

		public Transform HierarchyContainer => _hierarchyContainer;
		[SerializeField] private Transform _hierarchyContainer;
	}
}
