<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
</Query>

var harryPotterTune = new Tune()
{
    new Note(Tone.B, Duration.QUARTER),
    new Note(Tone.E, Duration.QUARTER),
    new Note(Tone.A, Duration.QUARTER),
    new Note(Tone.Fsharp, Duration.QUARTER),
    new Note(Tone.D, Duration.QUARTER),
    new Note(Tone.G, Duration.QUARTER),
    new Note(Tone.E, Duration.HALF),
    new Note(Tone.B, Duration.QUARTER),
    new Note(Tone.A, Duration.QUARTER),
    new Note(Tone.Fsharp, Duration.HALF),
    new Note(Tone.D, Duration.QUARTER),
    new Note(Tone.G, Duration.QUARTER),
    new Note(Tone.E, Duration.HALF)
};
var starWarsTune = new Tune()
{
    new Note(Tone.GbelowC, Duration.QUARTER),
    new Note(Tone.GbelowC, Duration.QUARTER),
    new Note(Tone.GbelowC, Duration.QUARTER),
    new Note(Tone.D, Duration.HALF),
    new Note(Tone.B, Duration.HALF),

    new Note(Tone.GbelowC, Duration.QUARTER),
    new Note(Tone.D, Duration.HALF),
    new Note(Tone.B, Duration.HALF),

    new Note(Tone.GbelowC, Duration.QUARTER),
    new Note(Tone.E, Duration.HALF),
    new Note(Tone.Dsharp, Duration.HALF),
    new Note(Tone.D, Duration.QUARTER),
    new Note(Tone.Csharp, Duration.HALF),
};

var superMarioTune = new Tune()
{
    new Note(Tone.E, Duration.QUARTER),
    new Note(Tone.E, Duration.QUARTER),
    new Note(Tone.REST, Duration.QUARTER),
    new Note(Tone.E, Duration.QUARTER),
    new Note(Tone.REST, Duration.QUARTER),
    new Note(Tone.C, Duration.QUARTER),
    new Note(Tone.E, Duration.QUARTER)
};

var tetrisTune2 = new Tune()
{

            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.B, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.D, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.B, Duration.QUARTER),
            new Note(Tone.A, Duration.QUARTER),
            new Note(Tone.A, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.D, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.B, Duration.HALF),

            // Continue with the remaining notes of the Tetris theme
            new Note(Tone.D, Duration.QUARTER),
            new Note(Tone.F, Duration.QUARTER),
            new Note(Tone.A, Duration.QUARTER),
            new Note(Tone.G, Duration.HALF),

            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.G, Duration.HALF),

            new Note(Tone.A, Duration.QUARTER),
            new Note(Tone.A, Duration.QUARTER),
            new Note(Tone.G, Duration.QUARTER),
            new Note(Tone.F, Duration.QUARTER),
            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.D, Duration.QUARTER),

            new Note(Tone.E, Duration.HALF),

            new Note(Tone.G, Duration.QUARTER),
            new Note(Tone.F, Duration.QUARTER),
            new Note(Tone.D, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.D, Duration.QUARTER),
            new Note(Tone.E, Duration.QUARTER),
            new Note(Tone.C, Duration.QUARTER),
            new Note(Tone.A, Duration.QUARTER),

            new Note(Tone.A, Duration.HALF)
};

var tetrisTune = new Tune()
{
    new Note(Tone.E, Duration.QUARTER),
    new Note(Tone.B, Duration.QUARTER),
    new Note(Tone.C, Duration.QUARTER),
    new Note(Tone.D, Duration.QUARTER),
    new Note(Tone.C, Duration.QUARTER),
    new Note(Tone.B, Duration.QUARTER),
    new Note(Tone.A, Duration.QUARTER),
    new Note(Tone.A, Duration.QUARTER),
    new Note(Tone.C, Duration.QUARTER),
    new Note(Tone.E, Duration.QUARTER),
    new Note(Tone.D, Duration.QUARTER),
    new Note(Tone.C, Duration.QUARTER),
    new Note(Tone.B, Duration.HALF)
};

var pacmanTune = new Tune()
{
    new Note(Tone.B, Duration.QUARTER),
    new Note(Tone.A, Duration.QUARTER),
    new Note(Tone.G, Duration.QUARTER),
    new Note(Tone.F, Duration.QUARTER),
    new Note(Tone.G, Duration.QUARTER),
    new Note(Tone.A, Duration.QUARTER)
};



var tune = tetrisTune2;

// Create a loop
while (true)
{
    foreach (var note in tune)
    {
        Console.Beep((int)note.NoteTone, (int)note.NoteDuration);
    }
}

public enum Tone
{
    REST = 0,
    GbelowC = 196,
    A = 220,
    Asharp = 233,
    B = 247,
    C = 262,
    Csharp = 277,
    D = 294,
    Dsharp = 311,
    E = 330,
    F = 349,
    Fsharp = 370,
    G = 392,
    Gsharp = 415,
}

public struct Note
{
    public Note(Tone frequency, Duration time)
    {
        NoteTone = frequency;
        NoteDuration = time;
    }

    public Tone NoteTone { get; }
    public Duration NoteDuration { get; }
}

public enum Duration
{
    WHOLE = 1600,
    HALF = WHOLE / 2,
    QUARTER = HALF / 2,
    EIGHTH = QUARTER / 2,
    SIXTEENTH = EIGHTH / 2,
}
public class Tune : List<Note>
{
}