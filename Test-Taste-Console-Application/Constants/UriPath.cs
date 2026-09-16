namespace Test_Taste_Console_Application.Constants
{
    public static class UriPath
    {
        public const string BaseUri = "https://api.le-systeme-solaire.net/rest/";
        private const string BodiesUri = "bodies";
        public const string GetAllPlanets = BodiesUri + "?data=id,semimajorAxis,moons&filter[]=isPlanet,eq,true";
        public const string GetAllMoons = BodiesUri + "?data=id,mass,massValue,massExponent,avgTemp,aroundPlanet,planet,rel" + "&filter[]=bodyType,eq,Moon";
    }
}