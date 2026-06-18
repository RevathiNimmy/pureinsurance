SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_ActiveRenewalQuote_upd'
GO

CREATE PROCEDURE spu_SIRRen_ActiveRenewalQuote_upd
    @SelectedInsuranceFileCnt int,
    @CurrentActiveInsuranceFileCnt int,
    @OriginalRenewalQuoteInsuranceFileCnt int,
    @Userid int
AS
BEGIN


-- Update the renewal_insurance_file_cnt in the renewal_control table to be @SelectedInsuranceFileCnt
UPDATE renewal_control
SET    renewal_insurance_file_cnt = @SelectedInsuranceFileCnt
WHERE  renewal_insurance_file_cnt = @CurrentActiveInsuranceFileCnt


-- Make the new Active renewal quote have a type of 'Renewal'
UPDATE insurance_file
SET    insurance_file_type_id = (SELECT insurance_file_type_id
                                 FROM   insurance_file_type
				 WHERE  code = 'RENEWAL')
WHERE  insurance_file_cnt = @SelectedInsuranceFileCnt


-- Make the previous Active renewal quote have a type of 'RenewalWIF'
UPDATE insurance_file
SET    insurance_file_type_id = (SELECT insurance_file_type_id
                                 FROM   insurance_file_type
				 WHERE  code = 'RENEWALWIF')
WHERE  insurance_file_cnt = @CurrentActiveInsuranceFileCnt


-- If the previous Active renewal quote is the Original renewal quote then 
-- give it a 'what if reason' of 'Original Renewal'  
IF @CurrentActiveInsuranceFileCnt = @OriginalRenewalQuoteInsuranceFileCnt
   BEGIN
      UPDATE insurance_file_system
      SET    last_trans_description = 'Original Renewal', 
             modified_by_id = @Userid,
             last_modified = getdate()
      WHERE  insurance_file_cnt = @CurrentActiveInsuranceFileCnt 
   END


END
GO