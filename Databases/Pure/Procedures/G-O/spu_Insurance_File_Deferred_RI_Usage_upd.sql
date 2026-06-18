-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Insurance_File_Deferred_RI_Usage_upd'
GO

CREATE PROCEDURE spu_Insurance_File_Deferred_RI_Usage_upd
(
	@ins_file_deferred_RI_usage_id  int,
	@insurance_file_cnt             int,
	@deferred_RI_status_type_id     int
)
AS 

UPDATE 
	Insurance_File_Deferred_RI_Usage
SET
	insurance_file_cnt = @insurance_file_cnt,
	deferred_RI_status_type_id = @deferred_RI_status_type_id
WHERE
	ins_file_deferred_RI_usage_id = @ins_file_deferred_RI_usage_id 

GO
