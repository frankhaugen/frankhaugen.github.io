<Query Kind="Statements" />

var text = "📁";

//var unicodeValue = Char.GetNumericValue(text, 0);
var unicodeValue = Char.ConvertToUtf32(text, 0);

unicodeValue.Dump();