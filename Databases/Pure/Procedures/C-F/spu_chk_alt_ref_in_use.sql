EXECUTE DDLDropProcedure 'spu_chk_alt_ref_in_use'
GO

-- returns 1 if any policies have a matching alternate_reference, else return 0
-- changed due to code expecting 1 for matching policies
CREATE PROCEDURE spu_chk_alt_ref_in_use
    @alternate_reference varchar(80)
AS
BEGIN
    DECLARE @temp_ref varchar(255)
    SELECT @temp_ref = 'E-' + @alternate_reference
    IF EXISTS (SELECT * FROM insurance_file
    WHERE alternate_reference = @alternate_reference
    AND insurance_ref = @temp_ref)
    SELECT 1
    ELSE
    SELECT 0
END
GO
