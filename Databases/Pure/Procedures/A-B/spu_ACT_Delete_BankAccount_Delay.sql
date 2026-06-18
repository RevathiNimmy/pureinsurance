EXECUTE DDLDropProcedure 'spu_ACT_Delete_BankAccount_Delay'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_ACT_Delete_BankAccount_Delay
	@bankaccount_delay_id int,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS

Update BankAccount_Delay SET UserId = @user_id, ScreenHierarchy = @screen_hierarchy, UniqueId = @unique_id
WHERE bankaccount_delay_id = @bankaccount_delay_id

DELETE BankAccount_Delay
WHERE bankaccount_delay_id = @bankaccount_delay_id

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO