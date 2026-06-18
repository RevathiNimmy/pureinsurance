SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_claim_policy_upd'
GO

CREATE PROCEDURE spu_claim_policy_upd
    @Claim_id int,  
    @Policy_id int,  
    @Policy_Number varchar(30),  
    @Last_modified_date datetime,  
    @Client_name varchar(50),  
    @Client_address int,  
    @Client_tel_no varchar(255),  
    @Client_fax_no varchar(255),  
    @Client_mobile_no varchar(255),  
    @Client_email varchar(255),  
    @Client_claim_number varchar(20),  
    @Insurer_name varchar(50),  
    @insurer_address int,  
    @insurer_tel_no varchar(255),  
    @insurer_fax_no varchar(255),  
    @insurer_email varchar(255),  
    @insurer_claim_number varchar(20),  
    @insurer_contact varchar(50),  
    @Client_Short_name char(20),  
    @Insurer_Short_name char(20),  
    @Client_tel_no_off varchar(255)  
AS  

BEGIN  

    UPDATE CLAIM SET  
    Policy_id = @Policy_id,  
    Policy_Number = @Policy_Number,  
    Last_modified_date = @Last_modified_date,  
    Client_name = @Client_name,  
    Client_address = @Client_address,  
    Client_tel_no = @Client_tel_no,  
    Client_fax_no = @Client_fax_no,  
    Client_mobile_no = @Client_mobile_no,  
    Client_email = @Client_email,  
    Client_claim_number = @Client_claim_number,  
    Insurer_name = @Insurer_name,  
    insurer_address = @insurer_address,  
    insurer_tel_no = @insurer_tel_no,  
    insurer_fax_no = @insurer_fax_no,  
    insurer_email = @insurer_email,  
    insurer_claim_number = @insurer_claim_number,  
    insurer_contact = @insurer_contact,  
    Client_Short_name =@Client_Short_name,  
    Insurer_Short_name=@Insurer_Short_name,  
    Client_tel_no_off=@Client_tel_no_off  
    WHERE base_claim_id =(SELECT base_claim_id 
							FROM claim where claim_id = @Claim_id)  


END
GO

SET QUOTED_IDENTIFIER OFF	SET ANSI_NULLS ON	SET NOCOUNT OFF
GO

