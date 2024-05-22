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
	public class GlassCircles
	{
		public Circle topMilk = new Circle(32, 0.3f);
		public Circle bottomGlass = new Circle(32, 0.3f);

		TexCoord texCoord = new TexCoord();

		public int topMilkVAO;
		public int topMilkVBO;
		public int topMilkEBO;
		public int topMilkTextureID;
		public int topMilkTextureVBO;

		public int bottomGlassVAO;
		public int bottomGlassVBO;
		public int bottomGlassEBO;
		public int bottomGlassTextureID;
		public int bottomGlassTextureVBO;

		public void LoadGlassCircles(){
			topMilkVAO = GL.GenVertexArray();
			GL.BindVertexArray(topMilkVAO);
			topMilkVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topMilkVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, topMilk.vertices.Count * Vector3.SizeInBytes, topMilk.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topMilkVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			topMilkEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topMilkEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, topMilk.indices.Count * sizeof(uint), topMilk.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			topMilkTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, topMilkTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(topMilkVAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);

			bottomGlassVAO = GL.GenVertexArray();
			GL.BindVertexArray(bottomGlassVAO);
			bottomGlassVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomGlassVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, bottomGlass.vertices.Count * Vector3.SizeInBytes, bottomGlass.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomGlassVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			bottomGlassEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomGlassEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, bottomGlass.indices.Count * sizeof(uint), bottomGlass.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			bottomGlassTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, bottomGlassTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(bottomGlassVAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
		}

		public void TextureGlassCircles()
		{
			topMilkTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, topMilkTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult topMilkTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/milk.png"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, topMilkTexture.Width, topMilkTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, topMilkTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);

			bottomGlassTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, bottomGlassTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult bottomGlassTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/glass.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bottomGlassTexture.Width, bottomGlassTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bottomGlassTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public void BindGlassBottomCircle()
		{
			GL.BindVertexArray(bottomGlassVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, bottomGlassEBO);
			GL.BindTexture(TextureTarget.Texture2D, bottomGlassTextureID);
		}

		public void BindGlassTopCircle()
		{
			GL.BindVertexArray(topMilkVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, topMilkEBO);
			GL.BindTexture(TextureTarget.Texture2D, topMilkTextureID);
		}

		public void UnLoadGlassCircles()
		{

			GL.DeleteBuffer(bottomGlassVAO);
			GL.DeleteBuffer(bottomGlassVBO);
			GL.DeleteBuffer(bottomGlassEBO);
			GL.DeleteTexture(bottomGlassTextureID);

			GL.DeleteBuffer(topMilkVAO);
			GL.DeleteBuffer(topMilkVBO);
			GL.DeleteBuffer(topMilkEBO);
			GL.DeleteTexture(topMilkTextureID);
		}

	}
}
