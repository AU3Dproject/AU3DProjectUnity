using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate {
	private List<ScrollerData> _data;
	public EnhancedScroller myScroller;
	public AnimalCellView animalCellViewPrefab;

	private NavigationDetail[] details = null;

	void Start() {
		if (details == null) {
			details = transform.GetComponentsInChildren<NavigationDetail>();
		}

		_data = new List<ScrollerData>();
		foreach (NavigationDetail detail in details) {
			_data.Add(new ScrollerData() {
				place_name = detail.NavigationName,
				description = detail.NavigationDescription,
				thumbnail_url = detail.ThumbnailAddr
			});
		}
		
		myScroller.Delegate = this;
		myScroller.ReloadData();
	}

	public int GetNumberOfCells(EnhancedScroller scroller) {
		return _data.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) {
		return animalCellViewPrefab.GetComponent<RectTransform>().sizeDelta.y;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int
	dataIndex, int cellIndex) {
		AnimalCellView cellView = scroller.GetCellView(animalCellViewPrefab) as
		AnimalCellView;
		cellView.SetData(_data[dataIndex]);
		return cellView;
	}

}