SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_index_linking_detail_sel'
GO


CREATE PROCEDURE spu_index_linking_detail_sel
    @index_linking_id int,
    @effective_date datetime
AS


SELECT
    index_linking_detail_id,
    index_linking_id,
    effective_date,
    is_deleted,
    percentage

FROM    index_linking_detail

WHERE   is_deleted <> 1
AND index_linking_id = @index_linking_id
AND     effective_date <= @effective_date

ORDER BY effective_date DESC
GO


