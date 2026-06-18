SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

DDLDropProcedure 'spu_get_earning_pattern_usage'
GO

CREATE PROCEDURE spu_get_earning_pattern_usage
                @rating_section_type_id INT
                
AS
  BEGIN
    SELECT Earning_Pattern_id,
           effective_date
    FROM   Earning_Pattern_Usage
    WHERE  Rating_Section_type_id = @rating_section_type_id
           AND Earning_Pattern_Usage_id = (SELECT MAX(Earning_Pattern_Usage_id)
                                 FROM   Earning_Pattern_Usage
                                 WHERE  Rating_Section_type_id = @rating_section_type_id)
  END

GO