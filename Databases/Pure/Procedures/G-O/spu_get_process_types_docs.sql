SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_process_types_docs'
GO

CREATE PROCEDURE spu_get_process_types_docs

AS

-- PW160702 Created

    SELECT process_types_docs_id,
           description
      FROM process_types_docs
     WHERE Allow_Filtering = 1
GO
