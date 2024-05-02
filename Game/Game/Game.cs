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
	internal class Game : GameWindow
	{


		float[] vertices =
		{
			 0.5f,  0.5f, 0.0f,  // top right 0
			 0.5f, -0.5f, 0.0f,  // bottom right 1
			-0.5f, -0.5f, 0.0f,  // bottom left 2
			-0.5f,  0.5f, 0.0f   // top left 3
			 
		};

		float[] cookieCoords =
		{
			0f,1f,
			1f,1f,
			1f,0f,
			0f,0f,
		};



		uint[] indices =
		{
			//top triangle
			0,1,2,
			//bottom triangle
			2,3,0
		};


		int vertexArrayObject;
		int shaderHandle;
		int vertexBufferObject;
		int elementBufferObject;
		int textureID;
		int textureVBO;

		int width, height;

		public Game(int width, int height) : base (GameWindowSettings.Default, NativeWindowSettings.Default)
		{
			//center
			CenterWindow(new Vector2i(width, height));
			//initialize
			this.width = width;
			this.height = height;
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			GL.Viewport(0,0, e.Width, e.Height);
			this.width = e.Width;
			this.height = e.Height;
		}
		protected override void OnLoad()
		{
			base.OnLoad();

			//create vao
			vertexArrayObject = GL.GenVertexArray();
			//bind vao
			GL.BindVertexArray(vertexArrayObject);
			//create vertex vbo
			vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			//point slot of vao 0
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(vertexArrayObject, 0);
			//unbind vbo
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			//GL.BindVertexArray(0);

			elementBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
			//create texture
			textureVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, cookieCoords.Length * sizeof(float), cookieCoords, BufferUsageHint.StaticDraw);

			//point slot of vao 1
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
			GL.EnableVertexArrayAttrib(vertexArrayObject, 1);
			//unbind vbo
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);



			shaderHandle = GL.CreateProgram();

			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
			GL.CompileShader(vertexShader);

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
			GL.CompileShader(fragmentShader);

			GL.AttachShader(shaderHandle, vertexShader);
			GL.AttachShader(shaderHandle, fragmentShader);

			GL.LinkProgram(shaderHandle);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);

			// -- TEXTURES --
			textureID = GL.GenTexture();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, textureID);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			StbImage.stbi_set_flip_vertically_on_load(1);
			ImageResult cookieTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/cookie.jpg"), ColorComponents.RedGreenBlueAlpha);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, cookieTexture.Width, cookieTexture.Height, 0,
				PixelFormat.Rgba, PixelType.UnsignedByte, cookieTexture.Data);

			GL.BindTexture(TextureTarget.Texture2D, 0);

		}
		protected override void OnUnload()
		{
			base.OnUnload();

			GL.DeleteVertexArray(vertexArrayObject);
			GL.DeleteBuffer(vertexBufferObject);
			GL.DeleteBuffer(elementBufferObject);
			GL.DeleteTexture(textureID);
			GL.DeleteProgram(shaderHandle);
		}
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.ClearColor(0.1f, 0.3f, 0.8f, 0.5f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.UseProgram(shaderHandle);
			GL.BindTexture(TextureTarget.Texture2D, textureID);


			GL.BindVertexArray(vertexArrayObject);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);


			Context.SwapBuffers();

			base.OnRenderFrame(args);

		}
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);
		}

		public static string LoadShaderSource(string filepath)
		{
			string shaderSource = "";
			try
			{
				using (StreamReader reader = new StreamReader("../../../Shaders/" + filepath))
				{
					shaderSource = reader.ReadToEnd();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to load shader source file: " + e.Message);
			}
			return shaderSource;
		}

	}
}
