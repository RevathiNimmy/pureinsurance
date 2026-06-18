SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_Period_Next_Number_saa'
GO
--Start - Renuka - (WPR87 Paralleling)
CREATE  PROCEDURE spu_Period_Next_Number_saa
	@numbering_scheme_id INT,
	@year_name VARCHAR(10)  
AS
BEGIN
    SELECT
		Numbering_Scheme_Id,
		Year_Name,
		Next_Number
	FROM
		Period_Next_Number
	WHERE
		Numbering_Scheme_Id=@numbering_scheme_id
		AND Year_Name=@year_name
END
--End - Renuka - (WPR87 Paralleling)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

