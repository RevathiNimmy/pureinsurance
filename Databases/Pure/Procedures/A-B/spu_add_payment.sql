SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_payment'
GO

CREATE PROCEDURE spu_add_payment  
    @paymentid int OUTPUT,  
    @reserveid int,  
    @perilid int,  
    @claimid int,  
    @CreatedBy smallint  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--  AK 21/05/2003  Added another parametr to capture user id  
--*******************************************************************************************  
Declare @currencyid int  
  
DECLARE @AgentUnderwriter varchar(1)  
DECLARE @claim_payment_id integer  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    BEGIN  
  
        Select @currencyid=currency_id from Claim where claim_id=@claimid  
  
 INSERT INTO claim_payment (  
   claim_id,  
   claim_peril_id,  
   date_of_Payment,  
   created_by,  
   currency_id  
   )  
  
  VALUES (  
   @claimid,  
   @perilid,  
   getdate(),  
   @createdby,  
   @currencyid  
   )  
  
  SELECT @claim_payment_id =  @@IDENTITY  
  
  
  
       Insert into claim_payment_item(  
        claim_payment_id,  
        reserve_id,  
        this_payment,  
        tax_amount)  
        values(  
        @claim_payment_id,  
 @reserveid,  
        0,  
        0  
 )  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
