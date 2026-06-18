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

EXEC DDLDropProcedure 'spu_ACT_Get_All_Sources_Linked_With_BankAc'
GO


CREATE PROCEDURE spu_ACT_Get_All_Sources_Linked_With_BankAc(    
 @BankAccount_Id INT)  
  
AS      
BEGIN    
	SELECT source_id FROM BankAccount_Source where bankaccount_id = @BankAccount_Id  
END    
