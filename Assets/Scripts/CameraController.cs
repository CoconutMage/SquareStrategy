using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

	Data data;

	[SerializeField]
	float cameraZoom = 10f;

	public bool canZoom;

	public Vector3 difference;

	float startingCameraSize;

	UI ui;

	public GameObject map;
	Vector3 offset;

	public GameObject selectedTileIndicator;
	public GameObject selectedTilePrefab;

	void Start()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = Data.Instance;
		
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
			SetSelectedTileIndicator(currentTile);
		}
	}
	public void SetSelectedTileIndicator(MeshInteractionResult currentTile)
	{
		if (selectedTileIndicator != null) Destroy(selectedTileIndicator);
		selectedTileIndicator = Instantiate(selectedTilePrefab, currentTile.meshRenderer.transform);
		Debug.Log("Tile Clicked: " + currentTile.tileIndex + ", ChunkIndex: " + currentTile.chunkIndex + ", TileIndex: " + currentTile.tileIndex + 
			", Pos: " + ((selectedTileIndicator.GetComponent<SelectedTileUI>().height * 0.5f) * currentTile.tileIndex % 2) + "\n" +
			"Country: " + currentTile.tile.parentCountry.name + ", City: " + currentTile.tile.city.name
		);

		if (map.GetComponent<Map>() != null)
		{
			selectedTileIndicator.transform.localPosition =
					new Vector3((currentTile.tileIndex % map.GetComponent<Map>().chunkXSize) *
					(selectedTileIndicator.GetComponent<SelectedTileUI>().offsetSide +
					selectedTileIndicator.GetComponent<SelectedTileUI>().offsetEdge),

					Mathf.FloorToInt(currentTile.tileIndex / map.GetComponent<Map>().chunkXSize) +
					((selectedTileIndicator.GetComponent<SelectedTileUI>().height * 0.5f) *
					(currentTile.tileIndex % 2)), -1);
		}
		else
		{
			selectedTileIndicator.transform.localPosition =
				new Vector3((currentTile.tileIndex % map.GetComponent<MapEditorGeneration>().chunkXSize) *
				(selectedTileIndicator.GetComponent<SelectedTileUI>().offsetSide +
				selectedTileIndicator.GetComponent<SelectedTileUI>().offsetEdge),

				Mathf.FloorToInt(currentTile.tileIndex / map.GetComponent<MapEditorGeneration>().chunkXSize) +
				((selectedTileIndicator.GetComponent<SelectedTileUI>().height * 0.5f) *
				(currentTile.tileIndex % 2)), -1);
		}
	}

	MeshInteractionResult currentTile;
	void RightClick()
	{
		if (Input.GetMouseButtonDown(1))
		{
			currentTile = InteractWithMesh(data);
			if (currentTile.mesh == null) return;
			SetSelectedTileIndicator(currentTile);

			if (data.mapEditorScene != true)
			{
				ui.PoliticalPanelHandler(currentTile.tile.parentCountry);
				ui.DisplayCountryAndCityInfo(currentTile.tile);
			}

			//ui.DisplayUnits(currentTile.tile.landUnits, currentTile.tile.airUnits);
			//ui.DisplayResources(currentTile.tile.resources);
			//ui.DisplayCity(currentTile.tile.city);
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

		/*float oneX = 18f, twoX = 1f, threeX = 18f, fourX = 53f, fiveX = 70.5f, sixX = 53f;
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

		tileToSet.mesh.uv = tempMeshUVs;*/

		if (tileType.Equals("Empty"))
		{
			int[] triangles = new int[12];

			tileToSet.mesh.SetTriangles(triangles, 1);

			tileToSet.tile.tileType = tileType;
			data.map[tileToSet.chunkIndex].tiles[tileToSet.tileIndex] = tileToSet.tile;
		}
		else
		{
			int[] triangles = new int[12];
			int ti = 0;
			int i = tileToSet.tileIndex * 6;

			triangles[ti] = i;
			triangles[ti + 1] = i + 1;
			triangles[ti + 2] = i + 2;

			triangles[ti + 3] = i;
			triangles[ti + 4] = i + 2;
			triangles[ti + 5] = i + 5;

			triangles[ti + 6] = i + 2;
			triangles[ti + 7] = i + 4;
			triangles[ti + 8] = i + 5;

			triangles[ti + 9] = i + 2;
			triangles[ti + 10] = i + 3;
			triangles[ti + 11] = i + 4;

			tileToSet.mesh.SetTriangles(triangles, 1);

			tileToSet.tile.tileType = tileType;
			data.map[tileToSet.chunkIndex].tiles[tileToSet.tileIndex] = tileToSet.tile;
		}
	}
	/*private void SetTriangleHighlight(MeshInteractionResult tileToSet, string tileType)
	{
		MeshRenderer targetRenderer = tileToSet.meshRenderer;
		Color highlightColor = Color.yellow;
		MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		if (targetRenderer == null)
			return;

		propertyBlock.SetColor("_HighlightColor", highlightColor);

		targetRenderer.SetPropertyBlock(propertyBlock, index1);
		targetRenderer.SetPropertyBlock(propertyBlock, index2);
		targetRenderer.SetPropertyBlock(propertyBlock, index3);

		targetRenderer.SetPropertyBlock(propertyBlock, tileToSet.tileIndex);
		targetRenderer.SetPropertyBlock(propertyBlock, tileToSet.tileIndex + 1);
		targetRenderer.SetPropertyBlock(propertyBlock, tileToSet.tileIndex + 2);
		targetRenderer.SetPropertyBlock(propertyBlock, tileToSet.tileIndex + 3);
	}*/

	public struct MeshInteractionResult
	{
		public RaycastHit hit;
		public Mesh mesh;
		public MeshRenderer meshRenderer;
		public Vector2[] meshUVs;
		public int[] triangles;
		public int triangleIndex;
		public int uvIndex;
		public Vector2 hitUVCoords;
		public int tileIndex;
		public int chunkIndex;
		public Data.Tile tile;
		public Chunk chunk;
	}

	public static MeshInteractionResult InteractWithMesh(Data data)
	{
		Mesh mesh = null;
		MeshRenderer meshRenderer = null;
		Vector2[] meshUVs = null;
		int[] triangles = null;
		int triangleIndex = -1;
		int uvIndex = -1;
		Vector2 hitUVCoords = Vector2.zero;
		int tileIndex = -1;
		int chunkIndex = -1;
		Data.Tile tile = data.nullTile;
		Chunk chunk = null;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider != null && meshCollider.sharedMesh != null)
			{
				mesh = meshCollider.sharedMesh;
				meshRenderer = meshCollider.gameObject.GetComponent<MeshRenderer>();
				meshUVs = mesh.uv;

				triangles = mesh.triangles;
				triangleIndex = hit.triangleIndex;
				int numTriangles = 30;

				tileIndex = triangleIndex / numTriangles;
				chunk = hit.transform.gameObject.GetComponent<Chunk>();
				if(chunk != null) chunkIndex = chunk.chunkIndex;
				else chunkIndex = hit.transform.gameObject.GetComponent<MapEditorChunk>().chunkIndex;

				Dictionary<int, Data.Tile> tiles = data.map[chunkIndex].tiles;
				tile = tiles[tileIndex];
			}
		}

		MeshInteractionResult result = new MeshInteractionResult
		{
			hit = hit,
			mesh = mesh,
			meshRenderer = meshRenderer,
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
