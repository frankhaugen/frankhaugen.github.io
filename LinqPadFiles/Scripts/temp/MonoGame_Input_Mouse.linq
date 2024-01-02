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


public enum MouseButton
{
	Left,
	Right,
	Middle
}


public class MouseInputSource : InputSource
{
	private MouseState _mouseState;
	private MouseState _previousMouseState;
	private float _deadzone;

	public MouseInputSource(float deadzone = 1.0f)
	{
		_deadzone = deadzone;
	}

	public override void Update(GameTime gameTime)
	{
		_previousMouseState = _mouseState;
		_mouseState = Mouse.GetState();

		if (_mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
		{
			RaiseMouseClickEvent(MouseButton.Left);
		}
		if (_mouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released)
		{
			RaiseMouseClickEvent(MouseButton.Right);
		}
		if (_mouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released)
		{
			RaiseMouseClickEvent(MouseButton.Middle);
		}

		if (_mouseState.ScrollWheelValue != _previousMouseState.ScrollWheelValue)
		{
			MouseScrollEvent?.Invoke(this, new MouseScrollEventArgs(_mouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue));
		}

		var distance = Vector2.Distance(_mouseState.Position.ToVector2(), _previousMouseState.Position.ToVector2());
		if (distance > _deadzone)
		{
			var velocity = (_mouseState.Position.ToVector2() - _previousMouseState.Position.ToVector2()) / (float)gameTime.ElapsedGameTime.TotalSeconds;
			MouseMoveEvent?.Invoke(this, new MouseMoveEventArgs(_mouseState.Position - _previousMouseState.Position, _mouseState.Position, velocity));
		}
	}

	public event EventHandler<MouseClickEventArgs>? MouseClickEvent;
	public event EventHandler<MouseScrollEventArgs>? MouseScrollEvent;
	public event EventHandler<MouseMoveEventArgs>? MouseMoveEvent;

	private void RaiseMouseClickEvent(MouseButton button)
	{
		MouseClickEvent?.Invoke(this, new MouseClickEventArgs(button, _mouseState.Position));
	}
}

public struct MouseClickEventArgs
{
	public MouseButton Button { get; }
	public Point Position { get; }

	public MouseClickEventArgs(MouseButton button, Point position)
	{
		Button = button;
		Position = position;
	}
}

public struct MouseScrollEventArgs
{
	public int ScrollAmount { get; }

	public MouseScrollEventArgs(int scrollAmount)
	{
		ScrollAmount = scrollAmount;
	}
}

public struct MouseMoveEventArgs
{
	public Point Direction { get; }
	public Point Position { get; }
	public Vector2 Velocity { get; }

	public MouseMoveEventArgs(Point direction, Point position, Vector2 velocity)
	{
		Direction = direction;
		Position = position;
		Velocity = velocity;
	}
}
