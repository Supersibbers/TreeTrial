using UnityEngine;
using System.Collections;

public class TreeManager : MonoBehaviour {

	// this class works out the xSize and zSize and stuff in its awake method and then everyone else can ask this class what that shit izzz.
	// uses a singleton patetrn

	public static TreeManager instance;

//	[HideInInspector]public AutumnController autumnController;

	[HideInInspector]public float xSize = 0;
	[HideInInspector]public float zSize = 0;
	[HideInInspector]public float xMin = 0;
	[HideInInspector]public float zMin = 0;
	[HideInInspector]public float xMax = 0;
	[HideInInspector]public float zMax = 0;

	[HideInInspector]public MyTree[] treeArray;

	public GameObject[] treePrefabs;

	void Awake () {
		instance = this;
//		autumnController = FindObjectOfType<AutumnController> ();
		PlaceSomeTrees (); // in the final project, this will be done manually, so it's important that this little proc-gen addon is self-contained (might be useful for a music scene, though)
		treeArray = FindObjectsOfType<MyTree>();
		FindSizeOfForest ();
	}

	void PlaceSomeTrees () { // see comment in Awake about what this is and how it works
		int xOrigin = -100;
		int zOrigin = -100;
		int width = 200;
		int height = 200;

		NoiseMap map = new NoiseMap (35);
		NoiseMap map1 = new NoiseMap (35);

		int treesPlacesSoFar = 0;
		int desiredTrees = 200;

		while (treesPlacesSoFar < desiredTrees) {
			float x = Random.Range (xOrigin, xOrigin + width);
			float z = Random.Range (zOrigin, zOrigin + height);

			int adjustedX = Mathf.FloorToInt(((x - xOrigin) / width) * NoiseMap.perlinSize-1);
			adjustedX = Mathf.Clamp (adjustedX, 0, NoiseMap.perlinSize-1);

			int adjustedZ = Mathf.FloorToInt(((z - zOrigin) / height) * NoiseMap.perlinSize-1);
			adjustedZ = Mathf.Clamp (adjustedZ, 0, NoiseMap.perlinSize-1);


			if (Random.value < map.map [adjustedX, adjustedZ]) {
				Vector3 specPosition = new Vector3 (x, 0, z);
				Quaternion rotation = Quaternion.Euler (-90 + Random.Range(-10, 10), Random.Range(0,360), 0);
				GameObject tree = GameObject.Instantiate (treePrefabs [Random.Range(0,treePrefabs.Length)], specPosition, rotation, gameObject.transform) as GameObject;
				float scale = tree.transform.localScale.x * (1 + (map1.map[adjustedX, adjustedZ]*2)) - 30;
				Vector3 desiredScale = new Vector3(scale, scale, scale);
				tree.transform.localScale = desiredScale;
				treesPlacesSoFar++;
			}
		}
	}

	void FindSizeOfForest(){
		foreach (MyTree tree in treeArray) {
			if (tree.transform.localPosition.x > xMax) {xMax = tree.transform.localPosition.x;};
			if (tree.transform.localPosition.z > zMax) {zMax = tree.transform.localPosition.z;};
			if (tree.transform.localPosition.x < xMin) {xMin = tree.transform.localPosition.x;};
			if (tree.transform.localPosition.z < zMin) {zMin = tree.transform.localPosition.z;};
		}
		xSize = xMax - xMin;
		zSize = zMax - zMin;
	}
}
