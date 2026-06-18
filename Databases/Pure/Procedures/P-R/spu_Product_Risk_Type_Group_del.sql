SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Product_Risk_Type_Group_del'
GO


CREATE PROCEDURE spu_Product_Risk_Type_Group_del
    @product_id int,
	@UserId int =Null,
	@UniqueId Varchar(50) =Null,
	@ScreenHierarchy varchar(500) = Null
AS

UPDATE Product_Risk_Type_Group set UserId =@UserId , UniqueId = @UniqueId , ScreenHierarchy =@ScreenHierarchy 

DELETE FROM Product_Risk_Type_Group

WHERE product_id = @product_id
GO


