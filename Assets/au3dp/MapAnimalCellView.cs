using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using UnityEngine.EventSystems;

/// <summary>
/// This delegate handles the UI's button click
/// </summary>
/// <param name="cellView">The cell view that had the button click</param>
public delegate void DecidedDelegate(EnhancedScrollerCellView cellView);
public delegate void SelectedDelegate(EnhancedScrollerCellView cellView);

public class MapAnimalCellView : EnhancedScrollerCellView {

	public Text place_name_text;
	public Text description_text;
	public Image thumbnail_image;
	public Image selection_panel;

	public Color selected_color = Color.red;
	public Color normaled_color = Color.white;

	public DecidedDelegate decided;
	public SelectedDelegate selected;

	/// <summary>
	/// Public reference to the index of the data
	/// </summary>
	public int DataIndex {
		get; private set;
	}

	public void SetData(int data_index , MapData data) {
		if (data != null) {
			data.selectedChanged -= SelectedChanged;
		}

		place_name_text.text = data.place_name;
		description_text.text = data.description;
		thumbnail_image.sprite = Resources.Load<Sprite>(data.thumbnail_url);

		DataIndex = data_index;

		data.selectedChanged = SelectedChanged;

		// update the selection state UI
		SelectedChanged(data.Selected);
	}

	/// <summary>
	/// This function changes the UI state when the item is 
	/// selected or unselected.
	/// </summary>
	/// <param name="selected">The selection state of the cell</param>
	private void SelectedChanged(bool selected) {
		selection_panel.color = (selected ? selected_color : normaled_color);
		//if(selected)EventSystem.current.SetSelectedGameObject(gameObject);
	}

	/// <summary>
	/// This function is called by the cell's button click event
	/// </summary>
	public void OnDecided() {
		// if a handler exists for this cell, then
		// call it.
		if (decided != null) {
			decided(this);
		}
	}

	public void OnSelected() {
		if (selected != null) {
			selected(this);
		}
	}

}