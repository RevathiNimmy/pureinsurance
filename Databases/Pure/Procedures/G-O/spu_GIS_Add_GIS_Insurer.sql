SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Add_GIS_Insurer'
GO


CREATE PROCEDURE spu_GIS_Add_GIS_Insurer
    @gis_insurer_id int OUTPUT,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @method varchar(6),
    @icr decimal,
    @polaris_insurer_no int,
    /* @mail_box varchar(13), */

    @abi_1_edi_directory varchar(6),
    @abi_81_insurer varchar(3)
AS


BEGIN

--IF @source_id = 0
-- SELECT @source_id = 1

--IF @source_id IS NULL
-- SELECT @source_id = 1

IF @GIS_Insurer_id = 0
    SELECT @GIS_Insurer_id = NULL

IF @GIS_Insurer_id IS NULL
    SELECT @GIS_Insurer_id = MAX(GIS_Insurer_id) + 1
        FROM GIS_Insurer
        --WHERE source_id = @source_id

IF @GIS_Insurer_id IS NULL
    SELECT @GIS_Insurer_id = 1

INSERT INTO GIS_Insurer (
    gis_insurer_id,
    code,
    caption_id,
    description,
    is_deleted,
    effective_date,
    method,
    icr,
    polaris_insurer_no,
    /* mail_box, */
    abi_1_edi_directory,
    abi_81_insurer)
VALUES (
    @gis_insurer_id,
    @code,
    @caption_id,
    @description,
    @is_deleted,
    @effective_date,
    @method,
    @icr,
    @polaris_insurer_no,
    /* @mail_box, */
    @abi_1_edi_directory,
    @abi_81_insurer)
END

--BEGIN
--SELECT @gis_insurer_id = @@IDENTITY
--END
GO


