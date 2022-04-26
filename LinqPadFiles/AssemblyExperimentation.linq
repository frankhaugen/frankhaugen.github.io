<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

	    var caller = MyClass.Call();

caller.Dump();

public static class MyClass
{
	public static string Call() => GetCaller();
	
	public static string GetCaller([CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int line = 0)
	{
		var stacktrace = new StackTrace();
		
		var frames = stacktrace.GetFrames();

		frames.Select(x => $"{x.GetMethod().ReflectedType.Name}.{x.GetMethod().Name}()").Reverse().Dump();
				
		stacktrace.GetFrames()[2].GetMethod().ReflectedType.Name.Dump();
		stacktrace.GetFrames()[2].GetFileLineNumber().Dump();
				
		stacktrace.GetFrames()[1].GetMethod().DeclaringType.Name.Dump();
		stacktrace.GetFrames()[1].GetFileLineNumber().Dump();
		
		var output = string.Empty;

		output += filePath.Split("\\").Last();

		var classFileName = filePath.Split("\\").Last();
		var allMembers = Assembly.GetExecutingAssembly().GetTypes().SelectMany(x => x.GetMembers());

		//allMembers.Select(x => x.Name).Dump();

		var test = allMembers.Where(x => x.ReflectedType.Name.Contains(classFileName));
		
		test.Dump();

		//output += ".";
		//output += memberName;
		//output += "()";

		//output += filePath;
		//output += ".";
		//output += memberName;
		//output += "()";
		//output += Environment.NewLine;
		//output += line.ToString();

		//var type = Type.GetType(filePath.Split("/").Last(), false, true);
		//output = type?.Name ?? "";

		return output;
	}
}