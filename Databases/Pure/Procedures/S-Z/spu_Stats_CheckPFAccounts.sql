SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Stats_CheckPFAccounts'
GO


CREATE PROCEDURE spu_Stats_CheckPFAccounts
    @insurance_file_cnt INT
AS BEGIN

    SELECT ISNULL(s.interest_account_id,0) ,ISNULL(s.tax_suspense_account_id,0)
    FROM Insurance_File i, PFPremiumFinance pf, PFScheme s
    WHERE 
        i.insurance_file_cnt = pf.insurance_file_cnt
    AND pf.CompanyNo = s.CompanyNo
    AND pf.SchemeNo = s.SchemeNo
    AND pf.SchemeVersion = s.SchemeVersion
    AND i.insurance_file_cnt = @insurance_file_cnt
END
GO



