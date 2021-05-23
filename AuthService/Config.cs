// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthService
{
    //https://identityserver4.readthedocs.io/en/latest/topics/resources.html
    public static class Config
    {
        //What a client application is allowed to do
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("Microservice2.Read"),
                new ApiScope("Microservice2.Write"),
                new ApiScope("Microservice2.FullAccess"),

            };


        //Identity server needs to know ,which all clients are allowed to use it.
        //These are clients that need to access the ApiResources
        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                new Client
                {
                    ClientId = "Microservice1_ClientID",
                    ClientName = "Microservice1",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("Microservice1_Secret".Sha256()) },

                    AllowedScopes = { "Microservice2.Read" }
                },

            };


        //Information that includes user info ike user id , email.
        //We can asign claim types linked to them.
        //This info is defined in the identity token.
        //An identity resource is a named group of claims that can be requested using the scope parameter.        
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
                    new IdentityResources.OpenId(),//When OpenId is requested, Subject Id will be returned
                    new IdentityResources.Profile(),//When profile is requested, profile related data is returned
        };



        //Apiresource
        //When the API surface gets larger, a flat list of scopes like the one used above might not be feasible.
        //You typically need to introduce some sort of namespacing to organize the scope names, 
        //and maybe you also want to group them together and get some higher-level constructs like an audience claim in access tokens.
        //You might also have scenarios, where multiple resources should support the same scope names, 
        //whereas sometime you explicitly want to isolate a scope to a certain resource.
        public static IEnumerable<ApiResource> ApiResources =>
           new ApiResource[]
           {
               new ApiResource("Microservice2.Audience")
               {
                   // Pass the scopes that will come under this audience

                   Scopes= { "Microservice2.Read" }
               }

           };










    }
}