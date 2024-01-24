using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
	public class TreeViewNode : IDisposable
	{
		#region fields

        private readonly HierarchyManager mMgrRef;
        private readonly TreeViewNode mParentRef;
        private readonly Transform mTransform;
        private readonly List<TreeViewNode> mChildren;
        private readonly int mRowIdx; // vertical depth
        private readonly int mColIdx; // horizontal depth

		private UINode mUIInstance;
        
		private bool mUnfold = true;
		private bool disposedValue;

		#endregion

		#region lifecycle

		public TreeViewNode(HierarchyManager manager, Transform transform)
        {
            mMgrRef = manager;
            mParentRef = null;
            mTransform = transform;
            mChildren = new();
			mRowIdx = 0;
			mColIdx = 0;

			InitializeNode();
            InitializeChildren();
		}

        private TreeViewNode(HierarchyManager manager, Transform transform, TreeViewNode parent, int rowIdx, int colIdx)
        {
            mMgrRef = manager;
			mParentRef = parent;
            mTransform = transform;
			mChildren = new();
			mRowIdx = rowIdx;
			mColIdx = colIdx;

			InitializeNode();
            InitializeChildren();
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
					GameObject.Destroy(mTransform.gameObject);
					foreach (var child in mChildren)
						child.Dispose();
				}

				disposedValue = true;
			}
		}

		#endregion

		#region private methods

		private void InitializeNode()
        {
			mUIInstance = GameObject.Instantiate(
                mMgrRef.NodePrefab,
				mMgrRef.HierarchyContainer
			);
			mUIInstance.gameObject.SetActive(mUnfold);
			mUIInstance.OnFold += TriggerFold;
			mUIInstance.OnActivate += TriggerActivate;
			mUIInstance.Initiate(mTransform, mTransform.childCount != 0, mRowIdx, mColIdx);
		}

		private void InitializeChildren()
        {
			int rowIdx = mRowIdx+1;
			for (int i = 0; i < mTransform.childCount; i++)
			{
				TreeViewNode child = new(mMgrRef, mTransform.GetChild(i), this, rowIdx++, mColIdx+1);
				mChildren.Add(child);
			}
		}

        private void TriggerFold()
        {
			mUnfold = !mUnfold;
			foreach (TreeViewNode child in mChildren)
				child.FoldMe(mUnfold);
			mUIInstance.OnFolded(mUnfold);
        }

        public void FoldMe(bool unfold)
        {
			mUIInstance.gameObject.SetActive(unfold);
			foreach (TreeViewNode child in mChildren)
				child.FoldMe(unfold);
		}

        private void TriggerActivate()
        {
            if (mTransform.TryGetComponent<HierarchyViewActivatable>(out var comp))
                comp.OnActivate?.Invoke();
        }

		#endregion
	}
}