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

		public Cyclinder glass = new Cyclinder(32, 0.3f, 0.7f);


		TexCoord texCoord = new TexCoord();

		public int glassVAO;
		public int glassVBO;
		public int glassEBO;
		public int glassTextureID;
		public int glassTextureVBO;

		public void LoadGlass()
		{
			glassVAO = GL.GenVertexArray();
			GL.BindVertexArray(glassVAO);
			glassVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, glassVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, glass.vertices.Count * Vector3.SizeInBytes, glass.vertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(glassVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			glassEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, glassEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, glass.indices.Count * sizeof(uint), glass.indices.ToArray(), BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			glassTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, glassTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(glassVAO, 1);
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
