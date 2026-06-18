SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_ClaimLossAdjuster
GO

CREATE PROCEDURE spu_wp_ClaimLossAdjuster
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
        @resolved_name VARCHAR(100),  
        @address1 VARCHAR(60),  
        @address2 VARCHAR(60),  
        @address3 VARCHAR(60),  
        @address4 VARCHAR(60),  
        @postal_code VARCHAR(20),  
        @country VARCHAR(255)  
  
    SELECT 
        @resolved_name = NULL,  
        @address1 = NULL,  
        @address2 = NULL,  
        @address3 = NULL,  
        @address4 = NULL,  
        @postal_code = NULL,  
        @country = NULL  
  
    SELECT 
        @resolved_name = p.resolved_name,  
        @address1 = a.address1,  
        @address2 = a.address2,  
        @address3 = a.address3,  
        @address4 = a.address4,  
        @postal_code = (
                        CASE a.postal_code  
                            WHEN a.address_id THEN ''  
                        ELSE a.postal_code  
                        END
                       ),  
        @country = c.description  
    FROM
        claim_expert_service ces
        INNER JOIN party p  
           ON ces.party_claim_id = p.party_cnt
           AND ces.claim_id = @ClaimCnt  
           AND ces.Service_type_id = 2  
           AND p.party_type_id IN (  
                                    SELECT party_type_id  
                                    FROM party_type  
                                    WHERE code = 'OTLOSTADJ'  
                                   ) 
        INNER JOIN party_address_usage pau
            ON pau.party_cnt = p.party_cnt 
        INNER JOIN address a
            ON pau.address_cnt = a.address_cnt
        INNER JOIN address_usage_type aut
            ON pau.address_usage_type_id = aut.address_usage_type_id 
            AND aut.code = '3131 XCO'   
        LEFT OUTER JOIN country c  
            ON  a.country_id = c.country_id  
     SELECT 
        @resolved_name AS resolved_name,  
        @address1 AS address1,  
        @address2 AS address2,  
        @address3 AS address3,  
        @address4 AS address4,  
        @postal_code AS postal_code,  
        @country AS country

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO