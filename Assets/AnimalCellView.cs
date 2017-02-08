using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EnhancedUI.EnhancedScroller;

/// <summary>
/// This delegate handles the UI's button click
/// </summary>
/// <param name="cellView">The cell view that had the button click</param>
public delegate void SelectedDelegate(EnhancedScrollerCellView cellView);

public class AnimalCellView : EnhancedScrollerCellView {

	public Text place_name_text;
	public Text description_text;
	public Image thumbnail_image;
	public Image selection_panel;

	public Color selected_color = Color.red;
	public Color normaled_color = Color.white;

	SelectedDelegate selected;

	public void SetData(ScrollerData data) {
		place_name_text.text = data.place_name;
		description_text.text = data.description;
		thumbnail_image.sprite = Resources.Load<Sprite>(data.thumbnail_url);
	}

	/// <summary>
	/// This function changes the UI state when the item is 
	/// selected or unselected.
	/// </summary>
	/// <param name="selected">The selection state of the cell</param>
	private void SelectedChanged(bool selected) {
		selection_panel.color = (selected ? selected_color : normaled_color);
	}

	/// <summary>
	/// This function is called by the cell's button click event
	/// </summary>
	public void OnSelected() {
		// if a handler exists for this cell, then
		// call it.
		if (selected != null) {
			selected(this);
		}
	}

}