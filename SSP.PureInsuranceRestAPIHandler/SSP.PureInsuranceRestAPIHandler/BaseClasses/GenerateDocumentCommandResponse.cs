using System.Collections.Generic;
using System.Linq;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class GenerateDocumentCommandResponse
    {
        public IEnumerable<GenerateDocumentCommandBaseResponse> GenerateDocumentResponse { get; set; } = Enumerable.Empty<GenerateDocumentCommandBaseResponse>();
        public string MergedFilePath { get; set; }
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        public byte[] SpooledZipFile { get; set; }
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        public System.Collections.Generic.List<SAMErrors> Errors { get; set; }
        public STSErrorType STSError { get; set; }
        public List<BaseGenerateDocumentV2ResponseTypeDocument> SplitDocuments { get; set; } = new List<BaseGenerateDocumentV2ResponseTypeDocument>();
    }
}
