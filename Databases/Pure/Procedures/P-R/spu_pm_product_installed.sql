SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_pm_product_installed'
GO
CREATE PROCEDURE spu_pm_product_installed
    @product_code char(10),
    @description varchar(255),
    @database_name varchar(255)
AS
/****************************************************************************************/
/* sp_pm_product_installed adds an entry in the PMProduct table so that the             */
/* Sirius Architecture knows that the product has been installed & then                 */
/* performs an initial synchronisation of commom table data.                            */
/* INPUTS:  @product_code                                                               */
/*          @description                                                                */
/*          @database_name                                                              */
/* OUTPUTS: n/a                                                                         */
/* RETURN:  0 Table found in database.                                                  */
/*          -100 Database supplied does NOT exist.                                      */
/*          -101 Incorrect Product Code                                                 */
/*          -102 Product Already Installed                                              */
/****************************************************************************************/
/* Revision Description of Modification         Date        Who                         */
/* -------- ---------------------------         ----        ---                         */
/* 1.0      Original                            13/01/1999  RFC                         */
/* 1.1      Additional columns on Source table  17/05/2000  DAK                         */
/*          Add product code for GeminiII                                               */
/* 1.2      Add Company when checking Source    30/05/2000  DAK                         */
/* 1.3      New product family: Claims          07/08/2000  RDC                         */
/****************************************************************************************/
BEGIN
    SET NOCOUNT ON

    DECLARE @caption_id int,
    @pmproduct_id int,
    @SQL varchar(255),
    @SQL2 varchar(255),
    @SQL3 varchar(255),
    @SQL4 varchar(255),
    @table_in_db integer,
    @print varchar(255),
    @db_id smallint

    /* If Product Code is Sirius */
    IF @product_code = 'Sirius' BEGIN
        /* Just update the database name for Sirius Architecture */

        /* Ignore the supplied database for Sirius as we can */
        /* work out what it is */
        SELECT @db_id = DB_ID()
        SELECT @database_name = name from master..sysdatabases where dbid = @db_id

        /* Update the Product entry */
        UPDATE PMProduct
        SET database_name = @database_name
        WHERE code = @product_code

        SELECT @print = 'Sirius PMProduct entry database_name Updated.....'
        PRINT @print
        RETURN
    END

    /* Does the supplied Database Exist */
    IF NOT EXISTS (SELECT name FROM master..sysdatabases WHERE name = @database_name) BEGIN
        SELECT @print = 'A Database named : ' + @database_name + ' does NOT exist!'
        PRINT @print
        RETURN -100
    END

    /* Do we recognise the Product Code */
    IF @product_code NOT IN (
        'SirUnd',
        'Orion',
        'Gemini',
        'Voyager',
        'Mercury',
        'Documaster',
        'SirBroking',
        'SirSol',
        'Nirvana',
        'GeminiII',
        'Claims'
    ) BEGIN
        SELECT @print = 'Product Code must be one of: '
        SELECT @print = @print + ' ' + 'Sirius'
        SELECT @print = @print + ', ' + 'SirUnd'
        SELECT @print = @print + ', ' + 'Orion'
        SELECT @print = @print + ', ' + 'Gemini'
        SELECT @print = @print + ', ' + 'Voyager'
        SELECT @print = @print + ', ' + 'Mercury'
        SELECT @print = @print + ', ' + 'Documaster'
        SELECT @print = @print + ', ' + 'SirBroking'
        SELECT @print = @print + ', ' + 'SirSol'
        SELECT @print = @print + ', ' + 'Nirvana'
        SELECT @print = @print + ', ' + 'GeminiII'
        SELECT @print = @print + ', ' + 'Claims'
        PRINT @print
        RETURN -101
    END

    /* Is this Product Already Installed */
    IF EXISTS (SELECT pmproduct_id FROM PMProduct WHERE code = @product_code) BEGIN
        SELECT @print = 'Product Already Installed!'
        PRINT @print
        RETURN -102
    END

    /************************************************************************/
    /* PMProduct                                                            */
    /************************************************************************/

    /* Get the Next Product ID */
    SELECT @pmproduct_id = ISNULL((SELECT MAX(pmproduct_id) + 1 FROM PMProduct), 1)

    /* Get the Caption ID */
    EXECUTE sp_pm_caption_id_return 1, @description, @caption_id OUTPUT

    /* Insert the Product Record */
    INSERT INTO PMProduct (
        pmproduct_id,
        caption_id,
        code,
        description,
        is_deleted,
        effective_date,
        database_name
    ) VALUES (
        @pmproduct_id,
        @caption_id,
        @product_code,
        @description,
        0,
        getdate(),
        @database_name
    )

    SELECT @print = 'PMProduct entry added.....'
    PRINT @print

    SELECT @print = 'Syncronising data in Common Tables.....'
    PRINT @print

    /************************************************************************/
    /* Languages                                                            */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'Language',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying Language Data.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..Language ("
        SELECT @SQL = @SQL + "Language_id, "
        SELECT @SQL = @SQL + "caption_id, "
        SELECT @SQL = @SQL + "code, "
        SELECT @SQL = @SQL + "description, "
        SELECT @SQL = @SQL + "is_deleted, "
        SELECT @SQL = @SQL + "effective_date "
        SELECT @SQL = @SQL + ") SELECT "

        SELECT @SQL2 = "Language_id, "
        SELECT @SQL2 = @SQL2 + "caption_id, "
        SELECT @SQL2 = @SQL2 + "code, "
        SELECT @SQL2 = @SQL2 + "description, "
        SELECT @SQL2 = @SQL2 + "is_deleted, "
        SELECT @SQL2 = @SQL2 + "effective_date "
        SELECT @SQL2 = @SQL2 + " FROM Language"

        EXECUTE (@SQL + @SQL2)
    END

    /************************************************************************/
    /* Currency                                                             */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'Currency',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying Currency Data.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..Currency ("
        SELECT @SQL = @SQL + "currency_id, "
        SELECT @SQL = @SQL + "caption_id, "
        SELECT @SQL = @SQL + "iso_code, "
        SELECT @SQL = @SQL + "description, "
        SELECT @SQL = @SQL + "symbol, "
        SELECT @SQL = @SQL + "minor_part, "
        SELECT @SQL = @SQL + "code, "
        SELECT @SQL = @SQL + "alignment, "

        SELECT @SQL2 = "decimal_places, "
        SELECT @SQL2 = @SQL2 + "is_deleted, "
        SELECT @SQL2 = @SQL2 + "effective_date, "
        SELECT @SQL2 = @SQL2 + "format_string, "
        SELECT @SQL2 = @SQL2 + "round_to_places "
        SELECT @SQL2 = @SQL2 + ") SELECT "
        SELECT @SQL2 = @SQL2 + "currency_id, "
        SELECT @SQL2 = @SQL2 + "caption_id, "
        SELECT @SQL2 = @SQL2 + "iso_code, "
        SELECT @SQL2 = @SQL2 + "description, "

        SELECT @SQL3 = "symbol, "
        SELECT @SQL3 = @SQL3 + "minor_part, "
        SELECT @SQL3 = @SQL3 + "code, "
        SELECT @SQL3 = @SQL3 + "alignment, "
        SELECT @SQL3 = @SQL3 + "decimal_places, "
        SELECT @SQL3 = @SQL3 + "is_deleted, "
        SELECT @SQL3 = @SQL3 + "effective_date, "
        SELECT @SQL3 = @SQL3 + "format_string, "
        SELECT @SQL3 = @SQL3 + "round_to_places "
        SELECT @SQL3 = @SQL3 + " FROM Currency"

        EXECUTE (@SQL + @SQL2 + @SQL3)
    END

    /************************************************************************/
    /* Country                                                              */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'Country',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying Country Data.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..Country ("
        SELECT @SQL = @SQL + "Country_id, "
        SELECT @SQL = @SQL + "code, "
        SELECT @SQL = @SQL + "description, "
        SELECT @SQL = @SQL + "currency_id, "
        SELECT @SQL = @SQL + "iso_code, "
        SELECT @SQL = @SQL + "caption_id, "

        SELECT @SQL2 = "telephone_code, "
        SELECT @SQL2 = @SQL2 + "country, "
        SELECT @SQL2 = @SQL2 + "is_deleted, "
        SELECT @SQL2 = @SQL2 + "effective_date "
        SELECT @SQL2 = @SQL2 + ") SELECT "
        SELECT @SQL2 = @SQL2 + "Country_id, "
        SELECT @SQL2 = @SQL2 + "code, "
        SELECT @SQL2 = @SQL2 + "description, "

        SELECT @SQL3 = "currency_id, "
        SELECT @SQL3 = @SQL3 + "iso_code, "
        SELECT @SQL3 = @SQL3 + "caption_id, "
        SELECT @SQL3 = @SQL3 + "telephone_code, "
        SELECT @SQL3 = @SQL3 + "country, "
        SELECT @SQL3 = @SQL3 + "is_deleted, "
        SELECT @SQL3 = @SQL3 + "effective_date "
        SELECT @SQL3 = @SQL3 + " FROM Country"

        EXECUTE (@SQL + @SQL2 + @SQL3)
    END

    /************************************************************************/
    /* Source                                                               */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'Source',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying Source Data.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..Source ("
        SELECT @SQL = @SQL + " source_id, "
        SELECT @SQL = @SQL + " caption_id, "
        SELECT @SQL = @SQL + " code, "
        SELECT @SQL = @SQL + " description, "
        SELECT @SQL = @SQL + " parent_id, "
        SELECT @SQL = @SQL + " is_deleted, "
        SELECT @SQL = @SQL + " effective_date, "
        SELECT @SQL = @SQL + " base_currency_id, "
        SELECT @SQL = @SQL + " reg_no_1, "
        SELECT @SQL = @SQL + " reg_no_2, "
        SELECT @SQL = @SQL + " address1, "
        SELECT @SQL = @SQL + " address2, "
        SELECT @SQL = @SQL + " address3, "
        SELECT @SQL = @SQL + " address4, "
        SELECT @SQL = @SQL + " postal_code, "
        SELECT @SQL = @SQL + " country_id, "

        SELECT @SQL2 = " phone_area_code, "
        SELECT @SQL2 = @SQL2 + " phone_number, "
        SELECT @SQL2 = @SQL2 + " phone_extension, "
        SELECT @SQL2 = @SQL2 + " fax_area_code, "
        SELECT @SQL2 = @SQL2 + " fax_number, "
        SELECT @SQL2 = @SQL2 + " fax_extension, "
        SELECT @SQL2 = @SQL2 + " email, "
        SELECT @SQL2 = @SQL2 + " vat_no, "
        SELECT @SQL2 = @SQL2 + " sender_mailbox_id, "
        SELECT @SQL2 = @SQL2 + " broker_abi_id, "
        SELECT @SQL2 = @SQL2 + " user_licence_id, "
        SELECT @SQL2 = @SQL2 + " pm_company_number, "
        SELECT @SQL2 = @SQL2 + " default_indicator)"
        SELECT @SQL2 = @SQL2 + " SELECT"

        SELECT @SQL3 = " source_id, "
        SELECT @SQL3 = @SQL3 + " caption_id, "
        SELECT @SQL3 = @SQL3 + " code, "
        SELECT @SQL3 = @SQL3 + " description, "
        SELECT @SQL3 = @SQL3 + " parent_id, "
        SELECT @SQL3 = @SQL3 + " is_deleted, "
        SELECT @SQL3 = @SQL3 + " effective_date, "
        SELECT @SQL3 = @SQL3 + " base_currency_id, "
        SELECT @SQL3 = @SQL3 + " reg_no_1, "
        SELECT @SQL3 = @SQL3 + " reg_no_2, "
        SELECT @SQL3 = @SQL3 + " address1, "
        SELECT @SQL3 = @SQL3 + " address2, "
        SELECT @SQL3 = @SQL3 + " address3, "
        SELECT @SQL3 = @SQL3 + " address4, "
        SELECT @SQL3 = @SQL3 + " postal_code, "
        SELECT @SQL3 = @SQL3 + " country_id, "

        SELECT @SQL4 = " phone_area_code, "
        SELECT @SQL4 = @SQL4 + " phone_number, "
        SELECT @SQL4 = @SQL4 + " phone_extension, "
        SELECT @SQL4 = @SQL4 + " fax_area_code, "
        SELECT @SQL4 = @SQL4 + " fax_number, "
        SELECT @SQL4 = @SQL4 + " fax_extension, "
        SELECT @SQL4 = @SQL4 + " email, vat_no, "
        SELECT @SQL4 = @SQL4 + " sender_mailbox_id, "
        SELECT @SQL4 = @SQL4 + " broker_abi_id, "
        SELECT @SQL4 = @SQL4 + " user_licence_id, "
        SELECT @SQL4 = @SQL4 + " pm_company_number, "
        SELECT @SQL4 = @SQL4 + " default_indicator"
        SELECT @SQL4 = @SQL4 + " FROM Source"

        EXECUTE (@SQL + @SQL2 + @SQL3 + @SQL4)
    END

    /************************************************************************/
    /* Transaction Type                                                     */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'Transaction_Type',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying Transaction Type Data.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..Transaction_Type ("
        SELECT @SQL = @SQL + "transaction_type_id, "
        SELECT @SQL = @SQL + "caption_id, "
        SELECT @SQL = @SQL + "code, "
        SELECT @SQL = @SQL + "description, "
        SELECT @SQL = @SQL + "transaction_type_basis, "
        SELECT @SQL = @SQL + "is_deleted, "
        SELECT @SQL = @SQL + "effective_date "
        SELECT @SQL = @SQL + ") SELECT "

        SELECT @SQL2 = "transaction_type_id, "
        SELECT @SQL2 = @SQL2 + "caption_id, "
        SELECT @SQL2 = @SQL2 + "code, "
        SELECT @SQL2 = @SQL2 + "description, "
        SELECT @SQL2 = @SQL2 + "transaction_type_basis, "
        SELECT @SQL2 = @SQL2 + "is_deleted, "
        SELECT @SQL2 = @SQL2 + "effective_date "
        SELECT @SQL2 = @SQL2 + " FROM Transaction_Type"

        EXECUTE (@SQL + @SQL2)
    END

    /************************************************************************/
    /* PMCaption                                                            */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'PMCaption',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying PMCaption Data.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..PMCaption ("
        SELECT @SQL = @SQL + "caption_id, "
        SELECT @SQL = @SQL + "language_id, "
        SELECT @SQL = @SQL + "caption "
        SELECT @SQL = @SQL + ") SELECT "
        SELECT @SQL = @SQL + "caption_id, "
        SELECT @SQL = @SQL + "language_id, "
        SELECT @SQL = @SQL + "caption "
        SELECT @SQL = @SQL + " FROM PMCaption"

        EXECUTE (@SQL)
    END

    /************************************************************************/
    /* PMUser                                                               */
    /************************************************************************/

    EXECUTE @table_in_db = spu_pm_table_in_database
        @table_name = 'PMUser',
        @database_name = @database_name

    IF @table_in_db = 0 BEGIN
        SELECT @print = 'Copying Users.....'
        PRINT @print

        SELECT @SQL = "INSERT INTO " + @database_name + "..PMUser ("
        SELECT @SQL = @SQL + "user_id, "
        SELECT @SQL = @SQL + "party_cnt, "
        SELECT @SQL = @SQL + "language_id, "
        SELECT @SQL = @SQL + "username, "
        SELECT @SQL = @SQL + "password, "
        SELECT @SQL = @SQL + "password_change_date, "
        SELECT @SQL = @SQL + "date_created, "
        SELECT @SQL = @SQL + "lastlogin, "
        SELECT @SQL = @SQL + "is_pmb_link_required, "

        SELECT @SQL2 = "logged_on_at_client, "
        SELECT @SQL2 = @SQL2 + "server_printer, "
        SELECT @SQL2 = @SQL2 + "is_printer_changeable, "
        SELECT @SQL2 = @SQL2 + "is_deleted, "
        SELECT @SQL2 = @SQL2 + "effective_date "
        SELECT @SQL2 = @SQL2 + ") SELECT "
        SELECT @SQL2 = @SQL2 + "user_id, "
        SELECT @SQL2 = @SQL2 + "party_cnt, "

        SELECT @SQL2 = @SQL2 + "language_id, "
        SELECT @SQL2 = @SQL2 + "username, "
        SELECT @SQL2 = @SQL2 + "password, "

        SELECT @SQL3 = "password_change_date, "
        SELECT @SQL3 = @SQL3 + "date_created, "
        SELECT @SQL3 = @SQL3 + "lastlogin, "
        SELECT @SQL3 = @SQL3 + "is_pmb_link_required, "
        SELECT @SQL3 = @SQL3 + "logged_on_at_client, "
        SELECT @SQL3 = @SQL3 + "server_printer, "
        SELECT @SQL3 = @SQL3 + "is_printer_changeable, "
        SELECT @SQL3 = @SQL3 + "is_deleted, "
        SELECT @SQL3 = @SQL3 + "effective_date "
        SELECT @SQL3 = @SQL3 + " FROM PMUser"

        EXECUTE (@SQL + @SQL2 + @SQL3)
    END

    SELECT @print = 'Complete'
    PRINT @print

END
GO

