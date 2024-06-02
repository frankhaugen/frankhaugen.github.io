<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <NuGetReference>PacketDotNet</NuGetReference>
  <NuGetReference>SharpPcap</NuGetReference>
  <Namespace>SharpPcap</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>PacketDotNet</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


async Task Main()
{
	// Open the first device
	var devices = CaptureDeviceList.Instance;
	var device = devices.First(x => x.Name == @"\Device\NPF_Loopback");
	if (device == null)
	{
		return;
	}

	// Open the device for capturing
	device.Open(DeviceModes.Promiscuous);

	// Set a filter to capture only TCP packets
	device.Filter = "tcp";


	// Start capturing packets
	device.OnPacketArrival += PacketHandler;
	device.StartCapture();

	await Task.Delay(TimeSpan.FromMinutes(10), QueryCancelToken);

	// Close the device
	device.StopCapture();
	device.Close();
}

static void PacketHandler(object sender, PacketCapture e)
{
	sender.GetType().Dump();
	
	var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
	var ipPacket = packet.Extract<IPPacket>();
	var tcpPacket = ipPacket.Extract<TcpPacket>();

	// Process the packet
	var sourceIp = ipPacket.SourceAddress;
	var destinationIp = ipPacket.DestinationAddress;
	var sourcePort = tcpPacket.SourcePort;
	var destinationPort = tcpPacket.DestinationPort;

	var packetDto = new PacketDto(new IpPort(sourceIp.ToString(), sourcePort),
								  new IpPort(destinationIp.ToString(), destinationPort),
								  packet,
								  ipPacket,
								  tcpPacket);
								  
	if(packetDto.Source.port == 6667 || packetDto.Destination.port == 6667)
	{
		// TODO
	}
}

public record PacketDto(IpPort Source, IpPort Destination, Packet Packet, IPPacket IpPacket, TcpPacket TcpPacket);

public record IpPort(string IpAddress, int port);