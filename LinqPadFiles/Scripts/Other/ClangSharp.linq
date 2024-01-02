<Query Kind="Program">
  <NuGetReference>ClangSharp</NuGetReference>
  <Namespace>ClangSharp</Namespace>
  <Namespace>ClangSharp.Interop</Namespace>
</Query>

void Main()
{
    var index = new ClangSharp.Index();
    var translationUnit = index.Handle..CreateTranslationUnit("path/to/your/source/file.cpp", TranslationUnitFlags.None);

    var cursor = translationUnit.Cursor;
    PrintCursor(cursor);
}

private void PrintCursor(Cursor cursor, int depth = 0)
{
	Console.WriteLine($"{new string(' ', depth * 2)}{cursor.KindSpelling} {cursor.Spelling}");

	foreach (var child in cursor.Children)
	{
		PrintCursor(child, depth + 1);
	}
}