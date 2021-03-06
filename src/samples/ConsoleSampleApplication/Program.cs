﻿using Newtonsoft.Json.Linq;
using SensorThings.Client;
using SensorThings.Core;
using System;
using sensorthings.Core;

namespace ConsoleSampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sample console app for SensorThings API client");
            // var server = "http://scratchpad.sensorup.com/OGCSensorThings/v1.0/";
            var server = "https://gost.geodan.nl/v1.0";

            var client = new SensorThingsClient(server);

            Console.WriteLine("Create observation for datastream 18");
            var datastream = new Datastream {Id = "263"};
            var observation = new Observation
            {
                Datastream = datastream,
                PhenomenonTime = new DateTimeRange(DateTime.UtcNow),
                Result = 100
            };
            // do not create observations for now
            var returnedObservation = client.CreateObservation(observation).Result;

            Console.WriteLine("Retrieve all paged datastreams...");
            var response = client.GetDatastreamCollection().Result;
            var page = response.Result;

            var pagenumber = 1;
            while (page != null)
            {
                Console.WriteLine("---------------------------------------");
                WritePage(response.Result);
                var pageResponse = page.GetNextPage().Result;
                page = pageResponse?.Result;

                pagenumber++;
            }
            Console.WriteLine("End retrieving datastreams...");
            Console.WriteLine("Number of pages: " + pagenumber);

            var datastreamResponse = client.GetDatastream("263").Result;
            datastream = datastreamResponse.Result;
            var observationsResponse = datastream.GetObservations(client).Result;
            var observations = observationsResponse.Result;

            Console.WriteLine("Number if observations: " + observations.Count);

            Console.WriteLine("Sample with locations");
            var locationsResponse = client.GetLocationCollection().Result;
            var locations = locationsResponse.Result;

            // Get location without using GeoJSON.NET (works only for points)
            var firstlocation = locations.Items[0];
            var feature = (JObject)firstlocation.Feature;
            var lon = feature.First.First.First.Value<double>();
            var lat = feature.First.First.Last.Value<double>();
            Console.WriteLine($"Location: {lon},{lat}");

            // if using GeoJSON.NET use something like:
            // var p = JsonConvert.DeserializeObject<Point>(feature.ToString());
            //  var ipoint = (GeographicPosition)p.Coordinates;
            // Console.WriteLine("Location: " + ipoint.Longitude + ", " + ipoint.Latitude);

            Console.WriteLine("Program ends... Press a key to continue.");
            Console.ReadKey();
        }

        static void WritePage(SensorThingsCollection<Datastream> datastreams) {
            foreach (var datastream in datastreams.Items)
            {
                Console.WriteLine(datastream.Id + ": " + datastream.Description);
            }
        }
    }
}
