SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Party_GIS_Insurer_Synch'
GO
-- Given some SBO Insurer data, make sure there is a matching row in the GIS Insurers.
-- Returns 0 for success, or nonzero error code for failure.
-- This proc is designed to be called from inside a trigger, and as such, has only
-- simple error checking and *no* transaction calls.
--
CREATE  PROCEDURE spu_Party_GIS_Insurer_Synch
    @party_cnt integer,
    @shortname char(20),
    @resolved_name varchar(100),
    @abi_code_on_81 varchar(3),
    @is_deleted tinyint,
    @method varchar(6),
    @icr decimal(18, 0),
    @polaris_insurer_no integer,
    @abi_1_edi_directory varchar(6)
AS
BEGIN
    DECLARE @gis_insurer_id integer
    DECLARE @error integer
	DECLARE @caption_id integer

	/* Get the caption ID */
	Execute spu_pm_caption_id_return 1, @resolved_name, @caption_id OUTPUT

    -- Check whether the GIS Insurer already exists.
    IF EXISTS (SELECT NULL FROM GIS_Insurer WHERE party_cnt = @party_cnt) BEGIN

        -- Update the GIS Insurer.
        UPDATE GIS_Insurer SET
            code = LEFT(@shortname, 10),
            caption_id = @caption_id,
            description = @resolved_name,
            is_deleted = @is_deleted,
            method = @method,
            icr = @icr,
            polaris_insurer_no = @polaris_insurer_no,
            abi_1_edi_directory = @abi_1_edi_directory,
            abi_81_insurer = @abi_code_on_81
            WHERE party_cnt = @party_cnt

        IF @@ERROR <> 0 BEGIN
            RETURN @@ERROR
        END

    END ELSE BEGIN

        -- Create the GIS Insurer.
        INSERT INTO GIS_Insurer (
            code,
            caption_id,
            description,
            is_deleted,
            effective_date,
            method,
            icr,
            polaris_insurer_no,
            abi_1_edi_directory,
            abi_81_insurer,
            party_cnt
        ) VALUES (
            LEFT(@shortname, 10),
            @caption_id,
            @resolved_name,
            @is_deleted,
            getdate(),
            @method,
            @icr,
            @polaris_insurer_no,
            @abi_1_edi_directory,
            @abi_code_on_81,
            @party_cnt
        )

        IF @@ERROR <> 0 BEGIN
            RETURN @@ERROR
        END

    END

    RETURN 0
END
GO