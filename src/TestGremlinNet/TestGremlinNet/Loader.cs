﻿using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Remote;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;
using TestGremlinNet.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Structure.IO.GraphSON;

namespace TestGremlinNet
{
	public class Loader : IDisposable
	{
		private GraphTraversalSource GremlinRequest;
		private GremlinClient ClientGremlin;


		private const string LABEL_VERTEX = "SystemSolar";
		private const string EDGE_NAME = "jumpTo";



		public Loader()
		{
			ClientGremlin = new GremlinClient(new GremlinServer("localhost", 8180));
			GremlinRequest = AnonymousTraversalSource.Traversal().WithRemote(new DriverRemoteConnection(ClientGremlin));
		}

		/// <summary>
		/// Supprime tous les Vextices
		/// </summary>
		public void DropDatabase()
		{
			try
			{
				GremlinRequest.V().Drop().Iterate();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR : {ex.Message}");
				throw;
			}
		}

		/// <summary>
		/// Ajoute les Vertices et les Edges.
		/// </summary>
		/// <param name="allSolarSystems"></param>
		/// <param name="allJumps"></param>
		/// <returns></returns>
		internal Task PopulateGraphAsync(List<SolarSystem> allSolarSystems, List<Jumps> allJumps)
		{
			try
			{
				return Task.Factory.StartNew(() => 
				{
					Console.WriteLine("--> Create all solar systems.");
					int iSytem = 0;
					int totalSystem = allSolarSystems.Count;
					foreach (var system in allSolarSystems)
					{
						GremlinRequest.AddV(LABEL_VERTEX)
									.Property("SolarSystemId", system.SolarSystemId)
									.Property("SolarSystemName", system.SolarSystemName)
									.Property("Securite", system.Securite)
									.Property("RegionName", system.RegionName)
									.As(system.SolarSystemId.ToString())
									.Iterate();

						Console.WriteLine($"--> System {iSytem++} on {totalSystem} created...");
					}

					Console.WriteLine("Create all Jumps between systems.");
					int totalJump = allJumps.Count;
					int counterJump = 0;

					foreach (var jump in allJumps)
					{
						var debut = GremlinRequest.V().HasLabel(LABEL_VERTEX).Has("SolarSystemId", jump.FromSystemID);
						var arrive = GremlinRequest.V().HasLabel(LABEL_VERTEX).Has("SolarSystemId", jump.ToSystemID);
						if (debut != null && arrive != null)
						{

						
						GremlinRequest.V().HasLabel(LABEL_VERTEX).Has("SolarSystemId", jump.FromSystemID)
									.AddE("jumpTo")
									//.To(GremlinRequest.V().Has("SolarSystemId", jump.ToSystemID))
									.To(__.V().Has("SolarSystemId", jump.ToSystemID))
									.Iterate();

						Console.WriteLine($"--> Jump {counterJump++} sur {totalJump} créé...");
						}
					}
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR : {ex.Message}");
				throw;
			}
		}


		internal Task<List<SolarSystem>> Test(string depart, string arrive)
		{
			return Task.Factory.StartNew(() =>
			{
				List<SolarSystem> systemsRegion = new List<SolarSystem>();

				try
				{
					var allSystems = GremlinRequest.V().HasLabel("SystemSolar").Has("SolarSystemName", depart)
											.Repeat(__.Out().SimplePath())
											.Until(__.HasLabel("SystemSolar").Has("SolarSystemName", arrive))
											.Path()
											.Limit<Path>(1)
											.Project<Object>("SolarSystemId", "SolarSystemName", "Securite", "RegionName")
											.By("SolarSystemId")
											.By("SolarSystemName")
											.By("Securite")
											.By("RegionName")
											.ToList();

					foreach (var item in allSystems)
					{
						SolarSystem system = new SolarSystem();

						foreach (var prop in item)
						{
							// Key : correspond au nom de la propriété
							// Value : la valeur de la propriété
							var property = typeof(SolarSystem).GetProperty(prop.Key);
							property.SetValue(system, prop.Value);
						}

						systemsRegion.Add(system);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine("#############");
					Console.WriteLine(ex.StackTrace);
				}

				return systemsRegion;
			});
		}

		public void Dispose()
		{
			ClientGremlin.Dispose();
		}
	}
}
