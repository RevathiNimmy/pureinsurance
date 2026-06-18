SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Renewal_Control_Update'
GO


CREATE PROCEDURE spu_Renewal_Control_Update
    @insurance_folder_cnt int,
    @renewal_status_type_code char(10),
    @reset_flag smallint = 1
AS


DECLARE @renewal_status_type_id int

/* Get the Renewal_Status_Type_ID given the code */
/* AK 251001 - also change the Reset_Flag, so that it can be flagged in Renewal manager */

SELECT @renewal_status_type_id = (SELECT renewal_status_type_id
                       FROM Renewal_Status_Type
                       WHERE code = @renewal_status_type_code)

UPDATE Renewal_Control
SET renewal_status_type_id = @renewal_status_type_id,
    Reset_Flag = @reset_flag
WHERE insurance_folder_cnt = @insurance_folder_cnt
GO


