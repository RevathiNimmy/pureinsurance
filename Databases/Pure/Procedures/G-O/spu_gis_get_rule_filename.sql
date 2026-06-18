SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_Get_Rule_Filename    Script Date: 13/02/2002 15:55:47 ******/
EXECUTE DDLDropProcedure 'spu_GIS_Get_Rule_Filename'
GO

CREATE PROCEDURE spu_GIS_Get_Rule_Filename
    (@Schemeid int)
AS

declare @RuleFileName varchar(255)

select @RuleFileName = ISNULL([GSC].rule_filename, '')
from   GIS_Scheme [GSC]
where  [GSC].gis_scheme_id = @SchemeId

IF (RTRIM(LTRIM(@RuleFileName)) = '')
BEGIN
    update GIS_Scheme
    set rule_filename = 
        (select DISTINCT RTRIM(m.code) + RTRIM(cast(s.gis_insurer_id AS varchar(10))) +
        SUBSTRING('00000',1,5-LEN(s.scheme_no)) + CAST(S.SCHEME_NO AS VARCHAR(10)) +
        SUBSTRING('00000',1,5-LEN(s.scheme_ver)) + CAST(s.scheme_ver AS VARCHAR(10)) + '.rul'
        from gis_scheme s
        inner join gis_qem_usage u
        on s.gis_scheme_id = u.gis_scheme_id
        inner join gis_data_model m
        on u.gis_data_model_id=m.gis_data_model_id
        where s.gis_scheme_id=@schemeid)
    where gis_scheme_id = @SchemeId
    
    select @RuleFileName = [GSC].rule_filename
    from   GIS_Scheme [GSC]
    where  [GSC].gis_scheme_id = @SchemeId
END

Select @RuleFileName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO