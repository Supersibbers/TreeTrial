using UnityEngine;
using System.Collections;

//grabs every object tagged Tree and applies colours to them
//needs to have perlin-noise-generation and calculations about trees refactored out
//ideally just grabs a list of trees and sets their colours using their own coordinates and a pair of noise maps that it will generate
//and ultimately will colorise the layers of the tree's foliage, ideally without having to know what kind of model it's looking at

public class AutumnController : MonoBehaviour {

	public int materialArrayXLargestIndex;
	public int materialArrayYLargestIndex;

	public float scale;
	private NoiseMap map1, map2, map3;

	private Color color1, color2, color3, color4;
	public Material material;

	public int spreadWidth;

	public Material[,] materials;

	void Start () {
		materials = new Material[materialArrayXLargestIndex+1,materialArrayYLargestIndex+1];

		color3 = Color.yellow;
		color2 = new Color (0, 0.2f, 0);
		color4 = Color.red;
		color1 = new Color (0.25f, 0.1f, 0);

		map1 = new NoiseMap (scale); // describes position on one axis of mat array
		map2 = new NoiseMap (scale); // describes position on other axis of mat array
		map3 = new NoiseMap (scale); // describes how spread apart the three leaf materials on a given tree are

		InitialiseMaterialArray ();
		ColorizeLeavesAccordingToPerlinNoise();
//		MoveTreesAround ();
	}

	void InitialiseMaterialArray ()
	{
		materials [0, 0] = new Material (material);
		materials [0, 0].color = color1;
		materials [0, materialArrayYLargestIndex] = new Material (material);
		materials [0, materialArrayYLargestIndex].color = color2;
		materials [materialArrayXLargestIndex, materialArrayYLargestIndex] = new Material (material);
		materials [materialArrayXLargestIndex, materialArrayYLargestIndex].color = color3;
		materials [materialArrayXLargestIndex, 0] = new Material (material);
		materials [materialArrayXLargestIndex, 0].color = color4;

		int i;
		int j;

		for (i = 1; i < materialArrayXLargestIndex; i++) {
			materials [i, 0] = new Material (material);
			materials [i, 0].color = Color.Lerp (materials [0, 0].color, materials [materialArrayXLargestIndex, 0].color, (float) 1/materialArrayXLargestIndex * i);
		}

		for (i = 1; i < materialArrayXLargestIndex; i++) {
			materials [i, materialArrayYLargestIndex] = new Material (material);
			materials [i, materialArrayYLargestIndex].color = Color.Lerp (materials [0, materialArrayYLargestIndex].color, materials [materialArrayXLargestIndex, materialArrayYLargestIndex].color, (float) 1/materialArrayXLargestIndex * i);
		}

		for (i = 0; i < materialArrayXLargestIndex+1; i++) {
			for (j = 1; j < materialArrayYLargestIndex ; j++) {
				materials [i, j] = new Material (material);
				materials[i,j].color = Color.Lerp(materials[i,0].color, materials[i,materialArrayYLargestIndex].color, (float) 1/materialArrayYLargestIndex * j);
			}
		}
	}
		
	void ColorizeLeavesAccordingToPerlinNoise(){
		
		foreach (MyTree tree in TreeManager.instance.treeArray) {
			int materialArrayXPosition; // an int between 0 and MaterialArrayXLargestIndex inclusive
			int materialArrayYPosition; // ditto
			int materialArraySpread;//

			materialArrayXPosition = (int)(map1.map [tree.treeXScaledToSizeOfNoiseMap, tree.treeZScaledToSizeOfNoiseMap] * materialArrayXLargestIndex);
			materialArrayXPosition = Mathf.Clamp (materialArrayXPosition, 0, materialArrayXLargestIndex);

			materialArrayYPosition = (int)(map2.map [tree.treeXScaledToSizeOfNoiseMap, tree.treeZScaledToSizeOfNoiseMap] * materialArrayXLargestIndex);
			materialArrayYPosition = Mathf.Clamp (materialArrayYPosition, 0, materialArrayYLargestIndex);

			materialArraySpread = Mathf.FloorToInt(map3.map [tree.treeXScaledToSizeOfNoiseMap, tree.treeZScaledToSizeOfNoiseMap] * spreadWidth);
			materialArraySpread = Mathf.Clamp (materialArraySpread, -1, spreadWidth - 1);

			while (materialArrayXPosition - 2 * materialArraySpread < 1) {
				materialArraySpread--;
			}

			while (materialArrayYPosition - 2 * materialArraySpread < 1) {
				materialArraySpread--;
			}


//			Debug.Log (materialArraySpread);

			ColorTreeLeaves(tree.gameObject, materialArrayXPosition, materialArrayYPosition, materialArraySpread);
			}
		}
		
	void ColorTreeLeaves(GameObject tree, int x, int y, int spread){ //TODO make trunks cooler
		MyTree treeScript = tree.GetComponent<MyTree>();
		treeScript.leafMaterialX = x;
		treeScript.leafMaterialY = y;
		treeScript.leafMaterialSpread = spread;
		Renderer renderer = tree.GetComponent<Renderer> ();
//		Debug.Log (renderer);
//		Debug.Log (x + " + " + y);
		Material[] materialsInTree = renderer.materials;
//		Debug.Log (materialsInTree.Length);
		for (int i = 0; i < materialsInTree.Length; i++) {

//			Debug.Log (materialsInTree [i].name);

			if (materialsInTree [i].name == "Leaf1 (Instance)") {
				materialsInTree [i] = materials [x, y];
			}
			if (materialsInTree [i].name == "Leaf2 (Instance)") {
				materialsInTree[i] = materials [x-spread, y-spread];
			}
			if (materialsInTree [i].name == "Leaf3 (Instance)") {
				materialsInTree[i] = materials [x-(2*spread), y-(2*spread)];
			}
			if (materialsInTree [i].name == "Trunk (Instance)") {
				materialsInTree[i] = materials [0,0];
			}

			if (materialsInTree [i].name == "Leaf1") {
				materialsInTree [i] = materials [x, y];
			}
			if (materialsInTree [i].name == "Leaf2") {
				materialsInTree[i] = materials [x-spread, y-spread];
			}
			if (materialsInTree [i].name == "Leaf3") {
				materialsInTree[i] = materials [x-(2*spread), y-(2*spread)];
			}
			if (materialsInTree [i].name == "Trunk") {
				materialsInTree[i] = materials [0,0];
			}
		}
		renderer.materials = materialsInTree;
	}
}

//	void MoveTreesAround() {
//
//		float xSize = 0;
//		float zSize = 0;
//
//		float xMin = 0;
//		float zMin = 0;
//		float xMax = 0;
//		float zMax = 0;
//
//		foreach (GameObject tree in trees) {
//			if (tree.transform.localPosition.x > xMax) {xMax = tree.transform.localPosition.x;};
//			if (tree.transform.localPosition.z > zMax) {zMax = tree.transform.localPosition.z;};
//			if (tree.transform.localPosition.x < xMin) {xMin = tree.transform.localPosition.x;};
//			if (tree.transform.localPosition.z < zMin) {zMin = tree.transform.localPosition.z;};
//		}
//
//		xSize = xMax - xMin;
//		zSize = zMax - zMin;
//
//			foreach (GameObject tree in trees) {
//				int perledValue; // an int between 0 and 100 for moving trees about
//				int adjustedTreeX; //an int between -1 and perlinSize, not inclusive
//				int adjustedTreeY; //an int between -1 and perlinSize, not inclusive
//
//				adjustedTreeX = (int)Mathf.Floor(((tree.transform.position.x - xMin )/xSize)*perlinSize-1);
//				adjustedTreeY = (int)Mathf.Floor(((tree.transform.position.z - zMin )/zSize)*perlinSize-1);
//
//				adjustedTreeX = Mathf.Clamp (adjustedTreeX, 0, perlinSize);
//				adjustedTreeY = Mathf.Clamp (adjustedTreeY, 0, perlinSize);
//
//				perledValue = (int)(perlin3 [adjustedTreeX, adjustedTreeY] * 100);
//				perledValue = Mathf.Clamp (perledValue, 0, perledValue - 1);
//
//				TweakTree(tree, perledValue);
//			}
//		}

//	void TweakTree(GameObject tree, int x){
//		Debug.Log (x);
//		float newY = tree.transform.position.y + (3*x);
//		Vector3 newPosition = new Vector3 (tree.transform.position.x, newY, tree.transform.position.z);
//		tree.transform.position = newPosition;
//	}

