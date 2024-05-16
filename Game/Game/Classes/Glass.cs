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
	public class Glass
	{
		List<Vector3> glassVertices = new List<Vector3>()
		{
			//front face
			new Vector3(-0.8f, -0.3f, -2f), // top left vertex
			new Vector3(-0.6f, -0.3f, -2f), // top right vertex 
			new Vector3(-0.6f, -0.5f, -2f), // bottom right vertex
			new Vector3(-0.8f, -0.5f, -2f), // bottom left vertex
			//right face
			new Vector3(-0.6f, -0.3f, -2f), // top left vertex
			new Vector3(-0.6f, -0.3f, -2.2f), // top right vertex 
			new Vector3(-0.6f, -0.5f, -2.2f), // bottom right vertex
			new Vector3(-0.6f, -0.5f, -2f), // bottom left vertex
			//back face
			new Vector3(-0.6f, -0.3f, -2.2f), // top left vertex
			new Vector3(-0.8f, -0.3f, -2.2f), // top right vertex 
			new Vector3(-0.8f, -0.5f, -2.2f), // bottom right vertex
			new Vector3(-0.6f, -0.5f, -2.2f), // bottom left vertex
			//left face
			new Vector3(-0.8f, -0.3f, -2.2f), // top left vertex
			new Vector3(-0.8f, -0.3f, -2f), // top right vertex 
			new Vector3(-0.8f, -0.5f, -2f), // bottom right vertex
			new Vector3(-0.8f, -0.5f, -2.2f), // bottom left vertex
			//top face
			new Vector3(-0.8f, -0.3f, -2.2f), // top left vertex
			new Vector3(-0.6f, -0.3f, -2.2f), // top right vertex 
			new Vector3(-0.6f, -0.3f, -2f), // bottom right vertex
			new Vector3(-0.8f, -0.3f, -2f), // bottom left vertex
			//bottom face
			new Vector3(-0.8f, -0.5f, -2.2f), // top left vertex
			new Vector3(-0.6f, -0.5f, -2.2f), // top right vertex 
			new Vector3(-0.6f, -0.5f, -2f), // bottom right vertex
			new Vector3(-0.8f, -0.5f, -2f), // bottom left vertex
		};
		Indices indices = new Indices();
		TexCoord texCoord = new TexCoord();

		public int glassVAO;
		public int glassVBO;
		public int glassEBO;
		public int glassTextureID;
		public int glassTextureVBO;

		public void LoadGlass()
		{
			//Create, bind VAO
			glassVAO = GL.GenVertexArray();
			GL.BindVertexArray(glassVAO);
			//Create, bind VBO
			glassVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, glassVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, glassVertices.Count * Vector3.SizeInBytes, glassVertices.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(glassVAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			glassEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, glassEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.indices.Length * sizeof(uint), indices.indices, BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			glassTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, glassTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(glassVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureGlass()
		{
			glassTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, glassTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult glassTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/glass.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, glassTexture.Width, glassTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, glassTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

		}
		public void BindGlass()
		{
			GL.BindVertexArray(glassVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, glassEBO);
			GL.BindTexture(TextureTarget.Texture2D, glassTextureID);
		}
		public void UnLoadGlass()
		{
			GL.DeleteBuffer(glassVAO);
			GL.DeleteBuffer(glassVBO);
			GL.DeleteBuffer(glassEBO);
			GL.DeleteTexture(glassTextureID);
		}

	}
}
