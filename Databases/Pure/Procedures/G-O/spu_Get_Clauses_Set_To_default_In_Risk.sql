SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Clauses_Set_To_default_In_Risk'
GO
create procedure spu_Get_Clauses_Set_To_default_In_Risk
             @risk_type_id int,
			 @Source_id int,
			 @Property_Name VARCHAR(70)=NULL ,
			 @gis_object_name varchar(70)=NULL
As
Begin
	DECLARE @Specials_Type_Reference  Varchar(50)    
	SELECT  @Specials_Type_Reference = NULL  
	DECLARE @gis_datamodel_ID INT
	DECLARE @gis_object_ID INT

	DECLARE @applyfilter INT
	SELECT @applyfilter=0   
	
	IF  NOT ISNULL(@Property_Name,'')=''    
	Begin
		IF  NOT ISNULL(@gis_object_name,'')=''
		BEGIN
			SELECT @gis_object_ID= GIS_OBJECT_ID FROM GIS_Object WHERE object_name =@gis_object_name
		   
			SELECT @Specials_Type_Reference=Specials_Type_Reference FROM gis_property    
			WHERE property_name=@Property_Name  AND gis_object_id = @gis_object_ID
		END
		ELSE
			SELECT @Specials_Type_Reference=Specials_Type_Reference FROM gis_property    
			WHERE property_name=@Property_Name 
	End    
  
	IF ISNULL(@Specials_Type_Reference,'')<>''
	BEGIN
	SELECT @applyfilter=1
	END

	SELECT  dt.document_template_id,
            dt.code, 
            dt.description
	FROM wording_risk_type_link wrtl
 	INNER JOIN document_template dt 
    on dt.document_template_id = wrtl.document_template_id AND dt.document_template_id IN (	SELECT   distinct  dt.document_template_id    
																							FROM    document_template dt    
																							JOIN    document_type ty ON ty.document_type_id = dt.document_type_id    
																							JOIN    wording_risk_type_link wrtl ON wrtl.document_template_id = dt.document_template_id    
																							WHERE   wrtl.risk_type_id = @risk_type_id  and (wrtl.branch_id=@Source_id OR  (ISNULL(@Source_id,'')='') )    
																							AND     dt.effective_date < getdate() and dt.is_deleted=0    
																							AND ((ISNULL(@Specials_Type_Reference,'')='' AND @applyfilter=0) OR (dt.document_filter = @Specials_Type_Reference AND @applyfilter=1) ) )
 	WHERE wrtl.branch_id =@Source_id
        AND wrtl.risk_type_id=@risk_type_id
        AND wrtl.[default]=1
END
Go
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
