SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_CommissionTax'
GO


CREATE PROCEDURE spu_wp_CommissionTax
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT TP.Description as Tax_type,
tc.value as Tax_Value,
percentage as TAX_Rate

FROM tax_calculation TC

INNER JOIN Tax_Band TB ON
TB.tax_band_id = TC.tax_band_id

INNER JOIN TAX_TYPE TP ON
TP.tax_type_id = TB.tax_type_id

WHERE  TC.transtype = 'TTAC' AND TC.insurance_file_cnt = @insurancefilecnt
