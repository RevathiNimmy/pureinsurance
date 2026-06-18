-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Insurance_File_Deferred_RI_Usage_sel'
GO

CREATE PROCEDURE spu_Insurance_File_Deferred_RI_Usage_sel
(
	@ins_file_deferred_RI_usage_id int
)
AS 
SELECT 
	ins_file_deferred_RI_usage_id,
	insurance_file_cnt,
	deferred_RI_status_type_id
FROM 
	Insurance_File_Deferred_RI_Usage
WHERE
	ins_file_deferred_RI_usage_id = @ins_file_deferred_RI_usage_id
GO


