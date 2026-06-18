SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_SingleLookupHeader'
GO


CREATE PROCEDURE spu_Get_SingleLookupHeader  
    @gis_data_model_code varchar(10),
    @lookup_name varchar(100),
    @insurer_panel_member_key int,
    @scheme_number int,
    @effective_date datetime   
AS  
  
Declare @gis_data_model_id INT

Select @gis_data_model_id= gis_data_model_id from GIS_Data_Model where code=@gis_data_model_code  
  
SELECT TOP 1 insurer_panel_member_key,
		scheme_number,
		lookup_key,  
		lookup_name,
        effective_date,  
        modified_date,  
        status,  
        definition,  
        valid_constants,  
        default_value
  
FROM    Gis_Lookup_Header  
  
WHERE gis_data_model_id = @gis_data_model_id 
  AND lookup_name = @lookup_name
  AND insurer_panel_member_key = @insurer_panel_member_key
  AND scheme_number = @scheme_number
  AND effective_date <= @effective_date
ORDER BY effective_date desc

GO