SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure  "sp_pm_iccs"
GO
EXECUTE DDLDropProcedure "spu_pm_iccs"
GO
CREATE PROCEDURE spu_pm_iccs @ICCS char(4) output as select @ICCS = 
"[replace]"
GO
CREATE PROCEDURE sp_pm_iccs @ICCS char(4) output as execute spu_pm_iccs @ICCS output
GO