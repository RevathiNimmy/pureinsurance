SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Contact_Type_update'
GO


CREATE PROCEDURE spu_Contact_Type_update
    @contact_type_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @is_contact_type tinyint,
    @is_correspondence_type tinyint
AS


/* Update the values */
UPDATE  Contact_Type
SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    is_contact_type = @is_contact_type,
    is_correspondence_type = @is_correspondence_type
WHERE   contact_type_id = @contact_type_id
GO


