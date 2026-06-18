SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFPaymentMethod_sel'
GO
CREATE PROCEDURE spe_PFPaymentMethod_sel
    @PFPaymentMethod_cnt INT
AS

SELECT
    Description,
    Directory,
    Filename,
    Header,
    Detail,
    Footer,
    Delimeter,
    ASCIIValue,
    AmountInPence,
    QuoteCharacter,
    QuotedNumerics,
    QuotedStrings,
    AccountNumbersOnly,
    DateFormat,
    AllowAutoPost,
    DisableExport,
    UseZeroInstalment,
    mediatype_validation_id,
    ExcludeAudis
FROM
    PFPaymentMethod
WHERE
    PFPaymentMethod_cnt = @PFPaymentMethod_cnt
GO

