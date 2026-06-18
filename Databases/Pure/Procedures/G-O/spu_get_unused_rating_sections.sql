SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_unused_rating_sections'
GO


CREATE PROCEDURE spu_get_unused_rating_sections
    @insurance_file_cnt int,
    @risk_cnt int
AS

--*******************************************************************************************
-- Version      Author  Date            Desc
-- 1.00.0001        Tom 06 August 2001  Get unused rating sections
--
--*******************************************************************************************
SELECT  rs.rating_section_type_id,
    rs.sum_insured,
    rst.rate,
    rst.rate_type_id
FROM    rating_section rs,
    insurance_file_risk_link ifrl,
    rating_section_type rst
WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt
AND ifrl.risk_cnt = @risk_cnt
AND rs.risk_cnt = ifrl.original_risk_cnt
AND rs.rating_section_type_id = rst.rating_section_type_id
AND rs.rating_section_type_id NOT IN (
    SELECT  rating_section_type_id
    FROM    rating_section
    WHERE   risk_cnt = @risk_cnt)
ORDER BY rs.risk_cnt,
     rs.rating_section_id

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


