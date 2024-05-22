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
		public Cyclinder topCookie = new Cyclinder(32, 0.44f, 0.06f);
		public Cyclinder middleCookie = new Cyclinder(32, 0.38f, 0.06f);
		public Cyclinder bottomCookie = new Cyclinder(32, 0.44f, 0.06f);

		Indices indices = new Indices();
		TexCoord texCoord = new TexCoord();

		public int middleCookieVAO;
		public int middleCookieVBO;
		public int middleCookieEBO;
		public int middleCookieTextureID;
		public int middleCookieTextureVBO;

		public int topCookieVAO;
		public int topCookieVBO;
		public int topCookieEBO;
		public int topCookieTextureID;
		public int topCookieTextureVBO;

		public int bottomCookieVAO;
		public int bottomCookieVBO;
		public int bottomCookieEBO;
		public int bottomCookieTextureID;
		public int bottomCookieTextureVBO;


		public void LoadCookie()
		{
			LoadTopCookie();
			LoadMiddleCookie();
			LoadBottomCookie();
		}
		public void TextureCookie()
		{
			TextureTopCookie();
			TextureMiddleCookie();
			TextureBottomCookie();
		}
		public void UnLoadCookie()
		{
			UnLoadTopCookie();
			UnLoadMiddleCookie();
			UnLoadBottomCookie();
		}
		public void LoadTopCookie()
		{
			//Create, bind VAO
			topCookieVAO = GL.GenVertexArray();
			GL.BindVertexArray(topCookieVAO);
			//Create, bind VBO
			topCookieVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topCookieVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, topCookie.vertices.Count * Vector3.SizeInBytes, topCookie.vertices.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topCookieVAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			topCookieEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topCookieEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, topCookie.indices.Count * sizeof(uint), topCookie.indices.ToArray(), BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			topCookieTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topCookieTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topCookieVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		protected void TextureTopCookie()
		{
			topCookieTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, topCookieTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult topCookieTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/oreo.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, topCookieTexture.Width, topCookieTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, topCookieTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindTopCookie()
		{
			GL.BindVertexArray(topCookieVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topCookieEBO);
			GL.BindTexture(TextureTarget.Texture2D, topCookieTextureID);
		}
		protected void UnLoadTopCookie()
		{
			GL.DeleteBuffer(topCookieVAO);
			GL.DeleteBuffer(topCookieVBO);
			GL.DeleteBuffer(topCookieEBO);
			GL.DeleteTexture(topCookieTextureID);
		}
		protected void LoadMiddleCookie()
		{
			//Create, bind VAO
			middleCookieVAO = GL.GenVertexArray();
			GL.BindVertexArray(middleCookieVAO);
			//Create, bind VBO
			middleCookieVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, middleCookieVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, middleCookie.vertices.Count * Vector3.SizeInBytes, middleCookie.vertices.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(middleCookieVAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			middleCookieEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, middleCookieEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, middleCookie.indices.Count * sizeof(uint), middleCookie.indices.ToArray(), BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			middleCookieTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, middleCookieTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(middleCookieVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		protected void TextureMiddleCookie()
		{
			middleCookieTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, middleCookieTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult middleCookieTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/milk.png"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, middleCookieTexture.Width, middleCookieTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, middleCookieTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindMiddleCookie()
		{
			GL.BindVertexArray(middleCookieVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, middleCookieEBO);
			GL.BindTexture(TextureTarget.Texture2D, middleCookieTextureID);
		}
		protected void UnLoadMiddleCookie()
		{
			GL.DeleteBuffer(middleCookieVAO);
			GL.DeleteBuffer(middleCookieVBO);
			GL.DeleteBuffer(middleCookieEBO);
			GL.DeleteTexture(middleCookieTextureID);
		}
		protected void LoadBottomCookie()
		{
			//Create, bind VAO
			bottomCookieVAO = GL.GenVertexArray();
			GL.BindVertexArray(bottomCookieVAO);
			//Create, bind VBO
			bottomCookieVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomCookieVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, bottomCookie.vertices.Count * Vector3.SizeInBytes, bottomCookie.vertices.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomCookieVAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			bottomCookieEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomCookieEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, bottomCookie.indices.Count * sizeof(uint), bottomCookie.indices.ToArray(), BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			bottomCookieTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomCookieTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomCookieVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		protected void TextureBottomCookie()
		{
			bottomCookieTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, bottomCookieTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult bottomCookieTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/oreo.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bottomCookieTexture.Width, bottomCookieTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bottomCookieTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindBottomCookie()
		{
			GL.BindVertexArray(bottomCookieVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomCookieEBO);
			GL.BindTexture(TextureTarget.Texture2D, bottomCookieTextureID);
		}
		protected void UnLoadBottomCookie()
		{
			GL.DeleteBuffer(bottomCookieVAO);
			GL.DeleteBuffer(bottomCookieVBO);
			GL.DeleteBuffer(bottomCookieEBO);
			GL.DeleteTexture(bottomCookieTextureID);
		}

	}
}
