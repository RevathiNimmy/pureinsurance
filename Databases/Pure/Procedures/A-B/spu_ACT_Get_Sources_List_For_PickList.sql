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

EXEC DDLDropProcedure 'spu_ACT_Get_Sources_List_For_PickList'
GO


CREATE PROCEDURE spu_ACT_Get_Sources_List_For_PickList(    
 @BankAccount_Id INT)  
  
AS      
BEGIN    
	SELECT source_id,description,0 FROM source WHERE is_deleted = 0 
	AND source_id not in (SELECT source_id FROM BankAccount_Source where bankaccount_id = @BankAccount_Id  )
	UNION ALL SELECT bs.source_id,
	s.description,1
	FROM BankAccount_Source bs   
	LEFT JOIN source s ON bs.source_id = s.source_id   
	WHERE bs.bankaccount_id = @BankAccount_Id   
END    




