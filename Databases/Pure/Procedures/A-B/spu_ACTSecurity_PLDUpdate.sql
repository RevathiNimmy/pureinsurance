SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACTSecurity_PLDUpdate'
GO
CREATE PROCEDURE spu_ACTSecurity_PLDUpdate
    @node_id INT
AS

UPDATE PMUser_Group_Authorities
	SET Has_unrestricted_update = 0
        WHERE node_id = @node_id
GO

