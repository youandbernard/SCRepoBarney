namespace SnomedApi
{
    public class SnomedApiConfiguration
    {
        public string BaseUrl { get; set; }
        public string IsEnable { get; set; }
        public string Branch { get { return "MAIN"; } }
        public string Language { get { return "en"; } }
        public string BrowserConceptProcedureTypeKey { get { return "procedure site"; } }
        public string BrowserConceptMethodKey { get { return "method"; } }
    }
}
