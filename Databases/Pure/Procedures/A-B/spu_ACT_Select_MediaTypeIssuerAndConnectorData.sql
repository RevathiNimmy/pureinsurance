SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_MediaTypeIssuerAndConnectorData'
GO

CREATE PROCEDURE spu_ACT_Select_MediaTypeIssuerAndConnectorData 
    @mediatype_issuer_id int
AS

SELECT
    mti.csv_number_length,
    mtc.code,
    mti.min_amount,
    mti.max_amount,
	mtc.description
	
FROM MediaType_Issuer mti
LEFT JOIN MediaType_Connector mtc ON (mti.mediatype_connector_id = mtc.mediatype_connector_id) AND (getdate() >= mti.effective_date) AND (mti.is_deleted = 0)
WHERE mti.mediatype_issuer_id = @mediatype_issuer_id

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO