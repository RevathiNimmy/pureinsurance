using System.Collections.Generic;

namespace bSIRSharepointApi.Models
{
    public class SPFile
    {
        public string Name { get; set; }
        public string Folder { get; set; }
        public string Title { get; set; }
    }
    public class D
    {
        public List<SPFile> results { get; set; } = new List<SPFile>();

    }
    public class SPFileResponse
    {
        public D d { get; set; } = new D();

    }

    public class SPFileCollectionResponse
    {

        public SPFileCollectionResponse()
        {
            d = new D();
        }

        public D d { get; set; }

    }

    public class SPFileObjectResponse
    {

        public SPFile d { get; set; }

    }
}
