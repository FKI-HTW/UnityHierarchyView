using System;
using UnityEngine;

namespace VENTUS.UnityHierarchyView
{
    [Serializable]
    [CreateAssetMenu(fileName = "new HierarchyConfiguration", menuName = "VENTUS/HierarchyView")]
    public class HierarchyConfiguration : ScriptableObject
    {
        public UINode ItemPrefab { get => _itemPrefab; set => _itemPrefab = value; }
        [SerializeField] private UINode _itemPrefab;
	}
}
