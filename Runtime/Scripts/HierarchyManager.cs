using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
    public class HierarchyManager : MonoBehaviour
    {
        #region fields

        public HierarchyConfiguration HierarchyConfiguration { get => _hierarchyConfiguration; set => _hierarchyConfiguration = value; }
        [SerializeField] private HierarchyConfiguration _hierarchyConfiguration;

        public GameObject HierarchyContainer { get => _hierarchyContainer; }
        [SerializeField] private GameObject _hierarchyContainer;

        private TreeViewNode _parent;

        [SerializeField] private Transform _test;

        #endregion

        private void Start()
        {
            OpenTree(_test);
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
