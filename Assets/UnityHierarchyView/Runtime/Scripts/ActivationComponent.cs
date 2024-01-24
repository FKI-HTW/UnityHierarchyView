using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VENTUS.UnityHierarchyView
{
    public class ActivationComponent : MonoBehaviour
    {
        public UnityEvent OnActivate;

		private void Start()
		{
			OnActivate.AddListener(() => Debug.Log("a"));
		}
	}
}
