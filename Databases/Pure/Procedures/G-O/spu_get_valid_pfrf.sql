SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_valid_pfrf'
GO

CREATE PROCEDURE spu_get_valid_pfrf
    @MediaTypeID int,
    @FrequencyID int,
    @Date DATETIME,
    @ProductCode VARCHAR(20)
AS
BEGIN

    SELECT TOP 1 Frequency =
        CASE PF.period
        WHEN 'w' THEN 52/PF.amount
        WHEN 'm' THEN 12/PF.amount
        END,
    PFRF.CompanyNo, PFRF.SchemeNo, PFRF.SchemeVersion, PFRF.DaysDelay,
    PFRF.start_limit, PFRF.align_to, PF.Period, PFRF.deposit_type, PFRF.DepositPC,
    PFRF.deposit_charged_to, PFRF.PFRF_id, PFRF.MinInterest, PFRF.protection_type, PFRF.ProtectRate,
    PFRF.Min1, PFRF.Max1, PFRF.Rate1, PFRF.R1Com, null obselete1,
    PFRF.Min2, PFRF.Max2, PFRF.Rate2, PFRF.R2Com, null obselete2,
    PFRF.Min3, PFRF.Max3, PFRF.Rate3, PFRF.R3Com, null obselete3,
    PFRF.Min4, PFRF.Max4, PFRF.Rate4, PFRF.R4Com, null obselete4,
    PFRF.Min5, PFRF.Max5, PFRF.Rate5, PFRF.R5Com, null obselete5,
    PFRF.fee_type, PFRF.ArrangementFee, PFRF.tax_charged_to, PFRF.fee_charged_to, PFRF.protection_charged_to,
    SCH.SchemeName, PFRF.pffrequency_id, PF.description, MT.mediatype_id, MT.description,
    PFRF.ProductFamily, ST.Code, MTV.Code, Party.Name, PF.Amount, PFRF.remainder_amount_threshhold, PFRF.remainder_amount_at_end
    FROM PFRF
      INNER JOIN PFScheme AS SCH ON SCH.CompanyNo = PFRF.CompanyNo AND SCH.SchemeNo = PFRF.SchemeNo
            AND SCH.SchemeVersion = PFRF.SchemeVersion
          INNER JOIN MediaType AS MT ON MT.mediatype_id = SCH.mediatype_id
          INNER JOIN PFFrequency AS PF ON PF.pFFrequency_id = PFRF.pFFrequency_id
      INNER JOIN MediaType_Validation AS MTV ON MT.mediatype_validation_id = MTV.mediatype_validation_id
      INNER JOIN PFScheme_Type AS ST ON SCH.pfscheme_type_id = ST.pfscheme_type_id
      LEFT OUTER JOIN Party ON SCH.Party_Cnt = Party.Party_Cnt

WHERE PFRF.pffrequency_id = @FrequencyID
AND PFRF.ProductFamily = @ProductCode
AND SCH.mediatype_id = @MediaTypeID
AND SCH.StartDate <= @Date AND SCH.EndDate >= @Date
AND PFRF.StartDate <= @Date AND PFRF.EndDate >= @Date
AND SCH.QuoteableInd='Y'

END
GO