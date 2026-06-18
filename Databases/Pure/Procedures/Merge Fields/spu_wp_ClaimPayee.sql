SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_ClaimPayee
GO

CREATE PROCEDURE spu_wp_ClaimPayee
   @PartyCnt INT,  
   @InsuranceFileCnt INT,  
   @RiskID INT,  
   @ClaimCnt INT,  
   @DocumentRef VARCHAR(25),  
   @Instance1 INT,  
   @Instance2 INT,  
   @Instance3 INT  
AS 
BEGIN 
  
    DECLARE 
        @party_cnt INT,  
        @payeename VARCHAR(255),
        @address1 VARCHAR(60),
        @address2 VARCHAR(60),
        @address3 VARCHAR(60),
        @address4 VARCHAR(60),
        @postal_code VARCHAR(20),
        @country VARCHAR(50)
      
    DECLARE the_cursor CURSOR FAST_FORWARD FOR  
        SELECT p.party_cnt  
        FROM Claim_Payment p  
        WHERE p.claim_id = @ClaimCnt  
            AND isnull(p.amount, 0) <> 0  
        ORDER BY p.Claim_Payment_id DESC
      
    OPEN the_cursor  
      
    FETCH NEXT FROM the_cursor INTO @party_cnt  
      
    CLOSE the_cursor  
    DEALLOCATE the_cursor  
      
    IF @party_cnt IS NULL
    BEGIN  
        SELECT @payeename = 'Claim Payable Account'  
    END  
    ELSE  
    BEGIN  
        SELECT @payeename = resolved_name  
        FROM party  
        WHERE party_cnt = @party_cnt  
      
        SELECT 
            @address1  = a.address1,  
            @address2  = a.address2,  
            @address3  = a.address3,  
            @address4  = a.address4,  
            @postal_code  = a.postal_code,  
            @country = c.description  
         FROM 
            party_address_usage pau
            INNER JOIN address a
                ON pau.address_cnt = a.address_cnt
                AND pau.party_cnt = @party_cnt  
                AND pau.address_usage_type_id = 4  
            LEFT OUTER JOIN country c  
                ON a.country_id = c.country_id  

    END  
      
    SELECT  
        @payeename name,  
        @address1 address1,  
        @address2 address2,  
        @address3 address3,  
        @address4 address4,  
        @postal_code postal_code,  
        @country country  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO