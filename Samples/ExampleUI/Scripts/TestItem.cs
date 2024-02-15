using UnityEngine;
using CENTIS.UnityHierarchyView;
using TMPro;
using UnityEngine.UI;

public class TestItem : UINode
{
	[SerializeField] private RectTransform _indentation;
	[SerializeField] private Button _foldButton;
	[SerializeField] private TMP_Text _foldButtonText;
	[SerializeField] private TMP_Text _name;
	[SerializeField] private float _indentationMult;

	public override void Initiate(Transform transform, bool foldedOut, bool hasChildren, int rowIndex, int columnIndex)
	{
		_name.text = transform.name;
		gameObject.name = $"Hierarchy: {transform.name}";

		_foldButtonText.text = foldedOut ? "-" : "+";

		float indentation = _indentationMult * columnIndex;
		if (!hasChildren)
		{
			// compensate for lack of fold button
			indentation += _foldButton.GetComponent<RectTransform>().sizeDelta.x;
			_foldButton.gameObject.SetActive(false);
		}

		_indentation.sizeDelta = new(_indentation.sizeDelta.x + indentation, _indentation.sizeDelta.y);
	}

	public override void OnFolded(bool foldedOut)
	{
		_foldButtonText.text = foldedOut ? "-" : "+";
	}
}
