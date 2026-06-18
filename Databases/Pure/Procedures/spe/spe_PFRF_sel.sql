SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PFRF_sel'
GO
CREATE PROCEDURE spe_PFRF_sel
    @CompanyNo int,
    @SchemeNo int,
    @SchemeVersion int,
    @StartDate datetime,
    @ProductFamily char(1)
AS
SELECT
    CompanyNo,
    SchemeNo,
    SchemeVersion,
    StartDate,
    ProductFamily,
    ArrangementFee,
    Mnemonic,
    EndDate,
    Protect,
    DaysDelay,
    DepositReq,
    DepositPC,
    AllowProtection,
    ProtectRate,
    MinInterest,
    Min1,
    Max1,
    Rate1,
    APR1,
    R1Com,
    APR1Com,
    Com1PC,
    Min2,
    Max2,
    Rate2,
    APR2,
    R2Com,
    APR2Com,
    Com2PC,
    Min3,
    Max3,
    Rate3,
    APR3,
    R3Com,
    APR3Com,
    Com3PC,
    Min4,
    Max4,
    Rate4,
    APR4,
    R4Com,
    APR4Com,
    Com4PC,
    Min5,
    Max5,
    Rate5,
    APR5,
    R5Com,
    APR5Com,
    Com5PC,
    AlLowOveride,
    MinMTA,
    MinMTAInstalments
FROM PFRF
WHERE
    CompanyNo = @CompanyNo
AND SchemeNo = @SchemeNo
AND SchemeVersion = @SchemeVersion
AND StartDate = @StartDate
AND ProductFamily = @ProductFamily
GO

