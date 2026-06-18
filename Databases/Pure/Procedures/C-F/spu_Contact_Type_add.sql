SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Contact_Type_add'
GO


CREATE PROCEDURE spu_Contact_Type_add
    @contact_type_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @is_contact_type tinyint,
    @is_correspondence_type tinyint
AS


/* Insert the values */
INSERT INTO Contact_Type
( contact_type_id, caption_id, code, description, is_deleted, effective_date, is_contact_type, is_correspondence_type )
VALUES
( @contact_type_id, @caption_id, @code, @description, @is_deleted, @effective_date, @is_contact_type, @is_correspondence_type )
GO


