using System;
using System.Collections.Generic;
using UnityEngine;

namespace CENTIS.UnityHierarchyView
{
	internal class TreeViewNode : IDisposable, IEquatable<TreeViewNode>
	{
		#region fields

        private readonly HierarchyConfiguration mConfig;
        private readonly Transform mTransform;
        private readonly TreeViewNode mParentRef;
        private readonly List<TreeViewNode> mChildren;
        private readonly int mRowIdx;
        private readonly int mColIdx;

		private UINode mUIInstance;
        
		private bool mFoldedOut;
		private bool disposedValue;

		#endregion

		#region lifecycle

        public TreeViewNode(HierarchyConfiguration config, Transform transform, TreeViewNode parent, 
			int rowIdx, int colIdx, bool unfold)
        {
            mConfig = config;
            mTransform = transform;
			mParentRef = parent;
			mChildren = new();
			mRowIdx = rowIdx;
			mColIdx = colIdx;
			mFoldedOut = unfold;

			InitializeNode();
		}

		~TreeViewNode()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					mUIInstance.OnFold -= TriggerFold;
					mUIInstance.OnActivate -= TriggerActivate;
				}

				disposedValue = true;

				GameObject.Destroy(mUIInstance.gameObject);
				foreach (var child in mChildren)
					child.Dispose();
			}
		}

		public bool Equals(TreeViewNode other)
		{
			if (other == null) return false;
			return mTransform.Equals(other.mTransform);
		}

		public override int GetHashCode()
		{
			return mTransform.GetHashCode();
		}

		#endregion

		#region methods

		private void InitializeNode()
        {
			mUIInstance = GameObject.Instantiate(
                mConfig.NodePrefab,
				mConfig.HierarchyContainer
			);

			mUIInstance.gameObject.SetActive(mFoldedOut);
			mUIInstance.OnFold += TriggerFold;
			mUIInstance.OnActivate += TriggerActivate;
			mUIInstance.Initiate(mTransform, mFoldedOut, mTransform.childCount != 0, mRowIdx, mColIdx);

			if (mParentRef is null)
				mUIInstance.gameObject.SetActive(true);
		}

        private void TriggerFold()
        {
			mFoldedOut = !mFoldedOut;
			foreach (TreeViewNode child in mChildren)
				child.SetActive(mFoldedOut);
			mUIInstance.OnFolded(mFoldedOut);
        }

        private void SetActive(bool active)
        {
			mUIInstance.gameObject.SetActive(active);
			foreach (TreeViewNode child in mChildren)
				child.SetActive(active && mFoldedOut);
		}

        private void TriggerActivate()
        {
            if (mTransform.TryGetComponent<HierarchyViewActivatable>(out var comp))
                comp.OnActivate?.Invoke();
        }

		public void AddChild(TreeViewNode child)
		{
			mChildren.Add(child);
		}

		public void FoldOutStructure()
		{
			TriggerFold();
			mParentRef?.FoldOutStructure();
		}

		#endregion
	}
}