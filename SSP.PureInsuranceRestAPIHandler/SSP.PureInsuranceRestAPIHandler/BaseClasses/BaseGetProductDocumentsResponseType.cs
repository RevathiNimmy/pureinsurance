using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class BaseGetProductDocumentsResponseType : BaseResponseType
    {
        public List<BaseGetProductDocumentsResponseTypeProductDocumentsRow> ProductDocuments { get; set; }

    }
}
