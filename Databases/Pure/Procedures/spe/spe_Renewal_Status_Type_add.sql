SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_Type_add'
GO

CREATE PROCEDURE spe_Renewal_Status_Type_add
    @renewal_status_type_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime
AS
BEGIN
INSERT INTO Renewal_Status_Type (
    renewal_status_type_id ,
    caption_id ,
    code ,
    description ,
    is_deleted ,
    effective_date )
VALUES (
    @renewal_status_type_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date)
END

GO

