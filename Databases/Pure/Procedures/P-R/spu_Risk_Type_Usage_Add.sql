SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_Usage_Add'
GO


CREATE PROCEDURE spu_Risk_Type_Usage_Add
    @risk_type_id int,
    @risk_type_group_id int,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll
AS

IF @UniqueId IS NOT NULL
BEGIN
	SELECT @ScreenHierarchy = @ScreenHierarchy + '/Risk Group(' + RTRIM(LTRIM(Code)) + ')' 
	FROM Risk_Type_Group 
	WHERE risk_type_group_id = @risk_type_group_id
END

INSERT INTO Risk_Type_Usage
(
    risk_type_id,
    risk_type_group_id,
	UserId,
	UniqueId,
	ScreenHierarchy
)

VALUES

(
    @risk_type_id,
    @risk_type_group_id,
	@UserId,
	@UniqueId,
	@ScreenHierarchy
)
GO


