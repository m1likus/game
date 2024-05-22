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
			0, 1, 2,	//top triangle
			2, 3, 0,	//bottom triangle

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
		bool jumpDown = false;

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
		float stepUpY = 0f;

		//PosY for cookieTranslation
		float cookiePosY = 0.0f;

		float jumpHeight = 1.5f;

		//Glass Pos
		float glassStartPosX = 4.5f;
		float glassPosX = 4.5f;
		float glassLeft = -4.5f;

		//Limit for cookie
		float minY = 0f;

		//---OBJECTS---
		Indices indices = new Indices();
		Cookie cookie = new Cookie();
		Glass glass = new Glass();
		Table table = new Table();
		Shader shader = new Shader();
		Legs leg1 = new Legs();
		Legs leg2 = new Legs();
		Legs leg3 = new Legs();
		Legs leg4 = new Legs();
		Circles circles = new Circles();
		GlassCircles glassCircles = new GlassCircles();
		Floor floor = new Floor();
		Walls walls = new Walls();
		Ceiling ceiling = new Ceiling();

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
			table.LoadTable();
			leg1.LoadLegs();
			leg2.LoadLegs();
			leg3.LoadLegs();
			leg4.LoadLegs();
			circles.LoadCircles();
			glass.LoadGlass();
			glassCircles.LoadGlassCircles();
			floor.LoadFloor();
			walls.LoadWalls();
			ceiling.LoadCeiling();

			shader.LoadShader();

			cookie.TextureCookie();
			table.TextureTable();
			leg1.TextureLegs();
			leg2.TextureLegs();
			leg3.TextureLegs();
			leg4.TextureLegs();
			circles.TextureCircles();
			glass.TextureGlass();
			glassCircles.TextureGlassCircles();
			floor.TextureFloor();
			walls.TextureWalls();
			ceiling.TextureCeiling();

			camera = new Camera(width, height, (0, 2, 0));
			CursorState = CursorState.Grabbed;

		}
		protected override void OnUnload()
		{
			base.OnUnload();

			cookie.UnLoadCookie();
			table.UnLoadTable();
			leg1.UnLoadLegs();
			leg2.UnLoadLegs();
			leg3.UnLoadLegs();
			leg4.UnLoadLegs();
			circles.UnLoadCircles();
			glass.UnLoadGlass();
			glassCircles.UnLoadGlassCircles();
			floor.UnLoadFloor();
			walls.UnLoadWalls();
			ceiling.UnLoadCeiling();

			shader.DeleteShader();

		}
		protected override void OnRenderFrame(FrameEventArgs args)
		{
			time += args.Time;
			if (time > 0.000001)
			{
				time = 0;
				if (jump)
				{
					if ((cookiePosY < jumpHeight) && jumpDown == false)
					{
						cookiePosY += stepUpY;
						stepUpY -= speedY*2;
					}
					else
					{
						jumpDown = true;
						cookiePosY -= stepUpY;
						stepUpY += speedY;
					}
					if (cookiePosY <= minY)
					{
						cookiePosY = 0f;
						jump = false;
						jumpDown = false;
					}
				}

				glassPosX -= stepX;
				if (glassPosX <= glassLeft)
				{
					glassPosX = glassStartPosX;
				}

				if ((glassPosX <=-2.6f) && (cookiePosY <= 0.7f) && (glassPosX>= -3f))
				{
					stepX = 0;
					glassPosX = 4.5f;
				}

				GL.ClearColor(0.1f, 0.3f, 0.8f, 0.5f);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

				GL.UseProgram(shader.shaderHandle);

				DrawTable();
				DrawLegs();
				DrawCookie();
				DrawCircles();
				DrawGlass();
				DrawFloor();
				DrawWalls();
				DrawCeiling();

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
						stepUpY = startStepY;
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
		protected void DrawTable()
		{
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
		}
		protected void DrawLegs()
		{
			leg1.BindLegs();
			Matrix4 leg1Model = Matrix4.Identity;
			Matrix4 leg1View = camera.getViewMatrix();
			Matrix4 leg1Projection = camera.getProjectionMatrix();

			Matrix4 leg1Translation = Matrix4.CreateTranslation(0f, 0f, 0f);
			leg1Model *= leg1Translation;

			int leg1ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int leg1ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int leg1ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(leg1ModelLocation, true, ref leg1Model);
			GL.UniformMatrix4(leg1ViewLocation, true, ref leg1View);
			GL.UniformMatrix4(leg1ProjectionLocation, true, ref leg1Projection);

			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

			leg2.BindLegs();
			Matrix4 leg2Model = Matrix4.Identity;
			Matrix4 leg2View = camera.getViewMatrix();
			Matrix4 leg2Projection = camera.getProjectionMatrix();

			//translation = table back top left - leg1 back top left = -10f - 5.5f = -4.5f
			Matrix4 leg2Translation = Matrix4.CreateTranslation(0f, 0f, -4.5f); 
			leg2Model *= leg2Translation;

			int leg2ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int leg2ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int leg2ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(leg2ModelLocation, true, ref leg2Model);
			GL.UniformMatrix4(leg2ViewLocation, true, ref leg2View);
			GL.UniformMatrix4(leg2ProjectionLocation, true, ref leg2Projection);

			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

			leg3.BindLegs();
			Matrix4 leg3Model = Matrix4.Identity;
			Matrix4 leg3View = camera.getViewMatrix();
			Matrix4 leg3Projection = camera.getProjectionMatrix();

			//translation = table right top left - leg1 right top left = 5f - (-4.5f) = 9.5f
			Matrix4 leg3Translation = Matrix4.CreateTranslation(9.5f, 0f, 0f);
			leg3Model *= leg3Translation;

			int leg3ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int leg3ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int leg3ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(leg3ModelLocation, true, ref leg3Model);
			GL.UniformMatrix4(leg3ViewLocation, true, ref leg3View);
			GL.UniformMatrix4(leg3ProjectionLocation, true, ref leg3Projection);

			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

			leg4.BindLegs();
			Matrix4 leg4Model = Matrix4.Identity;
			Matrix4 leg4View = camera.getViewMatrix();
			Matrix4 leg4Projection = camera.getProjectionMatrix();

			//translation = Ox, oZ
			Matrix4 leg4Translation = Matrix4.CreateTranslation(9.5f, 0f, -4.5f);
			leg4Model *= leg4Translation;

			int leg4ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int leg4ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int leg4ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(leg4ModelLocation, true, ref leg4Model);
			GL.UniformMatrix4(leg4ViewLocation, true, ref leg4View);
			GL.UniformMatrix4(leg4ProjectionLocation, true, ref leg4Projection);

			GL.DrawElements(PrimitiveType.Triangles, indices.indices.Length, DrawElementsType.UnsignedInt, 0);

		}
		protected void DrawCookie()
		{
			cookie.BindTopCookie();
			Matrix4 topCookieModel = Matrix4.Identity;
			Matrix4 topCookieView = camera.getViewMatrix();
			Matrix4 topCookieProjection = camera.getProjectionMatrix();

			Matrix4 topCookieTranslation = Matrix4.CreateTranslation(-3f, cookiePosY+0.06f, -7.5f);
			topCookieModel *= topCookieTranslation;

			int topCookieModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int topCookieViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int topCookieProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(topCookieModelLocation, true, ref topCookieModel);
			GL.UniformMatrix4(topCookieViewLocation, true, ref topCookieView);
			GL.UniformMatrix4(topCookieProjectionLocation, true, ref topCookieProjection);

			GL.DrawElements(PrimitiveType.Triangles, cookie.topCookie.indices.Count, DrawElementsType.UnsignedInt, 0);

			cookie.BindMiddleCookie();
			Matrix4 middleCookieModel = Matrix4.Identity;
			Matrix4 middleCookieView = camera.getViewMatrix();
			Matrix4 middleCookieProjection = camera.getProjectionMatrix();

			Matrix4 middleCookieTranslation = Matrix4.CreateTranslation(-3f, cookiePosY, -7.5f);
			middleCookieModel *= middleCookieTranslation;

			int middleCookieModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int middleCookieViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int middleCookieProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(middleCookieModelLocation, true, ref middleCookieModel);
			GL.UniformMatrix4(middleCookieViewLocation, true, ref middleCookieView);
			GL.UniformMatrix4(middleCookieProjectionLocation, true, ref middleCookieProjection);

			GL.DrawElements(PrimitiveType.Triangles, cookie.middleCookie.indices.Count, DrawElementsType.UnsignedInt, 0);

			cookie.BindBottomCookie();
			Matrix4 bottomCookieModel = Matrix4.Identity;
			Matrix4 bottomCookieView = camera.getViewMatrix();
			Matrix4 bottomCookieProjection = camera.getProjectionMatrix();

			Matrix4 bottomCookieTranslation = Matrix4.CreateTranslation(-3f, cookiePosY - 0.06f, -7.5f);
			bottomCookieModel *= bottomCookieTranslation;

			int bottomCookieModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int bottomCookieViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int bottomCookieProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(bottomCookieModelLocation, true, ref bottomCookieModel);
			GL.UniformMatrix4(bottomCookieViewLocation, true, ref bottomCookieView);
			GL.UniformMatrix4(bottomCookieProjectionLocation, true, ref bottomCookieProjection);

			GL.DrawElements(PrimitiveType.Triangles, cookie.bottomCookie.indices.Count, DrawElementsType.UnsignedInt, 0);

		}
		protected void DrawCircles()
		{
			circles.BindCircles();

			Matrix4 middleCookieCircle1Model = Matrix4.Identity;
			Matrix4 middleCookieCircle1View = camera.getViewMatrix();
			Matrix4 middleCookieCircle1Projection = camera.getProjectionMatrix();

			Matrix4 middleCookieCircle1Translation = Matrix4.CreateTranslation(-3f, cookiePosY+0.14f, -7.5f);
			middleCookieCircle1Model *= middleCookieCircle1Translation;

			int middleCookieCircle1ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int middleCookieCircle1ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int middleCookieCircle1ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(middleCookieCircle1ModelLocation, true, ref middleCookieCircle1Model);
			GL.UniformMatrix4(middleCookieCircle1ViewLocation, true, ref middleCookieCircle1View);
			GL.UniformMatrix4(middleCookieCircle1ProjectionLocation, true, ref middleCookieCircle1Projection);

			GL.DrawElements(PrimitiveType.Triangles, circles.middleCookieCircle1.indices.Count, DrawElementsType.UnsignedInt, 0);


			Matrix4 middleCookieCircle2Model = Matrix4.Identity;
			Matrix4 middleCookieCircle2View = camera.getViewMatrix();
			Matrix4 middleCookieCircle2Projection = camera.getProjectionMatrix();

			Matrix4 middleCookieCircle2Translation = Matrix4.CreateTranslation(-3f, cookiePosY + 0.08f, -7.5f);
			middleCookieCircle2Model *= middleCookieCircle2Translation;

			int middleCookieCircle2ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int middleCookieCircle2ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int middleCookieCircle2ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(middleCookieCircle2ModelLocation, true, ref middleCookieCircle2Model);
			GL.UniformMatrix4(middleCookieCircle2ViewLocation, true, ref middleCookieCircle2View);
			GL.UniformMatrix4(middleCookieCircle2ProjectionLocation, true, ref middleCookieCircle2Projection);

			GL.DrawElements(PrimitiveType.Triangles, circles.middleCookieCircle2.indices.Count, DrawElementsType.UnsignedInt, 0);


			Matrix4 topCookieCircle1Model = Matrix4.Identity;
			Matrix4 topCookieCircle1View = camera.getViewMatrix();
			Matrix4 topCookieCircle1Projection = camera.getProjectionMatrix();

			Matrix4 topCookieCircle1Translation = Matrix4.CreateTranslation(-3f, cookiePosY + 0.2f, -7.5f);
			topCookieCircle1Model *= topCookieCircle1Translation;

			int topCookieCircle1ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int topCookieCircle1ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int topCookieCircle1ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(topCookieCircle1ModelLocation, true, ref topCookieCircle1Model);
			GL.UniformMatrix4(topCookieCircle1ViewLocation, true, ref topCookieCircle1View);
			GL.UniformMatrix4(topCookieCircle1ProjectionLocation, true, ref topCookieCircle1Projection);

			GL.DrawElements(PrimitiveType.Triangles, circles.topCookieCircle1.indices.Count, DrawElementsType.UnsignedInt, 0);

			Matrix4 topCookieCircle2Model = Matrix4.Identity;
			Matrix4 topCookieCircle2View = camera.getViewMatrix();
			Matrix4 topCookieCircle2Projection = camera.getProjectionMatrix();

			Matrix4 topCookieCircle2Translation = Matrix4.CreateTranslation(-3f, cookiePosY + 0.14f, -7.5f);
			topCookieCircle2Model *= topCookieCircle2Translation;

			int topCookieCircle2ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int topCookieCircle2ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int topCookieCircle2ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(topCookieCircle2ModelLocation, true, ref topCookieCircle2Model);
			GL.UniformMatrix4(topCookieCircle2ViewLocation, true, ref topCookieCircle2View);
			GL.UniformMatrix4(topCookieCircle2ProjectionLocation, true, ref topCookieCircle2Projection);

			GL.DrawElements(PrimitiveType.Triangles, circles.topCookieCircle2.indices.Count, DrawElementsType.UnsignedInt, 0);

			Matrix4 bottomCookieCircle1Model = Matrix4.Identity;
			Matrix4 bottomCookieCircle1View = camera.getViewMatrix();
			Matrix4 bottomCookieCircle1Projection = camera.getProjectionMatrix();

			Matrix4 bottomCookieCircle1Translation = Matrix4.CreateTranslation(-3f, cookiePosY + 0.08f, -7.5f);
			bottomCookieCircle1Model *= bottomCookieCircle1Translation;

			int bottomCookieCircle1ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int bottomCookieCircle1ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int bottomCookieCircle1ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(bottomCookieCircle1ModelLocation, true, ref bottomCookieCircle1Model);
			GL.UniformMatrix4(bottomCookieCircle1ViewLocation, true, ref bottomCookieCircle1View);
			GL.UniformMatrix4(bottomCookieCircle1ProjectionLocation, true, ref bottomCookieCircle1Projection);

			GL.DrawElements(PrimitiveType.Triangles, circles.bottomCookieCircle1.indices.Count, DrawElementsType.UnsignedInt, 0);

			Matrix4 bottomCookieCircle2Model = Matrix4.Identity;
			Matrix4 bottomCookieCircle2View = camera.getViewMatrix();
			Matrix4 bottomCookieCircle2Projection = camera.getProjectionMatrix();

			Matrix4 bottomCookieCircle2Translation = Matrix4.CreateTranslation(-3f, cookiePosY + 0.02f, -7.5f);
			bottomCookieCircle2Model *= bottomCookieCircle2Translation;

			int bottomCookieCircle2ModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int bottomCookieCircle2ViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int bottomCookieCircle2ProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(bottomCookieCircle2ModelLocation, true, ref bottomCookieCircle2Model);
			GL.UniformMatrix4(bottomCookieCircle2ViewLocation, true, ref bottomCookieCircle2View);
			GL.UniformMatrix4(bottomCookieCircle2ProjectionLocation, true, ref bottomCookieCircle2Projection);

			GL.DrawElements(PrimitiveType.Triangles, circles.bottomCookieCircle2.indices.Count, DrawElementsType.UnsignedInt, 0);
		}
		protected void DrawGlass()
		{
			glass.BindGlass();
			Matrix4 glassModel = Matrix4.Identity;
			Matrix4 glassView = camera.getViewMatrix();
			Matrix4 glassProjection = camera.getProjectionMatrix();

			Matrix4 glassTranslation = Matrix4.CreateTranslation(glassPosX, -0.07f, -7.5f);
			glassModel *= glassTranslation;

			int glassModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int glassViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int glassProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(glassModelLocation, true, ref glassModel);
			GL.UniformMatrix4(glassViewLocation, true, ref glassView);
			GL.UniformMatrix4(glassProjectionLocation, true, ref glassProjection);

			GL.DrawElements(PrimitiveType.Triangles, glass.glass.indices.Count, DrawElementsType.UnsignedInt, 0);


			glassCircles.BindGlassTopCircle();

			Matrix4 topMilkModel = Matrix4.Identity;
			Matrix4 topMilkView = camera.getViewMatrix();
			Matrix4 topMilkProjection = camera.getProjectionMatrix();

			Matrix4 topMilkTranslation = Matrix4.CreateTranslation(glassPosX, +0.51f, -7.5f);
			topMilkModel *= topMilkTranslation;

			int topMilkModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int topMilkViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int topMilkProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(topMilkModelLocation, true, ref topMilkModel);
			GL.UniformMatrix4(topMilkViewLocation, true, ref topMilkView);
			GL.UniformMatrix4(topMilkProjectionLocation, true, ref topMilkProjection);

			GL.DrawElements(PrimitiveType.Triangles, glassCircles.topMilk.indices.Count, DrawElementsType.UnsignedInt, 0);

			glassCircles.BindGlassBottomCircle();

			Matrix4 bottomGlassModel = Matrix4.Identity;
			Matrix4 bottomGlassView = camera.getViewMatrix();
			Matrix4 bottomGlassProjection = camera.getProjectionMatrix();

			Matrix4 bottomGlassTranslation = Matrix4.CreateTranslation(glassPosX, 0.01f, -7.5f);
			bottomGlassModel *= bottomGlassTranslation;

			int bottomGlassModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int bottomGlassViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int bottomGlassProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(bottomGlassModelLocation, true, ref bottomGlassModel);
			GL.UniformMatrix4(bottomGlassViewLocation, true, ref bottomGlassView);
			GL.UniformMatrix4(bottomGlassProjectionLocation, true, ref bottomGlassProjection);

			GL.DrawElements(PrimitiveType.Triangles, glassCircles.bottomGlass.indices.Count, DrawElementsType.UnsignedInt, 0);
		}
		protected void DrawFloor()
		{
			floor.BindFloor();
			Matrix4 floorModel = Matrix4.Identity;
			Matrix4 floorView = camera.getViewMatrix();
			Matrix4 floorProjection = camera.getProjectionMatrix();

			Matrix4 floorTranslation = Matrix4.CreateTranslation(0, -4f, 0);
			floorModel *= floorTranslation;

			int floorModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int floorViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int floorProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(floorModelLocation, true, ref floorModel);
			GL.UniformMatrix4(floorViewLocation, true, ref floorView);
			GL.UniformMatrix4(floorProjectionLocation, true, ref floorProjection);

			GL.DrawElements(PrimitiveType.Triangles, floor.indices.Length, DrawElementsType.UnsignedInt, 0);
		}
		protected void DrawWalls()
		{
			walls.BindWalls();
			Matrix4 wallsModel = Matrix4.Identity;
			Matrix4 wallsView = camera.getViewMatrix();
			Matrix4 wallsProjection = camera.getProjectionMatrix();

			Matrix4 wallsTranslation = Matrix4.CreateTranslation(0, -4f, 0);
			wallsModel *= wallsTranslation;

			int wallsModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int wallsViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int wallsProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(wallsModelLocation, true, ref wallsModel);
			GL.UniformMatrix4(wallsViewLocation, true, ref wallsView);
			GL.UniformMatrix4(wallsProjectionLocation, true, ref wallsProjection);

			GL.DrawElements(PrimitiveType.Triangles, walls.indices.Length, DrawElementsType.UnsignedInt, 0);
		}
		protected void DrawCeiling()
		{
			ceiling.BindCeiling();
			Matrix4 ceilingModel = Matrix4.Identity;
			Matrix4 ceilingView = camera.getViewMatrix();
			Matrix4 ceilingProjection = camera.getProjectionMatrix();

			Matrix4 ceilingTranslation = Matrix4.CreateTranslation(0, -4f, 0);
			ceilingModel *= ceilingTranslation;

			int ceilingModelLocation = GL.GetUniformLocation(shader.shaderHandle, "model");
			int ceilingViewLocation = GL.GetUniformLocation(shader.shaderHandle, "view");
			int ceilingProjectionLocation = GL.GetUniformLocation(shader.shaderHandle, "projection");

			GL.UniformMatrix4(ceilingModelLocation, true, ref ceilingModel);
			GL.UniformMatrix4(ceilingViewLocation, true, ref ceilingView);
			GL.UniformMatrix4(ceilingProjectionLocation, true, ref ceilingProjection);

			GL.DrawElements(PrimitiveType.Triangles, ceiling.indices.Length, DrawElementsType.UnsignedInt, 0);
		}
	}

}
