SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACTSecurity_PLDView'
GO
CREATE PROCEDURE spu_ACTSecurity_PLDView
    @node_id INT
AS

DELETE FROM
    PMUser_Group_Authorities
WHERE
    node_id = @node_id
GO

