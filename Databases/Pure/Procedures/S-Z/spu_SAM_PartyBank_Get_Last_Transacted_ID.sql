SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_SAM_PartyBank_Get_Last_Transacted_ID
GO
--Start (Prakash C Varghese)-(PartyBank functionality)
CREATE  PROCEDURE spu_SAM_PartyBank_Get_Last_Transacted_ID
    @insurance_file_cnt INT =NULL,
    @claim_id INT=NULL,
    @party_bank_id INT OUT
AS
BEGIN
    DECLARE @Party_Key AS INTEGER
    DECLARE @Account_Key AS INTEGER

    SET @party_bank_id=0

    IF ISNULL(@insurance_file_cnt,0) <> 0
    BEGIN
        DECLARE @PaymentMethod AS VARCHAR(60)
        --Get the payment method and party key of this insurance file
        SELECT 
            @PaymentMethod=Payment_Method,
            @Party_Key=Insured_Cnt
        FROM 
            Insurance_file
        WHERE 
            Insurance_file_cnt=@insurance_file_cnt

        --Get the account key of the party 
        SELECT 
            @Account_Key=Account_ID
        FROM 
            Account
        WHERE 
            Account_Key=@Party_Key
        
        --Select only the party bank key which belongs to selected party

        IF @PaymentMethod='PayNow'
        BEGIN
            SELECT DISTINCT
                @party_bank_id=ISNULL(P.Party_Bank_Id,0)
            FROM 
                Insurance_File I
                INNER JOIN Document D
                    ON D.Insurance_File_Cnt=I.Insurance_File_Cnt
                INNER JOIN TransDetail T
                    ON T.Document_Id=D.Document_Id
                INNER JOIN AllocationDetail A
                    ON A.TransDetail_Id=T.TransDetail_Id
                INNER JOIN CashListItem C
                    ON C.CashListItem_Id=A.CashListItem_Id
                INNER JOIN Party_Bank P
                    ON P.Party_Bank_Id=C.Party_Bank_Id
            WHERE 
                I.Insurance_File_Cnt=@insurance_file_cnt
                AND P.Account_Id=@Account_Key 
        END
        ELSE IF @PaymentMethod='Instalments' OR @PaymentMethod='Credit Card' OR @PaymentMethod='Direct Debit'
        BEGIN
            SELECT 
                @party_bank_id=ISNULL(P.Party_Bank_Id,0)
            FROM
                PFPremiumFinance PF
                INNER JOIN Party_Bank P
                    ON P.Party_Bank_Id=PF.Party_Bank_Id
            WHERE
                PF.Insurance_File_Cnt=@insurance_file_cnt
                AND PF.PFPrem_Finance_Version=(SELECT
                                                    MAX(PFPrem_Finance_Version)
                                                FROM 
                                                    PFPremiumFinance
                                                WHERE
                                                    PFPrem_Finance_Cnt=PF.PFPrem_Finance_Cnt)
                AND P.Account_Id=@Account_Key
        END
    END
    ELSE IF ISNULL(@claim_id,0) <> 0
    BEGIN
        SELECT 
            @Party_Key=I.Insured_Cnt
        FROM 
            Claim C
            INNER JOIN Insurance_File I
                ON I.Insurance_File_Cnt=C.Policy_Id
        WHERE 
            C.Claim_ID=@claim_id

        --Get the account key of the party 
        SELECT 
            @Account_Key=Account_ID
        FROM 
            Account
        WHERE 
            Account_Key=@Party_Key 

        SELECT 
            @party_bank_id=ISNULL(P.Party_Bank_Id,0)
        FROM 
            Claim_Payment CP
            INNER JOIN Party_Bank P
                ON P.Party_Bank_Id=CP.Party_Bank_Id
        WHERE
            CP.Claim_ID=@claim_id
            AND CP.Claim_Payment_ID=(SELECT
                                         MAX(Claim_Payment_ID)
                                     FROM 
                                         Claim_Payment
                                     WHERE
                                         Claim_ID=CP.Claim_ID
                                    )
            AND P.Account_Id=@Account_Key
    END
END
--End (Prakash C Varghese)-(PartyBank functionality)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

