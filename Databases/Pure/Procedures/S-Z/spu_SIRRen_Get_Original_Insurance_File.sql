SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Original_Insurance_File'
GO


CREATE PROCEDURE spu_SIRRen_Get_Original_Insurance_File
    @renewal_insurance_file_cnt INT,
    @old_insurance_file_cnt INT OUTPUT,
    @party_cnt INT OUTPUT,
    @amount_to_finance MONEY OUTPUT
AS

DECLARE @IsUnderWritting	VARCHAR(2)

SELECT 	@IsUnderWritting = Value 
FROM 	Hidden_options
WHERE	Option_Number=1

IF @IsUnderWritting <> 'U'
        SELECT
            @old_insurance_file_cnt = rc.old_insurance_file_cnt,
            @party_cnt = i.insured_cnt,
            @amount_to_finance = i.this_premium
        FROM insurance_file i
        JOIN renewal_control rc
            ON rc.insurance_folder_cnt = i.insurance_folder_cnt
        WHERE i.insurance_file_cnt = @renewal_insurance_file_cnt

ELSE
	SELECT  
	    TOP 1 	@old_insurance_file_cnt = IFI.insurance_file_cnt,  
	    		@party_cnt = IFI.insured_cnt,  
	    		@amount_to_finance = IFI.this_premium  
		FROM 	insurance_file IFI
		WHERE 	IFI.insurance_folder_cnt =
				(SELECT insurance_folder_cnt
					FROM Insurance_file
					WHERE Insurance_file_cnt = @renewal_insurance_file_cnt)
	AND 	IFI.insurance_file_cnt <> @renewal_insurance_file_cnt

GO


