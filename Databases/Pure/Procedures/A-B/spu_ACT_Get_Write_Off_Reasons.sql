EXEC DDLDropProcedure 'spu_ACT_Get_Write_Off_Reasons'
GO

CREATE PROCEDURE spu_ACT_Get_Write_Off_Reasons
AS

SELECT	write_off_reason_id,
	code,
	[description]
FROM 	write_off_reason
WHERE 	effective_date <= GetDate()
AND	is_deleted = 0

GO