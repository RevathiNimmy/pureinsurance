/****** Object:  StoredProcedure [dbo].[spu_XMLGenerator_GetPropertyUDLData]    Script Date: 18-05-2016 5:22:41 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spu_XMLGenerator_GetPropertyUDLData]
( 
	@PropertyName varchar(60)
)

AS



Declare @sSQL Varchar(500)
Declare @UDLName varchar(255) 
Declare @UDLFirstColumnName varchar(255) 
Declare @UDLDescriptionColumnName varchar(255)
select @UDLName=Rtrim(Ltrim(Specials_Type_Reference))  from gis_property  where property_name =@PropertyName
Select @UDLFirstColumnName=sc.name from sys.COLUMNS SC where SC.object_id =(Select SS.object_id  from sys.tables SS where SS.NAME=@UDLName) AND SC.COLUMN_ID=1
SET @sSQL ='SELECT '+@UDLFirstColumnName+',Description FROM ' + @UDLName
Execute (@sSql)
