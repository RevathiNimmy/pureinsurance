EXECUTE DDLDropProcedure 'spu_SIRRen_UnConfirm'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIRRen_UnConfirm
    @Insurance_Folder_Cnt int
AS

/* AK 07/12/2001 - stored procedure to set things right at Renewal control / insurance file after
           a renewal confirmation has been revoked
*/
BEGIN

DECLARE @IsHolding int
DECLARE @IsWhatIF  int
DECLARE @RenewalInsFileCnt int
DECLARE @NewInsuranceFolderCnt int
DECLARE @IFStatus int
DECLARE @IFType int
DECLARE @RenInsFileCnt int
DECLARE @event_cnt INT
DECLARE @renewal_status_type_code CHAR(10)
DECLARE @OldInsFileCnt int
DECLARE @policy_type_code CHAR(10)
DECLARE @IsGemini INT
	   
	/* check if policy was confirmed with holding or alternative insurer */
	SELECT @NewInsuranceFolderCnt = i.Insurance_Folder_Cnt 
	    FROM Renewal_Control r, Insurance_File i 
	    WHERE i.Insurance_File_cnt = r.Renewal_Insurance_File_Cnt
	    AND   r.Insurance_Folder_Cnt = @Insurance_Folder_Cnt

	IF @NewInsuranceFolderCnt = @Insurance_Folder_Cnt
	    BEGIN
		/* Confirmation happend with the holding insurer */
		SELECT @IsHolding = 1
		
				
		SELECT @RenInsFileCnt = Renewal_Insurance_File_Cnt 
		    FROM Renewal_Control
		    WHERE Insurance_Folder_Cnt = @Insurance_Folder_Cnt
		IF (SELECT last_trans_description FROM insurance_file_system 
		    WHERE insurance_file_cnt = @RenInsFileCnt) = ''
		
		    SELECT @IsWhatIF = 0
		ELSE
		    SELECT @IsWhatIF = 1
			
	    END
	ELSE
		SELECT @IsHolding = 0


	/*If Alternative Insurer was chosen, we will have mark the confirmed version as replaced */
	If @IsHolding = 0 
	  BEGIN
		/* Extract Insurance file status for 'Replaced' */
		SELECT @IFStatus = Insurance_File_Status_Id 
		FROM Insurance_File_Status
		WHERE Code = 'REP'
		
		SELECT @IFType = Insurance_File_Type_Id 
		FROM Insurance_File_Type
		WHERE Code = 'RENEWAL'
		
		SELECT @RenInsFileCnt = Insurance_File_Cnt 
		FROM Insurance_File i, Insurance_File_Type s
		WHERE i.Insurance_Folder_Cnt = @Insurance_Folder_Cnt
		AND   i.Insurance_File_Type_Id = s.Insurance_File_Type_Id
		AND   s.Code = 'RENEWAL'
	  END
	ELSE
	/* it means that either the renewal version was made live or the WhatIf version 
	   so we will have to mark the record as Renewal/Whatif */
	   BEGIN
		/* Extract Insurance file status for 'Renewal' */
		SELECT @IFStatus = Insurance_File_Status_Id 
		FROM Insurance_File_Status
		WHERE Code = 'REN'
		
		IF @IsWhatIf = 1 
			SELECT @IFType = Insurance_File_Type_Id 
			FROM Insurance_File_Type
			WHERE code = 'RENEWALWIF'
		ELSE
			SELECT @IFType = Insurance_File_Type_Id 
			FROM Insurance_File_Type
			WHERE code = 'RENEWAL'

		SELECT @RenInsFileCnt = Renewal_Insurance_File_Cnt 
		FROM Renewal_Control
		WHERE Insurance_Folder_Cnt = @Insurance_Folder_Cnt

	   END
        
        /* Is this a gemini policy */
	SELECT @policy_type_code = code 
	    FROM policy_type pt, insurance_file i 
	    WHERE i.policy_type_id = pt.policy_type_id 
	    AND i.insurance_file_cnt = @RenInsFileCnt 

	SELECT @IsGemini = 0
	IF  (@policy_type_code = 'GEMINI IIM' OR @policy_type_code = 'GEMINI IIH' OR @policy_type_code = 'CV')
	    SELECT @IsGemini = 1
	    
	/* Update the status and type on the insurance_file record */
	UPDATE Insurance_File 
	SET Insurance_File_Status_Id = @IFStatus, Insurance_File_Type_Id = @IFType
	FROM Renewal_Control r INNER JOIN Insurance_File i
	ON i.Insurance_File_Cnt = r.Renewal_Insurance_File_Cnt
	WHERE r.insurance_folder_cnt = @Insurance_Folder_Cnt
	
	/* Are we revoking a lapse on the original policy if so reset the status to live */
	SELECT @OldInsFileCnt = Old_Insurance_File_Cnt 
		FROM Renewal_Control
		WHERE Insurance_Folder_Cnt = @Insurance_Folder_Cnt
	IF (SELECT insurance_file_status_id FROM insurance_file 
	    WHERE insurance_file_cnt = @OldInsFileCnt) = 2 
	BEGIN
	    --Lapsed
	    UPDATE insurance_file 
	    SET insurance_file_status_id = NULL, 
	        lapsed_reason_id = NULL, 
	        lapsed_date = NULL,
	        lapsed_description = NULL
	    WHERE insurance_file_cnt = @OldInsFileCnt
	END
		
	/* Set renewal_status_type_id back to INVITED or RENQUOTED and reset Renewal_Insurance_File_Cnt*/
	IF @IsGemini = 1
		-- For gemini policies we always want to revert back to the original renewal record
		SELECT @RenInsFileCnt = MAX(insurance_file_cnt)
			FROM event_log
			WHERE description = 'Policy Selected for Renewal'
			AND insurance_folder_cnt = @Insurance_Folder_Cnt

	SELECT @event_cnt = MAX(event_cnt) 
	FROM event_log 
	WHERE insurance_file_cnt = @RenInsFileCnt
	AND (description like 'Renewal Terms Generated%' OR description = 'Policy Invited for Renewal' OR description = 'Policy Selected For Renewal')
	IF (SELECT description FROM event_log WHERE event_cnt = @event_cnt) = 'Policy Invited for Renewal'
	    SELECT @renewal_status_type_code = 'INVITED'
	ELSE
	    IF (SELECT description FROM event_log WHERE event_cnt = @event_cnt) = 'Policy Selected for Renewal'
	        SELECT @renewal_status_type_code = 'RENSEL'
	    ELSE
                SELECT @renewal_status_type_code = 'RENQUOTED'
            
	UPDATE Renewal_Control 
	SET Renewal_Insurance_File_Cnt = @RenInsFileCnt,
	    renewal_status_type_id = (SELECT renewal_status_type_id 
				  FROM renewal_status_type
				  WHERE code = @renewal_status_type_code)
	WHERE Insurance_Folder_Cnt = @Insurance_Folder_Cnt
        
END


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

