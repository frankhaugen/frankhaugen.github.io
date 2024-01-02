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


public abstract class InputSource
{
	public abstract void Update(GameTime gameTime);
}

public class KeyboardInputSource : InputSource
{
	private KeyboardState _keyboardState;
	private KeyboardState _keyboardPreviousState;

	public override void Update(GameTime gameTime)
	{
		_keyboardPreviousState = _keyboardState;
		_keyboardState = Keyboard.GetState();

		foreach (var key in _keyboardState.GetPressedKeys())
		{
			bool isRepeat = _keyboardPreviousState.IsKeyDown(key);
			var keyPressedEventArgs = new KeyPressedEventArgs(key, isRepeat);
			KeyPressed?.Invoke(this, keyPressedEventArgs);
		}
	}

	public event EventHandler<KeyPressedEventArgs>? KeyPressed;
}

public struct KeyPressedEventArgs
{
	public Keys Key { get; }
	public bool IsRepeat { get; }

	public KeyPressedEventArgs(Keys key, bool isRepeat)
	{
		Key = key;
		IsRepeat = isRepeat;
	}
}

public enum InputCommand
{
	MoveUp,
	MoveDown,
	MoveLeft,
	MoveRight,
	Jump,
	Attack,
	Interact,
	Pause,
	Menu,
	ToggleFullscreen,
	Quit
}

public enum InputType
{
	Keyboard,
	Mouse,
	Gamepad,
	Touch,
	Voice,
	Joystick
}