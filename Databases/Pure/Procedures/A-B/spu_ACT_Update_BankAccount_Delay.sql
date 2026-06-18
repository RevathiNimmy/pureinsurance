EXECUTE DDLDropProcedure 'spu_ACT_Update_BankAccount_Delay'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_ACT_Update_BankAccount_Delay
	@bankaccount_delay_id int,
    @bankaccount_id int,
	@mediatype_id int,
	@delay int,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS

UPDATE BankAccount_Delay
SET
	bankaccount_id = @bankaccount_id, 
	mediatype_id = @mediatype_id,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy,
	delay = @delay
WHERE
	bankaccount_delay_id = @bankaccount_delay_id

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO