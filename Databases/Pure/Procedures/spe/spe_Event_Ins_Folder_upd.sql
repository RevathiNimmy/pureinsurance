SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spe_Event_Ins_Folder_upd'
GO

CREATE PROCEDURE spe_Event_Ins_Folder_upd
    @insurance_folder_cnt int,
    @insurance_folder_id int,
    @source_id smallint,
    @insurance_holder_cnt int,
    @code varchar(40),
    @description varchar(255),
    @inception_date datetime,
    @arc_archive_folder_id int,
    @quote_insurance_ref varchar(30),
    @next_insurance_ref varchar(30),
    @last_insurance_ref varchar(30),
    @renewal_count int  
  
AS
BEGIN
UPDATE Event_Insurance_Folder
    SET
    insurance_folder_id=@insurance_folder_id,
    source_id=@source_id,
    insurance_holder_cnt=@insurance_holder_cnt,
    code=@code,
    description=@description,
    inception_date=@inception_date,
    arc_archive_folder_id=@arc_archive_folder_id,
    quote_insurance_ref=@quote_insurance_ref,
    next_insurance_ref=@next_insurance_ref,
    last_insurance_ref=@last_insurance_ref,
    renewal_count=@renewal_count  
WHERE insurance_folder_cnt = @insurance_folder_cnt
END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
