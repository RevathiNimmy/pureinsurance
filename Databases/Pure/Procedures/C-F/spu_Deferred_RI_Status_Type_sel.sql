-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Deferred_RI_Status_Type_sel'
GO

CREATE PROCEDURE spu_Deferred_RI_Status_Type_sel
(
	@deferred_RI_status_type_id int
)
AS 

SELECT 
	deferred_RI_status_type_id,
	caption_id,
	code,
	[description],
	is_deleted,
	effective_date
FROM 
	Deferred_RI_Status_Type
WHERE
	deferred_RI_status_type_id = @deferred_RI_status_type_id

GO


