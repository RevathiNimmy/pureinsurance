SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_gis_model_properties_sel'
GO


CREATE PROCEDURE spu_gis_model_properties_sel
    @gis_data_model_code char(10)
AS


BEGIN

/********************************************************************************************************/
/* Stored Procedure Selects all of the Properties for the Supplied Data Model.                          */
/********************************************************************************************************/

/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                            				24/03/1999  RFC */
/* 1.1                  Parameter changed from data model id to data model code.        09/08/1999  RFC */
/* 1.2			Changed Parameters due to Claims Builderchanges 		16/07/2002  GSD */
/* 1.3                  Added sql to handle GII properties correctly			2002-12-11  IJR */
/********************************************************************************************************/

DECLARE @gis_data_model_id INT
DECLARE @data_model_type CHAR(3)

SELECT @data_model_type = LEFT(@gis_data_model_code, 3)

SELECT  @gis_data_model_id = gis_data_model_id
FROM    gis_data_model
WHERE   code = @gis_data_model_code

IF @data_model_type = 'GII' 
BEGIN
	DECLARE @SQ INT
	SELECT  @SQ = COUNT(gis_object_id)
	FROM    gis_object
	WHERE   gis_data_model_id = @gis_data_model_id
	AND		parent_object_id IS NULL
	AND		object_name = LTRIM(RTRIM(@gis_data_model_code)) + '_POLICY_BINDER'
	
	IF @SQ = 0
	--GII data model
	BEGIN
		SELECT  gp.gis_object_id ,
			go.object_name ,
	    		gp.gis_property_id ,
	    		gp.property_name ,
	    		gp.column_name ,
	    		gp.data_type ,
	    		gp.is_identifying_property ,
	    		gp.is_primary_key ,
	        	gp.polaris_property_id,
			gp.Edit_Flags,
			gp.Specials_Type,
			gp.Specials_Type_Reference
		FROM gis_object go INNER JOIN gis_property gp on go.gis_object_id = gp.gis_object_id
		INNER JOIN sysobjects so ON go.table_name = so.name
		INNER JOIN syscolumns sc ON so.id = sc.id and LTRIM(gp.column_name) = LTRIM(sc.name)
		WHERE go.gis_data_model_id = @gis_data_model_id
		ORDER BY go.is_quote_object DESC ,
	    		gp.gis_object_id ASC ,
			sc.colorder ASC
	END ELSE BEGIN
	--SQ data model
		SELECT  gp.gis_object_id ,
			go.object_name ,
			gp.gis_property_id ,
			gp.property_name ,
			gp.column_name ,
			gp.data_type ,
			gp.is_identifying_property ,
			gp.is_primary_key ,
			gp.polaris_property_id,
			gp.Edit_Flags,
			gp.Specials_Type,
			gp.Specials_Type_Reference
		FROM    gis_property gp ,
		gis_object go
		WHERE   go.gis_object_id = gp.gis_object_id
		AND   go.gis_data_model_id = @gis_data_model_id
		ORDER BY go.is_quote_object DESC ,
			gp.gis_object_id ASC ,
		    	gp.is_primary_key DESC,	
	    		gp.gis_property_id ASC
	END
END
ELSE
BEGIN
	SELECT  gp.gis_object_id ,
        	go.object_name ,
    		gp.gis_property_id ,
    		gp.property_name ,
    		gp.column_name ,
    		gp.data_type ,
    		gp.is_identifying_property ,
    		gp.is_primary_key ,
        	gp.polaris_property_id,
		gp.Edit_Flags,
		gp.Specials_Type,
		gp.Specials_Type_Reference
	FROM    gis_property gp ,
    	gis_object go
	WHERE   go.gis_object_id = gp.gis_object_id
  	AND   go.gis_data_model_id = @gis_data_model_id
	ORDER BY   go.is_quote_object DESC ,
    		gp.gis_object_id ASC ,
    		gp.gis_property_id ASC

END

END
GO