SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_deleteperil'
GO

CREATE PROCEDURE spu_deleteperil  
    @ClaimPerilId integer  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--*******************************************************************************************  
DECLARE @AgentUnderwriter varchar(1)  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter = 'A'  
  BEGIN  
    UPDATE Claim 
    SET Last_modified_date = Getdate()
    WHERE Claim_id = 
    (SELECT DISTINCT Claim_id 
    FROM claim_peril 
    WHERE claim_peril_id =@ClaimPerilId) 

    DELETE FROM claim_payment_item WHERE claim_payment_id IN (SELECT claim_payment_id FROM claim_payment WHERE claim_peril_id = @ClaimPerilId AND ( amount is null or amount = 0 ))  
	DELETE FROM Tax_calculation WHERE claim_payment_id IN (SELECT claim_payment_id FROM claim_payment WHERE claim_peril_id = @ClaimPerilId AND ( amount is null or amount = 0 ))  
    DELETE FROM claim_payment where claim_peril_id = @ClaimPerilId and ( amount is null or amount = 0 )  
    DELETE FROM reserve where claim_peril_id = @ClaimPerilId and ( initial_reserve is null or initial_reserve = 0 ) and ( paid_to_date is null or paid_to_date = 0 ) and ( revised_reserve is null or revised_reserve = 0 )  
    delete from Claim_Peril  
    where Claim_Peril_ID = @ClaimPerilId  
  END  
ELSE  
    DELETE FROM claim_peril WHERE claim_peril_id = @ClaimPerilId  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
