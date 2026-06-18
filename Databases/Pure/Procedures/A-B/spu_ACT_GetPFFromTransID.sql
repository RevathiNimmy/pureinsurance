EXECUTE DDLDropProcedure 'spu_ACT_GetPFFromTransID'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_ACT_GetPFFromTransID
    @TransID int

AS BEGIN

    SELECT pfPrem_Finance_cnt,pfPrem_Finance_version, ISNULL(Spread_Commission,0) as Spread_Commission,p.Plantransaction_ID
    FROM
        TransDetail t, Insurance_File i, pfPremiumFinance p, pfScheme s
    WHERE
        t.insurance_ref = i.insurance_ref
    AND i.Insurance_File_Cnt = p.Insurance_File_Cnt
    AND p.CompanyNo = s.CompanyNo
    AND p.SchemeNo = s.SchemeNo
    AND p.SchemeVersion = s.SchemeVersion    
    AND t.transdetail_id = @TransID

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
