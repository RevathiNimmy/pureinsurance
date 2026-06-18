--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 03/03/2008    
--
-- Task : Renewal Printing
--**********************************************************************************************  

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_ACT_Get_Currecny_Not_Linked_With_Source'
GO


CREATE PROCEDURE spu_ACT_Get_Currecny_Not_Linked_With_Source(  
    @Currency_Id INT,
    @Source_id INT)
AS    
BEGIN 
DECLARE @Base_Currency_id INT
    --First check is this currency is base for the given branch
    SELECT @Base_Currency_id = isnull(base_currency_id,0) from source where source_id = @Source_id
	IF @Base_Currency_id = @Currency_Id BEGIN
		SELECT currency_id FROM currency WHERE currency_id not in(
		SELECT currency_id FROM bankaccount WHERE bankaccount_id 
		in(SELECT bankaccount_id FROM bankaccount_source WHERE source_id = @Source_id)) Union SELECT @Currency_Id
	END
END  
GO




