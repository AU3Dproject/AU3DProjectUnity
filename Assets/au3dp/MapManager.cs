﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine.EventSystems;

/// <summary>
/// MapManager - マップデータ（ナビゲーションプレイス）の管理を行う
/// </summary>
public class MapManager : ManagerMonoBehaviour<MapManager>, IEnhancedScrollerDelegate {

	private List<MapData> _data;
	public EnhancedScroller myScroller;
	public MapAnimalCellView animalCellViewPrefab;

	public int jump_scroller_offset = 10;
	public int jump_cell_offset = 10;
	public bool useSpacing = true;

	private NavigationDetail[] details = null;
	private GameObject player = null;

	void Start() {
		if (details == null) {
			details = transform.GetComponentsInChildren<NavigationDetail>();
		}

		player = PlayerManager.Instance.player_model;

		_data = new List<MapData>();
		foreach (NavigationDetail detail in details) {
			_data.Add(new MapData() {
				place_name = detail.NavigationName,
				description = detail.NavigationDescription,
				thumbnail_url = detail.ThumbnailAddr
			});
		}
		
		myScroller.Delegate = this;
		myScroller.ReloadData();
	}

	/// <summary>
	/// GetNumberOfCells - マップのデータの数
	/// </summary>
	/// <param name="scroller">スクローラー</param>
	/// <returns>データ数</returns>
	public int GetNumberOfCells(EnhancedScroller scroller) {
		return _data.Count;
	}

	/// <summary>
	/// GetCellViewSize - データを表示するセルの高さ
	/// </summary>
	/// <param name="scroller">スクローラー</param>
	/// <param name="dataIndex">データ数</param>
	/// <returns>セルの高さ</returns>
	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) {
		return animalCellViewPrefab.GetComponent<RectTransform>().sizeDelta.y;
	}

	/// <summary>
	/// GetCellView - CellViewの取得
	/// </summary>
	/// <param name="scroller">スクローラー</param>
	/// <param name="dataIndex">データのインデックス</param>
	/// <param name="cellIndex">セルのインデックス</param>
	/// <returns>Cell</returns>
	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex) {
		MapAnimalCellView cellView = scroller.GetCellView(animalCellViewPrefab) as MapAnimalCellView;
		cellView.SetData(dataIndex,_data[dataIndex]);
		cellView.decided = CellViewDecided;
		cellView.selected = CellViewSelected;
		return cellView;
	}

	/// <summary>
	/// This function handles the cell view's button click event
	/// </summary>
	/// <param name="cellView">The cell view that had the button clicked</param>
	private void CellViewDecided(EnhancedScrollerCellView cellView) {
		if (cellView == null) {

		} else {
			// get the selected data index of the cell view
			var selectedDataIndex = (cellView as MapAnimalCellView).DataIndex;

			// loop through each item in the data list and turn
			// on or off the selection state. This is done so that
			// any previous selection states are removed and new
			// ones are added.
			for (var i = 0; i < _data.Count; i++) {
				if (_data[i].Selected = (selectedDataIndex == i)) {
					NavigationManager.Instance.setToTarget(transform.GetChild(i).gameObject);
				}
			}

		}
	}

	/// <summary>
	/// This function handles the cell view's button click event
	/// </summary>
	/// <param name="cellView">The cell view that had the button clicked</param>
	private void CellViewSelected(EnhancedScrollerCellView cellView) {
			// get the selected data index of the cell view
			var selectedDataIndex = (cellView as MapAnimalCellView).DataIndex;
			jump(selectedDataIndex);
	}

	public void jump(int dataIndex) {
		myScroller.JumpToDataIndex(dataIndex, jump_scroller_offset, jump_cell_offset, useSpacing, EnhancedScroller.TweenType.linear, 0.1f, null, myScroller.ScrollSize);
	}

	public void focus() {
		//EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(1).gameObject);
	}

}