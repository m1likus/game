using System;
using System.Collections.Generic;
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
using StbImageSharp;

namespace Game
{
	public class Cookie
	{
		List<Vector3> cookieVertices = new List<Vector3>()
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

		public int cookieVAO;
		public int cookieVBO;
		public int cookieEBO;
		public int cookieTextureID;
		public int cookieTextureVBO;


		public void LoadCookie()
		{
			//Create, bind VAO
			cookieVAO = GL.GenVertexArray();
			GL.BindVertexArray(cookieVAO);
			//Create, bind VBO
			cookieVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, cookieVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, cookieVertices.Count * Vector3.SizeInBytes, cookieVertices.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(cookieVAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			cookieEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, cookieEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.indices.Length * sizeof(uint), indices.indices, BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			cookieTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, cookieTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(cookieVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureCookie()
		{
			cookieTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, cookieTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult cookieTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/cookie.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, cookieTexture.Width, cookieTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, cookieTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

		}
		public void BindCookie()
		{
			GL.BindVertexArray(cookieVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, cookieEBO);
			GL.BindTexture(TextureTarget.Texture2D, cookieTextureID);
		}
		public void UnLoadCookie()
		{
			GL.DeleteBuffer(cookieVAO);
			GL.DeleteBuffer(cookieVBO);
			GL.DeleteBuffer(cookieEBO);
			GL.DeleteTexture(cookieTextureID);
		}

	}
}
