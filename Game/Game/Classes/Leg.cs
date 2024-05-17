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
	public class Leg
	{
		List<Vector3> leg1Vertices = new List<Vector3>() //left fron leg
		{
			//front face
			new Vector3(-5f,-1.7f,-1f), //top left 
			new Vector3(-4.5f,-1.7f,-1f), //top right
			new Vector3(-4.5f,-7f,-1f), //bottom right
			new Vector3(-5f,-7f,-1f), //bottom left

			//right face
			new Vector3(-4.5f,-1.7f,-1f), //top left 
			new Vector3(-4.5f,-1.7f,-1.5f), //top right
			new Vector3(-4.5f,-7f,-1.5f), //bottom right
			new Vector3(-4.5f,-7f,-1f), //bottom left

			//back face
			new Vector3(-4.5f,-1.7f,-1.5f), //top left 
			new Vector3(-5f,-1.7f,-1.5f), //top right
			new Vector3(-5f,-7f,-1.5f), //bottom right
			new Vector3(-4.5f,-7f,-1.5f), //bottom left

			//left face
			new Vector3(-5f,-1.7f,-1.5f), //top left 
			new Vector3(-5f,-1.7f,-1f), //top right
			new Vector3(-5f,-7f,-1f), //bottom right
			new Vector3(-5f,-7f,-1.5f), //bottom left

			//top face 
			new Vector3(-5f,-1.7f,-1.5f), //top left 
			new Vector3(-4.5f,-1.7f,-1.5f), //top right
			new Vector3(-4.5f,-1.7f,-1f), //bottom right
			new Vector3(-5f,-1.7f,-1f), //bottom left

			//bottom face
			new Vector3(-5f,-7f,-4f), //top left 
			new Vector3(5f,-7f,-4f), //top right
			new Vector3(5f,-7f,-1f), //bottom right
			new Vector3(-5f,-7f,-1f), //bottom left
		};

		Indices indices = new Indices();
		TexCoord texCoord = new TexCoord();

		public int leg1VAO;
		public int leg1VBO;
		public int leg1EBO;
		public int leg1TextureID;
		public int leg1TextureVBO;

		public void LoadLeg()
		{
			//Create, bind VAO
			leg1VAO = GL.GenVertexArray();
			GL.BindVertexArray(leg1VAO);
			//Create, bind VBO
			leg1VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, leg1VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, leg1Vertices.Count * Vector3.SizeInBytes, leg1Vertices.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(leg1VAO, 0);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//Create, bind EBO
			leg1EBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, leg1EBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.indices.Length * sizeof(uint), indices.indices, BufferUsageHint.StaticDraw);
			//Unbind EBO
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//Create, bind texture
			leg1TextureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, leg1TextureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, texCoord.texCoord.Count * Vector2.SizeInBytes, texCoord.texCoord.ToArray(), BufferUsageHint.StaticDraw);
			//Point slot of VAO 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(leg1VAO, 1);
			//Unbind VBO
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.Enable(EnableCap.DepthTest);
		}
		public void TextureLeg()
		{
			leg1TextureID = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, leg1TextureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult leg1Texture = ImageResult.FromStream(File.OpenRead("../../../Textures/wood.jpg"), ColorComponents.RedGreenBlueAlpha);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, leg1Texture.Width, leg1Texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, leg1Texture.Data);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}
		public void BindLeg()
		{
			GL.BindVertexArray(leg1VAO);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, leg1EBO);
			GL.BindTexture(TextureTarget.Texture2D, leg1TextureID);
		}
		public void UnLoadLeg()
		{
			GL.DeleteBuffer(leg1VAO);
			GL.DeleteBuffer(leg1VBO);
			GL.DeleteBuffer(leg1EBO);
			GL.DeleteTexture(leg1TextureID);
		}





	}
}
