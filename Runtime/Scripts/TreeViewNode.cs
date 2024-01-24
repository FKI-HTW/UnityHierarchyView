using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
	public class TreeViewNode : IDisposable
	{
        public UINode UIInstance { get; private set; }

        private readonly HierarchyManager mMgrRef;
        private readonly TreeViewNode mParentRef;
        private readonly Transform mTransform;

        private readonly List<TreeViewNode> mChildren;
        private bool mUnfold = true;
        private int mRowIndex = 0;
		private bool disposedValue;

		private const int mBtnSize = 25;

        public TreeViewNode(HierarchyManager manager, Transform transform)
        {
            mMgrRef = manager;
            mParentRef = null;
            mTransform = transform;
            mRowIndex = 0;
            mChildren = new();

			InititializeNode(manager);
            UpdateLocation(mRowIndex);

            InitializeChildren();
		}

        private TreeViewNode(HierarchyManager manager, Transform transform, TreeViewNode parent, int index)
        {
            mMgrRef = manager;
			mParentRef = parent;
            mTransform = transform;
			mRowIndex = index;
			mChildren = new();

			InititializeNode(manager);
			UpdateLocation(mRowIndex);

            InitializeChildren();
		}

        private void InitializeChildren()
        {
			int childCount = mTransform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				TreeViewNode child = new(mMgrRef, mTransform.GetChild(i), this, mRowIndex + 1);
				mChildren.Add(child);
			}
		}

        private void InititializeNode(HierarchyManager manager)
        {
            UIInstance = GameObject.Instantiate(
                manager.HierarchyConfiguration.ItemPrefab,
                mParentRef == null ? manager.HierarchyContainer.transform : mParentRef.UIInstance.transform);
            UIInstance.gameObject.SetActive(mUnfold);
            UIInstance.Initiate(mTransform.name);
            UIInstance.OnFold += TriggerFold;
            UIInstance.OnActivate += TriggerActivate;
        }

        private void TriggerFold()
        {
            foreach (var child in mChildren)
				child.FoldMe();
        }

        public void FoldMe()
        {
            mUnfold = !mUnfold;
            UIInstance.gameObject.SetActive(mUnfold);
		}

        public void TriggerActivate()
        {
            if (mTransform.TryGetComponent<ActivationComponent>(out var comp))
                comp.OnActivate?.Invoke();
        }

		// TODO : this
		private void UpdateLocation(int rowIndex)
		{
			mRowIndex = rowIndex;

			Vector3 pos = UIInstance.transform.position;
			pos.x = 795 + rowIndex * mBtnSize;
			pos.y = 670 - rowIndex * 2 * mBtnSize;
			pos.z = 0;

			UIInstance.transform.position = pos;
		}

        private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					GameObject.Destroy(mTransform.gameObject);
					foreach (var child in mChildren)
						child.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}