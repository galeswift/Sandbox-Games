
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Xen.Input.State;
using Xen.Input;
using Xen.Camera;
#endregion

namespace Xen.Ex.Camera
{
	/// <summary>
	/// Base class for a camera that is controlled by <see cref="PlayerInput"/>
	/// </summary>
	public abstract class ControlledCamera3D : Camera3D, IUpdate, IDisposable
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="updaterManager"></param>
		/// <param name="projection"></param>
		public ControlledCamera3D(UpdateManager updaterManager, Projection projection)
			: base(projection, Matrix.Identity)
		{
			if (updaterManager != null)
				updaterManager.Add(this);
		}

		private Vector2 movSens = Vector2.One;
		private Vector2 rotSens = Vector2.One;
		private bool disposed;
		private PlayerIndex playerIndex;

		/// <summary>
		/// Gets/Sets the player input index
		/// </summary>
		public PlayerIndex PlayerIndex
		{
			get { if (disposed) throw new ObjectDisposedException("this"); return playerIndex; }
			set { if (disposed) throw new ObjectDisposedException("this"); playerIndex = value; }
		}


		/// <summary>
		/// Gets/Sets the rotational sensitivity
		/// </summary>
		public Vector2 RotationSensitivity
		{
			get { if (disposed) throw new ObjectDisposedException("this"); return rotSens; }
			set { if (disposed) throw new ObjectDisposedException("this"); rotSens = value; }
		}
		/// <summary>
		/// Gets/Sets the movement sensitivity
		/// </summary>
		public Vector2 MovementSensitivity
		{
			get { if (disposed) throw new ObjectDisposedException("this"); return movSens; }
			set { if (disposed) throw new ObjectDisposedException("this"); movSens = value; }
		}

		/// <summary>
		/// Update method to implement
		/// </summary>
		/// <param name="state"></param>
		/// <param name="input"></param>
		protected abstract void Update(UpdateState state, InputState input);
		UpdateFrequency IUpdate.Update(UpdateState state)
		{
			if (disposed)
				return UpdateFrequency.Terminate;

			Update(state, state.PlayerInput[PlayerIndex].InputState);

			return UpdateFrequency.FullUpdate60hz;
		}

		/// <summary></summary>
		public bool IsDisposed
		{
			get { return disposed; }
		}

		/// <summary>
		/// Dispose this camera (the camera will stop listening to player input)
		/// </summary>
		public void Dispose()
		{
			disposed = true;
		}
	}

	/// <summary>
	/// Simple 'First Person' style free roaming camera controlled by a <see cref="PlayerInput"/>
	/// </summary>
	public sealed class FirstPersonControlledCamera3D : ControlledCamera3D
	{
		/// <summary>
		/// Construct the camera
		/// </summary>
		/// <param name="updateManager"></param>
		public FirstPersonControlledCamera3D(UpdateManager updateManager)
			: this(updateManager, Vector3.Zero)
		{
		}
		/// <summary>
		/// Construct the camera
		/// </summary>
		/// <param name="updateManager"></param>
		/// <param name="startPosition"></param>
		public FirstPersonControlledCamera3D(UpdateManager updateManager,Vector3 startPosition)
			: this(updateManager,startPosition,false)
		{
		}
		/// <summary>
		/// Construct the camera
		/// </summary>
		/// <param name="updateManager"></param>
		/// <param name="startPosition"></param>
		/// <param name="zUp">If true, the Z-Axis is treated as the up/down axis, otherwise Y-Axis is treated as up/down</param>
		public FirstPersonControlledCamera3D(UpdateManager updateManager, Vector3 startPosition, bool zUp)
			: this(updateManager,startPosition,zUp,new Projection())
		{
		}
		/// <summary>
		/// Construct the camera
		/// </summary>
		/// <param name="updateManager"></param>
		/// <param name="startPosition"></param>
		/// <param name="zUp">If true, the Z-Axis is treated as the up/down axis, otherwise Y-Axis is treated as up/down</param>
		/// <param name="projection"></param>
		public FirstPersonControlledCamera3D(UpdateManager updateManager, Vector3 startPosition, bool zUp, Projection projection)
			: base(updateManager, projection)
		{
			this.zUp = zUp;
			CameraMatrix = Matrix.CreateLookAt(startPosition, startPosition + new Vector3(1, 0, 0), new Vector3(0, 0, 1));
			this.Position = startPosition;
		}

		private Vector2 rotAcceleration, move, viewRotation;

		private float v_cap = (float)Math.PI / 2.05f;
		private bool zUp;

		/// <summary>
		/// By default the Y-axis is up/down, set to true to make the Z-axis up/down.
		/// </summary>
		public bool ZAxisUp
		{
			get { if (IsDisposed) throw new ObjectDisposedException("this"); return zUp; }
			set { if (IsDisposed) throw new ObjectDisposedException("this"); zUp = value; }
		}

		/// <summary>
		/// Set the maximum angle the camera can look up/down
		/// </summary>
		public float VerticalAxisCap
		{
			get { if (IsDisposed) throw new ObjectDisposedException("this"); return v_cap; }
			set { if (IsDisposed) throw new ObjectDisposedException("this"); if (value > (float)Math.PI * 0.5f || value < 0) throw new ArgumentException(); v_cap = value; }
		}


		/// <summary></summary>
		/// <param name="state"></param>
		/// <param name="input"></param>
		protected override sealed void Update(UpdateState state, InputState input)
		{
			Vector2 r = input.ThumbSticks.RightStick * (rotAcceleration * rotAcceleration * 0.05f + new Vector2(1, 1));

			viewRotation += r * RotationSensitivity * 0.04f;

			float cap = v_cap;

			if (viewRotation.Y > cap)
				viewRotation.Y = cap;
			if (viewRotation.Y < -cap)
				viewRotation.Y = -cap;

			rotAcceleration += input.ThumbSticks.RightStick * (new Vector2(0.5f, 0.25f) * (1.5f - move.Length() * 0.5f)) * 1.25f;
			rotAcceleration *= 0.9f;

#if !XBOX360
			if (state.PlayerInput[PlayerIndex].ControlInput == ControlInput.KeyboardMouse)
				rotAcceleration *= 0;//no rotation acceleration for mouse
#endif

			move *= 0.75f;
			move += input.ThumbSticks.LeftStick / 4;

			if (rotAcceleration.LengthSquared() > 0 && r.LengthSquared() > 0)
			{
				Vector2 v1 = rotAcceleration, v2 = r;
				v1.Normalize();
				v2.Normalize();
				float a = Vector2.Dot(v1, v2);
				if (a > 0)
					rotAcceleration *= a;
				else
					rotAcceleration *= 0.25f;
			}

			Matrix m1, rotation;

			if (zUp)
			{
				//z is up, so rotate left/right is around z.
				Matrix.CreateRotationZ(-viewRotation.X, out rotation);
				//x is always left to right, so rotate up/down is around x
				Matrix.CreateRotationX(viewRotation.Y + MathHelper.PiOver2, out m1);
			}
			else
			{
				//y is up, so rotate left/right is around y.
				Matrix.CreateRotationY(-viewRotation.X, out rotation);
				Matrix.CreateRotationX(viewRotation.Y, out m1);
			}

			Matrix.Multiply(ref m1, ref rotation, out rotation);

			Vector3 pos = Position;
			pos -= (rotation.Forward * move.Y * MovementSensitivity.Y * -8 + rotation.Left * move.X * MovementSensitivity.X * 8);// new Vector3(move.X * -8, 0, move.Y * 8);

			Matrix.CreateTranslation(ref pos, out m1);
			Matrix.Multiply(ref rotation, ref m1, out rotation);

			CameraMatrix = rotation;
		}

		/// <summary>
		/// Sets the <see cref="Camera3D.CameraMatrix"/> to a matrix that will make the camera look at a target
		/// </summary>
		/// <param name="cameraPosition"></param>
		/// <param name="lookAtTarget"></param>
		/// <param name="upVector"></param>
		/// <remarks>
		/// <para>Using <see cref="Matrix.CreateLookAt(Vector3,Vector3,Vector3)"/> is not recommended because it creats a View matrix, so it cannot be used for non-camera matrices. The <see cref="Camera3D.CameraMatrix"/> of a camera is the Inverse (<see cref="Matrix.Invert(Matrix)"/>) of the View Matrix (<see cref="ICamera.GetViewMatrix(out Matrix)"/>), so trying to set the camera matrix using Matrix.CreateLookAt will produce highly unexpected results.
		/// </para></remarks>
		public override void LookAt(ref Vector3 lookAtTarget, ref Vector3 cameraPosition, ref Vector3 upVector)
		{
			base.LookAt(ref lookAtTarget, ref cameraPosition, ref upVector);

			//update viewRotation
			Matrix matrix = this.CameraMatrix;
			Vector3 dir = new Vector3(matrix.M31, matrix.M32, matrix.M33);

			//this should be correct... :-)
			if (zUp)
			{
				viewRotation.X = (float)Math.Atan2(-dir.X, -dir.Y);
				viewRotation.Y = -(float)Math.Asin(dir.Z);
			}
			else
			{
				viewRotation.X = -(float)Math.Atan2(dir.X, dir.Z);
				viewRotation.Y = -(float)Math.Asin(dir.Y);
			}
		}
	}
}


