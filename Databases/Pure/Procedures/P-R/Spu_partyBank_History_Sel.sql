SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBank_History_Sel'
GO

CREATE PROCEDURE spu_PartyBank_History_Sel
@Party_Cnt	INT = NULL,
@Account_ID	INT = NULL

As
BEGIN

	IF ISNULL(@Account_ID,0) = 0 AND ISNULL(@Party_Cnt,0) <> 0
	SELECT @Account_ID = account_id 
		FROM Account 
		WHERE account_key = @Party_Cnt
	
	IF ISNULL(@Account_id,0) <> 0 
		SELECT
			PBH.action_code,
			PBH.party_bank_id,
			PBH.account_id,
			PBH.bank_payment_type_id,
			PBH.account_type,
			PBH.account_holder_name,	
			PBH.account_number,
			PBH.bank_name_id,
			PBH.bank_branch,
			PBH.bank_branch_code,
			PBH.bank_add1,
			PBH.bank_add2,
			PBH.bank_add3,
			PBH.bank_town,
			PBH.bank_pcode,
			PBH.bank_region,
			PBH.bank_country,
			PBH.cc_num,
			PBH.cc_start_date,
			PBH.cc_expiry_date,
			PBH.cc_issue_num,
			PBH.cc_pin,
			PBH.is_registered,
			PBH.cc_add1,
			PBH.cc_add2,
			PBH.cc_add3,
			PBH.cc_town,
			PBH.cc_pcode,
			PBH.cc_country,
			PBH.user_id,
			PBH.date_modified,
		        PBH.name_on_card,
	                PBH.manual_auth_number,
			U.username,
			PBH.business_identifier_code,
			PBH.international_bank_account_number,
			ISNULL(pbh.is_default,0)
		FROM Party_Bank_History PBH
		INNER JOIN PMUser U
		ON PBH.user_id=U.user_id
		WHERE PBH.account_id = @account_id

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO