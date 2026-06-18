EXECUTE DDLDropProcedure 'spu_SAM_ValidateAndGet_Underwriting_Year'
GO
CREATE PROCEDURE spu_SAM_ValidateAndGet_Underwriting_Year

	@required_date DATETIME,
	@iCheckUnderWritingYearId INT,
	@underwriting_year_id INT OUTPUT

AS 

BEGIN

	IF EXISTS(SELECT underwriting_year_id FROM	Underwriting_Year
				WHERE	(end_date<=@required_date) 
				AND 	is_deleted = 0
				AND     underwriting_year_id=@iCheckUnderWritingYearId)
		BEGIN	
			SELECT @underwriting_year_id=@iCheckUnderWritingYearId
		END
	ELSE
		BEGIN
			SELECT	@underwriting_year_id=underwriting_year_id
			FROM	Underwriting_Year
			WHERE	@required_date BETWEEN start_date AND end_date
			AND 	is_deleted = 0
		END
END
