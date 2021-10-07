using ConsoleAppGremlinq.Models;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleAppGremlinq
{
	using static ExRam.Gremlinq.Core.GremlinQuerySource;
	
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("##### Application START ######");
			Console.WriteLine("#:> Press Key to continue...");
			Console.ReadKey();
			
			try
			{
				string pathSolarSystem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "systemSolar.json");
				string pathJumps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Jumps.json");

				Console.WriteLine("Loading file of Solar system...");
				string jsonContentSystems = File.ReadAllText(pathSolarSystem);
				List<SolarSystem> allSolarSystems = JsonSerializer.Deserialize<List<SolarSystem>>(jsonContentSystems);
				Console.WriteLine($"#:> There are {allSolarSystems.Count} solar systems.");

				Console.WriteLine("Loading Jumps file...");
				string jsonContentJumps = File.ReadAllText(pathJumps);
				List<Jumps> allJumps = JsonSerializer.Deserialize<List<Jumps>>(jsonContentJumps);
				Console.WriteLine($"#:> There are {allJumps.Count} interstellar jumps.");

				Console.WriteLine("#:> Creating Graph !");

				var gremlinQuerySource = g.ConfigureEnvironment(env => env.UseModel(GraphModel
																.FromBaseTypes<Models.Graph.Vertex, Models.Graph.Edge>(lookup => lookup.IncludeAssembliesOfBaseTypes()))
					.UseJanusGraph(builder => builder.AtLocalhost()));

				Loader loadData = new Loader(gremlinQuerySource);

				Console.WriteLine("#--> Drop base");
				loadData.DropBase().Wait();

				Console.WriteLine("#--> Create all solar systems");
				Console.WriteLine("pause.....");
				Task.Delay(1000).Wait();
				loadData.CreateAllSystems(allSolarSystems).Wait();

				Console.WriteLine("#--> Create all Edges");
				Console.WriteLine("pause.....");
				Task.Delay(1000).Wait();
				loadData.CreateEdges(allJumps).Wait();

				loadData.GetRegion("The Forge").Wait();
				loadData.GetRegion("The Forge", 0.5).Wait();
				loadData.GetSystemVoisin("Jita").Wait();
				loadData.GetSystemVoisin("Mitsolen").Wait();

				// Error HERE !
				loadData.GetItineraire("Jita", "Mitsolen").Wait();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR : {ex.Message}");
			}

			Console.WriteLine("##### END ######");
			Console.ReadKey();
		}
	}
}
