﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sensorthings.Core;
using sensorthings.ODATA;
using SensorThings.Client;

namespace SensorThings.Core
{
    public class ObservedProperty : AbstractEntity
    {
        private string _description;
        private string _definition;
        private string _name;
        private string _datastreamsNavigationLink;
        private ObservableCollection<Datastream> _datastreams;

        [JsonProperty("description")]
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        [JsonProperty("definition")]
        public string Definition
        {
            get => _definition;
            set => SetProperty(ref _definition, value);
        }

        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        [JsonProperty("Datastreams@iot.navigationLink")]
        public string DatastreamsNavigationLink
        {
            get => _datastreamsNavigationLink;
            set => SetProperty(ref _datastreamsNavigationLink, value);
        }

        [JsonProperty("Datastreams")]
        public ObservableCollection<Datastream> Datastreams
        {
            get => _datastreams;
            set => SetProperty(ref _datastreams, value);
        }

        public async Task<Response<SensorThingsCollection<Datastream>>> GetDatastreams(SensorThingsClient client, OdataQuery odata = null)
        {
            if (!string.IsNullOrEmpty(DatastreamsNavigationLink))
            {
                return await Http.GetJson<SensorThingsCollection<Datastream>>(DatastreamsNavigationLink);
            }

            return await client.GetDatastreamCollectionByObservedProperty(Id, odata);
        }
    }
}
