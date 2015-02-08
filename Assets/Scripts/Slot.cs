using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

	public Text text;

	public Image fill;

	public void update (string label, float percentage, Color color)
	{
		text.text = label;
		text.color = color;
		text.rectTransform.anchorMax = Vector2.one;
		fill.rectTransform.anchorMax = new Vector2 (1, percentage);
		// The background color is the same as the main color, but lighter - GDD -
		color.r += 0.5f;
		color.g += 0.5f;
		color.b += 0.5f;
		fill.color = color;
	}
}
