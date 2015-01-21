using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

	public Text text;

	public Image fill;

	public void update (string label, float percentage, Color color)
	{
		text.text = label;
		fill.rectTransform.anchorMax = new Vector2 (1, percentage);
		fill.color = color;
	}
}
