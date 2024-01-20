<Query Kind="Statements">
  <NuGetReference>Microsoft.AspNetCore.App.Ref</NuGetReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.Extensions.Caching.Memory</Namespace>
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>System.Numerics</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>


















public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddChannels(this IServiceCollection services)
	{
		services.AddMemoryCache();
		services.AddSingleton<ChannelFactory>();
		services.AddSingleton(provider => provider.GetRequiredService<ChannelFactory>().CreateChannel<string>());
		return services;
	}
}

public class ChannelFactory
{
	private readonly IMemoryCache _cache;

	public ChannelFactory(IMemoryCache cache)
	{
		_cache = cache;
	}

	public Channel<T> CreateChannel<T>() where T : class => _cache.GetOrCreate<Channel<T>>(typeof(T).Name, x =>
	{
		x.SlidingExpiration = TimeSpan.FromMinutes(60);
		return Channel.CreateUnbounded<T>();
	}) ?? throw new InvalidOperationException("Channel not found");
}

internal class DisposableChannel<T> : IChannel<T>, IDisposable
{
	private Channel<T>? _channel = System.Threading.Channels.Channel.CreateUnbounded<T>();

	public void Dispose()
	{
		_channel?.Writer.Complete();
		_channel?.Reader.Completion.GetAwaiter().GetResult();
		_channel = null;
	}

	public IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken)
	{
		if (_channel == null)
			return default;
		return _channel.Reader.ReadAllAsync(cancellationToken);
	}

	public Task WriteAsync(T source, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}

internal interface IChannel<T>
{
	IAsyncEnumerable<T> ReadAllAsync(CancellationToken cancellationToken);
	
	Task WriteAsync(T source, CancellationToken cancellationToken);
}