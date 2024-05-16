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

namespace Game
{
	public class Object
	{
		private List<Vector3> vertices = new List<Vector3>();
		private List<Vector3> normals = new List<Vector3>();
		private List<Vector2> texCoords = new List<Vector2>();
		private List<int> indices = new List<int>();

		private int vao, vbo, ebo;
		private int vertexCount;

		public Object(string fileName) {
			LoadObj(fileName);
			InitBuffers();
		}

		private void LoadObj(string fileName)
		{
			StreamReader reader = null;
			try
			{
				reader = new StreamReader("../../../Textures/" + fileName);
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.StartsWith("v "))
					{
						string[] tokens = line.Split(' ');
						float x = float.Parse(tokens[1], CultureInfo.GetCultureInfo("en-US"));
						float y = float.Parse(tokens[2], CultureInfo.GetCultureInfo("en-US"));
						float z = float.Parse(tokens[3], CultureInfo.GetCultureInfo("en-US"));
						vertices.Add(new Vector3(x, y, z));
					}
					if (line.StartsWith("vn "))
					{
						string[] tokens = line.Split(' ');
						float x = float.Parse(tokens[1], CultureInfo.GetCultureInfo("en-US"));
						float y = float.Parse(tokens[2], CultureInfo.GetCultureInfo("en-US"));
						float z = float.Parse(tokens[3], CultureInfo.GetCultureInfo("en-US"));
						normals.Add(new Vector3(x, y, z));
					}
					else if (line.StartsWith("vt "))
					{
						string[] tokens = line.Split(' ');
						float u = float.Parse(tokens[1], CultureInfo.GetCultureInfo("en-US"));
						float v = float.Parse(tokens[2], CultureInfo.GetCultureInfo("en-US"));
						texCoords.Add(new Vector2(u, v));
					}
					else if (line.StartsWith("f "))
					{
						string[] tokens = line.Split(' ');
						for (int i = 1; i < tokens.Length; i++)
						{
							string[] parts = tokens[i].Split('/');
							int vi = int.Parse(parts[0], CultureInfo.GetCultureInfo("en-US")) -1;
							int ti = int.Parse(parts[1], CultureInfo.GetCultureInfo("en-US")) - 1;
							int ni = int.Parse(parts[2], CultureInfo.GetCultureInfo("en-US")) -1;
							vertices.Add(vertices[vi]);
							texCoords.Add(texCoords[ti]);
							normals.Add(normals[ni]);
							indices.Add(vertexCount++);
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				
			}
			finally
			{
				if (reader != null) reader.Close();
			}
		}
		private void InitBuffers()
		{
			float[] vertexData = new float[vertexCount * 8];
			for (int i = 0, vi = 0, ti = 0, ni = 0; i < vertexCount; i++, vi += 3, ti += 2, ni += 3)
			{
				vertexData[i * 8] = vertices[vi].X;
				vertexData[i * 8 + 1] = vertices[vi].Y;
				vertexData[i * 8 + 2] = vertices[vi].Z;
				if (ni == 192)
				{
					ni = 192;
				}
				vertexData[i * 8 + 3] = normals[ni].X;
				vertexData[i * 8 + 4] = normals[ni].Y;
				vertexData[i * 8 + 5] = normals[ni].Z;
				vertexData[i * 8 + 6] = texCoords[ti].X;
				vertexData[i * 8 + 7] = texCoords[ti].Y;
			}

			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);

			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, vertexData.Length * sizeof(float), vertexData, BufferUsageHint.StaticDraw);

			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

			GL.GenBuffers(1, out ebo);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);
		}
		public void Draw()
		{
			GL.BindVertexArray(vao);
			GL.DrawElements(BeginMode.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
		}
	}
}
