SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_SAM_PartyBank_Validate
GO
--Start (Prakash C Varghese)-(PartyBank functionality)
CREATE  PROCEDURE spu_SAM_PartyBank_Validate
    @party_cnt INT=NULL ,
    @account_id INT =NULL,
    @party_bank_id INT,
    @is_bank TINYINT=NULL,
    @account_number VARCHAR(50)=NULL,
    @cc_number VARCHAR(30)=NULL,
    @isValid TINYINT OUT,
    @isNumberExisting TINYINT OUT
AS
BEGIN
    SET @isValid=0
    SET @isNumberExisting=0

    IF ISNULL(@Account_ID,0) = 0 AND ISNULL(@Party_Cnt,0) <> 0
        SELECT 
            @Account_ID = account_id
        FROM 
            Account
        WHERE 
            account_key = @Party_Cnt
    
    IF ISNULL(@Account_id,0) <> 0
        IF EXISTS( 
                  SELECT
                      party_bank_id
                  FROM
                      Party_Bank
                  WHERE
                      party_bank_id=@party_bank_id
                     OR account_id=@account_id
                 )
        BEGIN
            SET @isValid=1
            
            --If account/card number is provided, check the existing party bank item has the same account/card number
            IF ISNULL(@account_number,'') <>'' OR ISNULL(@cc_number,'') <> ''
            BEGIN
                IF EXISTS(
                          SELECT 1 
                          FROM 
                              party_bank
                          WHERE
                              party_bank_id=@party_bank_id
                              AND (
                                    (
                                     @is_bank=1 
                                     AND (
                                          ISNULL(@account_number,'')<>'' 
                                          AND LTRIM(RTRIM(account_number))=LTRIM(RTRIM(@account_number))
                                         )
                                    )
                                    OR (
                                        @is_bank=0
                                        AND (
                                             ISNULL(@cc_number,'')<>'' 
                                             AND LTRIM(RTRIM(cc_num))=LTRIM(RTRIM(@cc_number))
                                            )
                                       )
                                  )
                        )
                BEGIN
                    SET @isNumberExisting=1
                END      
            END                                                                                           
        END
END
--End (Prakash C Varghese)-(PartyBank functionality)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


