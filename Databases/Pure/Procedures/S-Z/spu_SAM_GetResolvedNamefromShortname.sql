SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_GetResolvedNamefromShortname'
GO

CREATE PROCEDURE spu_SAM_GetResolvedNamefromShortname
    @shortname varchar(20)
AS
BEGIN
	select resolved_name from party where shortname=@shortname
END
GO
