using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace Game
{
	public class Walls
	{
		List<Vector3> wallsVertices = new List<Vector3>()
		{
			new Vector3(-50f,50f,50f),
			new Vector3(50f,50f,50f),
			new Vector3(50f,0f,50f),
			new Vector3(-50f,0f,50f),

			new Vector3(50f,50f,50f),
			new Vector3(50f,50f,-50f),
			new Vector3(50f,0f,-50f),
			new Vector3(50f,0f,50f),

			new Vector3(-50f,50f,-50f),
			new Vector3(50f,50f,-50f),
			new Vector3(50f,0f,-50f),
			new Vector3(-50f,0f,-50f),

			new Vector3(-50f,50f,50f),
			new Vector3(-50f,50f,-50f),
			new Vector3(-50f,0f,-50f),
			new Vector3(-50f,0f,50f),
		};
		public uint[] indices =
		{
			//first face
			0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

			4, 5, 6,
			6, 7, 4,

			8, 9, 10,
			10, 11, 8,

			12, 13, 14,
			14, 15, 12,

		};

		TexCoord texCoord = new TexCoord();

		public int wallsVAO;
		public int wallsVBO;
		public int wallsEBO;
		public int wallsTextureID;
		public int wallsTextureVBO;

		public void LoadWalls()
		{
			wallsVAO = GL.GenVertexArray();
			GL.BindVertexArray(wallsVAO);
			wallsVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, wallsVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, wallsVertices.Count * Vector3.SizeInBytes, wallsVertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(wallsVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			wallsEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, wallsEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			wallsTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, wallsTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(wallsVAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureWalls()
		{
			wallsTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, wallsTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult wallsTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/walls.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, wallsTexture.Width, wallsTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, wallsTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindWalls()
		{
			GL.BindVertexArray(wallsVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, wallsEBO);
			GL.BindTexture(TextureTarget.Texture2D, wallsTextureID);
		}
		public void UnLoadWalls()
		{
			GL.DeleteBuffer(wallsVAO);
			GL.DeleteBuffer(wallsVBO);
			GL.DeleteBuffer(wallsEBO);
			GL.DeleteTexture(wallsTextureID);
		}
	}
}
