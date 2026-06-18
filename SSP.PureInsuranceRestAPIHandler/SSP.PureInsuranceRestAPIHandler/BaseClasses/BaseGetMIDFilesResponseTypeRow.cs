namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetMIDFilesResponseTypeRow
    {
        public System.DateTime DateGenerated { get; set; }
        public string FileName { get; set; }
        public string FileSequenceNumber { get; set; }
        public int MIDFileKey { get; set; }
        public bool MIDFileKeySpecified { get; set; }
        public string StatusDescription { get; set; }
    }
}
