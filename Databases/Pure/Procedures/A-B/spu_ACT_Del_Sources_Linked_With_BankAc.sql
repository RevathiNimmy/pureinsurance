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

EXEC DDLDropProcedure 'spu_ACT_Del_Sources_Linked_With_BankAc'
GO


CREATE PROCEDURE spu_ACT_Del_Sources_Linked_With_BankAc(  
	@BankAccount_Id INT,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null)
    
AS    
BEGIN  
	Update BankAccount_Source SET UserId = @user_id, ScreenHierarchy = @screen_hierarchy, UniqueId = @unique_id
	WHERE BankAccount_Id = @BankAccount_Id

	DELETE BankAccount_Source WHERE BankAccount_Id = @BankAccount_Id
END  
GO

