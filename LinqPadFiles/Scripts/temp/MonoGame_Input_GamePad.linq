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

public class GameControllerInputSource : InputSource
{
	private readonly float _deadzone;
	private readonly Dictionary<PlayerIndex, (GamePadState, GamePadState)> _gamePadStates;

	public GameControllerInputSource(float deadzone = 0.2f, int maxGamePads = 4)
	{
		_deadzone = deadzone;
		_gamePadStates = new Dictionary<PlayerIndex, (GamePadState, GamePadState)>();

		for (int i = 0; i < maxGamePads; i++)
		{
			_gamePadStates[(PlayerIndex)i] = (new GamePadState(), new GamePadState());
		}
	}

	public event EventHandler<GamePadButtonPressedEventArgs> GamePadButtonPressed;
	public event EventHandler<GamePadMoveEventArgs> GamePadMove;
	public event EventHandler<GamePadTriggerEventArgs> GamePadTrigger;

	public override void Update(GameTime gameTime)
	{
		foreach (var gamePadState in _gamePadStates)
		{
			var (previousState, currentState) = gamePadState.Value;
			previousState = currentState;
			currentState = GamePad.GetState(gamePadState.Key);

			UpdateGamePadButtons(gamePadState.Key, previousState, currentState);
			UpdateGamePadMove(gamePadState.Key, previousState, currentState);
			UpdateGamePadTriggers(gamePadState.Key, previousState, currentState);
		}
	}

	private void UpdateGamePadButtons(PlayerIndex gamePadId, GamePadState previousState, GamePadState currentState)
	{
		foreach (var button in Enum.GetValues<Buttons>())
		{
			if (currentState.IsButtonDown(button) && !previousState.IsButtonDown(button))
			{
				GamePadButtonPressed?.Invoke(this, new GamePadButtonPressedEventArgs(gamePadId, button, currentState));
			}
		}
	}

	private void UpdateGamePadMove(PlayerIndex gamePadId, GamePadState previousState, GamePadState currentState)
	{
		var leftStickDelta = previousState.ThumbSticks.Left - currentState.ThumbSticks.Left;
		var rightStickDelta = previousState.ThumbSticks.Right - currentState.ThumbSticks.Right;
		if (leftStickDelta.Length() > _deadzone)
		{
			GamePadMove?.Invoke(this, new GamePadMoveEventArgs(gamePadId, currentState));
		}
		if (rightStickDelta.Length() > _deadzone)
		{
			GamePadMove?.Invoke(this, new GamePadMoveEventArgs(gamePadId, currentState));
		}
	}

	private void UpdateGamePadTriggers(PlayerIndex gamePadId, GamePadState previousState, GamePadState currentState)
	{
		if (Math.Abs(currentState.Triggers.Left - previousState.Triggers.Left) > _deadzone)
		{
			GamePadTrigger?.Invoke(this, new GamePadTriggerEventArgs(gamePadId, GamePadTriggerType.LeftTrigger, currentState.Triggers.Left));
		}

		if (Math.Abs(currentState.Triggers.Right - previousState.Triggers.Right) > _deadzone)
		{
			GamePadTrigger?.Invoke(this, new GamePadTriggerEventArgs(gamePadId, GamePadTriggerType.RightTrigger, currentState.Triggers.Right));
		}
	}

}
public enum GamePadTriggerType
{
	LeftTrigger,
	RightTrigger
}

public class GamePadTriggerEventArgs : EventArgs
{
	public PlayerIndex GamePadId { get; }
	public GamePadTriggerType Type { get; }
	public float Value { get; }

	public GamePadTriggerEventArgs(PlayerIndex gamePadId, GamePadTriggerType type, float value)
	{
		GamePadId = gamePadId;
		Type = type;
		Value = value;
	}
}
public class GamePadButtonPressedEventArgs
{
	public PlayerIndex GamePadId { get; }
	public Buttons Button { get; }
	public GamePadState CurrentState { get; }

	public GamePadButtonPressedEventArgs(PlayerIndex gamePadId, Buttons button, GamePadState currentState)
	{
		GamePadId = gamePadId;
		Button = button;
		CurrentState = currentState;
	}
}

public class GamePadMoveEventArgs
{
	public PlayerIndex GamePadId { get; }
	public GamePadState CurrentState { get; }

	public GamePadMoveEventArgs(PlayerIndex gamePadId, GamePadState currentState)
	{
		GamePadId = gamePadId;
		CurrentState = currentState;
	}
}
