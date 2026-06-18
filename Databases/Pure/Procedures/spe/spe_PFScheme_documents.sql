SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_documents'
GO
CREATE PROCEDURE spe_PFScheme_documents
    @CompanyNo int,
    @SchemeNo int,
    @SchemeVersion int
AS

SELECT
    QuoteDocID,
    BankDocID,
    CreditDocID
FROM
    PFScheme
WHERE
    CompanyNo = @CompanyNo
    AND SchemeNo = @SchemeNo
    AND SchemeVersion = @SchemeVersion
ORDER BY
    schemeno ASC,
    schemeversion ASC

GO

