SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_branch_defaults'
GO

-- Created: PW151002

CREATE PROCEDURE spu_get_branch_defaults
    @source_id integer
AS
BEGIN

        SELECT agent_id,
               direct_business
          FROM source_defaults
         WHERE source_id = @source_id

END
GO
