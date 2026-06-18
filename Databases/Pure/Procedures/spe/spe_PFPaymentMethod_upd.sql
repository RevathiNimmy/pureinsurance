SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFPaymentMethod_upd'
GO
CREATE PROCEDURE spe_PFPaymentMethod_upd
    @PFPaymentMethod_cnt INT,
    @Directory VARCHAR(255),
    @Filename VARCHAR(50),
    @Header VARCHAR(512),
    @Detail VARCHAR(255),
    @Footer VARCHAR(512),
    @Delimeter VARCHAR(3),
    @ASCIIValue TINYINT,
    @AmountInPence TINYINT,
    @QuoteCharacter VARCHAR(1),
    @QuotedNumerics TINYINT,
    @QuotedStrings TINYINT,
    @AccountNumbersOnly TINYINT,
    @DateFormat VARCHAR(20),
    @AllowAutoPost TINYINT,
    @DisableExport TINYINT,
    @UseZeroInstalment TINYINT,
    @ExcludeAUDIS Tinyint
AS

UPDATE
    PFPaymentMethod
SET
    Directory = @Directory,
    Filename = @Filename,
    Header = @Header,
    Detail = @Detail,
    Footer = @Footer,
    Delimeter = @Delimeter,
    ASCIIValue = @ASCIIValue,
    AmountInPence = @AmountInPence,
    QuoteCharacter = @QuoteCharacter,
    QuotedNumerics = @QuotedNumerics,
    QuotedStrings = @QuotedStrings,
    AccountNumbersOnly = @AccountNumbersOnly,
    DateFormat = @DateFormat,
    AllowAutoPost = @AllowAutoPost,
    DisableExport = @DisableExport,
    UseZeroInstalment = @UseZeroInstalment,
    ExcludeAudis = @ExcludeAudis
WHERE
    PFPaymentMethod_cnt = @PFPaymentMethod_cnt
GO

