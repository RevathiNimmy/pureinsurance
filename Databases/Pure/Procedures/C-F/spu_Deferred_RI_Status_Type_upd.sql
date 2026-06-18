-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Deferred_RI_Status_Type_upd'
GO

CREATE PROCEDURE spu_Deferred_RI_Status_Type_upd
(
	@deferred_RI_status_type_id int,
	@caption_id                 int,
	@code                       char(10),
	@description                varchar(255) = NULL,
	@is_deleted                 tinyint,
	@effective_date             datetime
)
AS 

UPDATE 
	Deferred_RI_Status_Type
SET
	caption_id = @caption_id,
	code = @code,
	[description] = @description,
	is_deleted = @is_deleted,
	effective_date = @effective_date
WHERE
	deferred_RI_status_type_id = @deferred_RI_status_type_id 

GO
