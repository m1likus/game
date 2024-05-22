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
	public class Floor
	{
		List<Vector3> floorVertices = new List<Vector3>()
		{
			new Vector3(-50f,0f,-50f),
			new Vector3(50f,0f,-50f),
			new Vector3(50f,0f,50f),
			new Vector3(-50f,0f,50f),
		};

		public uint[] indices =
		{
			0, 1, 2,
			2, 3, 0
		};
		TexCoord texCoord = new TexCoord();

		public int floorVAO;
		public int floorVBO;
		public int floorEBO;
		public int floorTextureID;
		public int floorTextureVBO;

		public void LoadFloor()
		{
			floorVAO = GL.GenVertexArray();
			GL.BindVertexArray(floorVAO);
			floorVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, floorVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, floorVertices.Count * Vector3.SizeInBytes, floorVertices.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(floorVAO, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			floorEBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, floorEBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			floorTextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, floorTextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(floorVAO, 1);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureFloor()
		{
			floorTextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, floorTextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult floorTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/floor.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, floorTexture.Width, floorTexture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, floorTexture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindFloor()
		{
			GL.BindVertexArray(floorVAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, floorEBO);
			GL.BindTexture(TextureTarget.Texture2D, floorTextureID);
		}
		public void UnLoadFloor()
		{
			GL.DeleteBuffer(floorVAO);
			GL.DeleteBuffer(floorVBO);
			GL.DeleteBuffer(floorEBO);
			GL.DeleteTexture(floorTextureID);
		}
	}
}
