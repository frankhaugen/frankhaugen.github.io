<Query Kind="Program">
  <NuGetReference>MusicXml.NET</NuGetReference>
  <NuGetReference>NAudio.Midi</NuGetReference>
  <Namespace>NAudio.Midi</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

async Task Main()
{
    // Create a song
    var songs = SongFactory.GetJsonSongs();
    songs.Dump();
    var song = songs.First();

    // Initialize the MIDI output device and play the song
    MidiOut midiOut = new MidiOut(0);
    ISongPlayer player = new SongPlayer(midiOut);
    await player.PlaySong(song);
    midiOut.Dispose();

}

public static class SongFactory
{
    private static JsonSerializerOptions GetOptions()
    {
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        return options;   
    }
    
    public static IEnumerable<MidiSong> GetJsonSongs() => JsonSerializer.Deserialize<List<MidiSong>>(
    """
    [
      {
        "Name": "Imperial March",
        "Composer": "John Williams",
        "Tracks": [
             {
                "Instrument": "ElectricBassPick",
                "Channel": 1,
                "Notes": [
                  { "Note": "G2", "Duration": "Half" },
                  { "Note": "G2", "Duration": "Half" },
                  { "Note": "G2", "Duration": "Half" },
                  { "Note": "Eb2", "Duration": "Eighth" },
                  { "Note": "G2", "Duration": "Eighth" },
                  { "Note": "D2", "Duration": "Half" },
                  { "Note": "D2", "Duration": "Half" }
                  // ... more notes ...
                ]
              },
              {
                "Instrument": "Trumpet",
                "Channel": 2,
                "Notes": [
                  { "Note": "G4", "Duration": "Half" },
                  { "Note": "G4", "Duration": "Half" },
                  { "Note": "G4", "Duration": "Half" },
                  { "Note": "Eb4", "Duration": "Eighth" },
                  { "Note": "G4", "Duration": "Eighth" },
                  { "Note": "D4", "Duration": "Half" },
                  { "Note": "D4", "Duration": "Half" }
                ]
              },
              {
                "Instrument": "FrenchHorn",
                "Channel": 3,
                "Notes": [
                  { "Note": "G3", "Duration": "Half" },
                  { "Note": "G3", "Duration": "Half" },
                  { "Note": "G3", "Duration": "Half" },
                  { "Note": "Eb3", "Duration": "Eighth" },
                  { "Note": "G3", "Duration": "Eighth" },
                  { "Note": "D3", "Duration": "Half" },
                  { "Note": "D3", "Duration": "Half" }
                  // ... more notes ...
                ]
              },
              {
                "Instrument": "Timpani",
                "Channel": 4,
                "Notes": [
                  { "Note": "G1", "Duration": "Half" },
                  { "Note": "G1", "Duration": "Half" },
                  { "Note": "G1", "Duration": "Half" },
                  { "Note": "Eb1", "Duration": "Eighth" },
                  { "Note": "G1", "Duration": "Eighth" },
                  { "Note": "D1", "Duration": "Half" },
                  { "Note": "D1", "Duration": "Half" }
                  // ... more notes ...
                ]
              }
        ]
      }
      ,
      {
        "Name": "Chip 'n Dale: Rescue Rangers Theme",
        "Composer": "Mark Mueller",
        "Tracks": [
          {
            "Instrument": "ElectricBassPick",
            "Channel": 1,
            "Notes": [
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "A3", "Duration": "Eighth" },
              { "Note": "G3", "Duration": "Eighth" },
              { "Note": "E3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Half" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "A3", "Duration": "Eighth" },
              { "Note": "G3", "Duration": "Eighth" },
              { "Note": "E3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Half" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "A3", "Duration": "Eighth" },
              { "Note": "G3", "Duration": "Eighth" },
              { "Note": "E3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Half" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "G3", "Duration": "Quarter" },
              { "Note": "A3", "Duration": "Eighth" },
              { "Note": "G3", "Duration": "Eighth" },
              { "Note": "E3", "Duration": "Quarter" },
              { "Note": "C3", "Duration": "Half" }
            ]
          },
          {
            "Instrument": "OverdrivenGuitar",
            "Channel": 2,
            "Notes": [
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "A4", "Duration": "Eighth" },
              { "Note": "G4", "Duration": "Eighth" },
              { "Note": "E4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Half" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "A4", "Duration": "Eighth" },
              { "Note": "G4", "Duration": "Eighth" },
              { "Note": "E4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Half" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "A4", "Duration": "Eighth" },
              { "Note": "G4", "Duration": "Eighth" },
              { "Note": "E4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Half" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "G4", "Duration": "Quarter" },
              { "Note": "A4", "Duration": "Eighth" },
              { "Note": "G4", "Duration": "Eighth" },
              { "Note": "E4", "Duration": "Quarter" },
              { "Note": "C4", "Duration": "Half" }
            ]
          }
        ]
      }
    ]
    """, GetOptions());
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
    public List<MidiTrack>? Tracks { get; set; }
    public string Name { get; set; }
    public string Composer { get; set; }


    public MidiSong()
    {
    }
    
    public MidiSong(string name, string composer)
    {
        Name = name;
        Composer = composer;
        Tracks = new List<MidiTrack>();
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
    C0 = 12, Db0, D0, Eb0, E0, F0, Gb0, G0, Ab0, A0, Bb0, B0,
    C1, Db1, D1, Eb1, E1, F1, Gb1, G1, Ab1, A1, Bb1, B1,
    C2, Db2, D2, Eb2, E2, F2, Gb2, G2, Ab2, A2, Bb2, B2,
    C3, Db3, D3, Eb3, E3, F3, Gb3, G3, Ab3, A3, Bb3, B3,
    C4, Db4, D4, Eb4, E4, F4, Gb4, G4, Ab4, A4, Bb4, B4,
    C5, Db5, D5, Eb5, E5, F5, Gb5, G5, Ab5, A5, Bb5, B5,
    C6, Db6, D6, Eb6, E6, F6, Gb6, G6, Ab6, A6, Bb6, B6,
    C7, Db7, D7, Eb7, E7, F7, Gb7, G7, Ab7, A7, Bb7, B7,
    C8, Db8, D8, Eb8, E8, F8, Gb8, G8
}

public enum Duration
{
    Sixteenth = 125,
    Eighth = 250,
    Quarter = 500,
    Half = 1000,
    Whole = 2000
}
