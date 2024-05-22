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
	public class Ceiling
	{
		List<Vector3> ceilingVertices = new List<Vector3>()
		{
			new Vector3(-50f,50f,-50f),
			new Vector3(50f,50f,-50f),
			new Vector3(50f,50f,50f),
			new Vector3(-50f,50f,50f),
		};

		public uint[] indices =
		{
			0, 1, 2,
			2, 3, 0
		};
		TexCoord texCoord = new TexCoord();

		public int ceilingVAO;
		public int ceilingVBO;
		public int ceilingEBO;
		public int ceilingTextureID;
		public int ceilingTextureVBO;

		public void LoadCeiling()
		{
			ceilingVAO = GL.GenVertexArray();
			GL.BindVertexArray(ceilingVAO);
			ceilingVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, ceilingVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, ceilingVertices.Count * Vector3.SizeInBytes, ceilingVertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(ceilingVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			ceilingEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ceilingEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			ceilingTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, ceilingTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(ceilingVAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureCeiling()
		{
			ceilingTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, ceilingTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult ceilingTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/milk.png"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, ceilingTexture.Width, ceilingTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ceilingTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindCeiling()
		{
			GL.BindVertexArray(ceilingVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ceilingEBO);
			GL.BindTexture(TextureTarget.Texture2D, ceilingTextureID);
		}
		public void UnLoadCeiling()
		{
			GL.DeleteBuffer(ceilingVAO);
			GL.DeleteBuffer(ceilingVBO);
			GL.DeleteBuffer(ceilingEBO);
			GL.DeleteTexture(ceilingTextureID);
		}
	}
}
