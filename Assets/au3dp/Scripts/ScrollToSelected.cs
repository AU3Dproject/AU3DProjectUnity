using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class ScrollToSelected : MonoBehaviour {
	ScrollRect scrollRect;
	RectTransform viewport;
	RectTransform content;
	RectTransform selected;

	private bool init = true;

	void Start() {
		
	}

	// Update is called once per frame
	void Update() {

		if (init) {
			if (transform.GetChild(0).GetChild(0).childCount > 0) {
				Init();
			}
		} else {

			// update lastSelectedChild
			var selectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (selectedGameObject == null)
				selected = null;
			else {
				var selectedIsChildOfContent = selectedGameObject.transform.IsChildOf(content.transform);
				if (selectedIsChildOfContent)
					selected = selectedGameObject.GetComponent<RectTransform>();
				else
					selected = null;
			}

			// scroll
			if (selected != null)
				LerpToSelected();

		}
	}

	void Init() {
		scrollRect = GetComponent<ScrollRect>();
		viewport = scrollRect.viewport;
		content = scrollRect.content;

		var eventTrigger = GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();

		var entryBeginDrag = new EventTrigger.Entry();
		entryBeginDrag.eventID = EventTriggerType.BeginDrag;
		entryBeginDrag.callback.AddListener((x) => BeginDrag());
		eventTrigger.triggers.Add(entryBeginDrag);

		init = false;
	}

	void LerpToSelected() {
		var viewportRect = new Rect(
			(Vector2)viewport.position + viewport.rect.position,
			viewport.rect.size);
		var selectedRect = new Rect(
			(Vector2)selected.position + selected.rect.position,
			selected.rect.size);
		var clampedSelectedRect = selectedRect.Clamp(viewportRect);

		var dir = clampedSelectedRect.position - selectedRect.position;
		var vec = Vector3.Lerp(Vector3.zero, dir, Time.deltaTime * 10f);
		content.position += vec;
	}

	void BeginDrag() {
		EventSystem.current.SetSelectedGameObject(null);
	}
}

public class Interval {
	public float x;
	public float width;

	public float min {
		get {
			return x;
		}
	}
	public float max {
		get {
			return x + width;
		}
	}
	public Interval(float x, float width) {
		this.x = x;
		this.width = width;
	}
}

public static class MyExtension {
	public static Interval Clamp(this Interval self, Interval limit) {
		if (self.width > limit.width || self.x < limit.x)
			return new Interval(limit.x, self.width);
		else if (self.max < limit.max)
			return new Interval(self.x, self.width);
		else
			return new Interval(limit.max - self.width, self.width);
	}
	public static Interval xInterval(this Rect self) {
		return new Interval(self.x, self.width);
	}
	public static Interval yInterval(this Rect self) {
		return new Interval(self.y, self.height);
	}
	public static Rect Clamp(this Rect self, Rect limit) {
		var xInterval = self.xInterval().Clamp(limit.xInterval());
		var yInterval = self.yInterval().Clamp(limit.yInterval());

		return new Rect(xInterval.x, yInterval.x, xInterval.width, yInterval.width);
	}
}