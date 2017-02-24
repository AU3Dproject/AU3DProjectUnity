
/// <summary>
/// This delegate handles any changes to the selection state of the data
/// </summary>
/// <param name="val">The state of the selection</param>
public delegate void SelectedChangedDelegate(bool val);

public class MapData {
	public string place_name;
	public string description;
	public string thumbnail_url;


	/// <summary>
	/// The delegate to call if the data's selection state
	/// has changed. This will update any views that are hooked
	/// to the data so that they show the proper selection state UI.
	/// </summary>
	public SelectedChangedDelegate selectedChanged;

	/// <summary>
	/// The selection state
	/// </summary>
	private bool _selected;
	public bool Selected {
		get {
			return _selected;
		}
		set {
			// if the value has changed
			if (_selected != value) {
				// update the state and call the selection handler if it exists
				_selected = value;
				if (selectedChanged != null)
					selectedChanged(_selected);
			}
		}
	}


}