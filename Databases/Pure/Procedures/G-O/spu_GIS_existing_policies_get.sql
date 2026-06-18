EXECUTE DDLDropProcedure 'spu_GIS_existing_policies_get'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


--JAS 03/10/2002 ref804: created 

CREATE PROCEDURE spu_GIS_existing_policies_get
@prefix varchar(255)
 AS

SELECT INSURANCE_REF FROM INSURANCE_FILE WHERE INSURANCE_REF LIKE @prefix +'%'
GO
