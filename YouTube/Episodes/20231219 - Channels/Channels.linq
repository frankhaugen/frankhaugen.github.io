<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

var channel = Channel.CreateUnbounded<Message>();

public class Message
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Body { get; set; }
}
