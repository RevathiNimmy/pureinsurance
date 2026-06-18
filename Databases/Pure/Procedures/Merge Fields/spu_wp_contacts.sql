SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_contacts'
GO


CREATE PROCEDURE spu_wp_contacts
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS


DECLARE @telephone VARCHAR(255),
    @telephone_area_code VARCHAR(10),
    @telephone_number VARCHAR(255),
    @telephone_extension VARCHAR(6),
    @fax VARCHAR(255),
    @fax_area_code VARCHAR(10),
    @fax_number VARCHAR(255),
    @fax_extension VARCHAR(6),
    @email_area_code VARCHAR(10),
    @email_number VARCHAR(255),
    @email_extension VARCHAR(6),
    @mobile VARCHAR(255),
    @mobile_area_code VARCHAR(10),
    @mobile_number VARCHAR(255),
    @mobile_extension VARCHAR(6),
    @contact_code VARCHAR(10),
    @area_code VARCHAR(10),
    @number VARCHAR(255),
    @extension VARCHAR(6)

    SELECT @contact_code = "TELEPHONE"

    EXEC spu_wp_get_contact  @PartyCnt,
                @InsuranceFileCnt,
                @ClaimCnt,
                @contact_code,
                @area_code OUTPUT,
                @number OUTPUT,
                @extension OUTPUT

    SELECT  @telephone_area_code = @area_code,
        @telephone_number = @number,
        @telephone_extension = @extension

    SELECT  @telephone = @area_code + " " + @number

    IF @extension <> "      "
        SELECT @telephone = @telephone + " ext: " + @extension

    SELECT @contact_code = "FAX"

    EXEC spu_wp_get_contact  @PartyCnt,
                @InsuranceFileCnt,
                @ClaimCnt,
                @contact_code,
                @area_code OUTPUT,
                @number OUTPUT,
                @extension OUTPUT

    SELECT  @fax = @area_code + " " + @number,
        @fax_area_code = @area_code,
        @fax_number = @number,
        @fax_extension = @extension

    SELECT @contact_code = "EMAIL"

    EXEC spu_wp_get_contact  @PartyCnt,
                @InsuranceFileCnt,
                @ClaimCnt,
                @contact_code,
                @area_code OUTPUT,
                @number OUTPUT,
                @extension OUTPUT

    SELECT  @email_area_code = @area_code,
        @email_number = @number,
        @email_extension = @extension

    SELECT @contact_code = "MOBILE"

    EXEC spu_wp_get_contact  @PartyCnt,
                @InsuranceFileCnt,
                @ClaimCnt,
                @contact_code,
                @area_code OUTPUT,
                @number OUTPUT,
                @extension OUTPUT

    SELECT  @mobile = @area_code + " " + @number,
        @mobile_area_code = @area_code,
        @mobile_number = @number,
        @mobile_extension = @extension

    SELECT  'telephone' = @telephone,
        'telephone_area_code' = @telephone_area_code,
        'telephone_number' = @telephone_number,
        'telephone_extension' = @telephone_extension,
        'fax' = @fax,
        'fax_area_code' = @fax_area_code,
        'fax_number' = @fax_number,
        'fax_extension' = @fax_extension,
        'email_area_code' = @email_area_code,
        'email_number' = @email_number,
        'email_extension' = @email_extension,
        'mobile' = @mobile,
        'mobile_area_code' = @mobile_area_code,
        'mobile_number' = @mobile_number,
        'mobile_extension' = @mobile_extension
GO


