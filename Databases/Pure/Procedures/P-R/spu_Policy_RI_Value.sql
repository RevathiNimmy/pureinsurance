SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Policy_RI_Value'
GO


CREATE PROCEDURE spu_Policy_RI_Value
    @insurance_file_cnt int
AS

/********************************************************************************************************
-- Desc : (POLICY LEVEL REINSURANCE)
--        return number of policies and total reinsurance value for these policies
-- Hist : 19/07/2001 TN - Created
**********************************************************************************************************/
SELECT  COUNT(DISTINCT(ifra.ins_file_ri_arrangement_id)),
        SUM(ral.this_share_percent)
FROM    ins_file_ri_arrangement ifra,
        ri_arrangement_line ral
WHERE   ifra.insurance_file_cnt = @insurance_file_cnt
AND     ifra.ins_file_ri_arrangement_id = ral.ins_file_ri_arrangement_id
GO


