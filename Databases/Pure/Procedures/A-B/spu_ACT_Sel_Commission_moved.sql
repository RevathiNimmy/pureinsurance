SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Sel_Commission_moved'
GO


CREATE PROCEDURE spu_ACT_Sel_Commission_moved
    @transdetail_id int,
    @earned char(1) OUTPUT
AS
BEGIN

SELECT @earned = "N"

SELECT @earned = "Y"
FROM transdetail td
LEFT JOIN(
	SELECT transdetail_id, SUM(ROUND(ISNULL(tm.base_match_amount,0),2)) Amount
	FROM transmatch tm
	WHERE tm.transdetail_id = @transdetail_id
	AND ISNULL(tm.is_reversed,0)=0
	GROUP BY transdetail_id
) TM ON TM.transdetail_id = TD.transdetail_id 
WHERE TD.transdetail_id=@transdetail_id
AND ROUND(td.amount,2) = TM.amount

END

GO