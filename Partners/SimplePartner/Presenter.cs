namespace dotnet_starter.Partner.SimplePartners
{

    //---------------- model request ----------------//
    public class SimplePartnerRequest
    {
        public int Id { get; set; }

    }
    //---------------- model request ----------------//

    //---------------- model respones ----------------//
    public class SimplePartnerResponses
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    //---------------- model respones ----------------//
}