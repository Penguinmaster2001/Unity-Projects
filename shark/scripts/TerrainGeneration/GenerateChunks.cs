using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class GenerateChunks : MonoBehaviour
{
    public int maxGenPerFrame;

    public float genDistance;
    public float meshScale;
    public int meshSize;
    public int scaleFactor;

    public Transform player;
    public Vector3 playerPos;
    public Vector3 lockedPlayerPos;
    public Vector3 oldLockedPlayerPos;

    public Material terrainMat;

    public List<Vector3> queuedChunks;
    private Dictionary<Vector3, Chunk> PositionChunkPairs = new Dictionary<Vector3, Chunk>();

    int genThisFrame;
    bool finishedGen;

    void Update()
    {
        scaleFactor = (int) (meshSize * meshScale);

        playerPos = player.position;

        //put player position onto grid
        oldLockedPlayerPos = lockedPlayerPos;
        lockedPlayerPos = LockToGridV3(playerPos, scaleFactor);
        
        QueueChunks();
        DoGenerateChunks();
    }

    void QueueChunks()
    {
        for(int x = 0;  x < genDistance; x++)
        {
            for(int y = 0; y < genDistance; y++)
            {
                for(int z = 0; z < genDistance; z++)
                {
                    Vector3 offset = lockedPlayerPos - (Vector3.one * ((genDistance * scaleFactor) / 2)) + LockToGridV3(new Vector3(x, y, z) * scaleFactor, scaleFactor);

                    if(!PositionChunkPairs.ContainsKey(offset) && !queuedChunks.Contains(offset))
                    {
                        queuedChunks.Add(offset);
                        //queuedChunks.Insert(0, offset);
                    }
                }
            }
        }
    }

    void DoGenerateChunks()
    {
        int iterations;
        if(queuedChunks.Count < maxGenPerFrame)
        {
            iterations = queuedChunks.Count;
        }
        else
        {
            iterations = maxGenPerFrame;
        }

        for(int i = 0; i < iterations; i++)
        {
            Vector3 newChunkPos = queuedChunks[i];
            
            PositionChunkPairs.Add(newChunkPos, new Chunk() { m_material = terrainMat, meshScale = this.meshScale, MeshSize = this.meshSize });
            PositionChunkPairs[newChunkPos].GenerateMesh(newChunkPos);

            queuedChunks.RemoveAt(i);
        }
    }

    int LockToGrid(int input, int gridSize)
    {
        return Mathf.RoundToInt(input / gridSize) * gridSize;
    }

    Vector3Int LockToGridV3(Vector3 input, int gridSize)
    {
        return new Vector3Int(LockToGrid((int) input.x, gridSize), LockToGrid((int) input.y, gridSize), LockToGrid((int) input.z, gridSize));
    }
}
