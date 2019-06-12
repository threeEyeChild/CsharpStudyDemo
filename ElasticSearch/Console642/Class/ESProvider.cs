﻿using Nest;
using System.Collections.Generic;
using System.Linq;

namespace Console642.Class
{
    class ESProvider
    {
        public static ElasticClient client = new ElasticClient(Setting.ConnectionSettings);
        public static string strIndexName = @"meetup".ToLower();
        public static string strDocType = "events".ToLower();

        public bool PopulateIndex(MeetupEvents meetupevent)
        {
            var index = client.Index(meetupevent, i => i.Index(strIndexName).Type(strDocType).Id(meetupevent.eventid));
            //return index.Created;
            return index.IsValid;
        }

        public bool BulkPopulateIndex(List<MeetupEvents> posts)
        {
            var bulkRequest = new BulkRequest(strIndexName, strDocType) { Operations = new List<IBulkOperation>() };
            var idxops = posts.Select(o => new BulkIndexOperation<MeetupEvents>(o) { Id = o.eventid }).Cast<IBulkOperation>().ToList();
            bulkRequest.Operations = idxops;
            var response = client.Bulk(bulkRequest);
            return response.IsValid;
        }
    }
}
