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
		float[] glassVertices =
		{
			-0.2f, -0.3f, -2f, //top left 0
			 0.0f, -0.3f, -2f, //top right 1
			 0.0f, -0.5f, -2f, //bottom right 2
			-0.2f, -0.5f, -2f, // bottom left 3
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
			GL.BufferData(BufferTarget.ArrayBuffer, glassVertices.Length * sizeof(float), glassVertices, BufferUsageHint.StaticDraw);
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
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Length * sizeof(float), texCoord.texCoord, BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(glassVAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
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
