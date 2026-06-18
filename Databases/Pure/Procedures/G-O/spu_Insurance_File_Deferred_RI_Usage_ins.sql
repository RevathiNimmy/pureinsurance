-------------------------------------------------------------------------------
--  Author:  AMB
--  Date:    27 May 2003
--  Desc:    SFU 1.8.6 Deferred Reinsurance development
-------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Insurance_File_Deferred_RI_Usage_ins'
GO

CREATE PROCEDURE spu_Insurance_File_Deferred_RI_Usage_ins
(
	@insurance_file_cnt             int,
    @ins_file_deferred_RI_usage_id  int OUTPUT
)
AS 

-- get the id for the manual review status
DECLARE @iDefRIStatusID int

SELECT 
    @iDefRIStatusID = deferred_RI_status_type_id 
FROM 
    Deferred_RI_Status_Type 
WHERE 
    code = 'MANREVIEW'

SET @iDefRIStatusID = ISNULL(@iDefRIStatusID, 1) -- default id for 'MANREVIEW'


-- check that our record doesn't exist already, dupes would be 'bad'
IF NOT EXISTS 
    (
    SELECT 
        ins_file_deferred_RI_usage_id
    FROM
        Insurance_File_Deferred_RI_Usage
    WHERE
        insurance_file_cnt = @insurance_file_cnt
    )
BEGIN
    INSERT INTO 
    	Insurance_File_Deferred_RI_Usage
    (
    	insurance_file_cnt,
    	deferred_RI_status_type_id
    )
    VALUES 
    (
    	@insurance_file_cnt,
    	@iDefRIStatusID
    )
END
    
SELECT @ins_file_deferred_RI_usage_id = @@IDENTITY 

GO

