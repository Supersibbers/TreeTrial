using UnityEngine;
using System.Collections;

public class LeafDropper : MonoBehaviour {

	private TreeManager treeManager;
	private AutumnController autumnController;
	private MyTree[] treeArray;

	public GameObject[] leaves;
	public GameObject[] piles;


	// Use this for initialization
	void Start () {
		treeManager = TreeManager.instance;
		treeArray = treeManager.treeArray;
		autumnController = FindObjectOfType<AutumnController> ();

		foreach (MyTree treeScript in treeArray) {
			GameObject tree = treeScript.gameObject;
			for (int i = 0; i < Random.Range (5, 15); i++) {
				Vector3 position = tree.transform.position + new Vector3 (Random.Range (-10, 10), 0, Random.Range (-10, 10));
				Quaternion rotation = Quaternion.Euler (-90, Random.Range (0, 359), 0);
				GameObject fallenLeaf = Instantiate (leaves [0], position, rotation, this.transform) as GameObject;
				fallenLeaf.GetComponent<Renderer> ().material = autumnController.materials [treeScript.leafMaterialX, treeScript.leafMaterialY];
			}

			for (int i = 0; i < Random.Range (0, 5); i++) {
				Vector3 position = tree.transform.position + new Vector3 (Random.Range (-10, 10), 0, Random.Range (-10, 10));
				Quaternion rotation = Quaternion.Euler (-90, Random.Range (0, 359), 0);
				GameObject fallenLeaf = Instantiate (leaves [0], position, rotation, this.transform) as GameObject;
				fallenLeaf.GetComponent<Renderer> ().material = autumnController.materials [treeScript.leafMaterialX - (2 * treeScript.leafMaterialSpread), treeScript.leafMaterialY - (2 * treeScript.leafMaterialSpread)];
			}

			for (int i = 0; i < Random.Range (0, 3); i++) {
				Vector3 position = tree.transform.position + new Vector3 (Random.Range (-10, 10), 0, Random.Range (-10, 10));
				Quaternion rotation = Quaternion.Euler (-90, Random.Range (0, 359), 0);
				GameObject leafPile = Instantiate (piles [Random.Range(0,piles.Length)], position, rotation, this.transform) as GameObject;
				leafPile.GetComponent<Renderer> ().material = autumnController.materials [treeScript.leafMaterialX, treeScript.leafMaterialY];
				LeafPile pileScript = leafPile.GetComponent<LeafPile> ();
				pileScript.leafIndexX = (treeScript.leafMaterialX - (2 * treeScript.leafMaterialSpread));
				pileScript.leafIndexY = (treeScript.leafMaterialY - (2 * treeScript.leafMaterialSpread));
			}
		}
	}
}
