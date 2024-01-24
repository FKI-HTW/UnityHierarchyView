using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
    public class HierarchyManager : MonoBehaviour
    {
		#region fields

		public UINode NodePrefab { get => _nodePrefab; set => _nodePrefab = value; }
		[SerializeField] private UINode _nodePrefab;

		public GameObject HierarchyContainer { get => _hierarchyContainer; }
        [SerializeField] private GameObject _hierarchyContainer;

        private TreeViewNode _parent;

        [SerializeField] private Transform _test; // for testing

		#endregion

		private void Start()
        {
            OpenTree(_test); // for testing
        }

        #region public methods

        public void OpenTree(Transform transform)
        {
            _parent = new(this, transform);
		}

        public void CloseTree()
        {
            _parent.Dispose();
            _parent = null;
        }

        #endregion
    }
}
