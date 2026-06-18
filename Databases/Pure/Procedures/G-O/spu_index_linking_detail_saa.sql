SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_index_linking_detail_saa'
GO


CREATE PROCEDURE spu_index_linking_detail_saa
    @index_linking_id int
AS


SELECT
    index_linking_detail_id,
    index_linking_id,
    effective_date,
    is_deleted,
    percentage

FROM index_linking_detail

WHERE   index_linking_id = @index_linking_id
AND is_deleted <> 1

ORDER BY is_deleted DESC, effective_date ASC
GO


