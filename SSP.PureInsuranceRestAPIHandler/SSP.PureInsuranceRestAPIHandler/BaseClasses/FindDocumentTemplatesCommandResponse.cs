using System;
using System.Collections.Generic;
using System.Text;

namespace SSP.PureInsuranceRestAPIHandler.BaseClasses
{
    public class FindDocumentTemplatesCommandResponse /*: BasePagedResponse*/
    {
        public List<BaseFindDocumentTemplatesResponseTypeRow> DocumentTemplates { get; set; }
    }
}
