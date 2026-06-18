SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_gis_list_add_usage'
GO

CREATE PROCEDURE spu_gis_list_add_usage 

@ListType varchar(30),
@code varchar(30),
@version int,
@effdate datetime

AS

--declarations
DECLARE @ListID int
DECLARE @ListTypeID int

--is code already in the list?

SELECT @ListID=i.gis_list_items_id FROM  gis_list_items i 
INNER JOIN gis_list_type_usage u
ON i.gis_list_items_id = u.gis_list_items_id
INNER JOIN gis_list_type t
ON u.gis_list_type_id=t.gis_list_type_id
WHERE i.code=@code 
AND t.code=@Listtype

-- if code is there then skip this bit
IF @listID is null 
BEGIN
	--Add code to list
	INSERT gis_list_items (Code) VALUES (@Code)
	--get last identity
	SELECT @listID=@@Identity
END

--get list type id
SELECT @listTypeID=gis_list_type_id FROM gis_list_type where code=@ListType

--add to usage table
INSERT gis_list_type_usage (gis_list_type_id,gis_list_items_id,version,effective_date)
VALUES (@ListTypeID,@listID,@Version,@effdate)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

