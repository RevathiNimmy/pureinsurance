SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Folder_upd'
GO

CREATE PROCEDURE spe_Risk_Folder_upd
    @risk_folder_cnt int,
    @risk_folder_id int,
    @source_id smallint,
    @risk_folder_type_id int,
    @code varchar(40),
    @description varchar(255),
    @insurance_folder_cnt int

AS
BEGIN

UPDATE Risk_Folder
    SET
    risk_folder_id=@risk_folder_id,
    source_id=@source_id,
    risk_folder_type_id=@risk_folder_type_id,
    code=@code,
    description=@description,
    insurance_folder_cnt=@insurance_folder_cnt
WHERE risk_folder_cnt = @risk_folder_cnt

END
GO

