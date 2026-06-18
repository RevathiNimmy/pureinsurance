SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Add_Party_Contact'
GO

/*******************************************************************************************************/
/* spu_SAM_Add_Party_Contact     */                                                                              
/* Add Contacts for Party */
/*******************************************************************************************************/

CREATE PROCEDURE spu_SAM_Add_Party_Contact
    @party_cnt int,
    @source_id smallint ,
    @created_by_id smallint, 
    @contact_type_id smallint ,
    @area_code char(10) ,
    @number varchar(255),
    @description varchar(255) 
AS
BEGIN

SET NOCOUNT ON

Declare 
	@contact_cnt int, 
	@contact_id int ,
	@country_id smallint ,
	@extension char(6) ,
	@date_created datetime ,
	@last_modified datetime,
	@modified_by_id smallint ,
	@Party_Contact_Usage_description varchar(255) 

SET @date_created   = Getdate()
SET @last_modified  = Getdate()
SET @modified_by_id = @created_by_id
SET @country_id     = 0

Execute spe_Contact_add  
	@contact_cnt  OUTPUT ,  
	@contact_type_id  ,  
	@source_id  ,  
	@contact_id  ,  
	@country_id  ,  
	@description ,  
	@area_code  ,  
	@number  ,  
	@extension  ,  
	@created_by_id  ,  
	@date_created  ,  
	@modified_by_id  ,  
	@last_modified   


END
BEGIN
	INSERT INTO Party_Contact_Usage (  
	    party_cnt ,  
	    contact_cnt ,  
	    description )  
	VALUES (  
	    @party_cnt,  
	    @contact_cnt,  
	    @Party_Contact_Usage_description)  
END

SET NOCOUNT OFF

GO

