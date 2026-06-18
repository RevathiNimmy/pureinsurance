SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_MID_GetFileNameForReExport'
GO

CREATE Procedure spu_MID_GetFileNameForReExport        
	@batch_id int,
	@mid_type varchar(4)
AS
BEGIN

	SELECT R.MID_Rule_id, R.FileName
	FROM MID_Rule R
	WHERE	R.Is_Deleted <> 1
		AND R.MID_Type = @mid_type
		AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(date, R.Start_Date) AND CONVERT(DATE, R.Expiry_Date)
		AND R.Source_id = (SELECT B.company_id
							FROM Batch B
							WHERE Batch_id = @batch_id)

END	

GO