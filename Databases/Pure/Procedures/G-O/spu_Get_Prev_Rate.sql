SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


EXEC DDLDropProcedure 'spu_Get_Prev_Rate'
GO


CREATE PROCEDURE spu_Get_Prev_Rate
    @GisPolicyLinkID as int,
    @RatingSectionCode as varchar(10)
AS

-- Start at the gis policy link, get all linked insurance files and
-- restrict to those with the matching risk from gpl, this should 
-- always be 1 and it will point to the risk it has replaced.

SELECT 
    rs.annual_rate, rs.rate_type_id
FROM 
    gis_policy_link gpl 
JOIN    
    insurance_file ifi 
    ON ifi.insurance_folder_cnt = gpl.insurance_file_cnt -- Still using bad link
JOIN 
    insurance_file_risk_link ifrl    
    ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt
    AND ifrl.risk_cnt = gpl.risk_id
JOIN 
    rating_section rs    
    ON rs.risk_cnt = ifrl.original_risk_cnt
JOIN 
    rating_section_type rst
    ON rst.rating_section_type_id = rs.rating_section_type_id
WHERE 
    gpl.gis_policy_link_id = @GisPolicyLinkID
AND rst.code = @RatingSectionCode

GO


