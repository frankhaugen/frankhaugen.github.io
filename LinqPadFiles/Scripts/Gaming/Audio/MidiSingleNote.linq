<Query Kind="Program">
  <NuGetReference>NAudio.Midi</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>NAudio.Midi</Namespace>
</Query>

void Main()
{
    // Create a list of notes and their durations for the Tetris theme
    var tetrisTune = new List<(MidiNote, int)>()
    {
        (MidiNote.E5, 500),
        (MidiNote.B4, 500),
        (MidiNote.C5, 500),
        (MidiNote.D5, 500),
        (MidiNote.E5, 500),
        (MidiNote.F5, 500),
        (MidiNote.G5, 500),
        (MidiNote.G5, 1000),
        (MidiNote.F5, 500),
        (MidiNote.E5, 500),
        (MidiNote.D5, 1000),
        (MidiNote.D5, 500),
        (MidiNote.E5, 500),
        (MidiNote.F5, 1000),
        (MidiNote.G5, 500),
        (MidiNote.G5, 500),
        (MidiNote.F5, 500),
        (MidiNote.E5, 500),
        (MidiNote.D5, 500),
        (MidiNote.C5, 500),
        (MidiNote.C5, 1000)
    };

    var starWarsTheme = new List<(MidiNote, int)>()
    {
        // Main theme
        (MidiNote.A4, 500),
        (MidiNote.A4, 500),
        (MidiNote.A4, 500),
        (MidiNote.F4, 350),
        (MidiNote.C5, 150),
        (MidiNote.A4, 500),
        (MidiNote.F4, 350),
        (MidiNote.C5, 150),
        (MidiNote.A4, 1000),
        (MidiNote.E5, 500),
        (MidiNote.E5, 500),
        (MidiNote.E5, 500),
        (MidiNote.F5, 350),
        (MidiNote.C5, 150),
        (MidiNote.Gs4, 500),
        (MidiNote.F4, 350),
        (MidiNote.C5, 150),
        (MidiNote.A4, 1000),
        // Repeat main theme
        (MidiNote.A5, 500),
        (MidiNote.A4, 350),
        (MidiNote.A4, 150),
        (MidiNote.A5, 500),
        (MidiNote.G5, 350),
        (MidiNote.F5, 150),
        (MidiNote.E5, 500),
        (MidiNote.D5, 350),
        (MidiNote.C5, 150),
        (MidiNote.B4, 500),
        (MidiNote.C5, 350),
        (MidiNote.A4, 150),
        (MidiNote.F4, 1000)
    };


    // Initialize the MIDI output device
    var midiOut = new MidiOut(0); // Use 0 for the default MIDI output device

    // Play the melody
    //tetrisTune.ForEach(note => PlayNote(midiOut, note.Item1, note.Item2));
    starWarsTheme.ForEach(note => PlayNote(midiOut, note.Item1, note.Item2));
    

    // Dispose of the MIDI output device when done
    midiOut.Dispose();
}

// Helper method to play a single note with NAudio
void PlayNote(MidiOut midiOut, MidiNote note, int duration)
{
    midiOut.Send(MidiMessage.StartNote((int)note, 100, 1).RawData); // Use channel 0 and velocity 100
    Thread.Sleep(duration);
    midiOut.Send(MidiMessage.StopNote((int)note, 100, 1).RawData);
}

public enum MidiNote
{
    // MIDI note values for different musical notes
    C4 = 60, // Middle C
    Cs4 = 61, // C# / Db
    D4 = 62,
    Ds4 = 63, // D# / Eb
    E4 = 64,
    F4 = 65,
    Fs4 = 66, // F# / Gb
    G4 = 67,
    Gs4 = 68, // G# / Ab
    A4 = 69,
    As4 = 70, // A# / Bb
    B4 = 71,
    C5 = 72,
    Cs5 = 73, // C# / Db
    D5 = 74,
    Ds5 = 75, // D# / Eb
    E5 = 76,
    F5 = 77,
    Fs5 = 78, // F# / Gb
    G5 = 79,
    Gs5 = 80, // G# / Ab
    A5 = 81,
    As5 = 82, // A# / Bb
    B5 = 83,
}
