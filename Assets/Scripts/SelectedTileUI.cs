using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTileUI : MonoBehaviour
{
	public float height = 1;
	//The length of one flat side
	public float offsetSide;
	//The x length of the angled side on the left and right edges
	public float offsetEdge;
	private void Awake()
	{
		offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);
	}
	void Start()
	{
		RenderGraphic();
	}

	//private Vector3[] vertices;
	private List<Vector3> vertices;
	private Mesh mesh;
	private void RenderGraphic()
	{
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();

		//mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		mesh.name = "Chunk";
		mesh.Clear();

		//vertices = new Vector3[((xSize + 1) * (ySize + 1)) * 6];
		vertices = new List<Vector3>();
		//Vector2[] uv = new Vector2[vertices.Length];
		//Vector2[] uv2 = new Vector2[vertices.Length];
		List<int> triangles = new List<int>();
		List<Color> colors = new List<Color>();

		float y = 0;
		float x = 0;

		vertices.Add(new Vector3(x, y));

		vertices.Add(new Vector3(x - (offsetSide / 2), y - (height / 2)));
		vertices.Add(new Vector3(x - offsetEdge - (offsetSide / 2), y));
		vertices.Add(new Vector3(x - (offsetSide / 2), y + (height / 2)));

		vertices.Add(new Vector3(x + (offsetSide / 2), y + (height / 2)));
		vertices.Add(new Vector3(x + offsetEdge + (offsetSide / 2), y));
		vertices.Add(new Vector3(x + (offsetSide / 2), y - (height / 2)));

		triangles.Add(0);
		triangles.Add(1);
		triangles.Add(2);

		triangles.Add(0);
		triangles.Add(2);
		triangles.Add(3);

		triangles.Add(0);
		triangles.Add(3);
		triangles.Add(4);

		triangles.Add(0);
		triangles.Add(4);
		triangles.Add(5);

		triangles.Add(0);
		triangles.Add(5);
		triangles.Add(6);

		triangles.Add(0);
		triangles.Add(6);
		triangles.Add(1);

		mesh.vertices = vertices.ToArray();
		mesh.colors = colors.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
	}
}
