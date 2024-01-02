<Query Kind="Program">
  <NuGetReference>NAudio.Midi</NuGetReference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>NAudio.Midi</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
    // Create a song
    var song = SongFactory.GetMacGyverTheme();

    // Initialize the MIDI output device and play the song
    MidiOut midiOut = new MidiOut(0);
    ISongPlayer player = new SongPlayer(midiOut);
    await player.PlaySong(song);
    midiOut.Dispose();

}

public static class SongFactory
{
    
}

public interface ISongPlayer
{
    Task PlaySong(MidiSong song);
}

public class SongPlayer : ISongPlayer
{
    private readonly MidiOut _midiOut;

    public SongPlayer(MidiOut midiOut)
    {
        _midiOut = midiOut;
    }

    public async Task PlaySong(MidiSong song)
    {
        await Parallel.ForEachAsync(song.Tracks, async (track, cancellationToken) => { 
            await PlayTrack(_midiOut, track);
        });
    }

    private async Task PlayTrack(MidiOut midiOut, MidiTrack track)
    {
        _midiOut.Send(MidiMessage.ChangePatch((int)track.Instrument, track.Channel).RawData);
        foreach (var note in track.Notes)
        {
            await PlayNote(midiOut, (int)note.Note, (int)note.Duration, track.Channel);
        }
    }

    private async Task PlayNote(MidiOut midiOut, int note, int duration, int channel)
    {
        midiOut.Send(MidiMessage.StartNote(note, 100, channel).RawData);
        await Task.Delay(duration);
        midiOut.Send(MidiMessage.StopNote(note, 100, channel).RawData);
    }
}

public struct MidiNote
{
    public Note Note { get; set; }
    public Duration Duration { get; set; }

    public MidiNote(Note note, Duration duration)
    {
        Note = note;
        Duration = duration;
    }
}

public class MidiTrack
{
    public List<MidiNote> Notes { get; set; } = new List<MidiNote>();
    public MidiInstrument Instrument { get; set; }
    public int Channel { get; set; }
}

public class MidiSong
{
    public List<MidiTrack> Tracks { get; set; }
    public string Name { get; set; }
    public string Composer { get; set; }

    public MidiSong(string name, string composer)
    {
        Name = name;
        Composer = composer;
        Tracks = new List<MidiTrack>();
    }

    public MidiSong()
    {
    }
}

public enum MidiInstrument
{
    AcousticGrandPiano = 1,
    BrightAcousticPiano,
    ElectricGrandPiano,
    HonkyTonkPiano,
    ElectricPiano1,
    ElectricPiano2,
    Harpsichord,
    Clavi,
    Celesta,
    Glockenspiel,
    MusicBox,
    Vibraphone,
    Marimba,
    Xylophone,
    TubularBells,
    Dulcimer,
    DrawbarOrgan,
    PercussiveOrgan,
    RockOrgan,
    ChurchOrgan,
    ReedOrgan,
    Accordion,
    Harmonica,
    TangoAccordion,
    AcousticGuitarNylon,
    AcousticGuitarSteel,
    ElectricGuitarJazz,
    ElectricGuitarClean,
    ElectricGuitarMuted,
    OverdrivenGuitar,
    DistortionGuitar,
    GuitarHarmonics,
    AcousticBass,
    ElectricBassFinger,
    ElectricBassPick,
    FretlessBass,
    SlapBass1,
    SlapBass2,
    SynthBass1,
    SynthBass2,
    Violin,
    Viola,
    Cello,
    Contrabass,
    TremoloStrings,
    PizzicatoStrings,
    OrchestralHarp,
    Timpani,
    StringEnsemble1,
    StringEnsemble2,
    SynthStrings1,
    SynthStrings2,
    ChoirAahs,
    VoiceOohs,
    SynthVoice,
    OrchestraHit,
    Trumpet,
    Trombone,
    Tuba,
    MutedTrumpet,
    FrenchHorn,
    BrassSection,
    SynthBrass1,
    SynthBrass2,
    SopranoSax,
    AltoSax,
    TenorSax,
    BaritoneSax,
    Oboe,
    EnglishHorn,
    Bassoon,
    Clarinet,
    Piccolo,
    Flute,
    Recorder,
    PanFlute,
    BlownBottle,
    Shakuhachi,
    Whistle,
    Ocarina,
    Lead1Square,
    Lead2Sawtooth,
    Lead3Calliope,
    Lead4Chiff,
    Lead5Charang,
    Lead6Voice,
    Lead7Fifths,
    Lead8BassAndLead,
    Pad1NewAge,
    Pad2Warm,
    Pad3Polysynth,
    Pad4Choir,
    Pad5Bowed,
    Pad6Metallic,
    Pad7Halo,
    Pad8Sweep,
    FX1Rain,
    FX2Soundtrack,
    FX3Crystal,
    FX4Atmosphere,
    FX5Brightness,
    FX6Goblins,
    FX7Echoes,
    FX8SciFi,
    Sitar,
    Banjo,
    Shamisen,
    Koto,
    Kalimba,
    Bagpipe,
    Fiddle,
    Shanai,
    TinkleBell,
    Agogo,
    SteelDrums,
    Woodblock,
    TaikoDrum,
    MelodicTom,
    SynthDrum,
    ReverseCymbal,
    GuitarFretNoise,
    BreathNoise,
    Seashore,
    BirdTweet,
    TelephoneRing,
    Helicopter,
    Applause,
    Gunshot
}

public enum Note
{
    Rest = 0,  // Adding a rest note
    C0 = 12, Cs0, D0, Ds0, E0, F0, Fs0, G0, Gs0, A0, As0, B0,
    C1, Cs1, D1, Ds1, E1, F1, Fs1, G1, Gs1, A1, As1, B1,
    C2, Cs2, D2, Ds2, E2, F2, Fs2, G2, Gs2, A2, As2, B2,
    C3, Cs3, D3, Ds3, E3, F3, Fs3, G3, Gs3, A3, As3, B3,
    C4, Cs4, D4, Ds4, E4, F4, Fs4, G4, Gs4, A4, As4, B4,
    C5, Cs5, D5, Ds5, E5, F5, Fs5, G5, Gs5, A5, As5, B5,
    C6, Cs6, D6, Ds6, E6, F6, Fs6, G6, Gs6, A6, As6, B6,
    C7, Cs7, D7, Ds7, E7, F7, Fs7, G7, Gs7, A7, As7, B7,
    C8, Cs8, D8, Ds8, E8, F8, Fs8, G8
}

public enum Duration
{
    Sixteenth = 125,
    Eighth = 250,
    Quarter = 500,
    Half = 1000,
    Whole = 2000
}
