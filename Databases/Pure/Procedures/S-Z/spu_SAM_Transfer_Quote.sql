SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Transfer_Quote'
GO

CREATE PROCEDURE spu_SAM_Transfer_Quote
     @InsuranceFileCnt int,
	 @PartyToCnt int,
	 @PartyFromCnt int,
	 @PMUserID int
AS

DECLARE @InsuranceFolderCnt int
DECLARE @InsuranceRef varchar(30)
DECLARE @PartyFromShortName varchar(20)
DECLARE @PartyToShortName varchar(20)
DECLARE @PartyToFullName varchar(255)

--Get Details
SELECT @InsuranceFolderCnt = insurance_folder_cnt,
	   @InsuranceRef = RTRIM(insurance_ref)
FROM Insurance_File
WHERE insurance_file_cnt = @InsuranceFileCnt

SELECT @PartyFromShortName = RTRIM(p.shortname)
FROM Party p
WHERE p.party_cnt = @PartyFromCnt

SELECT @PartyToShortName = RTRIM(p.shortname),
@PartyToFullName = RTRIM(p.resolved_name)
FROM Party p
WHERE p.party_cnt = @PartyToCnt

--Transfer Ownership
UPDATE Insurance_File 
SET insured_cnt = @PartyToCnt,
insured_name = @PartyToFullName
WHERE insurance_file_cnt = @InsuranceFileCnt

UPDATE Insurance_Folder 
SET insurance_holder_cnt = @PartyToCnt 
WHERE insurance_folder_cnt = @InsuranceFolderCnt

--Add Event to From
INSERT INTO event_log(party_cnt, insurance_folder_cnt, insurance_file_cnt, claim_cnt,  
    document_cnt, new_address_cnt, old_address_cnt, campaign_id,  
    document_type_id, report_type_id, event_type_id, user_id, event_date,  
    description, old_party_type_id, transaction_export_folder_cnt,  
    account_key, event_log_subject_id, short_description,  
    fsa_complaint_folder_cnt, Priority_Code, Is_Completed, Sticky_top, Sticky_left)  
VALUES(@PartyFromCnt, NULL, NULL, NULL, NULL, NULL,  
    NULL, NULL, NULL, NULL, 5, @PMUserID, GETDATE(),  
    @InsuranceRef + ' transfered to party ' + @PartyToShortName, 0, NULL, NULL,  
    NULL, NULL, NULL, NULL, 1, NULL, NULL) 
        
--Add Event to To
INSERT INTO event_log(party_cnt, insurance_folder_cnt, insurance_file_cnt, claim_cnt,  
    document_cnt, new_address_cnt, old_address_cnt, campaign_id,  
    document_type_id, report_type_id, event_type_id, user_id, event_date,  
    description, old_party_type_id, transaction_export_folder_cnt,  
    account_key, event_log_subject_id, short_description,  
    fsa_complaint_folder_cnt, Priority_Code, Is_Completed, Sticky_top, Sticky_left)  
VALUES(@PartyToCnt, @InsuranceFolderCnt, @InsuranceFileCnt, NULL, NULL, NULL,  
    NULL, NULL, NULL, NULL, 5, @PMUserID, GETDATE(),  
    @InsuranceRef + ' transfered from party ' + @PartyFromShortName, 0, NULL, NULL,  
    NULL, NULL, NULL, NULL, 1, NULL, NULL) 
        
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO