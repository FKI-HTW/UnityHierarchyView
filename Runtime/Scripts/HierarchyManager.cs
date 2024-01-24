using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
    public class HierarchyManager : MonoBehaviour
    {
		#region fields

		public UINode NodePrefab { get => _nodePrefab; set => _nodePrefab = value; }
		[SerializeField] private UINode _nodePrefab;

		public Transform HierarchyContainer { get => _hierarchyContainer; }
        [SerializeField] private Transform _hierarchyContainer;

        private TreeViewNode _parent;

        [SerializeField] private Transform _test; // for testing

		#endregion

		private void Start()
        {
			OpenHierarchyView(_test); // for testing
        }

        #region public methods

        public void OpenHierarchyView(Transform transform)
        {
            _parent = new(this, transform);
		}

        public void CloseHierarchyView()
        {
            _parent.Dispose();
            _parent = null;
        }

        #endregion
    }
}
