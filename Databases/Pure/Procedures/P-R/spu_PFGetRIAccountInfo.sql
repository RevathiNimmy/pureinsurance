SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF 
GO


EXECUTE DDLDropProcedure 'spu_PFGetRIAccountInfo'
GO


CREATE PROCEDURE spu_PFGetRIAccountInfo
    @InsuranceFileCnt int
AS

    SELECT  tp.party_cnt, 
            a.account_id
    FROM    insurance_file_risk_link ifrl
    JOIN    RI_Arrangement ra
            ON ra.risk_cnt = ifrl.risk_cnt 
    JOIN    RI_Arrangement_Line ral
            ON ral.ri_arrangement_id = ra.ri_arrangement_id 
    JOIN    Treaty_Party tp
            ON tp.treaty_id = ral.treaty_id 
    JOIN    Account a
            ON tp.party_cnt = a.account_key
    WHERE   ifrl.insurance_file_cnt = @InsuranceFileCnt
    AND    (ral.commission_value <> 0 OR ral.premium_value <> 0) 
    UNION
    SELECT  ral.party_cnt, 
            a.account_id
    FROM    insurance_file_risk_link ifrl 
    JOIN    RI_Arrangement ra 
            ON ra.risk_cnt = ifrl.risk_cnt
    JOIN    RI_Arrangement_Line ral
            ON ral.ri_arrangement_id = ra.ri_arrangement_id 
    JOIN    Account a
            ON a.account_key = ral.party_cnt 
    WHERE   ifrl.insurance_file_cnt = @InsuranceFileCnt
    AND    (ral.commission_value <> 0 OR ral.premium_value <> 0) 

GO


