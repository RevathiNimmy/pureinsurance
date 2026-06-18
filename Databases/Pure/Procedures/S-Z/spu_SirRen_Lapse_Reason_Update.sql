SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Lapse_Reason_Update'
GO


CREATE PROCEDURE spu_SirRen_Lapse_Reason_Update
    @lapsed_reason_id int,
    @lapsed_description varchar(255),
    @insurance_folder_cnt int
AS

/*IJM 270601 Update lapsed reason on insurance file */
/* AK 151001 - need to update only the renewal version of insurance file */
BEGIN
    
    DECLARE @lapse_status_id int

    SELECT @lapse_status_id = insurance_file_status_id 
    FROM Insurance_File_Status
    WHERE code= 'LAP'

    UPDATE insurance_file
    SET lapsed_reason_id = @lapsed_reason_id,
       lapsed_description = @lapsed_description,
        insurance_file_status_id = @lapse_status_id 

    FROM insurance_file i, insurance_file_type ift

    WHERE i.insurance_folder_cnt = @insurance_folder_cnt
    AND i.insurance_file_type_id = ift.insurance_file_type_id
    AND i.insurance_file_status_id <> 4 -- Replaced
    AND (ift.code = 'RENEWAL' 
	OR ift.code = 'RENEWALWIF')

END

GO


