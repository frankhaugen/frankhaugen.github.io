<Query Kind="Statements">
  <NuGetReference>MonoGame.Framework.WindowsDX</NuGetReference>
  <NuGetReference>System.Speech</NuGetReference>
  <Namespace>Microsoft.Xna.Framework</Namespace>
  <Namespace>Microsoft.Xna.Framework.Graphics</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input</Namespace>
  <Namespace>Microsoft.Xna.Framework.Input.Touch</Namespace>
  <Namespace>System.Speech.Recognition</Namespace>
  <Namespace>System.Speech.Recognition.SrgsGrammar</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

public class TouchInputSource : InputSource
{
	private readonly float _deadzone;
	private TouchCollection _touchState;

	public TouchInputSource(float deadzone = 1.0f)
	{
		_deadzone = deadzone;
	}

	public event EventHandler<TouchMoveEventArgs> TouchMoveEvent;
	public event EventHandler<TouchGestureEventArgs> TouchGestureEvent;

	public override void Update(GameTime gameTime)
	{
		_touchState = TouchPanel.GetState();

		foreach (var touch in _touchState)
		{
			if (touch.State == TouchLocationState.Moved && _touchState.FindById(touch.Id, out var previousTouch))
			{
				var deltaX = touch.Position.X - previousTouch.Position.X;
				var deltaY = touch.Position.Y - previousTouch.Position.Y;

				if (Math.Abs(deltaX) > _deadzone || Math.Abs(deltaY) > _deadzone)
				{
					var touchMoveEventArgs = new TouchMoveEventArgs(touch.Id, touch.Position, deltaX, deltaY);
					TouchMoveEvent?.Invoke(this, touchMoveEventArgs);
				}
			}
		}

		while (TouchPanel.IsGestureAvailable)
		{
			var gesture = TouchPanel.ReadGesture();
			var touchGestureEventArgs = new TouchGestureEventArgs(gesture);
			TouchGestureEvent?.Invoke(this, touchGestureEventArgs);
		}
	}
}

/// <summary>
/// Event arguments for touch move events.
/// </summary>
public struct TouchMoveEventArgs
{
	/// <summary>
	/// The unique identifier for the touch.
	/// </summary>
	public int TouchId { get; }

	/// <summary>
	/// The current position of the touch.
	/// </summary>
	public Vector2 Position { get; }

	/// <summary>
	/// The change in position of the touch since the last update.
	/// </summary>
	public Vector2 Delta { get; }

	/// <summary>
	/// Creates a new instance of <see cref="TouchMoveEventArgs"/>.
	/// </summary>
	/// <param name="touchId">The unique identifier for the touch.</param>
	/// <param name="position">The current position of the touch.</param>
	/// <param name="deltaX">The change in the x-coordinate of the touch since the last update.</param>
	/// <param name="deltaY">The change in the y-coordinate of the touch since the last update.</param>
	public TouchMoveEventArgs(int touchId, Vector2 position, float deltaX, float deltaY)
	{
		TouchId = touchId;
		Position = position;
		Delta = new Vector2(deltaX, deltaY);
	}
}
/// <summary>
/// Event arguments for touch gesture events.
/// </summary>
public struct TouchGestureEventArgs
{
	/// <summary>
	/// The type of gesture that was performed.
	/// </summary>
	public GestureType GestureType { get; }

	/// <summary>
	/// The position of the gesture in screen coordinates.
	/// </summary>
	public Vector2 Position { get; }

	/// <summary>
	/// The distance that the gesture moved, if applicable.
	/// </summary>
	public Vector2 Delta { get; }

	public TouchGestureEventArgs(GestureSample gesture)
	{
		GestureType = gesture.GestureType;
		Position = gesture.Position;
		Delta = gesture.Delta;
	}
}

