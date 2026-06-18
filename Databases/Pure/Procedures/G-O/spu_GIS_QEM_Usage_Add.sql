EXECUTE DDLDropProcedure 'spu_GIS_QEM_Usage_Add'
GO


SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


--added risk group id for CNIC jes 07/05/2002
--ED1006002 - made risk group id optional to cope with existing Product Builder 
CREATE PROCEDURE spu_GIS_QEM_Usage_Add
    @gis_data_model_id int,
    @gis_business_type_id int,
    @gis_scheme_id int,
    @gis_qem_id int,		
    @Risk_group_id int = NULL
AS


BEGIN

/* RAG 25-10-2001
   You need to add Data_Model_ID and Business_Type_ID to GIS_Data_Model_Business first,
   otherwise the constraint will not be satisfied */

IF NOT EXISTS (
    SELECT *
    FROM GIS_Data_Model_Business
    WHERE  gis_data_model_id = @gis_data_model_id
    AND    gis_business_type_id = @gis_business_type_id
    )
    INSERT GIS_Data_Model_Business (gis_data_model_id, gis_business_type_id)
    VALUES (@gis_data_model_id, @gis_business_type_id)

/* RAG End */

IF NOT EXISTS (
    SELECT *
    FROM GIS_QEM_usage
    WHERE  gis_data_model_id = @gis_data_model_id
    AND    gis_business_type_id = @gis_business_type_id
    AND    gis_scheme_id = @gis_scheme_id
    AND    gis_qem_id = @gis_qem_id
    AND    risk_group_id = @Risk_group_ID
    )
    INSERT GIS_QEM_Usage (gis_data_model_id, gis_business_type_id, gis_scheme_id, gis_qem_id,risk_group_id)
    VALUES (@gis_data_model_id, @gis_business_type_id, @gis_scheme_id, @gis_qem_id,@risk_group_id)

END
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

