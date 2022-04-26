<Query Kind="Expression">
  <NuGetReference>HueSharp</NuGetReference>
  <Namespace>HueSharp</Namespace>
  <Namespace>HueSharp.Converters</Namespace>
  <Namespace>HueSharp.Enums</Namespace>
  <Namespace>HueSharp.Messages</Namespace>
  <Namespace>HueSharp.Messages.Groups</Namespace>
  <Namespace>HueSharp.Messages.Lights</Namespace>
  <Namespace>HueSharp.Messages.Scenes</Namespace>
  <Namespace>HueSharp.Messages.Schedules</Namespace>
  <Namespace>HueSharp.Messages.Sensors</Namespace>
  <Namespace>HueSharp.Net</Namespace>
  <Namespace>Microsoft.Extensions.Logging.Abstractions</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

await new HueClient(NullLoggerFactory.Instance, "frankhaugen", "http://192.168.0.2/").GetResponseAsync(new GetAllLightsRequest())