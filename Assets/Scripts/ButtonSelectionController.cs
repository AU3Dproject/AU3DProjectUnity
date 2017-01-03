using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ButtonSelectionController : MonoBehaviour {
	[SerializeField]
	private float m_lerpTime;
	private ScrollRect m_scrollRect;
	private MyButton[] m_buttons;
	private int m_index;
	private float m_verticalPosition;
	private bool m_up;
	private bool m_down;
	private int before_index = 0;

	public MapManager mapManager;

	bool init = true; 

	public EventSystem ev;

	public void Start() {
		
	}

	public void Update() {

		if (init) {
			Init();
		} else {
			if (ev.currentSelectedGameObject != null) {

				int index = -1;
				for (int i = 0; i < mapManager.transform.childCount; i++) {

					if (m_buttons[i].id.ToString() == ev.currentSelectedGameObject.name) {
						index = i;
						break;
					}

				}

				if (isAllVisible(index) == -1) {

					move_auto(index);

				} else if (isAllVisible(index) == 1) {

					move_auto(index);

				} else if (isAllVisible(index) == 2) {

					move_auto(index);

				}

			}
		}
	}

	private void Init() {
		if (transform.GetChild(0).GetChild(0).childCount == mapManager.transform.childCount) {
			m_scrollRect = GetComponent<ScrollRect>();
			Toggle[] btns = GetComponentsInChildren<Toggle>();
			m_buttons = new MyButton[btns.Length];
			for (int i=0;i<m_buttons.Length;i++) {
				RectTransform btnRect = btns[i].GetComponent<RectTransform>();
				m_buttons[i] = new MyButton(int.Parse(btns[i].gameObject.name), btnRect.transform.localPosition.y, btnRect.rect.height);
			}
			init = false;
		}
	}

	public void move_auto(int index) {
		if (index == 0) {
			m_verticalPosition = 1f;
		} else if (index == m_buttons.Length) {
			m_verticalPosition = 0f;
		} else {

			if (before_index != -1) {
				if (before_index - index < 0) {
					m_verticalPosition = 1f - (m_buttons[index].bottom_y - GetComponent<RectTransform>().rect.height) / (transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().rect.height - GetComponent<RectTransform>().rect.height);
				} else if (before_index - index > 0) {
					m_verticalPosition = 1f - (m_buttons[index].top_y) / (transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().rect.height - GetComponent<RectTransform>().rect.height);
				}
			}

		}
		if (m_verticalPosition < 0) {
			m_verticalPosition = 0;
		} else if (m_verticalPosition > 1) {
			m_verticalPosition = 1;
		}
		m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);
		before_index = index;
	}

	private int isAllVisible(int index) {
		if (index == -1) {
			return 0;
		} else {
			float scroll_y = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localPosition.y;
			float rect_height = GetComponent<RectTransform>().rect.height;
			bool is_top = m_buttons[index].top_y >= scroll_y;
			bool is_under = m_buttons[index].bottom_y <= (scroll_y + rect_height);
			if (is_top && is_under) {
			} else {
				if (is_top)
					return -1;
				if (is_under)
					return 1;
			}
			
		}
		return 0;
	}

	class MyButton {
		public int id;
		public float top_y;
		public float bottom_y;
		public float y;
		public float height;
		public MyButton(int id, float y,float height) {
			this.id = id;
			this.y = y;
			this.height = height;
			top_y = -y;
			bottom_y = -(y-height);
		}
	}
}