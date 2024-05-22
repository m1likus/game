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
	public class Circles
	{
		public Circle middleCookieCircle1 = new Circle(32, 0.38f);
		public Circle middleCookieCircle2 = new Circle(32, 0.38f);

		public Circle topCookieCircle1 = new Circle(32, 0.44f);
		public Circle topCookieCircle2 = new Circle(32, 0.44f);

		public Circle bottomCookieCircle1 = new Circle(32, 0.44f);
		public Circle bottomCookieCircle2 = new Circle(32, 0.44f);

		public TexCoord texCoord = new TexCoord();

		public int middleCookieCircle1VAO;
		public int middleCookieCircle1VBO;
		public int middleCookieCircle1EBO;
		public int middleCookieCircle1TextureID;
		public int middleCookieCircle1TextureVBO;

		public int middleCookieCircle2VAO;
		public int middleCookieCircle2VBO;
		public int middleCookieCircle2EBO;
		public int middleCookieCircle2TextureID;
		public int middleCookieCircle2TextureVBO;

		public int topCookieCircle1VAO;
		public int topCookieCircle1VBO;
		public int topCookieCircle1EBO;
		public int topCookieCircle1TextureID;
		public int topCookieCircle1TextureVBO;

		public int topCookieCircle2VAO;
		public int topCookieCircle2VBO;
		public int topCookieCircle2EBO;
		public int topCookieCircle2TextureID;
		public int topCookieCircle2TextureVBO;

		public int bottomCookieCircle1VAO;
		public int bottomCookieCircle1VBO;
		public int bottomCookieCircle1EBO;
		public int bottomCookieCircle1TextureID;
		public int bottomCookieCircle1TextureVBO;

		public int bottomCookieCircle2VAO;
		public int bottomCookieCircle2VBO;
		public int bottomCookieCircle2EBO;
		public int bottomCookieCircle2TextureID;
		public int bottomCookieCircle2TextureVBO;

		public void LoadCircles()
		{
			middleCookieCircle1VAO = GL.GenVertexArray();
			GL.BindVertexArray(middleCookieCircle1VAO);
			middleCookieCircle1VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, middleCookieCircle1VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, middleCookieCircle1.vertices.Count * Vector3.SizeInBytes, middleCookieCircle1.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(middleCookieCircle1VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			middleCookieCircle1EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, middleCookieCircle1EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, middleCookieCircle1.indices.Count * sizeof(uint), middleCookieCircle1.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			middleCookieCircle1TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, middleCookieCircle1TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(middleCookieCircle1VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			middleCookieCircle2VAO = GL.GenVertexArray();
			GL.BindVertexArray(middleCookieCircle2VAO);
			middleCookieCircle2VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, middleCookieCircle2VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, middleCookieCircle2.vertices.Count * Vector3.SizeInBytes, middleCookieCircle2.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(middleCookieCircle2VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			middleCookieCircle2EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, middleCookieCircle2EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, middleCookieCircle2.indices.Count * sizeof(uint), middleCookieCircle2.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			middleCookieCircle2TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, middleCookieCircle2TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(middleCookieCircle2VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			topCookieCircle1VAO = GL.GenVertexArray();
			GL.BindVertexArray(topCookieCircle1VAO);
			topCookieCircle1VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topCookieCircle1VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, topCookieCircle1.vertices.Count * Vector3.SizeInBytes, topCookieCircle1.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topCookieCircle1VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			topCookieCircle1EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topCookieCircle1EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, topCookieCircle1.indices.Count * sizeof(uint), topCookieCircle1.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			topCookieCircle1TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topCookieCircle1TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topCookieCircle1VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			topCookieCircle2VAO = GL.GenVertexArray();
			GL.BindVertexArray(topCookieCircle2VAO);
			topCookieCircle2VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topCookieCircle2VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, topCookieCircle2.vertices.Count * Vector3.SizeInBytes, topCookieCircle2.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topCookieCircle2VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			topCookieCircle2EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topCookieCircle2EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, topCookieCircle2.indices.Count * sizeof(uint), topCookieCircle2.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			topCookieCircle2TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topCookieCircle2TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topCookieCircle2VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			bottomCookieCircle1VAO = GL.GenVertexArray();
			GL.BindVertexArray(bottomCookieCircle1VAO);
			bottomCookieCircle1VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomCookieCircle1VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, bottomCookieCircle1.vertices.Count * Vector3.SizeInBytes, bottomCookieCircle1.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomCookieCircle1VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			bottomCookieCircle1EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomCookieCircle1EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, bottomCookieCircle1.indices.Count * sizeof(uint), bottomCookieCircle1.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			bottomCookieCircle1TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomCookieCircle1TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomCookieCircle1VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			bottomCookieCircle2VAO = GL.GenVertexArray();
			GL.BindVertexArray(bottomCookieCircle2VAO);
			bottomCookieCircle2VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomCookieCircle2VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, bottomCookieCircle2.vertices.Count * Vector3.SizeInBytes, bottomCookieCircle2.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomCookieCircle2VAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			bottomCookieCircle2EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomCookieCircle2EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, bottomCookieCircle2.indices.Count * sizeof(uint), bottomCookieCircle2.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			bottomCookieCircle2TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomCookieCircle2TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomCookieCircle2VAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureCircles()
		{
			middleCookieCircle1TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, middleCookieCircle1TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult middleCookieCircle1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/milk.png"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, middleCookieCircle1Texture.Width, middleCookieCircle1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, middleCookieCircle1Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			middleCookieCircle2TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, middleCookieCircle2TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult middleCookieCircle2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/milk.png"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, middleCookieCircle2Texture.Width, middleCookieCircle2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, middleCookieCircle2Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			topCookieCircle1TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, topCookieCircle1TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult topCookieCircle1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/oreo.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, topCookieCircle1Texture.Width, topCookieCircle1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, topCookieCircle1Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			topCookieCircle2TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, topCookieCircle2TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult topCookieCircle2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/oreo.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, topCookieCircle2Texture.Width, topCookieCircle2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, topCookieCircle2Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			bottomCookieCircle1TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, bottomCookieCircle1TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult bottomCookieCircle1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/oreo.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bottomCookieCircle1Texture.Width, bottomCookieCircle1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bottomCookieCircle1Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			bottomCookieCircle2TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, bottomCookieCircle2TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult bottomCookieCircle2Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/oreo.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bottomCookieCircle2Texture.Width, bottomCookieCircle2Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bottomCookieCircle2Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindCircles()
		{
			GL.BindVertexArray(middleCookieCircle1VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, middleCookieCircle1EBO);
			GL.BindTexture(TextureTarget.Texture2D, middleCookieCircle1TextureID);

			GL.BindVertexArray(middleCookieCircle2VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, middleCookieCircle2EBO);
			GL.BindTexture(TextureTarget.Texture2D, middleCookieCircle2TextureID);

			GL.BindVertexArray(topCookieCircle1VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topCookieCircle1EBO);
			GL.BindTexture(TextureTarget.Texture2D, topCookieCircle1TextureID);

			GL.BindVertexArray(topCookieCircle2VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topCookieCircle2EBO);
			GL.BindTexture(TextureTarget.Texture2D, topCookieCircle2TextureID);

			GL.BindVertexArray(bottomCookieCircle1VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomCookieCircle1EBO);
			GL.BindTexture(TextureTarget.Texture2D, bottomCookieCircle1TextureID);

			GL.BindVertexArray(bottomCookieCircle2VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomCookieCircle2EBO);
			GL.BindTexture(TextureTarget.Texture2D, bottomCookieCircle2TextureID);
		}
		public void UnLoadCircles()
		{
			GL.DeleteBuffer(middleCookieCircle1VAO);
			GL.DeleteBuffer(middleCookieCircle1VBO);
			GL.DeleteBuffer(middleCookieCircle1EBO);
			GL.DeleteTexture(middleCookieCircle1TextureID);

			GL.DeleteBuffer(middleCookieCircle2VAO);
			GL.DeleteBuffer(middleCookieCircle2VBO);
			GL.DeleteBuffer(middleCookieCircle2EBO);
			GL.DeleteTexture(middleCookieCircle2TextureID);

			GL.DeleteBuffer(topCookieCircle1VAO);
			GL.DeleteBuffer(topCookieCircle1VBO);
			GL.DeleteBuffer(topCookieCircle1EBO);
			GL.DeleteTexture(topCookieCircle1TextureID);

			GL.DeleteBuffer(topCookieCircle2VAO);
			GL.DeleteBuffer(topCookieCircle2VBO);
			GL.DeleteBuffer(topCookieCircle2EBO);
			GL.DeleteTexture(topCookieCircle2TextureID);

			GL.DeleteBuffer(bottomCookieCircle1VAO);
			GL.DeleteBuffer(bottomCookieCircle1VBO);
			GL.DeleteBuffer(bottomCookieCircle1EBO);
			GL.DeleteTexture(bottomCookieCircle1TextureID);

			GL.DeleteBuffer(bottomCookieCircle2VAO);
			GL.DeleteBuffer(bottomCookieCircle2VBO);
			GL.DeleteBuffer(bottomCookieCircle2EBO);
			GL.DeleteTexture(bottomCookieCircle2TextureID);
		}
	}
}
