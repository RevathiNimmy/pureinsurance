SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_get_contact'
GO


CREATE PROCEDURE spu_get_contact
    @PartyCnt INT,
    @address_usage_code VARCHAR(10),
    @contact_type_code VARCHAR(10),
    @contact VARCHAR(255) OUTPUT
AS


DECLARE @area_code CHAR(10),
    @number VARCHAR(255),
    @extension CHAR(6),
    @temp VARCHAR(255)
SELECT  @area_code = c.area_code,
    @number = c.number,
    @extension = c.extension
FROM    party_address_usage pau,
    address_usage_type aut,
    contact_address_usage cau,
    contact c,
    contact_type ct
WHERE   pau.party_cnt = @PartyCnt
AND pau.address_usage_type_id = aut.address_usage_type_id
AND aut.code = @address_usage_code
AND cau.address_cnt = pau.address_cnt
AND cau.contact_cnt = c.contact_cnt
AND c.contact_type_id = ct.contact_type_id
AND ct.code = @contact_type_code
SELECT @temp = ""
IF @area_code IS NOT NULL
BEGIN
    SELECT @temp = @area_code
END
IF @number IS NOT NULL
BEGIN
    IF @temp <> ""
    BEGIN
        SELECT @temp = @temp + " "
    END
    SELECT @temp = @temp + @number
END
IF @extension IS NOT NULL
BEGIN
    IF @temp <> ""
    BEGIN
        SELECT @temp = @temp + " "
    END
    SELECT @temp = @temp + @extension
END
SELECT @contact = @temp
GO


