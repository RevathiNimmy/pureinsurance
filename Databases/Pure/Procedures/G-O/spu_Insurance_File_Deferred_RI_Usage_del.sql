-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Insurance_File_Deferred_RI_Usage_del'
GO

CREATE PROCEDURE spu_Insurance_File_Deferred_RI_Usage_del
(
	@ins_file_deferred_RI_usage_id int
)
AS 

DELETE FROM 
	Insurance_File_Deferred_RI_Usage
WHERE
	ins_file_deferred_RI_usage_id = @ins_file_deferred_RI_usage_id

GO




