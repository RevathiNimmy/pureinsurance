SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmsystem_add'
GO


CREATE PROCEDURE spu_pmsystem_add
    @system_name CHAR(40),
    @licence_limit INT,
    @licence_key VARCHAR(30)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 26/10/1999 Tom */
/********************************************************************************************************/
DECLARE @system_id INT

    SELECT @system_id = system_id
    FROM PMSystem
    WHERE system_name = @system_name

    IF @system_id IS NULL
        BEGIN
		SELECT @system_id = (select max(system_id) from PMSystem)

        IF (@system_id IS NULL)
            SELECT @system_id = 1
        ELSE
            SELECT @system_id = @system_id + 1

        INSERT INTO PMSystem( system_id,
                    product_id,
                    system_name,
                    default_source_id,
                    home_country_id,
                    currency_id,
                    language_id,
                    licence_limit,
                    licence_key,
                    log_level,
                    pool_size)

        SELECT  @system_id,
			product_id,
            @system_name,
            default_source_id,
            home_country_id,
            currency_id,
            language_id,
            @licence_limit,
            @licence_key,
            log_level,
            pool_size
        FROM PMSystem
        WHERE system_id = 1
        END
GO


