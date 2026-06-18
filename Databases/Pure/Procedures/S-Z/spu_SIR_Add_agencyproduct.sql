SET ansi_nulls ON

GO

SET quoted_identifier ON

GO

EXECUTE Ddldropprocedure 'spu_sir_add_agencyproduct'

GO

-- =============================================
-- Author:    Gauri kapoor
-- Create date: 29 July 10
-- Description:  @party_cnt and @product_id to insert a record into party_agent_product 
-- =============================================
CREATE PROCEDURE Spu_sir_add_agencyproduct
-- Add the parameters for the stored procedure here
@party_cnt  INT,
@product_id INT=0,
@user_id INT,
@unique_id VARCHAR(50) = '',
@screen_hierarchy VARCHAR(500) = ''

AS
  BEGIN
      -- Insert statements for procedure here
      INSERT INTO party_agent_product
                  (party_cnt,
                   product_id,
				   UserId,
				   UniqueId,
				   ScreenHierarchy)
      VALUES     (@party_cnt,
                  @product_id,
				  @user_id,
				  @unique_id,
				  @screen_hierarchy)
  END

GO  