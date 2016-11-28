using UnityEngine;
using System.Collections;

public class NoiseMap {

	public static int perlinSize = 100; 

	public float[,] map;

	//This class generates perlin noise maps that can be instantiated and have their map property accessed. They need to be fed a Scale attribute in their construction. They're always going to be that same square shape.
	public NoiseMap (float scale) {
		map = new float[perlinSize, perlinSize];

		int randomSeed = Random.Range (0, 600);

		for (int i = 0; i < perlinSize; i++) {
			for (int j = 0; j < perlinSize; j++) {
				map [i, j] = Mathf.PerlinNoise (i / scale + randomSeed, j / scale + randomSeed);
			}
		}
	}
}
