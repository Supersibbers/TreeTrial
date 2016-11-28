using UnityEngine;
using System.Collections;

public class MyTree : MonoBehaviour {

	//each tree will store its own coordinates adjusted to the standard noise map size, so that any tree can take advantage of its own noisemap location. 
	public int treeXScaledToSizeOfNoiseMap; //an int between -1 and perlinSize, not inclusive
	public int treeZScaledToSizeOfNoiseMap; //an int between -1 and perlinSize, not inclusive

	public int leafMaterialX;
	public int leafMaterialY;
	public int leafMaterialSpread;

	void Start () {
		
		treeXScaledToSizeOfNoiseMap = (int)Mathf.Floor(((gameObject.transform.position.x - TreeManager.instance.xMin )/TreeManager.instance.xSize)*NoiseMap.perlinSize-1);
		treeXScaledToSizeOfNoiseMap = Mathf.Clamp (treeXScaledToSizeOfNoiseMap, 0, NoiseMap.perlinSize -1);

		treeZScaledToSizeOfNoiseMap = (int)Mathf.Floor(((gameObject.transform.position.z - TreeManager.instance.zMin )/TreeManager.instance.zSize)*NoiseMap.perlinSize-1);
		treeZScaledToSizeOfNoiseMap = Mathf.Clamp (treeZScaledToSizeOfNoiseMap, 0, NoiseMap.perlinSize -1);

	}

}
