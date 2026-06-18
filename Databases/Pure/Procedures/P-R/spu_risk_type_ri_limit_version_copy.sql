EXECUTE DDLDropProcedure 'spu_risk_type_ri_limit_version_copy'
GO

CREATE PROCEDURE spu_risk_type_ri_limit_version_copy
@risk_type_id INT ,
@risk_type_ri_limit_version_id INT
AS
DECLARE @desc varchar(255) , 
		@ri_limit_effective_date date, 
		@ri_limit_expiry_date date,
		@NEW_risk_type_ri_limit_version_id int

SELECT @desc = LTRIM(RTRIM(description)) , 
	   @ri_limit_effective_date = ri_limit_start_date , 
	   @ri_limit_expiry_date = ri_limit_end_date 
FROM Risk_Type_RI_Limit_Version 
WHERE risk_type_id =@risk_type_id 
AND risk_type_ri_limit_version_id =@risk_type_ri_limit_version_id 

SELECT @ri_limit_effective_date = DATEADD(yy,1,@ri_limit_effective_date) ,@ri_limit_expiry_date = DATEADD(YY,1,@ri_limit_expiry_date)  

INSERT INTO Risk_Type_RI_Limit_Version 
(risk_type_id ,
description ,
ri_limit_start_date ,
ri_limit_end_date )
SELECT @risk_type_id ,
@desc , 
@ri_limit_effective_date,
@ri_limit_expiry_date

/* Return the Count of the Record Added */  
SELECT @NEW_risk_type_ri_limit_version_id  = @@IDENTITY  

INSERT INTO risk_type_ri_properties 
(risk_type_id,
gis_property_id,
risk_type_ri_properties_seq_id,
risk_type_ri_limit_version_id )
SELECT risk_type_id ,
	   gis_property_id ,
	   risk_type_ri_properties_seq_id , 
	   @NEW_risk_type_ri_limit_version_id 
FROM risk_type_ri_properties
WHERE risk_type_id = @risk_type_id  
AND risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id 

INSERT INTO Risk_Type_RI_Values 
(risk_type_id ,
gis_user_def_header_inds_id1 ,
gis_user_def_header_inds_id2 ,
gis_user_def_header_inds_id3 ,
value ,
risk_type_ri_limit_version_id ) 
SELECT risk_type_id, 
	   gis_user_def_header_inds_id1 ,
	   gis_user_def_header_inds_id2 ,
	   gis_user_def_header_inds_id3 ,
	   value ,
	   @NEW_risk_type_ri_limit_version_id 
FROM Risk_Type_RI_Values 
WHERE risk_type_id =@risk_type_id 
AND risk_type_ri_limit_version_id =@risk_type_ri_limit_version_id 

