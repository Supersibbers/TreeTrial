using UnityEngine;
using System.Collections;
using System.Linq;

public class LeafPile : MonoBehaviour {

	public GameObject[] leaves;

	public int leafIndexX;
	public int leafIndexY;

	private AutumnController autumnController;

	Transform[] transforms;
	Vector3[] leafTransformPositions;
	Quaternion[] leafTransformRotations;

	void Start () {
		autumnController = FindObjectOfType<AutumnController> ();
		transforms = GetComponentsInChildren<Transform> ();
		leafTransformPositions = new Vector3[transform.childCount];
		leafTransformRotations = new Quaternion[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			leafTransformPositions[i] = transforms [i].transform.position;
			leafTransformRotations[i] = transforms [i].transform.rotation;
		}

		for (int i = 0; i < Random.Range (7, 11); i++) {
			int index = Random.Range (0, leafTransformPositions.Length);
			GameObject leaf = Instantiate (leaves[0], leafTransformPositions [index], leafTransformRotations [index], this.transform) as GameObject;
			leaf.transform.parent = this.transform;
			leaf.transform.localScale = Vector3.one;
			leaf.GetComponent<Renderer>().material = autumnController.materials[leafIndexX, leafIndexY];
		}
	}
}
