using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour {

	public GameObject worldGO;
	private World world;

	public int chunkX;
	public int chunkY;
	public int chunkZ;

	public List<Vector3> newVertices = new List<Vector3>();
	public List<int> newTriangles = new List<int>();
	public List<Vector2> newUV = new List<Vector2>();

	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();
	private int colCount;

	private MeshCollider col;
	
	public int chunkSize = 16;

	private Mesh mesh;

	private float tUnit = 0.25f;
	private Vector2 tStone = new Vector2(0, 0);
	private Vector2 tGrassTop = new Vector2(1, 1);
	private Vector2 tGrass = new Vector2(0, 1);

	private int faceCount;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();
		world = worldGO.GetComponent<World>();

		GenerateMesh();
	}

	void GenerateMesh() {
		for (int x=0; x<chunkSize; x++) {
			for (int y=0; y<chunkSize; y++) {
				for (int z=0; z<chunkSize; z++) {

					if (Block(x,y,z)!=0) {

						if (Block(x,y+1,z)==0)
							CubeTop(x,y,z,world.Block(x,y,z));

						if (Block(x,y-1,z)==0)
							CubeBot(x,y,z,world.Block(x,y,z));

						if (Block(x,y,z-1)==0)
							CubeFront(x,y,z,world.Block(x,y,z));

						if (Block(x,y,z+1)==0)
							CubeBack(x,y,z,world.Block(x,y,z));

						if (Block(x+1,y,z)==0)
							CubeRight(x,y,z,world.Block(x,y,z));

						if (Block(x-1,y,z)==0)
							CubeLeft(x,y,z,world.Block(x,y,z));
					}
				}
			}
		}

		UpdateMesh ();
	}

	void CubeTop(int x, int y, int z, byte block) {
		newVertices.Add( new Vector3(x, y, z+1));
		newVertices.Add( new Vector3(x+1, y, z+1));
		newVertices.Add( new Vector3(x+1, y, z));
		newVertices.Add( new Vector3(x, y, z));

		Vector2 texturePos;
		if (block==1)
			texturePos=tStone;
		else
			texturePos=tGrassTop;

		Quad(texturePos);
	}

	void CubeBot(int x, int y, int z, byte block) {
		newVertices.Add( new Vector3(x, y-1, z));
		newVertices.Add( new Vector3(x+1, y-1, z));
		newVertices.Add( new Vector3(x+1, y-1, z+1));
		newVertices.Add( new Vector3(x, y-1, z+1));

		Vector2 texturePos;
		if (block==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Quad(texturePos);
	}

	void CubeFront(int x, int y, int z, byte block) {
		newVertices.Add( new Vector3(x, y, z));
		newVertices.Add( new Vector3(x+1, y, z));
		newVertices.Add( new Vector3(x+1, y-1, z));
		newVertices.Add( new Vector3(x, y-1, z));

		Vector2 texturePos;
		if (block==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Quad(texturePos);
	}

	void CubeBack(int x, int y, int z, byte block) {
		newVertices.Add( new Vector3(x+1, y-1, z+1));
		newVertices.Add( new Vector3(x+1, y, z+1));
		newVertices.Add( new Vector3(x, y, z+1));
		newVertices.Add( new Vector3(x, y-1, z+1));

		Vector2 texturePos;
		if (block==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Quad(texturePos);
	}

	void CubeRight(int x, int y, int z, byte block) {
		newVertices.Add( new Vector3(x+1, y-1, z));
		newVertices.Add( new Vector3(x+1, y, z));
		newVertices.Add( new Vector3(x+1, y, z+1));
		newVertices.Add( new Vector3(x+1, y-1, z+1));

		Vector2 texturePos;
		if (block==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Quad(texturePos);
	}

	void CubeLeft(int x, int y, int z, byte block) {
		newVertices.Add( new Vector3(x, y-1, z+1));
		newVertices.Add( new Vector3(x, y, z+1));
		newVertices.Add( new Vector3(x, y, z));
		newVertices.Add( new Vector3(x, y-1, z));

		Vector2 texturePos;
		if (block==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Quad(texturePos);
	}

	void UpdateMesh() {

		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();

		col.sharedMesh=mesh;

		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();

		faceCount = 0;
	}

	void Quad(Vector2 texture) {
		newTriangles.Add(faceCount*4);
		newTriangles.Add(faceCount*4 + 1);
		newTriangles.Add(faceCount*4 + 3);
		newTriangles.Add(faceCount*4 + 1);
		newTriangles.Add(faceCount*4 + 2);
		newTriangles.Add(faceCount*4 + 3);

		newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
		newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

		faceCount++;
	}

	byte Block(int x, int y, int z) {
		return world.Block (x + chunkX, y + chunkY, z + chunkZ);
	}

	int Noise(int x, int y, float scale, float mag, float exp) {
		return (int) ( Mathf.Pow( (Mathf.PerlinNoise(x/scale, y/scale)*mag), exp ) );
	}
}