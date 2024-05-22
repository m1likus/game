﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
//using StbImageSharp;

namespace Game
{
	internal class Camera
	{
		//CONSTANTS
		private float SPEED = 8f;
		private float SCREENWIDTH;
		private float SCREENHEIGHT;
		private float SENSITIVITY = 180f;

		//POSITION VARS
		public Vector3 position;
		Vector3 up = Vector3.UnitY;
		Vector3 front = -Vector3.UnitZ;
		Vector3 right = Vector3.UnitX;

		//VIEW ROTATIONS
		private float pitch;
		private float yaw = -90.0f;

		private bool firstMove = true;
		public Vector2 lastPos;

		public Camera(float width, float height, Vector3 position) 
		{
			SCREENWIDTH = width;
			SCREENHEIGHT = height;
			this.position = position;
		}

		public Matrix4 getViewMatrix() 
		{
			return Matrix4.LookAt(position, position + front, up);
		}
		public Matrix4 getProjectionMatrix()
		{
			return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), SCREENWIDTH / SCREENHEIGHT, 0.1f, 100f);
		}

		private void UpdateVectors() 
		{

			if (pitch >89.0f)
			{
				pitch = 89.0f;
			}
			if (pitch <-89.0f)
			{
				pitch = -89.0f;
			}

			front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
			front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
			front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

			front = Vector3.Normalize(front);

			right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
			up = Vector3.Normalize(Vector3.Cross(right,front));
		}


		public void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
		{
			if (input.IsKeyDown(Keys.W))
			{
				position += front * SPEED * (float)e.Time;
			}
			if (input.IsKeyDown(Keys.A))
			{
				position -= right * SPEED * (float)e.Time;
			}
			if (input.IsKeyDown(Keys.S))
			{
				position -= front * SPEED * (float)e.Time;
			}
			if (input.IsKeyDown(Keys.D))
			{
				position += right * SPEED * (float)e.Time;
			}

			if (firstMove)
			{
				lastPos = new Vector2 (position.X, position.Y);
				firstMove = false;
			}
			else
			{
				var deltaX = mouse.X - lastPos.X;
				var deltaY = mouse.Y - lastPos.Y;
				lastPos = new Vector2(mouse.X, mouse.Y);

				yaw += deltaX * SENSITIVITY * (float)e.Time;
				pitch -= deltaY * SENSITIVITY * (float)e.Time;
			}
			UpdateVectors();
		}

		public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e) 
		{
			InputController(input, mouse, e);
		}
	}
}