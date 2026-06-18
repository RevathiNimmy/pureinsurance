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

EXEC DDLDropProcedure 'spu_ACT_Add_Sources_Linked_With_BankAc'
GO


CREATE PROCEDURE spu_ACT_Add_Sources_Linked_With_BankAc(  
    @BankAccount_Id INT,
    @Source_id INT,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null)
AS    
BEGIN  
    INSERT INTO BankAccount_Source ([bankaccount_id],[source_id],UserId,UniqueId,ScreenHierarchy) VALUES (@BankAccount_Id,@Source_id,@user_id,@unique_id,@screen_hierarchy)
END  
GO

