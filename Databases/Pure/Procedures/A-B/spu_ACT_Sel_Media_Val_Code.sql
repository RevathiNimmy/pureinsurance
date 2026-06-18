SET QUOTED_IDENTIFIER OFF 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Sel_Media_Val_Code'
GO

SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_ACT_Sel_Media_Val_Code
@mediaid int
as
select mv.code from mediatype_validation mv, mediatype m where
mv.mediatype_validation_id = m.mediatype_validation_id
and m.mediatype_id =  @mediaid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

