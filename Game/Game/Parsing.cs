using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Assimp;
using AssimpMesh = Assimp.Mesh;
using System.Numerics;

namespace Game
{
	public struct Vertex
	{
		public OpenTK.Mathematics.Vector3 Position;
		public OpenTK.Mathematics.Vector3 Normal;
		public OpenTK.Mathematics.Vector2 TexCoords;
	}
	public class Mesh
	{
		//MESH DATA
		readonly Vertex[] vertices;
		readonly int[] indices;
		public Mesh(Vertex[] vertices, int[] indices) 
		{
			this.vertices = vertices;
			this.indices = indices;

			SetupMesh();
		}
		//RENDER DATA
		private int VAO;
		private void SetupMesh()
		{
			VAO = GL.GenVertexArray();
			int VBO = GL.GenBuffer();
			int EBO = GL.GenBuffer();

			GL.BindVertexArray(VAO);
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 8 * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

			//Vertex Positions
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
			//Vertex Normals
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
			//Vertex Texture Coords
			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

			GL.BindVertexArray(0);
		}
		public void Draw()
		{
			// draw mesh
			GL.BindVertexArray(VAO);
			GL.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
			GL.BindVertexArray(0);
		}
	}
	public class Model
	{
		// model data
		private List<Mesh> meshes;

		// constructor, expects a filepath to a 3D model.
		public Model(string path)
		{
			LoadModel(path);
		}
		public void Draw()
		{
			for (int i = 0; i < meshes.Count; i++)
				meshes[i].Draw();
		}

		private void LoadModel(string path)
		{
			//Create a new importer
			AssimpContext importer = new AssimpContext();

			//This is how we add a logging callback
			LogStream logstream = new LogStream(delegate (String msg, String userData)
			{
				Console.WriteLine(msg);
			});
			logstream.Attach();

			//Import the model. All configs are set. The model
			//is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
			Scene scene = importer.ImportFile(path, PostProcessSteps.Triangulate);

			// check for errors
			if (scene == null || scene.SceneFlags.HasFlag(SceneFlags.Incomplete) || scene.RootNode == null)
			{
				Console.WriteLine("Unable to load model from: " + path);
				return;
			}

			//Reset the meshes and textures
			meshes = new List<Mesh>();

			//Set the scale of the model
			float scale = 1 / 200.0f;
			Assimp.Matrix4x4 scalingMatrix = new Assimp.Matrix4x4(scale, 0, 0, 0, 0, scale, 0, 0, 0, 0, scale, 0, 0, 0, 0, 1);

			// process ASSIMP's root node recursively. We pass in the scaling matrix as the first transform
			ProcessNode(scene.RootNode, scene, scalingMatrix);

			importer.Dispose();
		}

		private void ProcessNode(Node node, Scene scene, Assimp.Matrix4x4 parentTransform)
		{
			//Multiply the transform of each node by the node of the parent, this will place the meshes in the correct relative location
			Assimp.Matrix4x4 transform = node.Transform * parentTransform;

			// process each mesh located at the current node
			for (int i = 0; i < node.MeshCount; i++)
			{
				// the node object only contains indices to index the actual objects in the scene.
				// the scene contains all the data, node is just to keep stuff organized (like relations between nodes).
				AssimpMesh mesh = scene.Meshes[node.MeshIndices[i]];
				meshes.Add(ProcessMesh(mesh, transform));
			}

			for (int i = 0; i < node.ChildCount; i++)
			{
				ProcessNode(node.Children[i], scene, transform);
			}
		}

		private Mesh ProcessMesh(AssimpMesh mesh, Assimp.Matrix4x4 transform)
		{
			List<Vertex> vertices = new List<Vertex>();
			List<int> indices = new List<int>();

			for (int i = 0; i < mesh.VertexCount; i++)
			{
				Vertex vertex = new Vertex();
				// process vertex positions, normals and texture coordinates
				var transformedVertex = transform * mesh.Vertices[i];
				OpenTK.Mathematics.Vector3 vector;
				vector.X = transformedVertex.X;
				vector.Y = transformedVertex.Y;
				vector.Z = transformedVertex.Z;
				vertex.Position = vector;

				vertices.Add(vertex);

				if (mesh.HasNormals)
				{
					var transformedNormal = transform * mesh.Normals[i];
					vector.X = transformedNormal.X;
					vector.Y = transformedNormal.Y;
					vector.Z = transformedNormal.Z;
					vertex.Normal = vector;
				}

				if (mesh.HasTextureCoords(0)) // does the mesh contain texture coordinates?
				{
					OpenTK.Mathematics.Vector2 vec;
					vec.X = mesh.TextureCoordinateChannels[0][i].X;
					vec.Y = mesh.TextureCoordinateChannels[0][i].Y;
					vertex.TexCoords = vec;
				}
				else vertex.TexCoords = new OpenTK.Mathematics.Vector2(0.0f, 0.0f);

				vertices.Add(vertex);
			}
			
			// process indices
			for (int i = 0; i < mesh.FaceCount; i++)
			{
				Face face = mesh.Faces[i];
				for (int j = 0; j < face.IndexCount; j++)
					indices.Add(face.Indices[j]);
			}


			return new Mesh(vertices.ToArray(), indices.ToArray());
		}

	}
}
