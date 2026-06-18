EXECUTE DDLDropProcedure 'spu_ACT_Add_BankAccount_Delay'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_ACT_Add_BankAccount_Delay
    @bankaccount_id int,
	@mediatype_id int,
	@delay int,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null,
	@bankaccount_delay_id int OUTPUT
AS

INSERT INTO BankAccount_Delay
	(bankaccount_id, mediatype_id, delay,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES
	(@bankaccount_id, @mediatype_id, @delay,
	@user_id,
	@unique_id,
	@screen_hierarchy)

SELECT @bankaccount_delay_id = SCOPE_IDENTITY()

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO