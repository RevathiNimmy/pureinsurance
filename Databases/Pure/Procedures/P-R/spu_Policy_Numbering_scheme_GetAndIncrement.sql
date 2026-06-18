SET QUOTED_IDENTIFIER OFF 
GO
EXECUTE DDLDropProcedure 'spu_Policy_Numbering_scheme_GetAndIncrement'
GO
CREATE PROCEDURE spu_Policy_Numbering_scheme_GetAndIncrement
    @nNumbering_scheme_id INT,
    @nStep INT,
    @nNextNumber INT OUTPUT
AS

IF @nStep <> 0 
BEGIN 
    UPDATE numbering_scheme  
    SET @nNextNumber = next_number,
    next_number = next_number + step,
    date_last_generated = GETDATE()
    WHERE   numbering_scheme_id = @nNumbering_scheme_id
END
ELSE
BEGIN
    UPDATE numbering_scheme 
    SET @nNextNumber = next_number,
    next_number = next_number + 1,
    date_last_generated = GETDATE()
    WHERE   numbering_scheme_id = @nNumbering_scheme_id
END
GO