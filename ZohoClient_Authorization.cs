﻿using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;


namespace baseVISION.Tool.Connectors.Zoho
{
    public partial class ZohoClient
    {
        private void CheckToken()
        {
            if(Token == null || Token.ExpiryTime <= DateTime.Now.AddSeconds(60))
            {
                RefreshToken();
            }
        }
        private void RefreshToken()
        {
            RestSharp.RestRequest r = new RestSharp.RestRequest("oauth/v2/token", Method.POST);
            r.AlwaysMultipartFormData = true;
            r.AddParameter("client_id", clientId, ParameterType.GetOrPost);
            r.AddParameter("client_secret", clientSecret, ParameterType.GetOrPost);
            r.AddParameter("refresh_token", refreshToken, ParameterType.GetOrPost);
            r.AddParameter("grant_type", "refresh_token", ParameterType.GetOrPost);
            r.RequestFormat = DataFormat.Json;

            var response = restTokenClient.Execute<ZohoTokenInformation>(r);
            ResponseErrorCheck(response);
            Token = response.Data;
            InitializeDataClient();
        }
    }
}
