SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Folder_add'
GO
/*************************************************************************/
/* ERWIN generated add record and return Identity column                 */
/* and generate ID column if required.                                   */
/*************************************************************************/
/*************************************************************************/
/* 1.0  06/08/1997 RFC Original (Based on SP Original)                   */
/*************************************************************************/
CREATE PROCEDURE spe_Risk_Folder_add
    @risk_folder_cnt int OUTPUT ,
    @risk_folder_id int ,
    @source_id smallint ,
    @risk_folder_type_id int ,
    @code varchar(40) ,
    @description varchar(255) ,
    @insurance_folder_cnt int
AS BEGIN

DECLARE @table_name varchar(128)

IF ISNULL(@source_id, 0) = 0 BEGIN
    SELECT @source_id = 1
END

SELECT @risk_folder_id = 0

INSERT INTO Risk_Folder (
    risk_folder_id,
    source_id,
    risk_folder_type_id,
    code,
    description,
    insurance_folder_cnt)
VALUES (
    @risk_folder_id,
    @source_id,
    @risk_folder_type_id,
    NEWID(),
    @description,
    @insurance_folder_cnt)

SELECT @risk_folder_cnt = SCOPE_IDENTITY()

UPDATE risk_folder 
	SET Code = 'C' + convert(char(2), @source_id) + convert(char(10), @risk_folder_cnt)
	WHERE risk_folder_cnt = @risk_folder_cnt

END

GO

