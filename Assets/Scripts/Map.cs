using UnityEngine;

public class Map : MonoBehaviour
{
	public static Map Instance { get; private set; }

	public int xSize, ySize;
	public int chunkIndex;

	public GameObject chunkPrefab;

	MapData data;
	Chunk chunkScript;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		data = MapData.Instance;
	}

	private void Start()
	{
		Generate();
	}

	void Generate()
	{
		for (int y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize; x++, chunkIndex++)
			{
				Vector2 chunkPos = new Vector2(x * 10 + x, y * 10 + y);
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
