<Query Kind="Statements">
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
</Query>

#load "C:\Users\frank\OneDrive\Projects\Sudoku\*.cs /s"

var sudoku = new ServiceCollection().AddSudokuDependencies().BuildServiceProvider(true).GetRequiredService<ISudokuGame>();

sudoku.Run();