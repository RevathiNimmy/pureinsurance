SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_SAM_PartyBank_Search
GO
--Start (Prakash C Varghese)-(PartyBank functionality)
CREATE  PROCEDURE spu_SAM_PartyBank_Search
    @party_cnt INT=NULL,
    @account_id INT=NULL,
    @account_type VARCHAR(255),
    @bank_payment_type_id INT,
    @is_bank TINYINT=0,
    @account_number VARCHAR(50)=NULL,
    @cc_number VARCHAR(30)=NULL,
    @isAccountTypePresent TINYINT OUT,
    @isNumberExisting TINYINT OUT
AS
BEGIN
    SET @IsAccountTypePresent=0
    SET @isNumberExisting=0

    IF ISNULL(@account_id,0) = 0 AND ISNULL(@party_cnt,0) <> 0
        SELECT 
            @account_ID = account_id
        FROM 
            Account
        WHERE 
            account_key = @party_Cnt

    IF ISNULL(@account_id,0) <> 0
    BEGIN
        --Check account type already exists or not
        IF EXISTS(
                  SELECT
                      Account_Type
                  FROM
                      Party_Bank
                  WHERE
                      Account_ID=@account_id
                      AND Account_Type=@account_type
                      AND bank_payment_type_id=@bank_payment_type_id
                 ) 
        BEGIN
            SET @IsAccountTypePresent=1
        END
        --Check any existing party bank item of this party has same bank account number/ credit card number 
        IF EXISTS(
                  SELECT 1 
                  FROM 
                      party_bank
                  WHERE
                      Account_ID=@account_id
                      AND (
                           (
                            @is_bank=1 
                            AND (
                                 ISNULL(@account_number,'')<>'' 
                                 AND account_number=@account_number
                                )
                           )
                           OR (
                               @is_bank=0
                               AND (
                                    ISNULL(@cc_number,'')<>'' 
                                    AND cc_num=@cc_number
                                   )
                              )
                          )
                 )
        BEGIN
            SET @IsNumberExisting=1
        END
    END
END
--End (Prakash C Varghese)-(PartyBank functionality)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

