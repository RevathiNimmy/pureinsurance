SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SAM_MediaType_Sel' 
GO

--Start (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.3)
CREATE  PROCEDURE spu_SAM_MediaType_Sel
@MediaTypeCode varchar(10)
AS

SELECT
    mediatype_id,
    description,
    code,
    refund_delay
FROM 
    MediaType
WHERE 
    code = @MediaTypeCode
GO
--End (Sriram P )Tech Spec - PGR013 - SAM Cash Receipt.doc section(6.3)

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO