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


public class JoystickInputSource : InputSource
{
	private readonly float _deadzone;
	private readonly Dictionary<int, (JoystickState, JoystickState)> _joystickStates;
	public JoystickInputSource(float deadzone = 0.2f, int maxJoysticks = 4)
	{
		_deadzone = deadzone;
		_joystickStates = new Dictionary<int, (JoystickState, JoystickState)>();

		for (int i = 0; i < maxJoysticks; i++)
		{
			_joystickStates[i] = (new JoystickState(), new JoystickState());
		}
	}

	public event EventHandler<JoystickButtonPressedEventArgs> JoystickButtonPressed;
	public event EventHandler<JoystickMoveEventArgs> JoystickMove;

	public override void Update(GameTime gameTime)
	{
		for (var i = 0; i < _joystickStates.Count; i++)
		{
			var joystickState = _joystickStates[i];
			joystickState.Item1 = joystickState.Item2;
			joystickState.Item2 = Joystick.GetState(i);

			UpdateJoystickButtons(i, joystickState.Item1, joystickState.Item2);
			UpdateJoystickMove(i, joystickState.Item1, joystickState.Item2);

		}
	}

	private void UpdateJoystickButtons(int joystickId, JoystickState previousState, JoystickState currentState)
	{
		for (int i = 0; i < currentState.Buttons.Count(); i++)
		{
			if (currentState.Buttons[i] == ButtonState.Pressed && previousState.Buttons[i] == ButtonState.Released)
			{
				JoystickButtonPressed?.Invoke(this, new JoystickButtonPressedEventArgs(joystickId, i));
			}
		}
	}

	private void UpdateJoystickMove(int joystickId, JoystickState previousState, JoystickState currentState)
	{
		for (int i = 0; i < currentState.Axes.Count(); i++)
		{
			var axisDelta = currentState.Axes[i] - previousState.Axes[i];

			if (Math.Abs(axisDelta) > _deadzone)
			{
				JoystickMove?.Invoke(this, new JoystickMoveEventArgs(joystickId, i, axisDelta, currentState.Axes[i]));
			}
		}
	}


}
public class JoystickButtonPressedEventArgs
{
	public int JoystickId { get; }
	public int ButtonIndex { get; }

	public JoystickButtonPressedEventArgs(int joystickId, int buttonIndex)
	{
		JoystickId = joystickId;
		ButtonIndex = buttonIndex;
	}
}
/// <summary>
/// Provides data for the <see cref="JoystickInputSource.JoystickMove"/> event.
/// </summary>
public class JoystickMoveEventArgs : EventArgs
{
	/// <summary>
	/// Gets the identifier of the joystick that generated the event.
	/// </summary>
	public int JoystickId { get; }

	/// <summary>
	/// Gets the identifier of the axis that was moved.
	/// </summary>
	public int AxisId { get; }

	/// <summary>
	/// Gets the amount the axis was moved.
	/// </summary>
	public float Delta { get; }

	/// <summary>
	/// Gets the value of the axis after it was moved.
	/// </summary>
	public float Value { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="JoystickMoveEventArgs"/> class.
	/// </summary>
	/// <param name="joystickId">The identifier of the joystick that generated the event.</param>
	/// <param name="axisId">The identifier of the axis that was moved.</param>
	/// <param name="delta">The amount the axis was moved.</param>
	/// <param name="value">The value of the axis after it was moved.</param>
	public JoystickMoveEventArgs(int joystickId, int axisId, float delta, float value)
	{
		JoystickId = joystickId;
		AxisId = axisId;
		Delta = delta;
		Value = value;
	}
}
