
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Delete_AgencyProduct'
GO

-- =============================================
-- Author:		Gauri kapoor
-- Create date: 29 July 10
-- Description:	Deletes party_cnt from party_agent_product for the party
-- =============================================
CREATE PROCEDURE Spu_sir_delete_agencyproduct
-- Add the parameters for the stored procedure here
@party_cnt INT,
@product_id INT = 0,
@user_id INT,  
@unique_id VARCHAR(50) = '',  
@screen_hierarchy VARCHAR(500) = '' 
AS  
  
BEGIN
UPDATE  party_agent_product  
SET UserId = @user_id, UniqueId = @unique_id, ScreenHierarchy = @screen_hierarchy  
WHERE  
party_cnt = @Party_Cnt 


      -- Remove the Party_cnt from party_agent_product
DELETE FROM party_agent_product
WHERE  party_cnt = @party_cnt

END

GO  