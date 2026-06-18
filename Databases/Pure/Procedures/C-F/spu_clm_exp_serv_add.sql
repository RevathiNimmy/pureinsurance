SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_exp_serv_add'
GO

--*******************************************************************************************    
-- Version      Author  Date        Desc    
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting    
-- 1.00.0002    Thinh Nguyen 24/03/2003 change @Service and @Reference to varchar(50)    
--*******************************************************************************************    
    
CREATE PROCEDURE spu_clm_exp_serv_add    
    @Claim_Expert_Service_id int OUTPUT,    
    @Claim_id int,    
    @Expert_Service_id int,    
    @Party_Claim_id int,    
    @Service_type_id int,    
    @Service varchar(50),    
    @Description varchar(255),    
    @Reference varchar(50),    
    @Contact varchar(255),    
    @Date_requested datetime,    
    @Date_critical datetime,    
    @Date_received datetime    
AS    
 UPDATE Claim 
 SET Last_modified_date = Getdate()
 WHERE Claim_id = @claim_id   

 INSERT INTO Claim_Expert_Service(  
  Claim_id,   
  Expert_Service_id,    
  Party_Claim_id,   
  Service_type_id,    
  Service,    
  Description,    
  Reference,    
  Contact,    
  Date_requested,   
  Date_critical,    
  Date_received)    
 VALUES (@Claim_id,   
  @Expert_Service_id ,    
  @Party_Claim_id,   
  @Service_type_id ,    
  @Service,  
  @Description ,    
  @Reference,  
  @Contact ,    
  @Date_requested,    
  @Date_critical,    
  @Date_received )  
    
SELECT @Claim_Expert_Service_id = @@IDENTITY    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
