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

EXEC DDLDropProcedure 'spu_ACT_Get_All_Sources_Linked_With_BankAcDefault'
GO


CREATE PROCEDURE spu_ACT_Get_All_Sources_Linked_With_BankAcDefault(  
	@BankAccount_Id INT)

AS    
BEGIN  
	SELECT DISTINCT(bs.source_id) FROM BankAccount_Default bs 
	LEFT JOIN source s ON bs.source_id = s.source_id 
        WHERE bs.bankaccount_id = @BankAccount_Id
END  
GO


