SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBank_Details_ByID'
GO

CREATE PROCEDURE spu_PartyBank_Details_ByID
    @party_bank_id 			INT

As
BEGIN
		SELECT
			-1 	RowStatus,
			0 	RowIndex,
			PB.party_bank_id,
			PB.is_bank,
			PB.account_id,
			PB.bank_payment_type_id,
			PB.account_type,
			PB.account_holder_name,	
			PB.account_number,
			PB.bank_name_id,
			PB.bank_branch,
			PB.bank_branch_code,
			PB.bank_add1,
			PB.bank_add2,
			PB.bank_add3,
			PB.bank_town,
			PB.bank_pcode,
			PB.bank_region,
			PB.bank_country,
			PB.cc_num,
			PB.cc_start_date,
			PB.cc_expiry_date,
			PB.cc_issue_num,
			PB.cc_pin,
			PB.is_registered,
			PB.cc_add1,
			PB.cc_add2,
			PB.cc_add3,
			PB.cc_town,
			PB.cc_pcode,
			PB.cc_country,
			PB.is_deleted,
			PB.name_on_card,
			PB.manual_auth_number,
			CLIB.description,
	   (SELECT ISNULL(MAX(party_bank_id),0) FROM pfpremiumfinance PF 
			Where Party_bank_id =@party_bank_id) PFLINKEXISTS,  
          
			(SELECT  ISNULL(MAX(party_bank_id),0)  
			FROM CashListItem WHERE PArty_bank_id =@party_bank_id) CLILINKEXISTS,  
                             
			(SELECT   ISNULL(MAX(party_bank_id),0)  
			FROM Claim_Payment    where PArty_bank_id =@party_bank_id) CPLINKEXISTS, 
			PB.business_identifier_code,
			PB.international_bank_account_number,
			ISNULL(pb.is_default,0)
		FROM Party_Bank PB
		LEFT OUTER JOIN cashlistitem_bank CLIB
		ON CLIB.cashlistitem_bank_id=PB.bank_name_id  
		WHERE PB.party_bank_id = @party_bank_id
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO