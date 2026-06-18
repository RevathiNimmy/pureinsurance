SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_claims_stats_details_reins_process'
GO

CREATE PROCEDURE spu_add_claims_stats_details_reins_process    
    @copy_claim_id    INT    
AS    
  
SET NOCount ON  
Declare      
    @claim_id INT,    
    @base_claim_id    INT,    
    @insurance_file_cnt int,      
    @Document_comment varchar(100),      
    @transaction_type_code varchar(10),      
    @created_by_user_id int,      
    @Debit_Credit varchar(1),      
    @transaction_type_id int,      
    @user_id int,      
    @user_name varchar(25),      
    @document_type_id INT,      
    @stats_folder_cnt INT,      
    @stats_folder_cnt_new INT,    
    @ri_version_id INT    
      
        
Select     @Base_claim_id = base_claim_id     
from       claim     
where      Claim_id = @copy_claim_id      
    
Select @ri_version_id = ri_arrangement_version from claim_ri_arrangement where Claim_id = @copy_claim_id      
--#folders to store the Folder created  
Create Table #folders    
( Folder_id int )  
  
Declare stats_details_cur cursor      
    Static      
    For      
    SELECT  claim_id,    
            transaction_type_id    
    FROM    claim    
    WHERE   Base_claim_id = @Base_claim_id and Claim_id<>@copy_Claim_id   
     
Open stats_details_cur      
      
FETCH NEXT FROM stats_details_cur     
            INTO @claim_id,@transaction_type_id     
    WHILE @@Fetch_status=0      
        BEGIN      
            Select      
                    @stats_folder_cnt = Stats_folder_cnt,                                        
                    @insurance_file_cnt=insurance_file_cnt,      
                    @Document_comment=Document_comment,      
                    @transaction_type_code=transaction_type_code,      
                    @transaction_type_id=transaction_type_id,      
                    @created_by_user_id=created_by_user_id,      
                    @Debit_Credit=Debit_Credit,      
                    @user_id=created_by_user_id,      
                    @user_name=created_by_username    
                From Stats_folder      
                Where Stats_folder_cnt=(Select max(Stats_folder_cnt) from Stats_folder Where loss_id=@claim_id AND transaction_type_id = @transaction_type_id)      
              
                SELECT @transaction_type_code = Code    
                FROM   transaction_type    
                WHERE  transaction_type_id = @transaction_type_id    
       
                IF @transaction_type_code = 'C_CR'     
                    BEGIN    
                           SET @document_type_id = 41    
                    END    
                ELSE IF @transaction_type_code = 'C_CP'     
                    BEGIN    
                           SET @document_type_id = 28    
                    END    
                ELSE IF @transaction_type_code = 'C_CO'     
                    BEGIN    
                           SET @document_type_id = 40    
                    END    
                ELSE IF @transaction_type_code = 'C_SV' OR @transaction_type_code = 'C_RV'    
                    BEGIN    
                           SET @document_type_id = 29    
                    END    
                       
    
                --Create a New Stats Folder      
                ----------------------------      
                EXEC spu_add_stats_folder_claims      
                    @stats_folder_cnt=@stats_folder_cnt_new output,      
                    @insurance_file_cnt=@insurance_file_cnt,      
                    @Debit_Credit=@Debit_Credit,      
                    @document_comment=@Document_comment ,      
                    @transaction_type_id=@transaction_type_id ,      
                    @transaction_type_code=@transaction_type_code,      
                    @user_id=@user_id,      
                    @user_name=@user_name,      
                    @claim_id=@claim_id,      
                    @documenttype_id=@document_type_id      
                      
                    Insert into #folders Values(@stats_folder_cnt_new)            
    
Insert into stats_detail(      
            stats_folder_cnt,      
            stats_detail_id,      
            stats_detail_type,      
            risk_id,      
            risk_type_id,      
            risk_type_code,      
            peril_id,      
            peril_description,      
            peril_type_id,      
            peril_type_code,      
            policy_section_type_id,      
            policy_section_type_code,      
            class_of_business_id,      
            class_of_business_code,      
            tax_type_id,      
            tax_type_code,      
            tax_value,      
            ri_party_cnt,      
            ri_shortname,      
            ri_party_type,      
            ri_share_percent,      
            ri_agreement_code,      
            annual_premium,      
            currency_code,      
            currency_rate,      
            this_premium_original,      
            this_premium_home,      
            commission_percent,      
            lead_commission_value_home,      
            sub_commission_value_home,      
            sum_insured_home,      
            sum_insured_currency_code,      
            sum_insured_change,      
            transaction_ledger_id,      
            transaction_account_id,      
            account_type_code,      
            ceded_ref,      
            cover_share_percent,      
            sum_insured_total,      
            charges_total,      
            taxes_total,      
            recoveries_total,      
            commission_excluded,      
            withholding_tax_excluded,      
            purchase_order_no,      
            purchase_invoice_no,      
            stats_version,      
            this_premium_system,      
            lead_commission_value_system,      
            sub_commission_value_system,      
            sum_insured_system,      
            is_commission_modified,      
            original_flag,      
            cover_to_date,      
            Claim_RI_Only_Amendment)      
      
Select @stats_folder_cnt_New,      
    1,      
    stats_detail_type,      
    risk_id,      
    risk_type_id,      
    risk_type_code,      
    peril_id,      
    peril_description,      
    peril_type_id,      
    peril_type_code,      
    policy_section_type_id,      
    policy_section_type_code,      
    class_of_business_id,      
    class_of_business_code,      
    tax_type_id,      
    tax_type_code,      
    tax_value ,      
    ri_party_cnt,      
    ri_shortname,      
    ri_party_type,      
    ri_share_percent,      
    ri_agreement_code,      
    annual_premium ,      
    currency_code,      
    currency_rate,      
    this_premium_original ,      
    this_premium_home ,      
    commission_percent,      
    lead_commission_value_home ,      
    sub_commission_value_home ,      
    sum_insured_home ,      
    sum_insured_currency_code,      
    sum_insured_change,      
    transaction_ledger_id,      
    transaction_account_id,      
    account_type_code,      
    ceded_ref,      
    cover_share_percent,      
    sum_insured_total ,      
    charges_total ,      
    taxes_total ,      
    recoveries_total ,      
    commission_excluded ,      
    withholding_tax_excluded ,      
    purchase_order_no,      
    purchase_invoice_no,      
    stats_version,      
    this_premium_system ,      
    lead_commission_value_system ,      
    sub_commission_value_system ,      
    sum_insured_system ,      
    is_commission_modified,      
    original_flag,      
    cover_to_date,      
    1      
    From Stats_detail                    
    WHERE Stats_folder_cnt=@stats_folder_cnt    
    AND stats_detail_type = 'GRS'    
    
    -- Copy the Claim Ri arrangement lines in accordance to the new ri arragement    
    EXEC spu_Claim_Copy_RIArrangementLines    
                    @Claim_ID = @claim_id,      
                    @Copy_Claim_ID = @Copy_Claim_Id,    
                    @ri_version_id = @ri_version_id    
    
    
    EXEC spu_add_claims_stats_details_reins    
                    @ClaimID = @claim_id,      
                    @stats_folder_cnt = @stats_folder_cnt_New,    
                    @documenttype_id = @document_type_id,    
                    @ri_version_id = @ri_version_id    
    
        Delete from Claim_RI_arrangement_line where RI_Arrangement_id in(  
        Select RI_arrangement_ID From Claim_ri_arrangement where ri_band_id is null  
        AND ri_arrangement_version>1 and Claim_id=@Claim_id)     
           
         Delete from Claim_ri_arrangement where ri_band_id is null  
         and ri_arrangement_version>1 and Claim_id=@Claim_id  
   
        FETCH NEXT FROM stats_details_cur     
                    INTO @claim_id,@transaction_type_id     
            
        END    
    
            
CLOSE stats_details_cur    
DEALLOCATE stats_details_cur    
  
SET NOCount OFF  
  
Select * from #folders    
    
    
SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO
    
    
    
    
    
    
    
    
  
