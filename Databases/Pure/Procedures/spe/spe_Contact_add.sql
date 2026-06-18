SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Contact_add'
GO

CREATE  PROCEDURE spe_Contact_add
    @contact_cnt int OUTPUT ,
    @contact_type_id smallint ,
    @source_id smallint ,
    @contact_id int ,
    @country_id smallint ,
    @description varchar(255) ,
    @area_code char(10) ,
    @number varchar(255) ,
    @extension char(6) ,
    @created_by_id smallint ,
    @date_created datetime ,
    @modified_by_id smallint ,
    @last_modified datetime,  
    @UserId int = null,  
    @UniqueId VARCHAR(50) = null,  
    @ScreenHeirarchy VARCHAR(500) = null 

AS
BEGIN
IF @source_id = 0
                SELECT @source_id = 1
IF @source_id IS NULL
                SELECT @source_id = 1

SET @contact_id=0

INSERT INTO Contact (
    contact_type_id,
    source_id,
    contact_id,
    country_id,
    description,
    area_code,
    number,
    extension,
    created_by_id,
    date_created,
    modified_by_id,
    last_modified,  
    UserId,  
    UniqueId,  
    ScreenHierarchy)
VALUES (
    @contact_type_id,
    @source_id,
    @contact_id,
    @country_id,
    @description,
    @area_code,
    @number,
    @extension,
    @created_by_id,
    @date_created,
    @modified_by_id,
    @last_modified,  
    @UserId,  
    @UniqueId,  
    @ScreenHeirarchy)
END
BEGIN
SELECT @contact_cnt = SCOPE_IDENTITY()
END

GO

