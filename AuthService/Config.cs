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
                new ApiScope("read"),
                new ApiScope("write"),
                new ApiScope("read/write"),

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

                    AllowedScopes = { "read" }
                },



                //// m2m client credentials flow client
                //new Client
                //{
                //    ClientId = "m2m.client",
                //    ClientName = "Client Credentials Client",

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                //    AllowedScopes = { "scope1" }
                //},

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
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

                   Scopes= { "read" }
               }

           };










    }
}