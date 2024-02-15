using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.UnityHierarchyView
{
    public class HierarchyManager : MonoBehaviour
    {
        #region fields

        public UINode NodePrefab => _nodePrefab;
		[SerializeField] private UINode _nodePrefab;

		public Transform HierarchyContainer => _hierarchyContainer;
        [SerializeField] private Transform _hierarchyContainer;

        private TreeViewNode _parent;
        private readonly Dictionary<Transform, TreeViewNode> _hierarchyNodes = new();

		#endregion

        #region public methods

        /// <summary>
        /// Creates a UI representation of the given transform's hierarchy.
        /// </summary>
        /// <param name="transform">The transform of whose hierarchy a UI is constructed</param>
        /// <param name="startFoldedOut">Wether the UI hierarchy should be completely folded out or not</param>
        public virtual void OpenHierarchyView(Transform transform, bool startFoldedOut = true)
        {
			_parent = InitializeHierarchyNodes(transform, null, 0, 0, startFoldedOut);
		}

        /// <summary>
        /// Closes and unloads the created hierarchy nodes.
        /// </summary>
        public virtual void CloseHierarchyView()
        {
            _parent.Dispose();
            _parent = null;
        }

		#endregion

		#region private methods

        private TreeViewNode InitializeHierarchyNodes(Transform transform, TreeViewNode parent, int rowIdx, int colIdx, bool foldOut)
        {
			TreeViewNode node = new(this, transform, parent, rowIdx++, colIdx + 1, foldOut);
            _hierarchyNodes.Add(transform, node);

			for (int i = 0; i < transform.childCount; i++)
			{
                TreeViewNode childNode = InitializeHierarchyNodes(transform.GetChild(i), node, rowIdx, colIdx + 1, foldOut);
                node.AddChild(childNode);
			}

            return node;
		}

		#endregion
	}
}
