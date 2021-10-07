using ConsoleAppGremlinq.Models;
using ConsoleAppGremlinq.Models.Graph;
using ExRam.Gremlinq.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleAppGremlinq
{
	public class Loader
	{
		private readonly IGremlinQuerySource _g;

		public Loader(IGremlinQuerySource g)
		{
			_g = g;
		}

		internal async Task DropBase()
		{
			await _g
				.V()
				.Drop();
		}

		internal async Task CreateAllSystems(List<SolarSystem> solarSystems)
		{
			try
			{
				foreach (var system in solarSystems)
				{
					await _g.AddV(new SystemSolar
					{
						SolarSystemID = system.solarSystemID,
						SolarSystemName = system.solarSystemName,
						Securite = system.securite,
						SecuriteClass = system.securiteClass,
						RegionName = system.regionName
					}).FirstAsync();

					Console.WriteLine("Add system : " + system.solarSystemName);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR on create VERTICES : " + ex.Message);
			}
		}

		/// <summary>
		/// Create Edges between systems
		/// </summary>
		/// <param name="allJumps"></param>
		/// <returns></returns>
		internal async Task CreateEdges(List<Jumps> allJumps)
		{
			try
			{
				foreach (var jump in allJumps)
				{
					var systemDepart = await _g.V<SystemSolar>().Where(x => x.SolarSystemID == jump.FromSystemID);
					var systemArrive = await _g.V<SystemSolar>().Where(x => x.SolarSystemID == jump.ToSystemID);

					if (systemDepart.Length == 0 || systemArrive.Length == 0)
					{
						continue;
					}

					await _g.V(systemDepart.First().Id!)
							.AddE<JumpEdge>()
							.To(__ => __.V(systemArrive.First().Id!))
							.FirstAsync();

					Console.WriteLine("Add relationship : " + systemDepart.First().SolarSystemName + " to " + systemArrive.First().SolarSystemName);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERREUR on create EDGES : " + ex.Message);
			}
		}

		/// <summary>
		/// Get all solar system in this region.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal async Task GetRegion(string name)
		{

			var test = await _g.V<SystemSolar>().Where(x => x.RegionName == name).ToArrayAsync();

			Console.WriteLine("There are " + test.Length + " solar system in region : " + name);
			await Task.Delay(2000);
			foreach (var item in test)
			{
				Console.WriteLine("System name : " + item.SolarSystemName);
			}
		}

		/// <summary>
		/// Get all solar system in a region and with min security
		/// une sécurité minimum.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal async Task GetRegion(string name, double securiteMin)
		{

			var test = await _g.V<SystemSolar>().Where(x => x.RegionName == name && x.Securite >= securiteMin).ToArrayAsync();

			Console.WriteLine("There are " + test.Length + " system solar in this region : " + name
								+ " with min security : " + securiteMin);
			await Task.Delay(2000);
			foreach (var item in test)
			{
				Console.WriteLine($"System name : {item.SolarSystemName} - security : {item.Securite}");
			}
		}

		internal async Task GetItineraire(string startSystm, string arriveSystem)
		{
			try
			{
				//NOTE : Request Gremlin I will !
				//g.V().has('name', 'Airaken')
				//.repeat(out ().simplePath())
				//.until(has('name', 'Reisen'))
				//.path().limit(1)

				// View the request Gremlin
				//var requete = _g.V<SystemSolar>(depart.Id)
				//					.RepeatUntil(rep => rep.Out().SimplePath().Cast<SystemSolar>(),
				//								until => until.Where(x => x.SolarSystemName == arriveSystem))
				//					.Path().Limit(1)//;
				//.Debug();

				var paths = await _g.V<SystemSolar>().Where(x => x.SolarSystemName == startSystm)
									.RepeatUntil(repeat => repeat.Out().SimplePath().Cast<SystemSolar>(),
												until => until.Where(x => x.SolarSystemName == arriveSystem))
									.Path().Limit(1);

				foreach (var item in paths)
				{
					//item.Objects
					//foreach (var pp in item.Objects)
					//{
					//	var tt = (SystemSolar)pp;
					//	Console.WriteLine("FINDING : " + tt.SolarSystemName);
					//}
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Get all system solar connected to this.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal async Task GetSystemVoisin(string name)
		{
			Console.WriteLine($"System connected to {name}");

			var test = await _g.V<SystemSolar>().Where(x => x.SolarSystemName == name)
								.Out<JumpEdge>()
								.OfType<SystemSolar>()
								.ToArrayAsync();

			foreach (var item in test)
			{
				Console.WriteLine("System name : " + item.SolarSystemName);
			}
		}
	}
}
