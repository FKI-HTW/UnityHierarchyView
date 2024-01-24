using System;
using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
    public class UINode : MonoBehaviour
    {
		public event Action OnFold;
        public event Action OnActivate;

        public void Fold() => OnFold?.Invoke();
        public void Activate() => OnActivate?.Invoke();

        public virtual void Initiate(Transform transform) { }
        public virtual void OnFolded(bool fold) { }
    }
}
