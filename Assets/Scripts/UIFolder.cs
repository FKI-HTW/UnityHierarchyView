using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VENTUS.UnityHierarchyView;
using TMPro;

public class UIFolder : UINode
{
	[SerializeField] private TMP_Text _name;

	public override void Initiate(string name)
	{
		_name.text = name;
		gameObject.name = $"Hierarchy: {name}";
	}
}
