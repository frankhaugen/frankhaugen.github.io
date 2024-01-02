<Query Kind="Program">
  <NuGetReference Version="0.13.9">BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Configs</Namespace>
  <Namespace>BenchmarkDotNet.Diagnosers</Namespace>
  <Namespace>BenchmarkDotNet.Engines</Namespace>
  <Namespace>BenchmarkDotNet.Environments</Namespace>
  <Namespace>BenchmarkDotNet.Exporters</Namespace>
  <Namespace>BenchmarkDotNet.Jobs</Namespace>
  <Namespace>BenchmarkDotNet.Loggers</Namespace>
  <Namespace>BenchmarkDotNet.Reports</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>BenchmarkDotNet.Validators</Namespace>
  <Namespace>LINQPad.Benchmark</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>Perfolizer.Horology</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
  <Namespace>System.Diagnostics.Tracing</Namespace>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Runtime.InteropServices</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

// This query can be #load-ed into other queries for BenchmarkDotNet support. *V=1.5*
// You can modify the code to customize benchmarking behavior. LINQPad will merge any subsequent updates.

#LINQPad optimize+   // Enable compiler/JIT optimizations when benchmarking.
                     // You can override this in your query with #LINQPad optimize-

void Main()
{
	RunBenchmark();
}

#region private::Demo

byte[] _buffer = new byte [10000];
HashAlgorithm _sha1 = SHA1.Create();
HashAlgorithm _sha512 = SHA512.Create();

[Benchmark]
public void HashSHA1() => _sha1.ComputeHash (_buffer);

[Benchmark]
public void HashSHA512() => _sha512.ComputeHash (_buffer);

[Benchmark]
[Arguments (100)]
[Arguments (200)]
public void SpinWait (int iterations) => Thread.SpinWait (iterations);

// You can also benchmark methods in classes:
public class StackTracePerformance
{
	[Benchmark]
	public void StackTraceNoFileInfo() => new StackTrace (false).ToString();

	[Benchmark]
	public void StackTraceFileInfo() => new StackTrace (true).ToString();
}

#endregion

/// <summary>This method runs when you choose "Benchmark Selected Code" from the Query menu (Ctrl+Shift+B).
/// If you want to change the bahavior of this feature, here is the entry point.</summary>
Summary[] BenchmarkSelectedCode() => RunBenchmark();

/// <summary>Call this from the Main method to benchmark all methods marked with the [Benchmark] attribute.
/// When allowDumping is false, calls to .Dump() during benchmarking will become no-ops.</summary>
Summary[] RunBenchmark (bool shortRun = false, bool allowDumping = false, bool includeUpperOutliers = false)
{
	var config = GetBenchmarkConfig();
	if (shortRun || includeUpperOutliers)
	{
		var job = shortRun ? Job.ShortRun : Job.Default;
		if (includeUpperOutliers) job = job.WithOutlierMode (Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove);
		config = config.AddJob (job);
	}
	return RunBenchmark (config, allowDumping);
}

/// <summary>Call this from the Main method to benchmark all methods marked with the [Benchmark] attribute.
/// If allowDumping is false, calls to .Dump() during benchmarking will become no-ops.</summary>
Summary[] RunBenchmark (IConfig config, bool allowDumping = false)
{
	Util.AutoScrollResults = false;
	
	using (var logger = new LiveSummaryLogger (config))
	using (allowDumping ? null : ExecutionEngine.SuspendDump())
	{
		// BenchmarkDotNet changes the computer's power plan to "High Performance" during benchmarking.
		// We need to ensure that the power plan is restored should the query be prematurely cancelled.
		var powerRestorer = new PowerPlanRestorer();
		this.QueryCancelToken.Register (() => 
		{			
			powerRestorer.Restore();
			logger.Cancel();
			// Now that we've cleaned up, we can cancel benchmarking immediately by exiting the process.
			Environment.Exit (0);
		});
		var summary = BenchmarkRunner.Run (GetType().Assembly, config.AddLogger (logger));			
		logger.Complete (summary);
		return summary;
	}
}

IConfig GetBenchmarkConfig() => ManualConfig.CreateEmpty()
	.AddValidator (DefaultConfig.Instance.GetValidators().ToArray())
	.AddAnalyser (DefaultConfig.Instance.GetAnalysers().ToArray())
	.AddColumnProvider (DefaultConfig.Instance.GetColumnProviders().ToArray())
	.AddDiagnoser (MemoryDiagnoser.Default)
	// When optimizations are disabled, issue a warning, but still allow benchmarking to go ahead.
	// (It can sometimes be useful to benchmark unoptimized code.)
	.WithOptions (ConfigOptions.DisableOptimizationsValidator);

namespace LINQPad.Benchmark
{
	sealed class LiveSummaryLogger : ILogger, IDisposable
	{
		readonly bool _includeUpperOutliers;
		readonly BenchmarkEventListener _listener;
		readonly DumpContainer _resultsContainer, _logContainer = new(), _warningsContainer = new();
		readonly Control _resultsControl;
		readonly List<LiveResult> _results = new();
		string _lastException;
		bool _completed;

		public LiveSummaryLogger (IConfig config)
		{
			var job = config.GetJobs().FirstOrDefault() ?? Job.Default;
			_includeUpperOutliers = job.ResolveValue (AccuracyMode.OutlierModeCharacteristic, EngineResolver.Instance) == Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove;

			_listener = new();
			_resultsContainer = new DumpContainer (new { Status = "Waiting for first iteration to complete..." });
			_resultsControl = _resultsContainer.ToControl();	
			
			_logContainer.AppendContent ("Figures are per operation." + (_includeUpperOutliers ? "" : " Upper outliers are excluded by default."));
			
			Util.VerticalRun (GetSummaryInfo(), _resultsControl, _logContainer).Dump ("Benchmark Live Summary", noTotals:true);	
			_warningsContainer.Dump();
			
			var envInfo = BenchmarkEnvironmentInfo.GetCurrent().ToFormattedString();
			string.Join ("\r\n", envInfo.Skip(1)).Dump (envInfo.FirstOrDefault());
			
			new Hyperlinq (() => Util.OpenQuery (Path.Combine (Util.MyQueriesFolder, "BenchmarkDotNet.linq")), "Customize benchmarking behavior", true).Dump();			
			new Hyperlinq (() => Util.OpenSample ("LINQPad Tutorial & Reference/Benchmarking Your Code"), "Learn more about benchmarking in LINQPad", true).Dump();			
			new Hyperlinq ("https://benchmarkdotnet.org/", "Learn more about the BenchmarkDotNet library").Dump();
			
			object GetSummaryInfo() => Util.HorizontalRun (true, GetOptimizationInfo(), job.DisplayInfo);
			
			object GetOptimizationInfo() =>
				IsUnoptimized (GetType().Assembly)
					? Util.WithStyle ("Optimizations disabled.", "font-weight:bold")
					: "Optimizations enabled.";
		}
		
		public string Id => nameof (LiveSummaryLogger);
		
		public int Priority => 0;

		public void Write (LogKind logKind, string text) => Collect (logKind, text);			

		public void WriteLine (LogKind logKind, string text) => Collect (logKind, text);

		public void WriteLine() { }

		public void Flush() { }

		void Collect (LogKind logKind, string text)
		{
			if (logKind == LogKind.Default &&
				(text.StartsWith ("OverheadActual") || text.StartsWith ("WorkloadActual") || text.StartsWith ("WorkloadPilot") || text.StartsWith ("WorkloadWarmup")))
			{
				Record (Measurement.Parse (text, 0));
			}
			else if (logKind == LogKind.Default && text.Contains ("\n// GC: "))
			{
				string gcLine = text.Split ('\n').First (line => line.StartsWith ("// GC: "));
				Record (Util.Try (() => GcStats.Parse (gcLine)));
			}
			else if (logKind == LogKind.Default && text.Contains ("Exception: "))
			{
				// Exceptions are written out using a LogKind of Default and then a "ExitCode != 0" message with LogKind of Error is subsequently generated.
				_lastException = text;
			}
			else if (logKind == LogKind.Error && text.Contains ("ExitCode != 0") && _lastException != null)
			{
				_logContainer.AppendContent (Util.WithHeading (FormatException (_lastException), $"Exception reported during benchmarking"));
				_lastException = null;
			}
		}

		object GetCompleteToken() => Util.Metatext ("Complete");

		void Record (Measurement m)
		{
			var result = GetLiveResult();
			if (result == null) return;

			result.AddMeasurement (m, _includeUpperOutliers);
			double? maxDisplayMax = _results.Max (r => r.GetMax() ?? r.GetMean());   // Will not have min/max if in warmup
			if (maxDisplayMax < 0.05) return;

			double scaleFactor = 200;
			if (maxDisplayMax.HasValue && (_results.Count > 1 || _results [0].GetMin() != _results [0].GetMax()))
				foreach (var r in _results)
					if (r.Operations > 0)
						r.ResultsGraph = GetBar (Normalize (r.GetMean().Value), Normalize (r.GetMin().Value), Normalize (r.GetMax().Value));
					else if (r.GetMean() is double mean)
						r.ResultsGraph = GetBar (Normalize (mean));

			_resultsContainer.UpdateContent (_results);

			int Normalize (double n) => Convert.ToInt32 (n / maxDisplayMax * scaleFactor);
		}

		void Record (GcStats gcStats)
		{
			var result = GetLiveResult();
			if (result == null) return;
			
			result.AddGcStats (gcStats);
		}

		LiveResult GetLiveResult()
		{
			if (_listener.CurrentCase == null) return null;
			var result = _results.LastOrDefault();
			if (result == null || result.Case != _listener.CurrentCase)
			{
				if (result != null) result.Phase = GetCompleteToken();
				result = new LiveResult (_listener.CurrentCase);
				_results.Add (result);
			}
			return result;
		}

		public void Dispose()
		{
			_completed = true;
			_listener.Dispose();
		}

		object GetBar (int mean, int? min = null, int? max = null)
		{
			if (mean == 0) return "";
			
			int height = 18;
			int mid = height / 2;
			int tickExtension = height / 4;

			string svgContent = $"<rect x='0' y='0' width='{mean}' height='{height}' fill='{(Util.IsDarkThemeEnabled ? "#3887B5" : "#DAEAFA")}' />";

			if (min.HasValue && max.HasValue)
				svgContent += $@"
<line x1='{min}' y1='{mid}' x2='{max}' y2='{mid}'  stroke='red' stroke-width='2' />
<line x1='{min}' y1='{mid - tickExtension}' x2='{min}'  y2='{mid + tickExtension}' stroke='red' stroke-width='2' />
<line x1='{max}' y1='{mid - tickExtension}' x2='{max}'  y2='{mid + tickExtension}' stroke='red' stroke-width='2' />";

			var svg = new Svg (svgContent, (max ?? mean) + 2, height);
			svg.HtmlElement ["role"] = "img";
			svg.HtmlElement ["xmlns"] = "http://www.w3.org/2000/svg";
			
			// Instead of returning the svg as-is, wrap the svg in an image so that the HTML can paste more easily into other apps such as GMail.
			var img = new Image { Src = "data:image/svg+xml;base64," + Convert.ToBase64String (Encoding.UTF8.GetBytes (svg.HtmlElement.ToString())) };
			img.Styles ["margin"] = "0 0 0 -2px";
			img.Styles ["display"] = "block";
			return img;
		}

		internal void Complete (Summary[] summaries)
		{
			_completed = true;
			if (_results.Count > 0)
			{
				_results.Last().Phase = GetCompleteToken();
				_resultsContainer.Refresh();
				if (summaries.Length > 0)					
				{
					var summary = summaries.First();
					string newLogFilePath = summary.LogFilePath + ".txt";
					File.Move (summary.LogFilePath, newLogFilePath, true);
					var openLink = new Button ("View full BenchmarkDotNet log", _ => OpenLog (newLogFilePath));
					var copyLink = new Button ("Copy results to clipboard", _ => CopyClipboard());
					
					var controlsPanel = new StackPanel (true, ".5em", openLink, copyLink);
					controlsPanel.Styles["padding"] = ".3em .1em";
					_logContainer.AppendContent (controlsPanel);

					var unoptimizedAssemblies = Util.AssemblyLoadContext.Assemblies
						.Where (a => !a.FullName.EndsWith ("21353812cd2a2db5"))     // Exclude LINQPad dependencies
						.Where (a => !a.FullName.StartsWith ("TypedDataContext_"))  // Exclude typed datacontext
						.Where (a => !a.FullName.StartsWith ("MyExtensions."))      // Exclude My Extensions
						.Where (IsUnoptimized)
						.Select (a => new { a.GetName().Name, a.GetName().Version })
						.ToArray();

					if (unoptimizedAssemblies.Any())
						_warningsContainer.Content = Util.WithHeading (
							Util.VerticalRun (
								"The following referenced assemblies do not have optimizations enabled:",
								unoptimizedAssemblies),
							"Warnings");
				}		
			}
			
			void OpenLog (string path) => Process.Start (new ProcessStartInfo (path) { UseShellExecute = true });			
			void CopyClipboard() => _resultsControl.HtmlElement.CopyClipboard();								
		}

		internal void Cancel()
		{
			if (_completed) return;			
			if (_results.Count > 0) _results.Last().Phase = Util.Highlight ("Cancelled");
			_logContainer.AppendContent (Util.WithStyle ("Benchmarking cancelled - restoring Windows power plan.", "font-weight:bold"));
			_resultsContainer.Refresh();
			Dispose();
		}

		static object FormatException (string exceptionText)
		{
			const string stackTracePrefix = "   at ";
			int firstAt = exceptionText.IndexOf (stackTracePrefix);
			if (firstAt < 0) return exceptionText;
			
			return Util.VerticalRun (
				exceptionText.Substring (firstAt)
					.Split ("\n")
					.Select (FormatLine)
					.Prepend (exceptionText.Substring (0, firstAt)));
				
			static object FormatLine (string line)
			{
				// Add hyperlinks to allow clicking on the line number.
				if (!line.StartsWith (stackTracePrefix)) return line;
				
				var match = Regex.Match (line, @"(\s+at .+?)\s+in\s+.+?LINQPadQuery:line\s+(\d+)");
				if (!match.Success) return line;
				
				int lineNumber = int.Parse (match.Groups [2].Value);
				
				return Util.HorizontalRun (true,
					match.Groups [1].Value,
					lineNumber > 1000000 ? "" : new Hyperlinq (lineNumber - 1, 0, $"line {lineNumber}"));
			}
		}

		// We need this listener to keep track of the current benchmark case.
		class BenchmarkEventListener : EventListener
		{
			public string CurrentCase { get; private set; }
			
			protected override void OnEventSourceCreated (EventSource eventSource)
			{
				if (eventSource.Name == "BenchmarkDotNet.EngineEventSource")
					EnableEvents (eventSource, EventLevel.Verbose, EventKeywords.All);
			}

			protected override void OnEventWritten (EventWrittenEventArgs eventData)
			{
				if (eventData.EventName == "BenchmarkStart")
				{
					string cc = eventData.Payload.OfType<string>().FirstOrDefault();
					if (cc != null && cc.StartsWith ("UserQuery.")) cc = cc.Substring (10);
					CurrentCase = cc;
				}
			}
		}
		
		static bool IsUnoptimized (Assembly a) =>
			a.GetCustomAttributes().OfType<DebuggableAttribute>().FirstOrDefault()?.IsJITOptimizerDisabled ?? false;
	}

	record LiveResult (string Case)
	{
		TimeStats _pilot, _overhead, _warmup, _workload;
		MemoryStats _memory;

		public object ResultsGraph { get; set; } = "";

		public string Mean => GetMean() is double d ? FormatNanoseconds (d) : "";
		public string Min => GetMin() is double d ? FormatNanoseconds (d) : "";
		public string Max => GetMax() is double d ? FormatNanoseconds (d) : "";
		public string Range => GetRangePercent() is double d && d < 10000 ? Convert.ToInt32 (d) + "%" : "";
		public long? AllocatedBytes => _memory.BytesPerOperation;
		public long Operations => _workload.TotalOperations;
		public object Phase { get; set; } = Util.Highlight ("Starting...");

		internal double? GetMin() => SubtractOverhead (_workload.MinTime);  // Don't report min/max for warmups
		internal double? GetMax() => SubtractOverhead (_workload.MaxTime);  // Don't report min/max for warmups
		internal double? GetMean() => SubtractOverhead (_workload.MeanTime ?? _warmup.MeanTime ?? _pilot.MeanTime);
		internal double? GetRangePercent()
		{
			var mean = GetMean();
			if (mean < 0.05) return null;
			var range = GetMax() - GetMin();
			if (range < 0.05) return 0;
			return range / mean * 100;
		}
		
		double? SubtractOverhead (double? value)
		{
			if (value == null) return null;
			var overhead = _overhead.MeanTime.GetValueOrDefault (0);
			if (overhead > value) return 0;
			return value - overhead;
		}

		string FormatNanoseconds (double ns) => TimeInterval.FromNanoseconds (ns).ToString (CultureInfo.CurrentCulture, "N2");

		public void AddMeasurement (Measurement m, bool includeUpperOutliers = false)
		{
			if (m.Is (IterationMode.Workload, IterationStage.Pilot))
			{
				Phase = Util.Highlight ("Running " + m.IterationStage.ToString().ToLower() + "...");
				_pilot.Record (m, includeUpperOutliers);
			}
			else if (m.Is (IterationMode.Overhead, IterationStage.Actual))
			{
				Phase = Util.Highlight ("Calculating overhead...");
				_overhead.Record (m, false);
			}
			else if (m.Is (IterationMode.Workload, IterationStage.Warmup))
			{
				Phase = Util.Highlight ("Running " + m.IterationStage.ToString().ToLower() + "...");
				_warmup.Record (m, includeUpperOutliers);
			}
			else if (m.Is (IterationMode.Workload, IterationStage.Actual))
			{
				Phase = Util.Highlight ("Running workload...");
				_workload.Record (m, includeUpperOutliers);
			}
		}
		
		public void AddGcStats (GcStats g) => _memory.Record (g);

		struct TimeStats
		{
			List<Measurement> _measurements;
			List<Measurement> Measurements => _measurements ??= new();

			public long TotalOperations { get; private set; }
			public double TotalNanoseconds { get; private set; }
			
			public double? MinTime { get; private set; }
			public double? MaxTime { get; private set; }
			
			public double? MeanTime => TotalOperations == 0 ? null : ((double)TotalNanoseconds / TotalOperations);

			public bool HasData => TotalOperations > 0;

			public void Record (Measurement m, bool includeUpperOutliers = false)
			{
				// Insert measurement in order so that we can quickly identify quartiles
				int index = Measurements.BinarySearch (m);
				Measurements.Insert ((index >= 0) ? index : ~index, m);

				Update (includeUpperOutliers);
			}

			void Update (bool includeUpperOutliers)
			{
				Func<Measurement, bool> isOutlier = m => false;

				if (Measurements.Count > 4 && !includeUpperOutliers)
				{
					// Remove upper quartile for consistency with BenchmarkDotNet's default summary logic
					double q1 = GetQuartile (_measurements.Count / 2);
					double q3 = GetQuartile (_measurements.Count * 3 / 2);
					double interquartileRange = q3 - q1;
					double upperFence = q3 + 1.5 * interquartileRange;

					isOutlier = m => m.Nanoseconds > upperFence;
				}

				long totalOperations = 0;
				double totalNanoseconds = 0;
				double? min = null, max = null;

				foreach (var m in _measurements)
					if (!isOutlier (m))
					{
						totalOperations += m.Operations;
						totalNanoseconds += m.Nanoseconds;

						double time = m.Nanoseconds / (double)m.Operations;

						if (min == null || time < min) min = time;
						if (max == null || time > max) max = time;
					}

				TotalOperations = totalOperations;
				TotalNanoseconds = totalNanoseconds;
				MinTime = min;
				MaxTime = max;
			}

			double GetQuartile (int count) =>
				count % 2 == 0
				? (Measurements [count / 2 - 1].Nanoseconds + Measurements [count / 2].Nanoseconds) / 2
				: Measurements [count / 2].Nanoseconds;
		}
		
		struct MemoryStats
		{
			public long TotalOperations { get; private set; }
			public long TotalAllocatedBytes { get; private set; }
			public long? BytesPerOperation => TotalOperations == 0 ? null : TotalAllocatedBytes / TotalOperations;
			
			public void Record (GcStats g)
			{
				TotalOperations += g.TotalOperations;
				TotalAllocatedBytes += g.GetTotalAllocatedBytes (false) ?? 0;
			}
		}
	}

	class PowerPlanRestorer
	{	
		Guid? _existingPlanID;
		static readonly Guid HighPerformanceID = new Guid ("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
		
		public PowerPlanRestorer()
		{
			IntPtr activePolicyGuid = IntPtr.Zero;
			if (PowerGetActiveScheme (IntPtr.Zero, ref activePolicyGuid) == 0)
			{
				var planID = (Guid)Marshal.PtrToStructure (activePolicyGuid, typeof (Guid));
				if (planID != HighPerformanceID) _existingPlanID = planID;
			}
		}
		
		public void Restore()
		{			
			if (!_existingPlanID.HasValue) return;
			var value = _existingPlanID.Value;
			PowerSetActiveScheme (IntPtr.Zero, ref value);			
		}

		[DllImport ("powrprof.dll")]
		static extern int PowerSetActiveScheme (IntPtr reservedZero, ref Guid policyGuid);

		[DllImport ("powrprof.dll")]
		static extern uint PowerGetActiveScheme (IntPtr userRootPowerKey, ref IntPtr activePolicyGuid);
	}
}

static object ToDump (object input)
{
	if (input is long) return $"{input:N0}";
	return input;
}