--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 26/05/2008    
--
-- Task : Account Function and CCY Cash Allocation
--**********************************************************************************************  

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_ACT_Get_Source_Base_Currency'
GO

CREATE PROCEDURE spu_ACT_Get_Source_Base_Currency(  
    @Source_id INT)
AS    
BEGIN 
	SELECT base_currency_id FROM source WHERE source_id = @Source_id
END  
GO



