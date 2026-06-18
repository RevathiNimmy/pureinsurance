SET QUOTED_IDENTIFIER OFF 
GO
EXECUTE DDLDropProcedure 'spu_Policy_numbering_scheme_GetAndIncrement_Period'
GO
CREATE PROCEDURE spu_Policy_numbering_scheme_GetAndIncrement_Period
    @nNumbering_scheme_id INT,
	@sYear_name VARCHAR(10),
    @nStep INT,
    @nNextNumber INT OUTPUT
AS

IF @nStep <> 0 
BEGIN
    UPDATE
		period_next_number WITH (ROWLOCK) 
	SET
        @nNextNumber = PNN.next_number,
		next_number = PNN.next_number + step
	FROM
		period_next_number PNN
		INNER JOIN numbering_scheme NSM
			ON PNN.numbering_scheme_id = NSM. numbering_scheme_id
	WHERE
		PNN.numbering_scheme_id = @nNumbering_scheme_id
		AND PNN.year_name=@sYear_name
END
ELSE
BEGIN
    UPDATE period_next_number
	SET
        @nNextNumber = PNN.next_number,
		next_number = PNN.next_number + 1
	FROM
	period_next_number PNN
    		INNER JOIN numbering_scheme NSM
            ON PNN.numbering_scheme_id = NSM. numbering_scheme_id
    WHERE
		PNN.numbering_scheme_id = @nNumbering_scheme_id
		AND PNN.year_name=@sYear_name
END