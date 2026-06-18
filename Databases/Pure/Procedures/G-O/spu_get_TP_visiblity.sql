SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_TP_visiblity'
GO

CREATE PROCEDURE spu_get_TP_visiblity
    @TP_type varchar(20)
AS

    SELECT 
	is_visible 
    From party_agent_type 
    WHERE [description] = @TP_type
    AND is_deleted = 0

GO
GO

