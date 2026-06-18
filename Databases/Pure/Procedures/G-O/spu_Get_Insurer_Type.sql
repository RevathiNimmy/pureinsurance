SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLdropprocedure 'spu_Get_Insurer_Type'
go
CREATE  PROCEDURE spu_Get_Insurer_Type 
	
AS
    
SELECT insurer_type_id,description
FROM insurer_type

    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO