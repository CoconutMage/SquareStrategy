using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	MapData data;

	[SerializeField]
	float cameraZoom = 10f;

	public bool canZoom;

	public Vector3 difference;

	float startingCameraSize;

	UI ui;

	public GameObject map;
	Vector3 offset;


	void Start()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = MapData.Instance;
		ui = UI.Instance;

		startingCameraSize = Camera.main.orthographicSize;
		canZoom = true;
	}

	void Update()
	{
		//Camera Controls
		CameraZoom();
		CameraDrag();

		//Mouse Controls
		LeftClick();
		RightClick();

		//Keyboard Controls

	}

	void LeftClick()
	{
		if (Input.GetMouseButtonDown(0))
		{
			currentTile = InteractWithMesh(data);
			if (currentTile.tile.material == "Stone")
			{
				SetTileUVs(currentTile, "Grass");
			}
			else if (currentTile.tile.material == "Grass" || currentTile.tile.material == "Selected_Tile")
			{
				SetTileUVs(currentTile, "Stone");
			}
		}
	}

	MeshInteractionResult previousTile;
	MeshInteractionResult currentTile;
	void RightClick()
	{
		if (Input.GetMouseButtonDown(1))
		{
			currentTile = InteractWithMesh(data);
			if (currentTile.mesh == null) return;

			if (previousTile.mesh != null)
			{
				SetTileUVs(previousTile, "Stone");
			}
			ui.DisplayUnits(currentTile.tile.landUnits, currentTile.tile.airUnits);
			ui.DisplayResources(currentTile.tile.resources);

			SetTileUVs(currentTile, "Selected_Tile");
			previousTile = currentTile;

		}
	}

	void CameraDrag()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButton(2))
		{
			map.transform.position = mousePosition + offset;
		}
		else
		{
			offset = map.transform.position - mousePosition;
		}
	}

	void CameraZoom()
	{
		if (canZoom)
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				Camera.main.orthographicSize += (cameraZoom * (Camera.main.orthographicSize * 0.05f / startingCameraSize));
			}

			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if (Camera.main.orthographicSize - (cameraZoom * (Camera.main.orthographicSize * 0.05f / startingCameraSize)) <= 0.25f)
				{
					Camera.main.orthographicSize = 0.25f;
				}
				else
				{
					Camera.main.orthographicSize -= (cameraZoom * (Camera.main.orthographicSize * 0.05f / startingCameraSize));
				}
			}
		}
	}

	void SetTileUVs(MeshInteractionResult tileToSet, string material)
	{
		Vector2[] tempMeshUVs = tileToSet.mesh.uv;
		for (int zz = 0; zz < 4; zz++)
		{
			tempMeshUVs[tileToSet.uvIndex + zz] = data.TileMap[material][zz];
		}
		tileToSet.mesh.uv = tempMeshUVs;

		tileToSet.tile.material = material;
		data.Map[tileToSet.chunkIndex].tiles[tileToSet.tileIndex] = tileToSet.tile;
	}

	public struct MeshInteractionResult
	{
		public RaycastHit hit;
		public Mesh mesh;
		public Vector2[] meshUVs;
		public int[] triangles;
		public int triangleIndex;
		public int uvIndex;
		public Vector2 hitUVCoords;
		public int tileIndex;
		public int chunkIndex;
		public MapData.Tile tile;
	}

	public static MeshInteractionResult InteractWithMesh(MapData data)
	{
		Mesh mesh = null;
		Vector2[] meshUVs = null;
		int[] triangles = null;
		int triangleIndex = -1;
		int uvIndex = -1;
		Vector2 hitUVCoords = Vector2.zero;
		int tileIndex = -1;
		int chunkIndex = -1;
		MapData.Tile tile = data.nullTile;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider != null && meshCollider.sharedMesh != null)
			{
				mesh = meshCollider.sharedMesh;
				meshUVs = mesh.uv;
				triangles = mesh.triangles;
				triangleIndex = hit.triangleIndex;

				if (triangleIndex < triangles.Length / 3)
				{
					uvIndex = triangles[triangleIndex * 3];
					hitUVCoords = meshUVs[uvIndex];

					tileIndex = uvIndex / 4;
					chunkIndex = hit.transform.gameObject.GetComponent<Chunk>().chunkIndex;

					Dictionary<int, MapData.Tile> tiles = data.Map[chunkIndex].tiles;
					tile = tiles[tileIndex];
				}
			}
		}

		MeshInteractionResult result = new MeshInteractionResult
		{
			hit = hit,
			mesh = mesh,
			meshUVs = meshUVs,
			triangles = triangles,
			triangleIndex = triangleIndex,
			uvIndex = uvIndex,
			hitUVCoords = hitUVCoords,
			tileIndex = tileIndex,
			chunkIndex = chunkIndex,
			tile = tile
		};

		return result;
	}
}
