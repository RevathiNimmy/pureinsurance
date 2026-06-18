SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMUser_Auth_Rule_Link_del'
GO

CREATE PROCEDURE spe_PMUser_Auth_Rule_Link_del
AS

DELETE PMUser_Authority_Rule_Set_Link

GO

