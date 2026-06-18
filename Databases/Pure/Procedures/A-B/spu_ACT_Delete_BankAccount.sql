SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_BankAccount'
GO


CREATE PROCEDURE spu_ACT_Delete_BankAccount
    @bankaccount_id int,
	@user_id int = NULL,
	@unique_id varchar(50) = NULL,
	@screen_hierarchy varchar(500) = NULL 
AS

UPDATE BankAccount SET Userid = @user_id, UniqueId = @unique_id, ScreenHierarchy = @screen_hierarchy
WHERE bankaccount_id = @bankaccount_id

DELETE FROM BankAccount_Source
WHERE bankaccount_id = @bankaccount_id

DELETE FROM BankAccount_Delay
WHERE bankaccount_id = @bankaccount_id

DELETE FROM BankAccount
WHERE bankaccount_id = @bankaccount_id
GO


