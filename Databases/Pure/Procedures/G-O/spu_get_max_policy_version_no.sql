SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_max_policy_version_no'
GO

CREATE PROCEDURE spu_get_max_policy_version_no
    @insurance_file_cnt int,
    @max_version_no int output
AS

BEGIN
    Declare @intMAX as INT

    SELECT @intMAX = MAX(ifi.policy_version)
    FROM    insurance_file ifi,
            insurance_file ifi2
    WHERE   ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt
    AND     ifi.policy_ignore is null
    AND     ifi2.insurance_file_cnt = @insurance_file_cnt
    
    SELECT @max_version_no = IsNull(@intMAX,0)  
END 
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS ON 
GO