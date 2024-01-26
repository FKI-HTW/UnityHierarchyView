using UnityEngine;

namespace CENTIS.UnityHierarchyView
{
    public class HierarchyManager : MonoBehaviour
    {
		#region fields

		public UINode NodePrefab { get => _nodePrefab; set => _nodePrefab = value; }
		[SerializeField] private UINode _nodePrefab;

		public Transform HierarchyContainer { get => _hierarchyContainer; }
        [SerializeField] private Transform _hierarchyContainer;

        private TreeViewNode _parent;

		#endregion

        #region public methods

        public virtual void OpenHierarchyView(Transform transform)
        {
            _parent = new(this, transform);
		}

        public virtual void CloseHierarchyView()
        {
            _parent.Dispose();
            _parent = null;
        }

        #endregion
    }
}
