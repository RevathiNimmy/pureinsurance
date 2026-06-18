SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFScheme_saa'
GO
CREATE PROCEDURE spe_PFScheme_saa
AS

SELECT
    CompanyNo,
    SchemeNo,
    SchemeVersion,
    Party_Cnt,
    DataModelCode,
    StartDate,
    EndDate,
    PaymentMethod_Cnt,
    SystemTag,
    SchemeName,
    SchemeDescription,
    QuoteableInd,
    BasisOfCalcNew,
    BasisOfCalcMTA,
    BasisOfCalcRenewal,
    BasisOfCalcCancel,
    BasisOfPP,
    QuoteDocID,
    BankDocID,
    CreditDocID,
    InsrMailBoxNo,
    EdiMessageCount,
    NoOfInstallments,
    IsInHouse,
    ImmediateBankDetails
FROM
    PFScheme
ORDER BY
    schemeno ASC,
    schemeversion ASC
GO

