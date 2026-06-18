SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_reset_all_versions'
GO

CREATE PROCEDURE [dbo].[spu_reset_all_versions]
    @insurance_file_cnt int
AS
/*
		ModifiedBy	Date		Description
		---------------------------------------------------------------------------------------------------------------------------------------------------------------
		GHarris		01/08/18	Added 2 additional where clauses as it was previously updating thousands of records. The where clause should filter by the 
								Insurance Folder as well as only update where the insurance_file_status_id does not match.
								Replaced insurance_file_status_id with enum for better reading.
*/

	DECLARE @dtcover_start_date AS DATE
	DECLARE @ninsurance_folder_cnt AS INT
	DECLARE @enum_Replaced_Backdated_Endorsement AS INT = 309

	SELECT  @dtcover_start_date=cover_start_date
			,@ninsurance_folder_cnt=insurance_folder_cnt 
	FROM Insurance_File 
	WHERE insurance_file_cnt = @insurance_file_cnt
 
	--The insurance_file_status Set Replaced AFTER REINSTATEMENT ONLY BEFORE COVER START DATE AND LEFT STATUS CANCEL ALREDAY CANCEL(before cancellation)
	UPDATE insurance_file
	SET insurance_file_status_id = (CASE WHEN IFT.code='POLICY' THEN NULL ELSE 4 END)
	FROM  insurance_file INSF
		JOIN insurance_file_type IFT ON IFT.insurance_file_type_id  =INSF.insurance_file_type_id
		LEFT JOIN MTA_Insurance_file_link MIFL ON INSF.insurance_file_cnt = MIFL.cancelled_linked_insurance_file_cnt 
			AND MIFL.ISDIRTY=0 AND  MIFL.cancelled_linked_insurance_file_cnt IS NULL
	WHERE INSF.insurance_folder_cnt = @ninsurance_folder_cnt
		AND IFT.code NOT in('MTAQUOTE','MTAQTETEMP','MTAQREINS','MTAQCAN','VOID','VOIDRENREP','VOIDREP')  
		--AND INSF.cover_start_date<=@dtcover_start_date		
		AND INSF.insurance_file_cnt <> @insurance_file_cnt
		AND INSF.insurance_file_status_id <> 5 
 
	--The insurance_file_status Set REPLACED BACKDATED AFTER REINSTATEMENT ONLY BEFORE COVER START DATE
	UPDATE INSURANCE_FIle SET insurance_file_status_id = @enum_Replaced_Backdated_Endorsement 
	FROM INSURANCE_FILE INSF 
	JOIN MTA_Insurance_file_link MIFL ON INSF.insurance_file_cnt=MIFL.original_linked_insurance_file_cnt 
		AND MIFL.ISDIRTY = 0 
		AND MIFL.new_linked_insurance_file_cnt IS NOT NULL  
		AND MIFL.cancelled_linked_insurance_file_cnt IS NOT NULL
		AND INSF.cover_start_date <= @dtcover_start_date
		AND INSF.insurance_folder_cnt = @ninsurance_folder_cnt  
		AND insurance_file_status_id <> @enum_Replaced_Backdated_Endorsement


GO
