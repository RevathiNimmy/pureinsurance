SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Product_Risk_Type_Group_add'
GO


CREATE PROCEDURE spu_Product_Risk_Type_Group_add
    @product_id int,
    @risk_type_group_id int,
	@UserId int =Null,
	@UniqueId Varchar(50) =Null,
	@ScreenHierarchy varchar(500) = Null

AS


BEGIN
INSERT INTO Product_Risk_Type_Group (
    product_id ,
    risk_type_group_id ,
	UserId,
	UniqueId ,
	ScreenHierarchy 
	)
VALUES (
    @product_id,
    @risk_type_group_id,
	@UserId ,
	@UniqueId ,
	@ScreenHierarchy)
END
GO


