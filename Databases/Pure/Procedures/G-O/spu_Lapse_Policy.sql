SET QUOTED_IDENTIFIER OFF  SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Lapse_Policy'
GO

CREATE PROCEDURE spu_Lapse_Policy
    @insurance_folder_cnt int,
    @lapse_reason varchar(20)

AS

BEGIN
    DECLARE @Lapsed_Reason_Id int
    DECLARE @Insurance_File_Status_Id int
    DECLARE @insurance_file_cnt int

    SELECT @insurance_file_cnt = 
        old_insurance_file_cnt 
    FROM renewal_control 
    WHERE insurance_folder_cnt = @insurance_folder_cnt

    SELECT @Insurance_File_Status_Id = Insurance_File_Status_Id
    FROM Insurance_File_Status WHERE code = 'LAP'

    SELECT @Lapsed_Reason_Id = Lapsed_Reason_Id 
    FROM Lapsed_Reason where code = @lapse_reason

    UPDATE insurance_file 
    SET lapsed_reason_id = @Lapsed_Reason_Id,
        lapsed_date = getdate(),
        insurance_file_status_id = @Insurance_File_Status_Id
    WHERE insurance_file_cnt = @insurance_file_cnt

END

GO

SET QUOTED_IDENTIFIER OFF  SET ANSI_NULLS ON 
GO

