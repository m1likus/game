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
	public class Table
	{
		float[] tableVertices =
		{
			-1f, 1f, -2.3f, // top left 0
			1f, 1f, -2.3f, //top right 1
			1f, -1f, -2.3f, // bottom right 2
			-1f, -1f, -2.3f, //bottom left 3
		};

		Indices indices = new Indices();
		TexCoord texCoord = new TexCoord();

		public int tableVAO;
		public int tableVBO;
		public int tableEBO;
		public int tableTextureID;
		public int tableTextureVBO;


		public void LoadTable()
		{
			//Create, bind VAO
			tableVAO = GL.GenVertexArray();
			GL.BindVertexArray(tableVAO);
			//Create, bind VBO
			tableVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, tableVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, tableVertices.Length * sizeof(float), tableVertices, BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(tableVAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			tableEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, tableEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.indices.Length * sizeof(uint), indices.indices, BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			tableTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, tableTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(tableVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureTable()
		{
			tableTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, tableTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult tableTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/table.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, tableTexture.Width, tableTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, tableTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindTable()
		{
			GL.BindVertexArray(tableVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, tableEBO);
			GL.BindTexture(TextureTarget.Texture2D, tableTextureID);
		}
		public void UnLoadTable()
		{
			GL.DeleteBuffer(tableVAO);
			GL.DeleteBuffer(tableVBO);
			GL.DeleteBuffer(tableEBO);
			GL.DeleteTexture(tableTextureID);
		}

	}
}
