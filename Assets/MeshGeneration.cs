using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGeneration : MonoBehaviour {

	public List<Vector3> newVertices = new List<Vector3>();
	public List<int> newTriangles = new List<int>();
	public List<Vector2> newUV = new List<Vector2>();

	public List<Vector3> colVertices = new List<Vector3>();
	public List<int> colTriangles = new List<int>();
	private int colCount;

	private MeshCollider col;

	public byte[,,] blocks;

	private Mesh mesh;

	private float tUnit = 0.25f;
	private Vector2 tStone = new Vector2(0, 0);
	private Vector2 tGrass = new Vector2(0, 1);

	private int faceCount;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<MeshCollider>();

		GenTerrain();
		BuildMesh ();
		UpdateMesh();
	}

	void GenSquare(int x, int y, int z, Vector2 texture) {
		if (Block(x,y+1,z)==0)
			CubeTop(x,y,z);

		if (Block(x,y-1,z)==0)
			CubeBot(x,y,z);

		if (Block(x,y,z-1)==0)
			CubeFront(x,y,z);

		if (Block(x,y,z+1)==0)
			CubeBack(x,y,z);

		if (Block(x+1,y,z)==0)
			CubeRight(x,y,z);

		if (Block(x-1,y,z)==0)
			CubeLeft(x,y,z);
	}

	void CubeTop(int x, int y, int z) {
		newVertices.Add( new Vector3(x, y, z+1));
		newVertices.Add( new Vector3(x+1, y, z+1));
		newVertices.Add( new Vector3(x+1, y, z));
		newVertices.Add( new Vector3(x, y, z));

		Vector2 texturePos;
		if (blocks[x,y,z]==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Cube(texturePos);
	}

	void CubeBot(int x, int y, int z) {
		newVertices.Add( new Vector3(x, y-1, z));
		newVertices.Add( new Vector3(x+1, y-1, z));
		newVertices.Add( new Vector3(x+1, y-1, z+1));
		newVertices.Add( new Vector3(x, y-1, z+1));

		Vector2 texturePos;
		if (blocks[x,y,z]==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Cube(texturePos);
	}

	void CubeFront(int x, int y, int z) {
		newVertices.Add( new Vector3(x, y, z));
		newVertices.Add( new Vector3(x+1, y, z));
		newVertices.Add( new Vector3(x+1, y-1, z));
		newVertices.Add( new Vector3(x, y-1, z));

		Vector2 texturePos;
		if (blocks[x,y,z]==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Cube(texturePos);
	}

	void CubeBack(int x, int y, int z) {
		newVertices.Add( new Vector3(x+1, y-1, z+1));
		newVertices.Add( new Vector3(x+1, y, z+1));
		newVertices.Add( new Vector3(x, y, z+1));
		newVertices.Add( new Vector3(x, y-1, z+1));

		Vector2 texturePos;
		if (blocks[x,y,z]==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Cube(texturePos);
	}

	void CubeRight(int x, int y, int z) {
		newVertices.Add( new Vector3(x+1, y-1, z));
		newVertices.Add( new Vector3(x+1, y, z));
		newVertices.Add( new Vector3(x+1, y, z+1));
		newVertices.Add( new Vector3(x+1, y-1, z+1));

		Vector2 texturePos;
		if (blocks[x,y,z]==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Cube(texturePos);
	}

	void CubeLeft(int x, int y, int z) {
		newVertices.Add( new Vector3(x, y-1, z+1));
		newVertices.Add( new Vector3(x, y, z+1));
		newVertices.Add( new Vector3(x, y, z));
		newVertices.Add( new Vector3(x, y-1, z));

		Vector2 texturePos;
		if (blocks[x,y,z]==1)
			texturePos=tStone;
		else
			texturePos=tGrass;

		Cube(texturePos);
	}

	void UpdateMesh() {

		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.Optimize();
		mesh.RecalculateNormals();

		Mesh newMesh = new Mesh();
		newMesh.vertices = colVertices.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.sharedMesh = newMesh;

		faceCount = 0;
		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();

		colVertices.Clear();
		colTriangles.Clear();
		colCount = 0;
	}

	void GenCollider(int x, int y, int z) {
		if (Block(x,y+1,z)==0)
			GenColTop(x,y,z);

		if (Block(x,y-1,z)==0)
			GenColBot(x,y,z);

		if (Block(x,y,z-1)==0)
			GenColFront(x,y,z);

		if (Block(x,y,z+1)==0)
			GenColBack(x,y,z);

		if (Block(x+1,y,z)==0)
			GenColRight(x,y,z);

		if (Block(x-1,y,z)==0)
			GenColLeft(x,y,z);


	}

	void GenColTop(int x, int y, int z) {
		colVertices.Add( new Vector3 (x, y, z+1));
		colVertices.Add( new Vector3 (x+1, y, z+1));
		colVertices.Add( new Vector3 (x+1, y, z));
		colVertices.Add( new Vector3 (x, y, z));

		ColTraingles();
	}

	void GenColBot(int x, int y, int z) {
		colVertices.Add( new Vector3 (x, y-1, z));
		colVertices.Add( new Vector3 (x+1, y-1, z));
		colVertices.Add( new Vector3 (x+1, y-1, z+1));
		colVertices.Add( new Vector3 (x, y-1, z+1));

		ColTraingles();
	}

	void GenColFront(int x, int y, int z) {
		colVertices.Add( new Vector3 (x, y, z));
		colVertices.Add( new Vector3 (x+1, y, z));
		colVertices.Add( new Vector3 (x+1, y-1, z));
		colVertices.Add( new Vector3 (x, y-1, z));

		ColTraingles();
	}

	void GenColBack(int x, int y, int z) {
		colVertices.Add( new Vector3 (x, y, z+1));
		colVertices.Add( new Vector3 (x+1, y, z+1));
		colVertices.Add( new Vector3 (x+1, y-1, z+1));
		colVertices.Add( new Vector3 (x, y-1, z+1));
		
		ColTraingles();
	}

	void GenColLeft(int x, int y, int z) {
		colVertices.Add( new Vector3 (x, y-1, z+1));
		colVertices.Add( new Vector3 (x, y, z+1));
		colVertices.Add( new Vector3 (x, y, z));
		colVertices.Add( new Vector3 (x, y-1, z));

		ColTraingles();
	}
	
	void GenColRight(int x, int y, int z) {
		colVertices.Add( new Vector3 (x+1, y, z+1));
		colVertices.Add( new Vector3 (x+1, y-1, z+1));
		colVertices.Add( new Vector3 (x+1, y-1, z));
		colVertices.Add( new Vector3 (x+1, y, z));
		
		ColTraingles();
	}
	
	void ColTraingles() {
		colTriangles.Add(colCount*4);
		colTriangles.Add(colCount*4 + 1);
		colTriangles.Add(colCount*4 + 3);
		colTriangles.Add(colCount*4 + 1);
		colTriangles.Add(colCount*4 + 2);
		colTriangles.Add(colCount*4 + 3);

		colCount++;
	}

	void Cube(Vector2 texture) {
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
	
	void GenTerrain() {
		blocks = new byte[16,16,16];
		//Noise gen:
		for (int px = 0; px<blocks.GetLength(0); px++) {
			for (int pz = 0; pz<blocks.GetLength(2); pz++) {
				int stone = Noise(px,pz,80,8,1);
				stone += Noise (px,pz,25,10,1);
				stone += 3;

				int dirt = Noise (px,pz,50,15,1);
				dirt += Noise(px,pz,25,10,1);
				dirt += 3;

				for (int py = 0; py<blocks.GetLength(1);py++) {
					if (py<stone){
						blocks[px,py,pz] = 1;

//						//dirt sports
//						if (Noise (px,py,12,16,1)>4) {
//							blocks[px,py,pz]=2;
//						}

//						//caves
//						if (Noise (px,py*2,16,14,1)>4) {
//							blocks[px,py,pz] = 0;
//						}

					}
					else if (py<dirt) {
						blocks[px,py,pz] = 2;
					}
				}
			}
		}
	}

	void BuildMesh() {
		for (int px = 0; px<blocks.GetLength(0); px++) {
			for (int py = 0; py<blocks.GetLength(1); py++) {
				for (int pz = 0; pz<blocks.GetLength(2); pz++) {
					if (blocks[px,py,pz] != 0)
						GenCollider(px,py,pz);

					if (blocks[px,py,pz] == 1)
						GenSquare(px, py, pz, tStone);
					else if (blocks[px,py,pz] == 2)
						GenSquare(px, py, pz, tGrass);
				}
			}
		}
	}

	byte Block(int x, int y, int z) {
		if (x == -1 || x == blocks.GetLength(0) ||
		    y == -1 || y == blocks.GetLength(1) ||
		    z == -1 || z == blocks.GetLength(2) )
			return (byte)0;
		else
			return blocks[x,y,z];
	}

	int Noise(int x, int y, float scale, float mag, float exp) {
		return (int) ( Mathf.Pow( (Mathf.PerlinNoise(x/scale, y/scale)*mag), exp ) );
	}
}
