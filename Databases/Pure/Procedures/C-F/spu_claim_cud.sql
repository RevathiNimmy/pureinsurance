SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claim_cud'
GO

CREATE PROCEDURE spu_claim_cud    
    @Claim_id int OUTPUT ,    
    @Policy_id int ,    
    @Policy_Number varchar(30) ,    
    @Claim_Number varchar(30) ,    
    @Description varchar(1000)  ,    
    @Claim_Status_id int ,    
    @Progress_Status_id int ,    
    @Primary_Cause_id int ,    
    @Secondary_Cause_id int = NULL ,    
    @Catastrophe_code_id int = NULL ,    
    @Coinsurance_treatment_id int = NULL ,    
    @Loss_from_date datetime ,    
    @Loss_to_date datetime = NULL ,    
    @Reported_date datetime ,    
    @Reported_to_date datetime = NULL ,    
    @Last_modified_date datetime  ,    
    @Handler_id int ,    
    @Currency_id int ,    
    @Info_only Boolean ,    
    @Likely_claim Boolean ,    
    @Location varchar(50) = NULL ,    
    @Town int = NULL ,    
    @Risk_type_id int ,    
    @Client_name varchar(255)  ,    
    @Client_address int ,    
    @Client_tel_no varchar(50) = NULL ,    
    @Client_fax_no varchar(50) = NULL ,    
    @Client_mobile_no varchar(50) = NULL ,    
    @Client_email varchar(50) = NULL ,    
    @Client_claim_number varchar(20) = NULL ,    
    @Insurer_name varchar(255) = NULL ,    
    @insurer_address int = NULL ,    
    @insurer_tel_no varchar(50) = NULL ,    
    @insurer_fax_no varchar(50) = NULL ,    
    @insurer_email varchar(50) = NULL ,    
    @insurer_claim_number varchar(20) = NULL ,    
    @Insurer_Contact varchar(50) = NULL ,    
    @VAT_registered bit ,    
    @VAT_reg_no varchar(20) = NULL ,    
    @Comments varchar(255) = NULL ,    
    @Claims_status_date datetime = NULL ,    
    @Client_short_name char(20) = NULL ,    
    @Insurer_short_name char(20) = NULL ,    
    @Client_tel_no_off varchar(50) = NULL ,    
    @user_defined_field_A int = NULL ,    
    @user_defined_field_B int = NULL ,    
    @user_defined_field_C int = NULL ,    
    @user_defined_field_D int = NULL ,    
    @user_defined_field_E int = NULL ,    
    @Client_id int = NULL ,    
    @Original_Claim_id int = NULL ,    
    @Claim_folder_id           int = NULL ,    
    @Claim_version_number      int = NULL ,    
    @claim_version_status_id   int = NULL ,    
    @create_date              datetime = NULL ,    
    @created_by_id            int = NULL ,    
    @Modified_by_id           int = NULL ,    
    @Acceptance_Status_id     int = NULL ,    
    @gis_screen_id     int = NULL ,    
    @US tinyint    
AS    
BEGIN    
    IF @US = 0    
        RETURN    
    IF @US = 1    
        RETURN -900    
    ELSE IF @US = 2    
    BEGIN    
        UPDATE claim    
        SET    
            Policy_id=@Policy_id ,    
            Policy_Number=@Policy_Number  ,    
            Claim_Number=@Claim_Number  ,    
            Description=@Description   ,    
            Claim_Status_id=@Claim_Status_id  ,    
            Progress_Status_id=@Progress_Status_id  ,    
            Primary_Cause_id=@Primary_Cause_id  ,    
            Secondary_Cause_id=@Secondary_Cause_id  ,    
            Catastrophe_code_id=@Catastrophe_code_id  ,    
            Coinsurance_treatment_id=@Coinsurance_treatment_id  ,    
            Loss_from_date=@Loss_from_date  ,    
            Loss_to_date=@Loss_to_date  ,    
            Reported_date=@Reported_date  ,    
            Reported_to_date=@Reported_to_date  ,    
            Last_modified_date=@Last_modified_date  ,    
            Handler_id=@Handler_id  ,    
            Currency_id=@Currency_id  ,    
            Info_only=@Info_only ,    
            Likely_claim=@Likely_claim  ,    
            Location=@Location  ,    
            Town=@Town  ,    
            Risk_type_id=@Risk_type_id  ,    
            Client_name=@Client_name  ,    
            Client_address=@Client_address  ,    
            Client_tel_no=@Client_tel_no  ,    
            Client_fax_no=@Client_fax_no  ,    
            Client_mobile_no=@Client_mobile_no  ,    
            Client_email=@Client_email  ,    
            Client_claim_number=@Client_claim_number  ,    
            Insurer_name=@Insurer_name  ,    
            insurer_address=@insurer_address  ,    
            insurer_tel_no=@insurer_tel_no  ,    
            insurer_fax_no=@insurer_fax_no  ,    
            insurer_email=@insurer_email  ,    
            insurer_claim_number=@insurer_claim_number  ,    
            Insurer_Contact=@Insurer_Contact  ,    
            VAT_registered=@VAT_registered  ,    
            VAT_reg_no=@VAT_reg_no  ,    
            Comments=@Comments  ,    
            Claims_status_date=@Claims_status_date  ,    
            Client_short_name=@Client_short_name  ,    
            Insurer_short_name=@Insurer_short_name  ,    
            Client_tel_no_off=@Client_tel_no_off  ,    
            user_defined_field_A=@user_defined_field_A  ,    
            user_defined_field_B=@user_defined_field_B  ,    
            user_defined_field_C=@user_defined_field_C  ,    
            user_defined_field_D=@user_defined_field_D  ,    
            user_defined_field_E=@user_defined_field_E  ,    
            Client_id=@Client_id  ,    
            --base_claim_id =@original_claim_id,    
            Claim_folder_id         = @Claim_folder_id ,    
            Claim_version_number    = @Claim_version_number ,    
            claim_version_status_id = @claim_version_status_id ,    
            create_date             = @create_date ,    
            created_by_id           = @created_by_id ,    
            Modified_by_id          = @Modified_by_id ,    
            Acceptance_Status_id    = @Acceptance_Status_id,    
            gis_screen_id           = @gis_Screen_id    

            WHERE claim_id = @claim_id    
    END    
        ELSE    
        RETURN -901    
END    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
