SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spe_Insurance_Folder_add'
GO

CREATE  PROCEDURE spe_Insurance_Folder_add
    @insurance_folder_cnt int OUTPUT ,
    @insurance_folder_id int ,
    @source_id smallint ,
    @insurance_holder_cnt int ,
    @code varchar(40) ,
    @description varchar(255) ,
    @inception_date datetime ,
    @arc_archive_folder_id int ,
    @quote_insurance_ref varchar(30) ,
    @next_insurance_ref varchar(30) ,
    @last_insurance_ref varchar(30) ,
    @renewal_count int

AS

BEGIN

IF @source_id = 0
                SELECT @source_id = 1

IF @source_id IS NULL
                SELECT @source_id = 1

set @insurance_folder_id=0

INSERT INTO Insurance_Folder (
    insurance_folder_id,
    source_id,
    insurance_holder_cnt,
    code,
    description,
    inception_date,
    arc_archive_folder_id,
    quote_insurance_ref,
    next_insurance_ref,
    last_insurance_ref,
    renewal_count)
VALUES (
    @insurance_folder_id,
    @source_id,
    @insurance_holder_cnt,
    @code,
    @description,

    @inception_date,
    @arc_archive_folder_id,
    @code,
    @next_insurance_ref,
    @last_insurance_ref,
    @renewal_count)
END

BEGIN
SELECT @insurance_folder_cnt = @@IDENTITY
END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
