SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Business_Type_saa'
GO


CREATE PROCEDURE spu_Business_Type_saa
    @Insurance_ref varchar(30),
    @BusCode char(10) OUTPUT
AS


Select DISTINCT
   @BusCode = Business_Type.code
From
   Insurance_File,
   Business_Type
Where
   Insurance_File.insurance_ref = @Insurance_ref
   AND Insurance_File.business_type_id = Business_Type.business_type_id
Order By
   Business_Type.code ASC
GO


