SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_SAM_PartyBank_Validate_Last_Transaction
GO
--Start (Prakash C Varghese)-(PartyBank functionality)
CREATE  PROCEDURE spu_SAM_PartyBank_Validate_Last_Transaction
    @party_cnt INT,
    @insurance_file_cnt INT =NULL,
    @claim_id INT=NULL,
    @isValidTransaction TINYINT OUT,
    @isValidTransactionParty TINYINT OUT
AS
BEGIN
    DECLARE @TransactionPartyKey AS INTEGER
    
    SET @isValidTransaction=0
    SET @isValidTransactionParty=0
    SET @TransactionPartyKey=0

    IF ISNULL(@insurance_file_cnt,0) <> 0
    BEGIN
        
        --Check insurance file is valid
        IF EXISTS(
                  SELECT 
                      Insurance_File_Cnt
                  FROM 
                      Insurance_File               
                  WHERE 
                      Insurance_File_Cnt=@insurance_file_cnt)     
        BEGIN
            SET @isValidTransaction=1
            
            --Check insurance file belongs to provided party
            SELECT 
                @TransactionPartyKey=ISNULL(I.Insured_Cnt,0)
            FROM 
                Insurance_File I              
            WHERE 
                I.Insurance_File_Cnt=@insurance_file_cnt
            
            IF @TransactionPartyKey=@party_cnt
                SET @isValidTransactionParty=1
        END
    END
    ELSE IF ISNULL(@claim_id,0) <> 0
    BEGIN
        --Check claim is valid
        IF EXISTS(
                  SELECT 
                      Claim_Id
                  FROM 
                      Claim               
                  WHERE 
                      Claim_Id=@claim_id)
        BEGIN
            SET @isValidTransaction=1
            
             --Check insurance file belongs to provided party
            SELECT 
                @TransactionPartyKey=ISNULL(I.Insured_Cnt,0)
            FROM
                Claim C 
                INNER JOIN Insurance_File I              
                    ON I.Insurance_File_cnt=C.Policy_ID
            WHERE 
                C.Claim_Id=@claim_id
            
            IF @TransactionPartyKey=@party_cnt
                SET @isValidTransactionParty=1
        END
    END
END
--End (Prakash C Varghese)-(PartyBank functionality)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


