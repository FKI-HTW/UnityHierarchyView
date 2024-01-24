using UnityEngine;
using VENTUS.UnityHierarchyView;
using TMPro;

public class TestItem : UINode
{
	[SerializeField] private TMP_Text _name;
	[SerializeField] private TMP_Text _foldButton;

	public override void Initiate(Transform transform)
	{
		_name.text = transform.name;
		gameObject.name = $"Hierarchy: {transform.name}";
	}

	public override void OnFolded(bool folded)
	{
		_foldButton.text = folded ? "+" : "-";
	}
}
