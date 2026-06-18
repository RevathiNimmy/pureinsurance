-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Deferred_RI_Status_Type_ins'
GO

CREATE PROCEDURE spu_Deferred_RI_Status_Type_ins
(
	@caption_id                 int,
	@code                       char(10),
	@description                varchar(255) = NULL,
	@is_deleted                 tinyint,
	@effective_date             datetime,
    @deferred_RI_status_type_id int OUTPUT
)
AS 

INSERT INTO 
	Deferred_RI_Status_Type
(
	caption_id,
	code,
	[description],
	is_deleted,
	effective_date
)
VALUES 
(
	@caption_id,
	@code,
	@description,
	@is_deleted,
	@effective_date
)

SELECT @deferred_RI_status_type_id = @@IDENTITY 

GO
