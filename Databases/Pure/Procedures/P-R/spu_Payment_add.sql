SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Payment_add'
GO

CREATE PROCEDURE spu_Payment_add  

    @payment_id int OUTPUT,  
    @Peril_id int,  
    @recovery_id int,  
    @recovery_type_id int,  
    @Claim_id int,  
    @Currency_id int,  
    @Party_Claim_id int,  
    @Amount currency,  
    @Date_of_payment datetime,  
    @Comments varchar(255)  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
DECLARE @AgentUnderwriter varchar(1)  
  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
    INSERT INTO Payment (  
     claim_peril_id ,
     recovery_id,  
     recovery_type_id,  
     claim_id,  
     currency_id,  
     Party_cnt,  
     Amount ,  
     Date_of_payment,  
     Comments  
     )  
    VALUES (  
     @peril_id ,  
     @recovery_id,  
     @recovery_type_id,  
     @claim_id,  
     @currency_id,  
     @Party_Claim_id ,  
     @Amount,  
     @Date_of_payment,  
     @comments  
     )  
ELSE  
--UNDERWRITING  
    INSERT INTO Payment  
    (  
    claim_peril_id , recovery_id,  
    recovery_type_id,  
    claim_id,  
    currency_id,  
    Party_cnt,  
    Amount ,  
    Date_of_payment,  
    Comments  
    )  
    VALUES  
    (  
    @peril_id ,  
    @recovery_id,  
    @recovery_type_id,  
    @claim_id,  
    @currency_id,  
    @Party_Claim_id ,  
    @Amount,  
    @Date_of_payment,  
    @comments  
    )  
  
SELECT @payment_id = @@IDENTITY  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
