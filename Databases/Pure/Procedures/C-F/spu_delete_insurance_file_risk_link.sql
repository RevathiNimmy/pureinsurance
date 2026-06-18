SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_delete_insurance_file_risk_link'
GO

--*******************************************************************************************
-- Version              Author  Date            Desc
-- 1.00.0001            Tom     14/02/2002      delete the record, or just flag it as
--                                              deleted
-- 1.00.0002            Tom     21/08/2002      take account of renewals
--*******************************************************************************************

CREATE PROCEDURE spu_delete_insurance_file_risk_link
    @insurance_file_cnt int,
    @risk_cnt int

AS
BEGIN

    --If the original risk cnt is null, it's added freshly in either in NB or MTA
    --Or REN (which puts 0 in original risk cnt)
    --so we can get rid of it
    DELETE insurance_file_risk_link
    WHERE  insurance_file_cnt = @insurance_file_cnt
    AND    risk_cnt = @risk_cnt
    AND    ISNULL(original_risk_cnt, 0) = 0

    --If the original risk cnt is not null, it's updated in an MTA
    --so we have to update the flag so we can return premium etc
    UPDATE insurance_file_risk_link
    SET    status_flag = 'D'
    WHERE  insurance_file_cnt = @insurance_file_cnt
    AND    risk_cnt = @risk_cnt
    AND    ISNULL(original_risk_cnt, 0) <> 0
    
        --Clear Tax Entries
    DELETE Tax_calculation 
    WHERE risk_cnt = @risk_cnt	
	
	--Clear risk fee Entries
    DELETE policy_fee_u
    WHERE risk_cnt = @risk_cnt

END
GO
