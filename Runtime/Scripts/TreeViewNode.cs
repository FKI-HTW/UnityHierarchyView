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
        private readonly int mDepth;

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
            mDepth = 0;

			InitializeNode();
            InitializeChildren();
		}

        private TreeViewNode(HierarchyManager manager, Transform transform, TreeViewNode parent, int depth)
        {
            mMgrRef = manager;
			mParentRef = parent;
            mTransform = transform;
			mChildren = new();
			mDepth = depth;

			InitializeNode();
            InitializeChildren();
		}

		~TreeViewNode()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
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
                mParentRef == null 
					? mMgrRef.HierarchyContainer.transform 
					: mParentRef.mUIInstance.transform
			);
			mUIInstance.gameObject.SetActive(mUnfold);
			mUIInstance.Initiate(mTransform.name);
			mUIInstance.OnFold += TriggerFold;
			mUIInstance.OnActivate += TriggerActivate;

			{   // TODO : update this, so that it is modular
				Vector3 pos = mUIInstance.transform.position;
				pos.x = 795 + mDepth * 25;
				pos.y = 670 - mDepth * 2 * 25;
				pos.z = 0;

				mUIInstance.transform.position = pos;
			}
		}

		private void InitializeChildren()
        {
			int childCount = mTransform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				TreeViewNode child = new(mMgrRef, mTransform.GetChild(i), this, mDepth + 1);
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
		}

        private void TriggerActivate()
        {
            if (mTransform.TryGetComponent<HierarchyViewActivatable>(out var comp))
                comp.OnActivate?.Invoke();
        }

		#endregion
	}
}