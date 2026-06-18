SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_partyBankDetails'
GO


CREATE PROCEDURE spu_wp_partyBankDetails 
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  

DECLARE @Party_Bank_id INT 

Select @Party_Bank_id = @instance2

IF (Select is_bank FROM Party_Bank WHERE Party_bank_id= @Party_bank_id)=1 
BEGIN
        SELECT
            Account_Type, 
            Account_holder_name, 
            Account_number,
            bank_name_id,
            bank_branch,
            bank_branch_code,
            bank_add1 As PBAddress1,
            bank_add2 As PBAddress2,
            bank_add3 As PBAddress3,
            bank_town AS PBTown,
            bank_pcode As PINCODE ,
            bank_country as Country,
			business_identifier_code ,
			international_bank_account_number,
			CB.description bank_name   
            
         FROM Party_Bank  PB 
		 LEFT JOIN CashListItem_Bank CB on pb.bank_name_id=CB.cashlistitem_bank_id 
         WHERE  Party_Bank_id =@Party_Bank_id
END
ELSE
BEGIN
        SELECT
            Account_Type, 
            Account_holder_name, 
            cc_num As Account_number,
            bank_name_id,
            bank_branch,
            bank_branch_code,
            cc_add1 As PBAddress1,
            cc_add2 As PBAddress2,
            cc_add3 As PBAddress3,
            cc_town AS PBTown,
            cc_pcode As PINCODE ,
            cc_country as Country,
            cc_start_date,
            cc_expiry_date,
            name_on_card,
            manual_auth_number
        FROM Party_Bank 
        WHERE  Party_Bank_id =@Party_Bank_id
END

