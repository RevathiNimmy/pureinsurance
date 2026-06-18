SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_ClaimsExport_XML_Select'
GO

CREATE PROCEDURE spu_CLM_ClaimsExport_XML_Select  
 @batch_id int,
 @sXMLDataset VARCHAR (300),
 @sCaseXMLDataset VARCHAR (300),
 @export_by_date BIT = 0,
 @date_from datetime = NULL,
 @date_to datetime = NULL,
 @retrieve_all_versions bit = 0,
 @new_batch bit = 0,
 @override_batch_processing bit = 0 
AS

    -- Update batch table
    If (Select export_date From batch Where batch_id = @batch_id) Is Null Begin
        Update  batch
        Set     export_date = GetDate()
        Where   batch_id = @batch_id
    End Else Begin
        Update  batch
        Set     reexport_date = GetDate()
        Where   batch_id = @batch_id
    End

	--Set claims to have batch_id
If (@new_batch = 1)  
 BEGIN  
	 
	 IF (@override_batch_processing = 0)
	 BEGIN
	  UPDATE claim  
		SET batch_id = @batch_id  
		FROM claim  
		LEFT JOIN (SELECT MAX(version_id) as version_id, base_claim_id  
					FROM claim WHERE  
					   (claim.is_dirty = 0  
						AND(@export_by_date = 0  
						OR (create_date>=@date_from and  
						create_date<=@date_to)))  
					GROUP BY base_claim_id) latest_claim_version ON  
					(claim.base_claim_id = latest_claim_version.base_claim_id  
					AND claim.version_id = latest_claim_version.version_id)
		  WHERE  
			batch_id IS NULL  
		  AND  
		   (@retrieve_all_versions = 1 OR latest_claim_version.version_id is not null)  
		  AND  
		   (claim.is_dirty = 0  
		  AND  
		   (@export_by_date = 0  
			 OR (claim.create_date >= @date_from AND  
			 claim.create_date <= @date_to)))
	 END ELSE BEGIN
	  UPDATE claim  
		SET batch_id = @batch_id  
		FROM claim  
		LEFT JOIN (SELECT MAX(version_id) as version_id, base_claim_id  
					FROM claim WHERE  
					   (claim.is_dirty = 0  
						AND(@export_by_date = 0  
						OR (create_date>=@date_from and  
						create_date<=@date_to)))  
					GROUP BY base_claim_id) latest_claim_version ON  
					(claim.base_claim_id = latest_claim_version.base_claim_id  
					AND claim.version_id = latest_claim_version.version_id)
		  WHERE  
		   (@retrieve_all_versions = 1 OR latest_claim_version.version_id is not null)
		  AND  
		   (claim.is_dirty = 0  
		  AND  
		   (@export_by_date = 0  
			 OR (claim.create_date >= @date_from AND  
			 claim.create_date <= @date_to)))
	 END
 
 END  

DECLARE @parameters varchar (300)

SET @parameters = 'batch_id=' + Cast(@batch_id as varchar(10))

-- *******************************************************************************************
-- ROOT LEVEL - EXPORT_HEADER
-- *******************************************************************************************

            Select  1                         As Tag,
            Null                              As Parent,
            'http://www.siriusfs.com/SFI/Export/Claims_Export/20060419'
                                              As [EXPORT_HEADER!1!xmlns],
            'http://www.w3.org/2001/XMLSchema-instance'
                                              As [EXPORT_HEADER!1!xmlns:xsi],
            'http://www.siriusfs.com/SFI/Export/Claims_Export/20060419 Claims_Export.xsd'                As [EXPORT_HEADER!1!xsi:schemaLocation],
            GetDate()                         As [EXPORT_HEADER!1!date_exported],
            'Claims_Export'                   As [EXPORT_HEADER!1!interface_name],
            @parameters                       As [EXPORT_HEADER!1!parameters_used],
            @batch_id                         As [EXPORT_HEADER!1!batch_id],
            b.batch_ref                       As [EXPORT_HEADER!1!batch_reference],
            ISNULL(b.total_transactions,'0')  As [EXPORT_HEADER!1!total_transactions],
            ISNULL(b.total_amount,'0')        As [EXPORT_HEADER!1!total_amount],
            Null                              As [CLAIM!2!claim_id],
            Null                              As [CLAIM!2!claim_number],
            Null                              As [CLAIM!2!policy_number],
            Null                              As [CLAIM!2!claim_version],
            Null                              As [CLAIM!2!description],
            Null                              As [CLAIM!2!claim_status_code],
            Null                              As [CLAIM!2!progress_status_code],
            Null                              As [CLAIM!2!primary_cause_code],
            Null                              As [CLAIM!2!secondary_cause_code],
            Null                              As [CLAIM!2!catastrophe_code],
            Null                              As [CLAIM!2!coinsurance_treatment_code],
            Null                              As [CLAIM!2!loss_from_date],
            Null                              As [CLAIM!2!loss_to_date],
            Null                              As [CLAIM!2!reported_date],
            Null                              As [CLAIM!2!reported_to_date],
            Null                              As [CLAIM!2!claims_handler_name],
            Null                              As [CLAIM!2!loss_currency_code],
            Null                              As [CLAIM!2!info_only_code],
            Null                              As [CLAIM!2!likely_claim],
            Null                              As [CLAIM!2!location],
            Null                              As [CLAIM!2!town_description],
            Null                              As [CLAIM!2!risk_type_code],
            Null                              As [CLAIM!2!client_name],
            Null                              As [CLAIM!2!client_address1],
            Null                              As [CLAIM!2!client_address2],
            Null                              As [CLAIM!2!client_address3],
            Null                              As [CLAIM!2!client_address4],
            Null                              As [CLAIM!2!client_postal_code],
            Null                              As [CLAIM!2!client_country],
            Null                              As [CLAIM!2!client_telephone],
            Null                              As [CLAIM!2!client_fax],
            Null                              As [CLAIM!2!client_mobile],
            Null                              As [CLAIM!2!client_email],
            Null                              As [CLAIM!2!client_claim_number],
            Null                              As [CLAIM!2!insurer_name],
            Null                              As [CLAIM!2!insurer_address1],
            Null                              As [CLAIM!2!insurer_address2],
            Null                              As [CLAIM!2!insurer_address3],
            Null                              As [CLAIM!2!insurer_address4],
            Null                              As [CLAIM!2!insurer_postal_code],
            Null                              As [CLAIM!2!insurer_country],
            Null                              As [CLAIM!2!insurer_telephone],
            Null                              As [CLAIM!2!insurer_fax],
            Null                              As [CLAIM!2!insurer_email],
            Null                              As [CLAIM!2!insurer_claim_number],
            Null                              As [CLAIM!2!insurer_contact_name],
            Null                              As [CLAIM!2!domiciled_for_tax],
            Null                              As [CLAIM!2!tax_number],
            Null                              As [CLAIM!2!comments],
            Null                              As [CLAIM!2!underwriting_year_code],
            Null                              As [CLAIM!2!created_date],
            Null                              As [CLAIM!2!created_by_username],
            Null                              As [CLAIM!2!last_modified_date],
            Null                              As [CLAIM!2!last_modified_by_username],
            Null                              As [CLAIM!2!base_claim_id],
            Null                              As [CLAIM!2!case_id],
            Null                              As [CLAIM!2!case_number],
            Null                              As [CLAIM!2!case_opened_date],
            Null                              As [CLAIM!2!case_version],
            Null                              As [CLAIM!2!case_progress_code],
            Null                              As [CLAIM!2!case_analyst_handler],
            Null                              As [CLAIM!2!case_admin_handler],
            Null                              As [CLAIM!2!case_base_case_id],
            Null                              As [CLAIMBUILDER!3!!CDATA],
            Null                              As [CASEBUILDER!4!!CDATA],
            Null                              As [CLAIMPERIL!5!claim_peril_id],
            Null                              As [RESERVE!6!reserve_id],
            Null                              As [RESERVE!6!reserve_description],
            Null           					  As [RESERVE!6!reserve_amount],
            Null				              As [RESERVE!6!total_incurred],
		    NULL As [RECOVERY!7!recovery_id],
			NULL As [RECOVERY!7!recovery_type_code],
			NULL As [RECOVERY!7!recovery_description],
			NULL As [RECOVERY!7!recovery_amount],
			NULL As [RECOVERY!7!total_incurred]
    FROM    batch b
    WHERE   b.batch_id = @batch_id

    UNION ALL

-- *******************************************************************************************
-- FIRST LEVEL - CLAIM
-- *******************************************************************************************

 Select  2         As Tag,
  1         As Parent,
  Null             As [EXPORT_HEADER!1!xmlns],
  Null           As [EXPORT_HEADER!1!xmlns:xsi],
  Null          As [EXPORT_HEADER!1!xsi:schemaLocation],
  Null       As [EXPORT_HEADER!1!date_exported],
  Null        As [EXPORT_HEADER!1!interface_name],
  Null              As [EXPORT_HEADER!1!parameters_used],
  @batch_id           As [EXPORT_HEADER!1!batch_id],
  Null          As [EXPORT_HEADER!1!batch_reference],
  Null       As [EXPORT_HEADER!1!total_transactions],
  Null         As [EXPORT_HEADER!1!total_amount],
  claim.claim_id       As [CLAIM!2!claim_id],
  claim.claim_number      As [CLAIM!2!claim_number],
  claim.policy_number      As [CLAIM!2!policy_number],
  claim.version_id      As [CLAIM!2!claim_version],
  claim.description      As [CLAIM!2!description],
  claim_status.code      As [CLAIM!2!claim_status_code],
  progress_status.code      As [CLAIM!2!progress_status_code],
  primary_cause.code     As [CLAIM!2!primary_cause_code],
  secondary_cause.code      As [CLAIM!2!secondary_cause_code],
  catastrophe_code.code      As [CLAIM!2!catastrophe_code],
  coinsurance_treatment.code     As [CLAIM!2!coinsurance_treatment_code],
  claim.loss_from_date      As [CLAIM!2!loss_from_date],
  claim.loss_to_date      As [CLAIM!2!loss_to_date],
  claim.reported_date      As [CLAIM!2!reported_date],
  claim.reported_to_date      As [CLAIM!2!reported_to_date],
  handler.code       As [CLAIM!2!claims_handler_name],
  currency.code       As [CLAIM!2!loss_currency_code],
  claim.info_only      As [CLAIM!2!info_only_code],
  claim.likely_claim      As [CLAIM!2!likely_claim],
  claim.location       As [CLAIM!2!location],
  town.description     As [CLAIM!2!town_description],
  risk_type.code       As [CLAIM!2!risk_type_code],
  claim.client_name      As [CLAIM!2!client_name],
  client_address.address1     As [CLAIM!2!client_address1],
  client_address.address2     As [CLAIM!2!client_address2],
  client_address.address3     As [CLAIM!2!client_address3],
  client_address.address4     As [CLAIM!2!client_address4],
  client_address.postal_code     As [CLAIM!2!client_postal_code],
  client_address_country.code     As [CLAIM!2!client_country],
  claim.client_tel_no      As [CLAIM!2!client_telephone],
  claim.client_fax_no      As [CLAIM!2!client_fax],
  claim.client_mobile_no      As [CLAIM!2!client_mobile],
  claim.client_email      As [CLAIM!2!client_email],
  claim.client_claim_number     As [CLAIM!2!client_claim_number],
  claim.insurer_name      As [CLAIM!2!insurer_name],
  insurer_address.address1    As [CLAIM!2!insurer_address1],
  insurer_address.address2    As [CLAIM!2!insurer_address2],
  insurer_address.address3    As [CLAIM!2!insurer_address3],
  insurer_address.address4    As [CLAIM!2!insurer_address4],
  insurer_address.postal_code     As [CLAIM!2!insurer_postal_code],
  insurer_address_country.code     As [CLAIM!2!insurer_country],
  claim.insurer_tel_no      As [CLAIM!2!insurer_telephone],
  claim.insurer_fax_no      As [CLAIM!2!insurer_fax],
  claim.insurer_email      As [CLAIM!2!insurer_email],
  claim.insurer_claim_number     As [CLAIM!2!insurer_claim_number],
  claim.insurer_contact      As [CLAIM!2!insurer_contact_name],
  insurer.domiciled_for_tax     As [CLAIM!2!domiciled_for_tax],
  insurer.tax_number      As [CLAIM!2!tax_number],
  claim.comments      As [CLAIM!2!comments],
  underwriting_year.code      As [CLAIM!2!underwriting_year_code],
  claim.create_date      As [CLAIM!2!created_date],
  claim_created_by.username    As [CLAIM!2!created_by_username],
  claim.Last_modified_date     As [CLAIM!2!last_modified_date],
  claim_modified_by.username     As [CLAIM!2!last_modified_by_username],
  claim.base_claim_id                   As [CLAIM!2!base_claim_id],
  [case].case_id          As [CLAIM!2!case_id],
  [case].case_number                               As [CLAIM!2!case_number],
  [case].case_opened_date                          As [CLAIM!2!case_opened_date],
  [case].case_version      As [CLAIM!2!case_version],
  case_progress.code       As [CLAIM!2!case_progress_code],
  analyst_handler.code      As [CLAIM!2!case_analyst_handler],
  admin_handler.code     As [CLAIM!2!case_admin_handler],
  [case].base_case_id      As [CLAIM!2!case_base_case_id],
  Null                               As [CLAIMBUILDER!3!!CDATA],
  Null                               As [CASEBUILDER!4!!CDATA],
  Null       						As [CLAIMPERIL!5!claim_peril_id],
  Null       						As [RESERVE!6!reserve_id],
  Null					           As [RESERVE!6!reserve_description],
  Null           					As [RESERVE!6!reserve_amount],
  Null				              As [RESERVE!6!total_incurred] ,
  NULL As [RECOVERY!7!recovery_id],
  NULL As [RECOVERY!7!recovery_type_code],
  NULL As [RECOVERY!7!recovery_description],
  NULL As [RECOVERY!7!recovery_amount],
  NULL As [RECOVERY!7!total_incurred]
 FROM claim
  INNER JOIN batch ON
   batch.batch_id = claim.batch_id
  INNER JOIN claim_status ON
   claim.claim_status_id = claim_status.claim_status_id
  INNER JOIN progress_status ON
   claim.progress_status_id = progress_status.progress_status_id
  INNER JOIN primary_cause ON
   claim.primary_cause_id = primary_cause.primary_cause_id
  LEFT JOIN secondary_cause ON
   claim.secondary_cause_id = secondary_cause.secondary_cause_id
  LEFT JOIN catastrophe_code ON
   claim.catastrophe_code_id = catastrophe_code.catastrophe_code_id
  LEFT JOIN coinsurance_treatment ON
   claim.coinsurance_treatment_id = coinsurance_treatment.coinsurance_treatment_id
  INNER JOIN handler ON
   claim.handler_id =handler.handler_id
  INNER JOIN currency ON
   claim.currency_id = currency.currency_id
  LEFT JOIN town ON
   claim.town = town.town_id
  INNER JOIN risk ON
   claim.risk_type_id = risk.risk_cnt
  INNER JOIN risk_type ON
    risk.risk_type_id = risk_type.risk_type_id
  LEFT JOIN claim_address client_address ON
   claim.client_address = client_address.address_cnt
  LEFT JOIN country client_address_country ON
    client_address.country_id = client_address_country.country_id
  LEFT JOIN claim_address insurer_address ON
   claim.insurer_address = insurer_address.address_cnt
  LEFT JOIN country insurer_address_country ON
    insurer_address.country_id = insurer_address_country.country_id
  INNER JOIN insurance_file ON
   claim.policy_id = insurance_file.insurance_file_cnt
  LEFT JOIN party insurer ON
    insurance_file.lead_agent_cnt = insurer.party_cnt
  LEFT JOIN underwriting_year ON
   claim.underwriting_year_id = underwriting_year.underwriting_year_id
  LEFT JOIN pmuser claim_created_by ON
   claim.created_by_id = claim_created_by.user_id
  LEFT JOIN pmuser claim_modified_by ON
   claim.modified_by_id = claim_modified_by.user_id
  LEFT JOIN (SELECT MAX(version_id) as version_id, base_claim_id
       FROM claim WHERE
		(claim.is_dirty = 0
		AND(@export_by_date = 0
		OR (create_date>=@date_from and
			create_date<=@date_to)))
       GROUP BY base_claim_id) latest_claim_version ON
   		claim.base_claim_id = latest_claim_version.base_claim_id
      AND claim.version_id = latest_claim_version.version_id

  LEFT JOIN (SELECT  MAX(case_id) as case_id,base_case_id
			FROM [case]
			GROUP BY base_case_id) max_case_version ON
   claim.base_case_id = max_case_version.base_case_id
  LEFT JOIN [case] ON
	max_case_version.case_id=[case].case_id
  LEFT JOIN case_progress ON
   [case].case_progress_id = case_progress.case_progress_id
  LEFT JOIN handler AS analyst_handler ON
   [case].analyst_handler_id = analyst_handler.handler_id
  LEFT JOIN handler AS admin_handler ON
   [case].admin_handler_id = admin_handler.handler_id
 WHERE  batch.batch_id = @batch_id

UNION ALL

-- *******************************************************************************************
-- THIRD LEVEL - CLAIMBUILDER - PARENT - CLAIM
-- *******************************************************************************************

 Select  3         As Tag,
  2         As Parent,
  Null   As [EXPORT_HEADER!1!xmlns],
  Null       As [EXPORT_HEADER!1!xmlns:xsi],
  Null      As [EXPORT_HEADER!1!xsi:schemaLocation],
  Null   As [EXPORT_HEADER!1!date_exported],
  Null    As [EXPORT_HEADER!1!interface_name],
  Null   As [EXPORT_HEADER!1!parameters_used],
  @batch_id      As [EXPORT_HEADER!1!batch_id],
  Null      As [EXPORT_HEADER!1!batch_reference],
  Null   As [EXPORT_HEADER!1!total_transactions],
  Null    As [EXPORT_HEADER!1!total_amount],
  claim.claim_id   As [CLAIM!2!claim_id],
  Null    As [CLAIM!2!claim_number],
  Null    As [CLAIM!2!policy_number],
  Null    As [CLAIM!2!claim_version],
  Null    As [CLAIM!2!description],
  Null    As [CLAIM!2!claim_status_code],
  Null    As [CLAIM!2!progress_status_code],
  Null   As [CLAIM!2!primary_cause_code],
  Null    As [CLAIM!2!secondary_cause_code],
  Null    As [CLAIM!2!catastrophe_code],
  Null    As [CLAIM!2!coinsurance_treatment_code],
  null   As [CLAIM!2!loss_from_date],
  null   As [CLAIM!2!loss_to_date],
  null   As [CLAIM!2!reported_date],
  null   As [CLAIM!2!reported_to_date],
  null   As [CLAIM!2!claims_handler_name],
  null   As [CLAIM!2!loss_currency_code],
  null   As [CLAIM!2!info_only_code],
  null   As [CLAIM!2!likely_claim],
  null   As [CLAIM!2!location],
  null   As [CLAIM!2!town_description],
  null   As [CLAIM!2!risk_type_code],
  null   As [CLAIM!2!client_name],
  null   As [CLAIM!2!client_address1],
  null   As [CLAIM!2!client_address2],
  null   As [CLAIM!2!client_address3],
  null   As [CLAIM!2!client_address4],
  null   As [CLAIM!2!client_postal_code],
  null   As [CLAIM!2!client_country],
  null   As [CLAIM!2!client_telephone],
  null   As [CLAIM!2!client_fax],
  null   As [CLAIM!2!client_mobile],
  null   As [CLAIM!2!client_email],
  null   As [CLAIM!2!client_claim_number],
  null   As [CLAIM!2!insurer_name],
  null   As [CLAIM!2!insurer_address1],
  null   As [CLAIM!2!insurer_address2],
  null   As [CLAIM!2!insurer_address3],
  null   As [CLAIM!2!insurer_address4],
  null   As [CLAIM!2!insurer_postal_code],
  null   As [CLAIM!2!insurer_country],
  null   As [CLAIM!2!insurer_telephone],
  null   As [CLAIM!2!insurer_fax],
  null   As [CLAIM!2!insurer_email],
  null   As [CLAIM!2!insurer_claim_number],
  null   As [CLAIM!2!insurer_contact_name],
  null   As [CLAIM!2!domiciled_for_tax],
  null   As [CLAIM!2!tax_number],
  null   As [CLAIM!2!comments],
  null   As [CLAIM!2!underwriting_year_code],
  null   As [CLAIM!2!created_date],
  null   As [CLAIM!2!created_by_username],
  null   As [CLAIM!2!last_modified_date],
  null   As [CLAIM!2!last_modified_by_username],
  null   As [CLAIM!2!base_claim_id],
  Null                    As [CLAIM!2!case_id],
  Null                    As [CLAIM!2!case_number],
  Null                    As [CLAIM!2!case_opened_date],
  Null                    As [CLAIM!2!case_version],
  Null                    As [CLAIM!2!case_progress_code],
  Null                    As [CLAIM!2!case_analyst_handler],
  Null                    As [CLAIM!2!case_admin_handler],
  Null                    As [CLAIM!2!case_base_case_id],
  @sXMLDataset            As [CLAIMBUILDER!3!!CDATA],
  Null                    As [CASEBUILDER!4!!CDATA],
  NULL   As [CLAIMPERIL!5!claim_peril_id],
  Null                    As [RESERVE!6!reserve_id],
  Null           As [RESERVE!6!reserve_description],
  Null           As [RESERVE!6!reserve_amount],
  Null              As [RESERVE!6!total_incurred],
  NULL As [RECOVERY!7!recovery_id],
  NULL As [RECOVERY!7!recovery_type_code],
  NULL As [RECOVERY!7!recovery_description],
  NULL As [RECOVERY!7!recovery_amount],
  NULL As [RECOVERY!7!total_incurred]
 FROM claim
  INNER JOIN batch ON
   batch.batch_id = claim.batch_id
 WHERE  batch.batch_id = @batch_id
  
UNION ALL

-- *******************************************************************************************
-- FOURTH LEVEL - CASEBUILDER - PARENT - CLAIM
-- *******************************************************************************************

 Select  4         As Tag,
  2         As Parent,
  Null   As [EXPORT_HEADER!1!xmlns],
  Null       As [EXPORT_HEADER!1!xmlns:xsi],
  Null      As [EXPORT_HEADER!1!xsi:schemaLocation],
  Null   As [EXPORT_HEADER!1!date_exported],
  Null    As [EXPORT_HEADER!1!interface_name],
  Null   As [EXPORT_HEADER!1!parameters_used],
  @batch_id      As [EXPORT_HEADER!1!batch_id],
  Null      As [EXPORT_HEADER!1!batch_reference],
  Null   As [EXPORT_HEADER!1!total_transactions],
  Null    As [EXPORT_HEADER!1!total_amount],
  claim.claim_id   As [CLAIM!2!claim_id],
  Null    As [CLAIM!2!claim_number],
  Null    As [CLAIM!2!policy_number],
  Null    As [CLAIM!2!claim_version],
  Null    As [CLAIM!2!description],
  Null    As [CLAIM!2!claim_status_code],
  Null    As [CLAIM!2!progress_status_code],
  Null   As [CLAIM!2!primary_cause_code],
  Null    As [CLAIM!2!secondary_cause_code],
  Null    As [CLAIM!2!catastrophe_code],
  Null    As [CLAIM!2!coinsurance_treatment_code],
  null   As [CLAIM!2!loss_from_date],
  null   As [CLAIM!2!loss_to_date],
  null   As [CLAIM!2!reported_date],
  null   As [CLAIM!2!reported_to_date],
  null   As [CLAIM!2!claims_handler_name],
  null   As [CLAIM!2!loss_currency_code],
  null   As [CLAIM!2!info_only_code],
  null   As [CLAIM!2!likely_claim],
  null   As [CLAIM!2!location],
  null   As [CLAIM!2!town_description],
  null   As [CLAIM!2!risk_type_code],
  null   As [CLAIM!2!client_name],
  null   As [CLAIM!2!client_address1],
  null   As [CLAIM!2!client_address2],
  null   As [CLAIM!2!client_address3],
  null   As [CLAIM!2!client_address4],
  null   As [CLAIM!2!client_postal_code],
  null   As [CLAIM!2!client_country],
  null   As [CLAIM!2!client_telephone],
  null   As [CLAIM!2!client_fax],
  null   As [CLAIM!2!client_mobile],
  null   As [CLAIM!2!client_email],
  null   As [CLAIM!2!client_claim_number],
  null   As [CLAIM!2!insurer_name],
  null   As [CLAIM!2!insurer_address1],
  null   As [CLAIM!2!insurer_address2],
  null   As [CLAIM!2!insurer_address3],
  null   As [CLAIM!2!insurer_address4],
  null   As [CLAIM!2!insurer_postal_code],
  null   As [CLAIM!2!insurer_country],
  null   As [CLAIM!2!insurer_telephone],
  null   As [CLAIM!2!insurer_fax],
  null   As [CLAIM!2!insurer_email],
  null   As [CLAIM!2!insurer_claim_number],
  null   As [CLAIM!2!insurer_contact_name],
  null   As [CLAIM!2!domiciled_for_tax],
  null   As [CLAIM!2!tax_number],
  null   As [CLAIM!2!comments],
  null   As [CLAIM!2!underwriting_year_code],
  null   As [CLAIM!2!created_date],
  null   As [CLAIM!2!created_by_username],
  null   As [CLAIM!2!last_modified_date],
  null   As [CLAIM!2!last_modified_by_username],
  null   As [CLAIM!2!base_claim_id],
  NULL        As [CLAIM!2!case_id],
  Null                    As [CLAIM!2!case_number],
  Null                    As [CLAIM!2!case_opened_date],
  Null                    As [CLAIM!2!case_version],
  Null                    As [CLAIM!2!case_progress_code],
  Null                    As [CLAIM!2!case_analyst_handler],
  Null                    As [CLAIM!2!case_admin_handler],
  claim.base_case_id      As [CLAIM!2!case_base_case_id],
  NULL              As [CLAIMBUILDER!3!!CDATA],
  @sCaseXMLDataset        As [CASEBUILDER!4!!CDATA],
  NULL   As [CLAIMPERIL!5!claim_peril_id],
  Null                    As [RESERVE!6!reserve_id],
  Null           As [RESERVE!6!reserve_description],
  Null           As [RESERVE!6!reserve_amount],
  Null              As [RESERVE!6! total_incurred],
  NULL As [RECOVERY!7!recovery_id],
  NULL As [RECOVERY!7!recovery_type_code],
  NULL As [RECOVERY!7!recovery_description],
  NULL As [RECOVERY!7!recovery_amount],
  NULL As [RECOVERY!7!total_incurred]
 FROM claim
  INNER JOIN batch ON
   batch.batch_id = claim.batch_id
 WHERE  batch.batch_id = @batch_id

UNION ALL

-- *******************************************************************************************
-- FIFTH LEVEL - CLAIMPERIL - PARENT - CLAIM
-- *******************************************************************************************

 Select  5         As Tag,
  2         As Parent,
  Null   As [EXPORT_HEADER!1!xmlns],
  Null       As [EXPORT_HEADER!1!xmlns:xsi],
  Null      As [EXPORT_HEADER!1!xsi:schemaLocation],
  Null   As [EXPORT_HEADER!1!date_exported],
  Null    As [EXPORT_HEADER!1!interface_name],
  Null   As [EXPORT_HEADER!1!parameters_used],
  @batch_id      As [EXPORT_HEADER!1!batch_id],
  Null      As [EXPORT_HEADER!1!batch_reference],
  Null   As [EXPORT_HEADER!1!total_transactions],
  Null    As [EXPORT_HEADER!1!total_amount],
  claim.claim_id   As [CLAIM!2!claim_id],
  Null    As [CLAIM!2!claim_number],
  Null    As [CLAIM!2!policy_number],
  Null    As [CLAIM!2!claim_version],
  Null    As [CLAIM!2!description],
  Null    As [CLAIM!2!claim_status_code],
  Null    As [CLAIM!2!progress_status_code],
  Null   As [CLAIM!2!primary_cause_code],
  Null    As [CLAIM!2!secondary_cause_code],
  Null    As [CLAIM!2!catastrophe_code],
  Null    As [CLAIM!2!coinsurance_treatment_code],
  null   As [CLAIM!2!loss_from_date],
  null   As [CLAIM!2!loss_to_date],
  null   As [CLAIM!2!reported_date],
  null   As [CLAIM!2!reported_to_date],
  null   As [CLAIM!2!claims_handler_name],
  null   As [CLAIM!2!loss_currency_code],
  null   As [CLAIM!2!info_only_code],
  null   As [CLAIM!2!likely_claim],
  null   As [CLAIM!2!location],
  null   As [CLAIM!2!town_description],
  null   As [CLAIM!2!risk_type_code],
  null   As [CLAIM!2!client_name],
  null   As [CLAIM!2!client_address1],
  null   As [CLAIM!2!client_address2],
  null   As [CLAIM!2!client_address3],
  null   As [CLAIM!2!client_address4],
  null   As [CLAIM!2!client_postal_code],
  null   As [CLAIM!2!client_country],
  null   As [CLAIM!2!client_telephone],
  null   As [CLAIM!2!client_fax],
  null   As [CLAIM!2!client_mobile],
  null   As [CLAIM!2!client_email],
  null   As [CLAIM!2!client_claim_number],
  null   As [CLAIM!2!insurer_name],
  null   As [CLAIM!2!insurer_address1],
  null   As [CLAIM!2!insurer_address2],
  null   As [CLAIM!2!insurer_address3],
  null   As [CLAIM!2!insurer_address4],
  null   As [CLAIM!2!insurer_postal_code],
  null   As [CLAIM!2!insurer_country],
  null   As [CLAIM!2!insurer_telephone],
  null   As [CLAIM!2!insurer_fax],
  null   As [CLAIM!2!insurer_email],
  null   As [CLAIM!2!insurer_claim_number],
  null   As [CLAIM!2!insurer_contact_name],
  null   As [CLAIM!2!domiciled_for_tax],
  null   As [CLAIM!2!tax_number],
  null   As [CLAIM!2!comments],
  null   As [CLAIM!2!underwriting_year_code],
  null   As [CLAIM!2!created_date],
  null   As [CLAIM!2!created_by_username],
  null   As [CLAIM!2!last_modified_date],
  null   As [CLAIM!2!last_modified_by_username],
  null   As [CLAIM!2!base_claim_id],
  Null                    As [CLAIM!2!case_id],
  Null                    As [CLAIM!2!case_number],
  Null                    As [CLAIM!2!case_opened_date],
  Null                    As [CLAIM!2!case_version],
  Null                    As [CLAIM!2!case_progress_code],
  Null                    As [CLAIM!2!case_analyst_handler],
  Null                    As [CLAIM!2!case_admin_handler],
  Null                    As [CLAIM!2!case_base_case_id],
  Null                    As [CLAIMBUILDER!3!!CDATA],
  Null                    As [CASEBUILDER!4!!CDATA],
  claim_peril.claim_peril_id As [CLAIMPERIL!5!claim_peril_id],
  Null                    As [RESERVE!6!reserve_id],
  Null           As [RESERVE!6!reserve_description],
  Null           As [RESERVE!6!reserve_amount],
  Null              As [RESERVE!6! total_incurred],
  NULL As [RECOVERY!7!recovery_id],
  NULL As [RECOVERY!7!recovery_type_code],
  NULL As [RECOVERY!7!recovery_description],
  NULL As [RECOVERY!7!recovery_amount],
  NULL As [RECOVERY!7!total_incurred]
 FROM claim
  INNER JOIN batch ON
   batch.batch_id = claim.batch_id
  INNER JOIN claim_peril ON
   claim_peril.claim_id = claim.claim_id
 WHERE  batch.batch_id = @batch_id

UNION ALL

-- *******************************************************************************************
-- SIXTH LEVEL - RESERVE - PARENT - CLAIMPERIL
-- *******************************************************************************************

 Select  6         As Tag,
  5         As Parent,
  Null   As [EXPORT_HEADER!1!xmlns],
  Null       As [EXPORT_HEADER!1!xmlns:xsi],
  Null      As [EXPORT_HEADER!1!xsi:schemaLocation],
  Null   As [EXPORT_HEADER!1!date_exported],
  Null    As [EXPORT_HEADER!1!interface_name],
  Null   As [EXPORT_HEADER!1!parameters_used],
  @batch_id  As [EXPORT_HEADER!1!batch_id],
  Null      As [EXPORT_HEADER!1!batch_reference],
  Null   As [EXPORT_HEADER!1!total_transactions],
  Null    As [EXPORT_HEADER!1!total_amount],
  claim.claim_id  As [CLAIM!2!claim_id],
  Null    As [CLAIM!2!claim_number],
  Null    As [CLAIM!2!policy_number],
  Null    As [CLAIM!2!claim_version],
  Null    As [CLAIM!2!description],
  Null    As [CLAIM!2!claim_status_code],
  Null    As [CLAIM!2!progress_status_code],
  Null   As [CLAIM!2!primary_cause_code],
  Null    As [CLAIM!2!secondary_cause_code],
  Null    As [CLAIM!2!catastrophe_code],
  Null    As [CLAIM!2!coinsurance_treatment_code],
  null   As [CLAIM!2!loss_from_date],
  null   As [CLAIM!2!loss_to_date],
  null   As [CLAIM!2!reported_date],
  null   As [CLAIM!2!reported_to_date],
  null   As [CLAIM!2!claims_handler_name],
  null   As [CLAIM!2!loss_currency_code],
  null   As [CLAIM!2!info_only_code],
  null   As [CLAIM!2!likely_claim],
  null   As [CLAIM!2!location],
  null   As [CLAIM!2!town_description],
  null   As [CLAIM!2!risk_type_code],
  null   As [CLAIM!2!client_name],
  null   As [CLAIM!2!client_address1],
  null   As [CLAIM!2!client_address2],
  null   As [CLAIM!2!client_address3],
  null   As [CLAIM!2!client_address4],
  null   As [CLAIM!2!client_postal_code],
  null   As [CLAIM!2!client_country],
  null   As [CLAIM!2!client_telephone],
  null   As [CLAIM!2!client_fax],
  null   As [CLAIM!2!client_mobile],
  null   As [CLAIM!2!client_email],
  null   As [CLAIM!2!client_claim_number],
  null   As [CLAIM!2!insurer_name],
  null   As [CLAIM!2!insurer_address1],
  null   As [CLAIM!2!insurer_address2],
  null   As [CLAIM!2!insurer_address3],
  null   As [CLAIM!2!insurer_address4],
  null   As [CLAIM!2!insurer_postal_code],
  null   As [CLAIM!2!insurer_country],
  null   As [CLAIM!2!insurer_telephone],
  null   As [CLAIM!2!insurer_fax],
  null   As [CLAIM!2!insurer_email],
  null   As [CLAIM!2!insurer_claim_number],
  null   As [CLAIM!2!insurer_contact_name],
  null   As [CLAIM!2!domiciled_for_tax],
  null   As [CLAIM!2!tax_number],
  null   As [CLAIM!2!comments],
  null   As [CLAIM!2!underwriting_year_code],
  null   As [CLAIM!2!created_date],
  null   As [CLAIM!2!created_by_username],
  null   As [CLAIM!2!last_modified_date],
  null   As [CLAIM!2!last_modified_by_username],
  null   As [CLAIM!2!base_claim_id],
  Null                    As [CLAIM!2!case_id],
  Null                    As [CLAIM!2!case_number],
  Null                    As [CLAIM!2!case_opened_date],
  Null                    As [CLAIM!2!case_version],
  Null                    As [CLAIM!2!case_progress_code],
  Null                    As [CLAIM!2!case_analyst_handler],
  Null                    As [CLAIM!2!case_admin_handler],
  Null                    As [CLAIM!2!case_base_case_id],
  Null                    As [CLAIMBUILDER!3!!CDATA],
  Null                    As [CASEBUILDER!4!!CDATA],
  claim_peril.claim_peril_id As [CLAIMPERIL!5!claim_peril_id],
  reserve.reserve_id  As [RESERVE!6!reserve_id],
  Reserve_type.description           As [RESERVE!6!reserve_description],
  reserve.revised_reserve + reserve.initial_reserve - reserve.paid_to_date As [RESERVE!6!reserve_amount],
  reserve.revised_reserve + reserve.initial_reserve  As [RESERVE!6!total_incurred] ,
  NULL As [RECOVERY!7!recovery_id],
  NULL As [RECOVERY!7!recovery_type_code],
  NULL As [RECOVERY!7!recovery_description],
  NULL As [RECOVERY!7!recovery_amount],
  NULL As [RECOVERY!7!total_incurred]
 FROM claim
  INNER JOIN batch ON
    batch.batch_id = claim.batch_id
  INNER JOIN claim_peril ON
    claim_peril.claim_id = claim.claim_id
  INNER JOIN reserve ON
    reserve.claim_peril_id = claim_peril.claim_peril_id
  INNER JOIN Reserve_type ON
	reserve.reserve_type_id = Reserve_type.reserve_type_id
 WHERE  batch.batch_id = @batch_id

	UNION ALL

-- *******************************************************************************************
-- SEVENTH LEVEL - RECOVERY- PARENT - CLAIMPERIL
-- *******************************************************************************************

 Select  7         As Tag,
  5        As Parent,
  Null   As [EXPORT_HEADER!1!xmlns],
  Null       As [EXPORT_HEADER!1!xmlns:xsi],
  Null      As [EXPORT_HEADER!1!xsi:schemaLocation],
  Null   As [EXPORT_HEADER!1!date_exported],
  Null    As [EXPORT_HEADER!1!interface_name],
  Null   As [EXPORT_HEADER!1!parameters_used],
  @batch_id  As [EXPORT_HEADER!1!batch_id],
  Null      As [EXPORT_HEADER!1!batch_reference],
  Null   As [EXPORT_HEADER!1!total_transactions],
  Null    As [EXPORT_HEADER!1!total_amount],
  claim.claim_id  As [CLAIM!2!claim_id],
  Null    As [CLAIM!2!claim_number],
  Null    As [CLAIM!2!policy_number],
  Null    As [CLAIM!2!claim_version],
  Null    As [CLAIM!2!description],
  Null    As [CLAIM!2!claim_status_code],
  Null    As [CLAIM!2!progress_status_code],
  Null   As [CLAIM!2!primary_cause_code],
  Null    As [CLAIM!2!secondary_cause_code],
  Null    As [CLAIM!2!catastrophe_code],
  Null    As [CLAIM!2!coinsurance_treatment_code],
  null   As [CLAIM!2!loss_from_date],
  null   As [CLAIM!2!loss_to_date],
  null   As [CLAIM!2!reported_date],
  null   As [CLAIM!2!reported_to_date],
  null   As [CLAIM!2!claims_handler_name],
  null   As [CLAIM!2!loss_currency_code],
  null   As [CLAIM!2!info_only_code],
  null   As [CLAIM!2!likely_claim],
  null   As [CLAIM!2!location],
  null   As [CLAIM!2!town_description],
  null   As [CLAIM!2!risk_type_code],
  null   As [CLAIM!2!client_name],
  null   As [CLAIM!2!client_address1],
  null   As [CLAIM!2!client_address2],
  null   As [CLAIM!2!client_address3],
  null   As [CLAIM!2!client_address4],
  null   As [CLAIM!2!client_postal_code],
  null   As [CLAIM!2!client_country],
  null   As [CLAIM!2!client_telephone],
  null   As [CLAIM!2!client_fax],
  null   As [CLAIM!2!client_mobile],
  null   As [CLAIM!2!client_email],
  null   As [CLAIM!2!client_claim_number],
  null   As [CLAIM!2!insurer_name],
  null   As [CLAIM!2!insurer_address1],
  null   As [CLAIM!2!insurer_address2],
  null   As [CLAIM!2!insurer_address3],
  null   As [CLAIM!2!insurer_address4],
  null   As [CLAIM!2!insurer_postal_code],
  null   As [CLAIM!2!insurer_country],
  null   As [CLAIM!2!insurer_telephone],
  null   As [CLAIM!2!insurer_fax],
  null   As [CLAIM!2!insurer_email],
  null   As [CLAIM!2!insurer_claim_number],
  null   As [CLAIM!2!insurer_contact_name],
  null   As [CLAIM!2!domiciled_for_tax],
  null   As [CLAIM!2!tax_number],
  null   As [CLAIM!2!comments],
  null   As [CLAIM!2!underwriting_year_code],
  null   As [CLAIM!2!created_date],
  null   As [CLAIM!2!created_by_username],
  null   As [CLAIM!2!last_modified_date],
  null   As [CLAIM!2!last_modified_by_username],
  null   As [CLAIM!2!base_claim_id],
  Null                    As [CLAIM!2!case_id],
  Null                    As [CLAIM!2!case_number],
  Null                    As [CLAIM!2!case_opened_date],
  Null                    As [CLAIM!2!case_version],
  Null                    As [CLAIM!2!case_progress_code],
  Null                    As [CLAIM!2!case_analyst_handler],
  Null                    As [CLAIM!2!case_admin_handler],
  Null                    As [CLAIM!2!case_base_case_id],
  Null                    As [CLAIMBUILDER!3!!CDATA],
  Null                    As [CASEBUILDER!4!!CDATA],
  claim_peril.claim_peril_id As [CLAIMPERIL!5!claim_peril_id],
  NULL As [RESERVE!6!reserve_id],
  NULL As [RESERVE!6!reserve_description],
  NULL As [RESERVE!6!reserve_amount],
  NULL As [RESERVE!6!total_incurred],
  rec.recovery_id As [RECOVERY!7!recovery_id],
  rectype.code As [RECOVERY!7!recovery_type_code],
  rectype.description As [RECOVERY!7!recovery_description],
  (ISNULL(rec.received_to_date,0) + ISNULL(rec.tax_amount,0)) As [RECOVERY!7!recovery_amount],
  (ISNULL(rec.initial_reserve,0) + ISNULL(rec.revised_reserve,0)) As [RECOVERY!7!total_incurred]
 FROM claim
  INNER JOIN batch ON
   batch.batch_id = claim.batch_id
  INNER JOIN claim_peril ON
   claim_peril.claim_id = claim.claim_id
  INNER JOIN [recovery] As rec ON
    rec.claim_peril_id = claim_peril.claim_peril_id
  INNER JOIN recovery_type As rectype ON
	rectype.recovery_type_id = rec.recovery_type_id
 WHERE  batch.batch_id = @batch_id

 ORDER BY
  [EXPORT_HEADER!1!batch_id],
  [CLAIM!2!claim_id],
  [CLAIMBUILDER!3!!CDATA],
  [CASEBUILDER!4!!CDATA],
  [CLAIMPERIL!5!claim_peril_id],
  [Parent],
  [Tag]

  For Xml Explicit
