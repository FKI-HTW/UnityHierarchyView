using CENTIS.UnityHierarchyView;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	[SerializeField] private Transform _test;
    [SerializeField] private HierarchyManager _manager;

	private void Start()
	{
		_manager.OpenHierarchyView(_test); // for testing
	}
}
