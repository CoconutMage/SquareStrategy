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
			if (currentTile.tile.tileType == "Stone")
			{
				SetTileUVs(currentTile, "Grass");
			}
			else if (currentTile.tile.tileType == "Grass" || currentTile.tile.tileType == "Selected_Tile")
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
			ui.DisplayCity(currentTile.tile.city);

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

	void SetTileUVs(MeshInteractionResult tileToSet, string tileType)
	{
		Vector2[] tempMeshUVs = tileToSet.mesh.uv;
		/*for (int zz = 0; zz < 4; zz++)
		{
			tempMeshUVs[tileToSet.uvIndex + zz] = data.TileMap[material][zz];
		}*/
		//Debug.Log("UV: " + tileToSet.uvIndex);
		/*tempMeshUVs[tileToSet.uvIndex] = new Vector2(.3828125f, .01171875f);
		tempMeshUVs[tileToSet.uvIndex + 1] = new Vector2(.3046875f, .1328125f);
		tempMeshUVs[tileToSet.uvIndex + 2] = new Vector2(.3828125f, .2578125f);
		tempMeshUVs[tileToSet.uvIndex + 3] = new Vector2(.50390625f, .2578125f);
		tempMeshUVs[tileToSet.uvIndex + 4] = new Vector2(.56203125f, .1328125f);
		tempMeshUVs[tileToSet.uvIndex + 5] = new Vector2(.50390625f, .01171875f);*/

		float oneX = 18f, twoX = 1f, threeX = 18f, fourX = 53f, fiveX = 70.5f, sixX = 53f;
		float oneY = 2f, twoY = 32f, threeY = 62f, fourY = 62f, fiveY = 32f, sixY = 2f;
		//float sizeX = 72, sizeY = 63;
		float sizeX = 288, sizeY = 256;
		//float modifierX = -0.5f, modifierY = 0.5f;
		float modifierX = -0.5f, modifierY = 0.5f;
		int textureIndexX = 1, textureIndexY = 1;
		modifierX -= 72;

		tempMeshUVs[tileToSet.uvIndex] = new Vector2(((oneX - modifierX) / sizeX) * textureIndexX, ((oneY - modifierY) / sizeY) * textureIndexY);
		tempMeshUVs[tileToSet.uvIndex + 1] = new Vector2(((twoX - modifierX) / sizeX) * textureIndexX, ((twoY - modifierY) / sizeY) * textureIndexY);
		tempMeshUVs[tileToSet.uvIndex + 2] = new Vector2(((threeX - modifierX) / sizeX) * textureIndexX, ((threeY - modifierY) / sizeY) * textureIndexY);
		tempMeshUVs[tileToSet.uvIndex + 3] = new Vector2(((fourX - modifierX) / sizeX) * textureIndexX, ((fourY - modifierY) / sizeY) * textureIndexY);
		tempMeshUVs[tileToSet.uvIndex + 4] = new Vector2(((fiveX - modifierX) / sizeX) * textureIndexX, ((fiveY - modifierY) / sizeY) * textureIndexY);
		tempMeshUVs[tileToSet.uvIndex + 5] = new Vector2(((sixX - modifierX) / sizeX) * textureIndexX, ((sixY - modifierY) / sizeY) * textureIndexY);

		tileToSet.mesh.uv = tempMeshUVs;

		tileToSet.tile.tileType = tileType;
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
				//Debug.Log("Triangle Index: " + triangleIndex);

				if (triangleIndex < triangles.Length / 3)
				{
					uvIndex = (triangleIndex / 4) * 6;//triangles[triangleIndex * 3];
					hitUVCoords = meshUVs[uvIndex];

					tileIndex = uvIndex / 6;
					//Debug.Log("Tile Index: " + tileIndex);
					//Debug.Log("UV Index: " + uvIndex);
					chunkIndex = hit.transform.gameObject.GetComponent<Chunk>().chunkIndex;

					Dictionary<int, MapData.Tile> tiles = data.Map[chunkIndex].tiles;
					tile = tiles[tileIndex];

					//Debug.Log("===========================================================================================");
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
