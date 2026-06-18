SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_Increment_Period_Numbering_Scheme'
GO
--Start - Renuka - (WPR87 Paralleling)
CREATE  PROCEDURE spu_Increment_Period_Numbering_Scheme
    @numbering_scheme_id INT,
	@year_name VARCHAR(10)
AS
BEGIN
    UPDATE  
		period_next_number  
	SET 
		next_number = PNN.next_number + step  
	FROM
		period_next_number PNN
		INNER JOIN numbering_scheme NSM
			ON PNN.numbering_scheme_id = NSM. numbering_scheme_id
	WHERE   
		PNN.numbering_scheme_id = @numbering_scheme_id  
		AND PNN.year_name=@year_name
		AND NSM.step <> 0  
  
    UPDATE  
		period_next_number  
	SET 
		next_number = PNN.next_number + 1 
	FROM
		period_next_number PNN
		INNER JOIN numbering_scheme NSM
			ON PNN.numbering_scheme_id = NSM. numbering_scheme_id
	WHERE   
		PNN.numbering_scheme_id = @numbering_scheme_id  
		AND PNN.year_name=@year_name
		AND NSM.step = 0  

END
--End - Renuka - (WPR87 Paralleling)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

