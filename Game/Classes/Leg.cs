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
	public class Legs
	{
		List<Vector3> legVertices = new List<Vector3>() //left front leg
		{
			//front face
			new Vector3(-5f,   -0.2f, -5f), // top left
			new Vector3(-4.5f, -0.2f, -5f), // top right
			new Vector3(-4.5f, -4f,   -5f), // bottom right
			new Vector3(-5f,   -4f,   -5f), // bottom right

			//right face
			new Vector3(-4.5f, -0.2f, -5f), // top left
			new Vector3(-4.5f, -0.2f, -5.5f), // top right
			new Vector3(-4.5f, -4f,   -5.5f), // bottom right
			new Vector3(-4.5f, -4f,   -5f), // bottom left

			//back face
			new Vector3(-5f,   -0.2f, -5.5f), // top left
			new Vector3(-4.5f, -0.2f, -5.5f), // top right
			new Vector3(-4.5f, -4f,   -5.5f), // bottom right
			new Vector3(-5f,   -4f,   -5.5f), // bottom right

			//left face
			new Vector3(-5f,  -0.2f,  -5f), // top left
			new Vector3(-5f,  -0.2f,  -5.5f), // top right
			new Vector3(-5f,  -4f,    -5.5f), // bottom right
			new Vector3(-5f,  -4f,    -5f), // bottom right

			//top face 
			new Vector3(-5f,  -0.2f,  -5.5f), // top left
			new Vector3(-4.5f,-0.2f,  -5.5f), // top right
			new Vector3(-4.5f,-0.2f,  -5f), // bottom right
			new Vector3(-5f,  -0.2f,  -5f), // bottom left
			//bottom face
			new Vector3(-5f,  -4f,   -5.5f), // top left
			new Vector3(-4.5f,-4f,   -5.5f), // top right
			new Vector3(-4.5f,-4f,   -5f), // bottom right
			new Vector3(-5f,  -4f,   -5f), // bottom left

		};

		Indices indices = new Indices();
		TexCoord texCoord = new TexCoord();

		public int legVAO;
		public int legVBO;
		public int legEBO;
		public int legTextureID;
		public int legTextureVBO;

		public void LoadLegs()
		{
			legVAO = GL.GenVertexArray();
			GL.BindVertexArray(legVAO);
			legVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, legVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, legVertices.Count * Vector3.SizeInBytes, legVertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(legVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			legEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, legEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.indices.Length * sizeof(uint), indices.indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			legTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, legTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(legVAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureLegs()
		{
			legTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, legTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult legTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/wood.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, legTexture.Width, legTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, legTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindLegs()
		{
			GL.BindVertexArray(legVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, legEBO);
			GL.BindTexture(TextureTarget.Texture2D, legTextureID);
		}
		public void UnLoadLegs()
		{
			GL.DeleteBuffer(legVAO);
			GL.DeleteBuffer(legVBO);
			GL.DeleteBuffer(legEBO);
			GL.DeleteTexture(legTextureID);
		}

	}
}
