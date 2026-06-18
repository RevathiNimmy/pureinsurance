SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_cancel_all_versions'
GO


CREATE PROCEDURE spu_cancel_all_versions
    @insurance_file_cnt int
AS

/*************************************************************************************************
* Name: sp_cancel_all_versions  gives all policy versions status of Cancelled
*
* Version: 1.00.0000
*
* JMK 03/08/2001
***************************************************************************************************/

Declare @Insurance_file_Status_id int 

Select @Insurance_file_Status_id = Insurance_file_Status_id from Insurance_File_Status where code = 'VOID'

Set @Insurance_file_Status_id = isnull(@Insurance_file_Status_id,0)

UPDATE insurance_file  
SET insurance_file_status_id = 1  
WHERE insurance_folder_cnt =  
    (SELECT insurance_folder_cnt  
    FROM insurance_file  
    WHERE insurance_file_cnt = @insurance_file_cnt)
	AND ISNULL(insurance_file_status_id,0) <>  @Insurance_file_Status_id
	--AND 
	--insurance_folder_cnt =  
 --   (
	--	SELECT insurance_folder_cnt  
	--	FROM insurance_file  
	--	WHERE insurance_file_cnt = @insurance_file_cnt
	--) 


-- Set the actual cancellation version's type to 8 - 'MTA Cancelled'
UPDATE insurance_file
SET insurance_file_type_id = (SELECT insurance_file_type_id FROM Insurance_File_Type where code='MTACAN' )
WHERE insurance_file_cnt = @insurance_file_cnt


GO


