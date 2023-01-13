using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProceduralNoiseProject;
using MarchingCubesProject;
using TerrainGen;

public class Chunk
{
    public Transform parent;
    
    public int MeshSize = 16;
    public float meshScale = 5;

    public Material m_material;
    public int seed = 0;

    GameObject chunkMesh;

    private TerrainNoise tn = new TerrainNoise();

    public void GenerateMesh(Vector3 offset)
    {
        GameObject.Destroy(chunkMesh);

        Marching marching = new MarchingTertrahedron
        {
            //Surface is the value that represents the surface of mesh
            //For example the perlin noise has a range of -1 to 1 so the mid point is where we want the surface to cut through.
            //The target value does not have to be the mid point it can be any value with in the range.
            Surface = 0.0f
        };

        //The size of voxel array.
        int width = MeshSize + 1;
        int height = MeshSize + 1;
        int length = MeshSize + 1;

        float[] voxels = new float[width * height * length];

        //Fill voxels with values.
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int z = 0; z < length; z++)
                {
                    int idx = x + y * width + z * width * height;

                    float noise = tn.GetNoise(new Vector3(x, y, z) + (offset / meshScale));

                    voxels[idx] = noise;
                }
            }
        }

        List<Vector3> verts = new List<Vector3>();
        List<int> indices = new List<int>();

        //The mesh produced is not optimal. There is one vert for each index.
        //Would need to weld vertices for better quality mesh.
        marching.Generate(voxels, width, height, length, verts, indices);

        //A mesh in unity can only be made up of 65000 verts.
        //Need to split the verts between multiple meshes.

        int maxVertsPerMesh = 30000; //must be divisible by 3, ie 3 verts == 1 triangle
        int numMeshes = verts.Count / maxVertsPerMesh + 1;

        for(int i = 0; i < numMeshes; i++)
        {
            List<Vector3> splitVerts = new List<Vector3>();
            List<int> splitIndices = new List<int>();

            for(int j = 0; j < maxVertsPerMesh; j++)
            {
                int idx = i * maxVertsPerMesh + j;

                if(idx < verts.Count)
                {
                    splitVerts.Add(verts[idx]);
                    splitIndices.Add(j);
                }
            }

            if(splitVerts.Count == 0)
                continue;

            Mesh mesh = new Mesh();
            mesh.SetVertices(splitVerts);
            mesh.SetTriangles(splitIndices, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.Optimize();

            chunkMesh = new GameObject("Mesh");
            chunkMesh.transform.parent = parent;
            chunkMesh.transform.localScale = Vector3.one * meshScale;
            chunkMesh.AddComponent<MeshFilter>();
            chunkMesh.AddComponent<MeshRenderer>();
            chunkMesh.GetComponent<Renderer>().material = m_material;
            chunkMesh.GetComponent<MeshFilter>().mesh = mesh;
            chunkMesh.AddComponent<MeshCollider>();
            chunkMesh.transform.localPosition = new Vector3(-width / 2, -height / 2, -length / 2) + (offset);
        }
    }
}