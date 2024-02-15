using CENTIS.UnityHierarchyView;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	[SerializeField] private Transform _testTransform;
	[SerializeField] private Transform _testTransform2;
    [SerializeField] private HierarchyManager _manager;

	private void Start()
	{
		_manager.OpenHierarchyView(_testTransform, _testTransform2);
	}

	public void DebugActivatable(Transform transform)
	{
		Debug.Log($"Activated: {transform.name}");
	}
}
