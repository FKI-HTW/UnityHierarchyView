using System;
using UnityEngine;

namespace CENTIS.UnityHierarchyView
{
    public class UINode : MonoBehaviour
    {
        public event Action OnFold;
        public event Action OnActivate;

        public void FoldNode() => OnFold?.Invoke();
        public void ActivateNode() => OnActivate?.Invoke();

        public virtual void Initiate(Transform transform, bool hasChildren, int rowIndex, int columnIndex) { }
        public virtual void OnFolded(bool fold) { }
    }
}
