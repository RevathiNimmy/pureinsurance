SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_agent_docs'
GO

CREATE PROCEDURE spu_get_agent_docs
    @party_cnt INT

AS

-- PW160702 Created

    SELECT process_type
      FROM agent_docs
     WHERE party_cnt = @party_cnt

GO
