using System;
using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
    public abstract class UINode : MonoBehaviour
    {
		public event Action OnFold;
        public event Action OnActivate;

        public abstract void Initiate(string name);

        public void FireOnFold() => OnFold?.Invoke();
        public void FireOnActivate() => OnActivate?.Invoke();
    }
}
