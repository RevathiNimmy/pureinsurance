
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_SelectAll_AgencyProduct'
GO
-- =============================================
-- Author:		Gauri kapoor
-- Create date: 29 July 10
-- Description:	Returns product id and name and shows if it has been chosen in Party_agent_product
-- =============================================
CREATE PROCEDURE spu_SIR_SelectAll_AgencyProduct
	-- Add the parameters for the stored procedure here
	@party_cnt INT,
	@Product_id int,
	@user_id INT='',  
	@unique_id VARCHAR(50)='',  
	@screen_hierarchy VARCHAR(500)=''  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
 SELECT p.product_id,  
   p.description,  
   CASE WHEN pap.product_id IS Null THEN 0 ELSE 1 END As Chosen
 FROM product p 
 LEFT JOIN party_agent_product pap 
  ON p.product_id = pap.product_id
  AND pap.party_cnt = @party_cnt
 WHERE p.is_deleted = 0  
 ORDER BY p.product_id

END
GO

