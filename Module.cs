﻿using System;

using baseVISION.Tool.Connectors.Zoho.Contracts;
using baseVISION.Tool.Connectors.Zoho.Model;
using RestSharp;
namespace baseVISION.Tool.Connectors.Zoho
{
    public class Module<T> where T : IZohoRecord
    {
        ZohoClient client = null;
        string module = "";
        public Module(ZohoClient client, string module)
        {
            this.client = client;
            this.module = module;
        }
        public Result<T> List(int page = 1, SortOrder order = SortOrder.asc, string sortBy = "Company")
        {
            RestRequest r = new RestRequest("crm/v2/" + module, Method.GET);
            r.AddQueryParameter("page", page.ToString());
            if (!String.IsNullOrWhiteSpace(sortBy))
            {
                r.AddQueryParameter("sort_order", order.ToString());
                r.AddQueryParameter("sort_by", sortBy);
            }
            r.JsonSerializer = client.serializer;
            return client.Execute<Result<T>>(r);
        }
        public Result<T> Search(string keyword, int page = 1)
        {
            RestRequest r = new RestRequest("crm/v2/" + module + "/search", Method.GET);
            r.AddQueryParameter("word", keyword);
            r.AddQueryParameter("page", page.ToString());
            r.JsonSerializer = client.serializer;
            return client.Execute<Result<T>>(r);
        }
        public Result<T> SearchByCriteria(string criteria, int page = 1)
        {
            RestRequest r = new RestRequest("crm/v2/" + module + "/search", Method.GET);
            r.AddQueryParameter("criteria", criteria);
            r.AddQueryParameter("page", page.ToString());
            r.JsonSerializer = client.serializer;
            return client.Execute<Result<T>>(r);
        }
        public Result<T> Get(string id)
        {
            RestRequest r = new RestRequest("crm/v2/" + module + "/{id}", Method.GET);
            r.AddUrlSegment("id", id);
            r.JsonSerializer = client.serializer;
            return client.Execute<Result<T>>(r);
        }
        public Result<ActionResult> Add(T record)
        {
            RestRequest r = new RestRequest("crm/v2/" + module, Method.POST);
            
            Input<T> i = new Input<T>();
            i.Data.Add(record);
            r.JsonSerializer = client.serializer;
            r.AddJsonBody(i);
            return client.Execute<Result<ActionResult>>(r);
        }
        public Result<ActionResult> Update(T record)
        {
            RestRequest r = new RestRequest("crm/v2/" + module + "/{id}", Method.PUT);
            r.AddUrlSegment("id", record.Id);
            Input<T> i = new Input<T>();
            i.Data.Add(record);
            r.JsonSerializer = client.serializer;
            r.AddJsonBody(i);
            return client.Execute<Result<ActionResult>>(r);
        }
    }
}
