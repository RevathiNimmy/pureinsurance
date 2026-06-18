SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PartyBank_Details_Sel'
GO

CREATE PROCEDURE spu_PartyBank_Details_Sel  
      @Party_Cnt INT = NULL,
    @Account_ID INT = NULL
AS
BEGIN

IF ISNULL(@Account_ID,0) = 0 AND ISNULL(@Party_Cnt,0) <> 0
 SELECT @Account_ID = Account_id FROM Account WHERE account_key = @Party_Cnt;

IF ISNULL(@Account_id,0) <> 0

BEGIN

  SELECT
        -1  RowStatus,
        bank_payment_type_id RowIndex,
        party_bank_id,
        is_bank,
        account_id,
        bank_payment_type_id,
        account_type,
        account_holder_name,
        account_number,
        bank_name_id,
        bank_branch,
        bank_branch_code,
        bank_add1,
        bank_add2,
        bank_add3,
        bank_town,
        bank_pcode,
        bank_region,
        bank_country,
        cc_num,
        cc_start_date,
        cc_expiry_date,
        cc_issue_num,
        cc_pin,
        ISNULL(is_registered,0) is_registered,
        cc_add1,
        cc_add2,
        cc_add3,
        cc_town,
        cc_pcode,
        cc_country,
        is_deleted,
        name_on_card,
        manual_auth_number,
        (SELECT ISNULL(MAX(party_bank_id),0) FROM pfpremiumfinance PF
   Where Party_bank_id = PB.party_bank_id  AND  PF.StatusInd IN ('040','140','010','011','012')) PFLINKEXISTS,

        (SELECT  ISNULL(MAX(party_bank_id),0)  FROM CashListItem
        WHERE PArty_bank_id = PB.party_bank_id ) CLILINKEXISTS,

        (SELECT   ISNULL(MAX(party_bank_id),0) FROM Claim_Payment
        where PArty_bank_id = PB.party_bank_id ) CPLINKEXISTS,
		business_identifier_code,
		international_bank_account_number,
		ISNULL(cc_tracking_number,'') cc_tracking_number,
		ISNULL(is_default,0) is_default
    FROM Party_Bank PB
    WHERE account_id = @Account_ID
END
END
