using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
public class HorizontalOrVerticalNavigationGroup : MonoBehaviour {
	public bool wrap;

	HorizontalOrVerticalLayoutGroup layoutGroup;
	bool vertical;
	bool horizontal;

	void Start() {
		layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
		vertical = layoutGroup.GetType() == typeof(VerticalLayoutGroup);
		horizontal = layoutGroup.GetType() == typeof(HorizontalLayoutGroup);

		SetNavigation();
	}

	void SetNavigation() {
		Selectable[] selectables = GetComponentsInChildren<Selectable>();
		for (int i = 0; i < selectables.Length; i++) {
			var prevIndex = (i + selectables.Length - 1) % selectables.Length;
			var nextIndex = (i + 1) % selectables.Length;

			var nav = selectables[i].navigation;

			nav.mode = Navigation.Mode.Explicit;

			nav.selectOnUp = vertical ? selectables[prevIndex] : null;
			nav.selectOnDown = vertical ? selectables[nextIndex] : null;
			nav.selectOnLeft = horizontal ? selectables[prevIndex] : null;
			nav.selectOnRight = horizontal ? selectables[nextIndex] : null;

			selectables[i].navigation = nav;
		}

		if (!wrap) {
			var firstSelectable = selectables[0];
			var firstNavigation = firstSelectable.navigation;
			firstNavigation.selectOnUp = null;
			firstNavigation.selectOnLeft = null;
			selectables[0].navigation = firstNavigation;

			var lastSelectable = selectables[selectables.Length - 1];
			var lastNavigation = selectables[selectables.Length - 1].navigation;
			lastNavigation.selectOnDown = null;
			lastNavigation.selectOnRight = null;
			lastSelectable.navigation = lastNavigation;
		}
	}
}