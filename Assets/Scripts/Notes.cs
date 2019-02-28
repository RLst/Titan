using UnityEngine;

public class Notes : MonoBehaviour {
	[TextArea]
	[SerializeField] string notes = "Write notes here.";

	void Start() {
		if (notes != null)
		{
			//Do nothing. This avoids stupid warnings
		}
	}
}
