if exists (select * from sysobjects where id = object_id(N'[dbo].[spe_Contact_upd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spe_Contact_upd]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO



/****** Object:  Stored Procedure dbo.spe_Contact_upd    Script Date: 16/10/00 12:12:16 ******/
CREATE PROCEDURE spe_Contact_upd
    @contact_cnt int,
    @contact_type_id smallint,
    @source_id smallint,
    @contact_id int,
    @country_id smallint,
    @description varchar(255),
    @area_code char(10),
    @number varchar(255),
    @extension char(6),
    @created_by_id smallint,
    @date_created datetime,
    @modified_by_id smallint,
    @last_modified datetime,  
    @UserId int=NULL,  
    @UniqueId VARCHAR(50)=NULL,  
    @ScreenHeirarchy VARCHAR(500)=NULL
AS
BEGIN
UPDATE Contact
    SET
    contact_type_id=@contact_type_id,
    source_id=@source_id,
  /*  contact_id=@contact_id,  KN (CMG)  15/08/02 */
  /*  KN (CMG) Do not update contact id as it is a unique index key with source_id */ 
    country_id=@country_id,
    description=@description,
    area_code=@area_code,
    number=@number,
    extension=@extension,
    created_by_id=@created_by_id,
    date_created=@date_created,
    modified_by_id=@modified_by_id,
    last_modified=@last_modified,  
    UserId = @UserId,  
    UniqueId = @UniqueId,  
    ScreenHierarchy = @ScreenHeirarchy
WHERE contact_cnt = @contact_cnt 
END




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

