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


public class VoiceInputSource : InputSource
{
	private readonly SpeechRecognitionEngine _speechRecognitionEngine;
	private readonly VoiceCommandDictionary _voiceCommands;

	public VoiceInputSource()
	{
		_voiceCommands = new VoiceCommandDictionary();
		_speechRecognitionEngine = new SpeechRecognitionEngine();

		foreach (var (command, grammar) in _voiceCommands)
		{
			_speechRecognitionEngine.LoadGrammar(grammar);
		}

		_speechRecognitionEngine.SpeechRecognized += (sender, e) =>
		{
			if (Enum.TryParse<VoiceCommand>(e.Result.Grammar.RuleName, out var commandName) && _voiceCommands.TryGetValue(commandName, out var command))
			{
				VoiceCommandRecognized?.Invoke(this, new VoiceCommandRecognizedEventArgs(commandName, e.Result.Confidence));
			}
		};
	}

	public override void Update(GameTime gameTime)
	{
	}
	public event EventHandler<VoiceCommandRecognizedEventArgs>? VoiceCommandRecognized;
}
public struct VoiceCommandRecognizedEventArgs
{
	public VoiceCommand Command { get; }
	public float Confidence { get; }

	public VoiceCommandRecognizedEventArgs(VoiceCommand command, float confidence)
	{
		Command = command;
		Confidence = confidence;
	}
}
public class VoiceCommandDictionary : Dictionary<VoiceCommand, Grammar>
{
	public VoiceCommandDictionary()
	{
		// Set up the grammar for each voice command
		Add(VoiceCommand.Fire, new Grammar(new Choices("fire", "shoot")) { Name = VoiceCommand.Fire.ToString() });
		Add(VoiceCommand.MoveLeft, new Grammar(new Choices("move left", "strafe left", "strafe leftward")) { Name = VoiceCommand.MoveLeft.ToString() });
		Add(VoiceCommand.MoveRight, new Grammar(new Choices("move right", "strafe right", "strafe rightward")) { Name = VoiceCommand.MoveRight.ToString() });
		Add(VoiceCommand.MoveForward, new Grammar(new Choices("move forward", "advance", "step forward")) { Name = VoiceCommand.MoveForward.ToString() });
		Add(VoiceCommand.MoveBackward, new Grammar(new Choices("move backward", "step back", "step backward")) { Name = VoiceCommand.MoveBackward.ToString() });
		Add(VoiceCommand.TurnLeft, new Grammar(new Choices("turn left", "rotate left", "rotate leftward")) { Name = VoiceCommand.TurnLeft.ToString() });
		Add(VoiceCommand.TurnRight, new Grammar(new Choices("turn right", "rotate right", "rotate rightward")) { Name = VoiceCommand.TurnRight.ToString() });
		Add(VoiceCommand.TurnAround, new Grammar(new Choices("turn around", "rotate around", "spin around")) { Name = VoiceCommand.TurnAround.ToString() });
		Add(VoiceCommand.Jump, new Grammar(new Choices("jump", "leap", "hop")) { Name = VoiceCommand.Jump.ToString() });
		Add(VoiceCommand.Crouch, new Grammar(new Choices("crouch", "duck", "hunch")) { Name = VoiceCommand.Crouch.ToString() });
		Add(VoiceCommand.StandUp, new Grammar(new Choices("stand up", "stand", "rise")) { Name = VoiceCommand.StandUp.ToString() });
		Add(VoiceCommand.Pause, new Grammar(new Choices("pause", "pause game", "pause the game")) { Name = VoiceCommand.Pause.ToString() });
		Add(VoiceCommand.Resume, new Grammar(new Choices("resume", "resume game", "resume the game")) { Name = VoiceCommand.Resume.ToString() });
		Add(VoiceCommand.Reload, new Grammar(new Choices("reload", "reload weapon", "refill magazine")) { Name = VoiceCommand.Reload.ToString() });
		Add(VoiceCommand.SwitchWeapon, new Grammar(new Choices("switch weapon", "change weapon", "cycle weapon")) { Name = VoiceCommand.SwitchWeapon.ToString() });
		Add(VoiceCommand.Interact, new Grammar(new Choices("interact", "use", "activate")) { Name = VoiceCommand.Interact.ToString() });
		Add(VoiceCommand.Inventory, new Grammar(new Choices("inventory", "open inventory", "view inventory")) { Name = VoiceCommand.Inventory.ToString() });
		Add(VoiceCommand.Map, new Grammar(new Choices("map", "open map", "view map")) { Name = VoiceCommand.Map.ToString() });
		Add(VoiceCommand.Options, new Grammar(new Choices("options", "open options", "view options")) { Name = VoiceCommand.Options.ToString() });
		Add(VoiceCommand.Save, new Grammar(new Choices("save", "save game", "save the game")) { Name = VoiceCommand.Save.ToString() });
	}
}

public enum VoiceCommand
{
	SwitchWeapon,
	Resume,
	Pause,
	StandUp,
	Interact,
	Fire,
	MoveLeft,
	MoveRight,
	MoveForward,
	MoveBackward,
	TurnLeft,
	TurnRight,
	TurnAround,
	Jump,
	Crouch,
	Reload,
	UseItem,
	ToggleFlashlight,
	ToggleMap,
	OpenMenu,
	CloseMenu,
	SelectMenuOption,
	PauseGame,
	QuitGame,
	Save,
	Options,
	Map,
	Inventory,
}
