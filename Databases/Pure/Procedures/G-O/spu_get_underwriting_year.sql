EXEC DDLDropProcedure 'spu_get_underwriting_year'
GO

CREATE PROCEDURE spu_get_underwriting_year
	@required_date DATETIME,
	@underwriting_year_id INT OUTPUT
AS BEGIN

SELECT	@underwriting_year_id=underwriting_year_id
FROM	Underwriting_Year
WHERE	@required_date BETWEEN start_date AND end_date
AND 	is_deleted = 0
END
GO