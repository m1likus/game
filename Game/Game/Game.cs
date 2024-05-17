using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
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
			//first face
			0, 1, 2, //top triangle
			2, 3, 0,//bottom triangle

			4, 5, 6,
			6, 7, 4,

			8, 9, 10,
			10, 11, 8,

			12, 13, 14,
			14, 15, 12,

			16, 17, 18,
			18, 19, 16,

			20, 21, 22,
			22, 23, 20
		};
	}
	public class TexCoord
	{
		public List<Vector2> texCoord = new List<Vector2>()
		{
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),

			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f),
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
		float width, height;


		//---CAMERA---
		Camera camera;

		//---GAME MECHANICS---
		double time;
		bool jump = false;

		//Basic steps and speeds
		static float baseStepX = 0.001f;
		static float baseStepY = 0.001f;
		static float baseSpeedY = 0.0000001f;

		//Steps and speeds for glass
		float startStepX = baseStepX;
		float startStepY = baseStepY;
		float stepX = 0f;

		//Steps and speeds for cookie
		float speedY = baseSpeedY;
		float stepY = 0f;

		//PosY for cookieTranslation
		float cookiePosY = 0.0f;

		//Glass Pos
		float glassStartPosX = 2f;
		float glassPosX;
		float glassLeft = -1f;

		//Limit for cookie
		float minY = 0f;

		//---OBJECTS---
		Indices indices = new Indices();
		Cookie cookie = new Cookie();
		Glass glass = new Glass();
		Table table = new Table();
		Shader shader = new Shader();

		public Game(float width, float height) : base (GameWindowSettings.Default, NativeWindowSettings.Default)
		{
			//center
			CenterWindow(new Vector2i((int)width, (int)height));
			//initialize
			this.width = width;
			this.height = height;
		}
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			GL.Viewport(0,0, e.Width, e.Height);
			startStepX = baseStepX / this.width * e.Width;
			startStepY = baseStepY / this.height * e.Height;
			speedY = baseSpeedY / this.height * e.Height;
			this.width = e.Width;
			this.height = e.Height;
			stepX = 0f;
			
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

			camera = new Camera(width, height, Vector3.Zero);
			CursorState = CursorState.Grabbed;

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
			time += args.Time;
			if (time > 0.001)
			{
				time = 0;
				if (jump)
				{
					cookiePosY += stepY;
					stepY -= speedY;
					if (cookiePosY <= minY)
					{
						cookiePosY = 0f;
						jump = false;
					}
				}

				glassPosX -= stepX;
				if (glassPosX <= glassLeft)
				{
					glassPosX = glassStartPosX;
				}

				GL.ClearColor(0.1f, 0.3f, 0.8f, 0.5f);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

				GL.UseProgram(shader.shaderHandle);

				//Draw table
				table.BindTable();
				Matrix4 tableModel = Matrix4.Identity;
				Matrix4 tableView = camera.getViewMatrix();
				Matrix4 tableProjection = camera.getProjectionMatrix();

				Matrix4 tableTranslation = Matrix4.CreateTranslation(0f, 0f, 0f);
				tableModel *= tableTranslation;

				int tableModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
				int tableViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
				int tableProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

				GL.UniformMatrix4(tableModelLocation, true, ref tableModel);
				GL.UniformMatrix4(tableViewLocation, true, ref tableView);
				GL.UniformMatrix4(tableProjectionLocation, true, ref tableProjection);

				GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

				//Draw glass of milk
				glass.BindGlass();
				Matrix4 glassModel = Matrix4.Identity;
				Matrix4 glassView = camera.getViewMatrix();
				Matrix4 glassProjection = camera.getProjectionMatrix();

				Matrix4 glassTranslation = Matrix4.CreateTranslation(glassPosX, 0f, 0f);
				glassModel *= glassTranslation;

				int glassModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
				int glassViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
				int glassProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

				GL.UniformMatrix4(glassModelLocation, true, ref glassModel);
				GL.UniformMatrix4(glassViewLocation, true, ref glassView);
				GL.UniformMatrix4(glassProjectionLocation, true, ref glassProjection);

				GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

				//Draw cookie
				cookie.BindCookie();
				Matrix4 cookieModel = Matrix4.Identity;
				Matrix4 cookieView = camera.getViewMatrix();
				Matrix4 cookieProjection = camera.getProjectionMatrix();

				Matrix4 cookieTranslation = Matrix4.CreateTranslation(0f, cookiePosY, 0f);
				cookieModel *= cookieTranslation;

				int cookieModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
				int cookieViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
				int cookieProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

				GL.UniformMatrix4(cookieModelLocation, true, ref cookieModel);
				GL.UniformMatrix4(cookieViewLocation, true, ref cookieView);
				GL.UniformMatrix4(cookieProjectionLocation, true, ref cookieProjection);

				GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

				Context.SwapBuffers();

				base.OnRenderFrame(args);
			}
		}
		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			{
				if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
				{
					Close();
				}
				if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
				{
					if (!jump)
					{
						jump = true;
						stepY = startStepY;
					}
				}
				if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Enter))
				{
					stepX = startStepX;
				}

				MouseState mouse = MouseState;
				KeyboardState input = KeyboardState;

				base.OnUpdateFrame(args);
				camera.Update(input, mouse, args);
			}
		}


	}
}
