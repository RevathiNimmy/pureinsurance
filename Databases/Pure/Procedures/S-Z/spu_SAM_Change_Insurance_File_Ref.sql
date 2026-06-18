SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Change_Insurance_File_Ref'
GO
--Start - Renuka - Bug Fixing (PN 66156)

CREATE PROCEDURE spu_SAM_Change_Insurance_File_Ref
    @Insurance_File_Cnt INT,
    @Insurance_Ref VARCHAR(30)
AS
    UPDATE 
		Insurance_File
	SET 
		Insurance_Ref=@Insurance_Ref
	WHERE
		Insurance_File_Cnt=@Insurance_File_Cnt
GO
--End - Renuka - Bug Fixing (PN 66156)
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
