using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
	public class Indices
	{
		public uint[] indices =
		{
			//top triangle
			0, 1, 2,
			//bottom triangle
			2, 3, 0
		};
	}
	public class TexCoord
	{
		public float[] texCoord =
		{
			0f, 1f,
			1f, 1f,
			1f, 0f,
			0f, 0f,
		};
	}

	public class Shader {
		public int shaderHandle;
		public void LoadShader()
		{
			shaderHandle = GL.CreateProgram();
			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, LoadShaderSource("shader.vert"));
			GL.CompileShader(vertexShader);

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, LoadShaderSource("shader.frag"));
			GL.CompileShader(fragmentShader);

			GL.AttachShader(shaderHandle, vertexShader);
			GL.AttachShader(shaderHandle, fragmentShader);

			GL.LinkProgram(shaderHandle);

			GL.DetachShader(shaderHandle, vertexShader);
			GL.DetachShader(shaderHandle, fragmentShader);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		public void DeleteShader()
		{
			GL.DeleteProgram(shaderHandle);
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



	internal class Game : GameWindow
	{
		int width, height;

		Indices indices = new Indices();
		Cookie cookie = new Cookie();
		Glass glass = new Glass();
		Table table = new Table();

		Shader shader = new Shader();

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

			cookie.LoadCookie();
			glass.LoadGlass();
			table.LoadTable();

			shader.LoadShader();

			cookie.TextureCookie();
			glass.TextureGlass();
			table.TextureTable();

		}
		protected override void OnUnload()
		{
			base.OnUnload();

			cookie.UnLoadCookie();
			glass.UnLoadGlass();
			table.UnLoadTable();

			shader.DeleteShader();

		}
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.ClearColor(0.1f, 0.3f, 0.8f, 0.5f);
			GL.Clear(ClearBufferMask.ColorBufferBit);

			GL.UseProgram(shader.shaderHandle);
			
			//Draw table
			table.BindTable();
			Matrix4 tableModel = Matrix4.Identity;
			Matrix4 tableView = Matrix4.Identity;
			Matrix4 tableProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 100f);

			//Matrix4 tableTranslation = Matrix4.CreateTranslation(0f, 0f, 0f);
			//tableModel = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
			//tableModel *= tableTranslation;

			int tableModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int tableViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int tableProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(tableModelLocation, true, ref tableModel);
			GL.UniformMatrix4(tableViewLocation, true, ref tableView);
			GL.UniformMatrix4(tableProjectionLocation, true, ref tableProjection);

			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

			//Draw cookie
			cookie.BindCookie();
			Matrix4 cookieModel = Matrix4.Identity;
			Matrix4 cookieView = Matrix4.Identity;
			Matrix4 cookieProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 100f);

			//Matrix4 cookieTranslation = Matrix4.CreateTranslation(0f, 0f, 0f);
			//cookieModel = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
			//cookieModel *= cookieTranslation;

			int cookieModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int cookieViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int cookieProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(cookieModelLocation, true, ref cookieModel);
			GL.UniformMatrix4(cookieViewLocation, true, ref cookieView);
			GL.UniformMatrix4(cookieProjectionLocation, true, ref cookieProjection);

			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

			//Draw glass of milk
			glass.BindGlass();
			Matrix4 glassModel = Matrix4.Identity;
			Matrix4 glassView = Matrix4.Identity;
			Matrix4 glassProjection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)width / (float)height, 0.1f, 100f);

			Matrix4 glassTranslation = Matrix4.CreateTranslation(-0.1f, 0f, 0f);
			//glassModel = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f));
			glassModel *= glassTranslation;

			int glassModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int glassViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int glassProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(glassModelLocation, true, ref glassModel);
			GL.UniformMatrix4(glassViewLocation, true, ref glassView);
			GL.UniformMatrix4(glassProjectionLocation, true, ref glassProjection);



			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

			Context.SwapBuffers();

			base.OnRenderFrame(args);

		}
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);
		}


	}
}
