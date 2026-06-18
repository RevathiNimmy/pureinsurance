SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_save_branch_defaults'
GO

-- Created: PW041002

CREATE PROCEDURE spu_save_branch_defaults
    @source_id integer,
    @agent_id integer,
    @direct_business tinyint
AS
BEGIN

DECLARE @rec_found integer

    SELECT @rec_found = count(*)
      FROM source_defaults
     WHERE source_id = @source_id

    IF @rec_found = 0
        INSERT INTO source_defaults
                    (source_id,
                    agent_id,
                    direct_business)
             VALUES (@source_id,
                    @agent_id,
                    @direct_business)
    ELSE
        UPDATE source_defaults
           SET agent_id = @agent_id,
               direct_business = @direct_business
         WHERE source_id = @source_id

END
GO
