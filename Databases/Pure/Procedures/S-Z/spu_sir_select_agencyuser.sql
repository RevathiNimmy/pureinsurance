SET ansi_nulls ON

GO

SET quoted_identifier ON

GO

EXECUTE Ddldropprocedure 'spu_sir_select_agencyuser'

GO

-- =============================================
-- Author:    Gauri kapoor
-- Create date: 29 July 10
-- Description:  @party_cnt and @product_id to insert a record into party_agent_product 
-- =============================================
CREATE PROCEDURE Spu_sir_select_agencyuser  
-- Add the parameters for the stored procedure here  
@party_cnt INT,  
@deleted_users INT =NULL  
AS  
  BEGIN  
      -- Insert statements for procedure here  
      SELECT user_id,  
             username,  
             full_name ,  
             email_address ,  
             effective_date   
      FROM   pmuser pmu  
             INNER JOIN party_agent pa  
               ON pa.party_cnt = pmu.party_cnt  
      WHERE  pmu.party_cnt = @party_cnt  
      AND  
      (  
      (@deleted_users IS NULL)  
      OR  
      (@deleted_users IS NOT NULL AND is_deleted =@deleted_users )) and  pmu.effective_date <= getdate() 
  END
 GO