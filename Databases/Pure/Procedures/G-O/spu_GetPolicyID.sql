SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetPolicyID'
GO


CREATE PROCEDURE spu_GetPolicyID
    @PFPrem_finance_cnt INT,
    @PFPrem_finance_version INT
AS


SELECT	
    ifi.insurance_file_cnt, ifi.insurance_folder_cnt
FROM	
    PFPremiumFinance pf
INNER JOIN
	Insurance_File ifi 
    ON 
        pf.insurance_file_cnt = ifi.insurance_file_cnt
WHERE	
    pf.PFPrem_finance_cnt = @PFPrem_finance_cnt
AND	pf.PFPrem_finance_version = @PFPrem_finance_version
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


