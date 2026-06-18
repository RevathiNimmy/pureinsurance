SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_risk_code_section_sel'
GO

CREATE PROCEDURE spu_TXN_risk_code_section_sel(@risk_code_id int,@include_all_sections bit)

AS

IF @include_all_sections = 1

    SELECT 
	cob.COB_Rating_Section_id,
        cob.code,
        pmc.caption as description,
        is_selected= 
	CASE
	WHEN rtu.risk_code_id IS NULL THEN 0
	ELSE 1
	END,
    CASE
    WHEN EXISTS(SELECT NULL FROM Insurance_COB_Section ISB WHERE ISB.COB_rating_section_id=cob.COB_rating_section_id) THEN 0
    ELSE 1
    END AS can_be_deleted
    FROM  COB_Rating_section cob
    INNER JOIN PMCaption pmc ON pmc.caption_id=cob.caption_id
    LEFT OUTER JOIN risk_tax_usage rtu ON rtu.COB_rating_section_id=cob.COB_rating_section_id
    AND rtu.risk_code_id = @risk_code_id
    WHERE 
     cob.is_deleted=0
    ORDER BY rtu.sequence ASC,cob.COB_rating_section_id ASC

ELSE

   SELECT
       cob.COB_Rating_Section_id,
       cob.code,
       pmc.caption as description,
       1 as is_selected,
       CASE
	   WHEN EXISTS(SELECT NULL FROM Insurance_COB_Section ISB WHERE ISB.COB_rating_section_id=cob.COB_rating_section_id) THEN 0
	   ELSE 1
	   END AS can_be_deleted
   FROM  risk_tax_usage rtu
   INNER JOIN COB_rating_section cob ON rtu.COB_rating_section_id=cob.COB_rating_section_id
   INNER JOIN PMCaption pmc ON pmc.caption_id=cob.caption_id
   WHERE rtu.risk_code_id=@risk_code_id
   ORDER BY rtu.sequence ASC,cob.COB_rating_section_id ASC   


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

