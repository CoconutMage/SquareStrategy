using UnityEngine;

public class Map : MonoBehaviour
{
	public static Map Instance { get; private set; }

	public int xSize, ySize;
	public int chunkIndex;

	public GameObject chunkPrefab;

	MapData data;
	Chunk chunkScript;

	private void Start()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = MapData.Instance;

		Generate();
	}

	void Generate()
	{
		float height = 1;
		float offsetSide = (height / Mathf.Tan(60 * Mathf.Deg2Rad));
		float offsetEdge = (height * 0.5f) * Mathf.Tan(30 * Mathf.Deg2Rad);
		int chunkSize = 10;

		for (int y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, chunkIndex++)
			{
				Vector2 chunkPos = new Vector2(x * chunkSize * (offsetSide + offsetEdge), y * chunkSize * height);
				data.PopulateChunkData(chunkIndex, chunkPos);

				//Physical Characteristics
				GameObject chunk = Instantiate(chunkPrefab);
				chunk.transform.parent = transform;
				chunk.transform.localPosition = chunkPos;

				//Data Management
				chunkScript = chunk.GetComponent<Chunk>();
				chunkScript.chunkIndex = chunkIndex;
				chunkScript.chunkPosition = chunkPos;
			}
		}
	}
}
