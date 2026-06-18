SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_exp_serv_upd'
GO

CREATE PROCEDURE spu_clm_exp_serv_upd  
    @Claim_Expert_Service_id int,  
    @Claim_id int,  
    @Expert_Service_id int,  
    @Party_Claim_id int,  
    @Service_type_id int,  
    @Service varchar(100),  
    @Description varchar(255),  
    @Reference varchar(100),  
    @Contact varchar(255),  
    @Date_requested datetime,  
    @Date_critical datetime,  
    @Date_received datetime  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting  
--  
--*******************************************************************************************  
 
    UPDATE Claim_Expert_Service  
    SET     
	Claim_id = @Claim_id ,  
	Expert_Service_id = @Expert_Service_id ,  
	Party_Claim_id = @Party_Claim_id ,  
	Service_type_id= @Service_type_id,  
	Service= @Service,  
	Description= @Description,  
	Reference = @Reference,  
	Contact = @Contact,  
	Date_requested = @Date_requested,  
	Date_critical = @Date_critical,  
	Date_received = @Date_received  
    WHERE   Claim_Expert_Service_id = @Claim_Expert_Service_id  

	
    UPDATE Claim 
    SET Last_modified_date = Getdate()
    WHERE Claim_id = @claim_id	

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
