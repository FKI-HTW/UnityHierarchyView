using UnityEngine;
using VENTUS.UnityHierarchyView;
using TMPro;
using UnityEngine.UI;

public class TestItem : UINode
{
	[SerializeField] private RectTransform _indentation;
	[SerializeField] private Button _foldButton;
	[SerializeField] private TMP_Text _foldButtonText;
	[SerializeField] private TMP_Text _name;
	[SerializeField] private float _indentationMult;

	public override void Initiate(Transform transform, bool hasChildren, int rowIndex, int columnIndex)
	{
		_name.text = transform.name;
		gameObject.name = $"Hierarchy: {transform.name}";
		_indentation.sizeDelta += _indentationMult * columnIndex * Vector2.right;
		if (!hasChildren)
		{
			_foldButtonText.gameObject.SetActive(false);
			_foldButton.interactable = false;
		}
	}

	public override void OnFolded(bool folded)
	{
		_foldButtonText.text = folded ? "+" : "-";
	}
}
