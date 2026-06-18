SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_party_group_client'
GO


CREATE PROCEDURE spu_wp_party_group_client
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @party_group_type VARCHAR(255) OUTPUT,
    @is_registered_charity TINYINT OUTPUT,
    @charity_number VARCHAR(255) OUTPUT,
    @number_of_members INT OUTPUT,
    @turnover VARCHAR(255) OUTPUT,  
    @contact VARCHAR(255) OUTPUT  
AS

SELECT  
    @party_group_type = pgt.description,
    @is_registered_charity = pgc.is_registered_charity,
    @charity_number = pgc.charity_number,
    @number_of_members = pgc.number_of_members,
    @turnover = tb.description 
FROM party_group_client pgc
LEFT JOIN party_group_type pgt
    ON pgt.party_group_type_id = pgc.party_group_type_id
LEFT JOIN turnoverband tb
    ON tb.turnoverband_id = pgc.turnover
WHERE pgc.party_cnt = @PartyCnt

SELECT 
    @contact = c.description  
FROM contact c  
JOIN party_contact_usage pcu  
    ON pcu.contact_cnt = c.contact_cnt  
JOIN contact_type ct  
    ON ct.contact_type_id = c.contact_type_id  
WHERE pcu.party_cnt = @PartyCnt  
AND ct.code = 'MAIN' 

GO


