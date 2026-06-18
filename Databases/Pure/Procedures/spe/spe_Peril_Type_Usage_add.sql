SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Peril_Type_Usage_add'
GO

CREATE PROCEDURE spe_Peril_Type_Usage_add
    @peril_group_id int,
    @peril_type_id int,
    @allocate_percent numeric(12,8),
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL

AS

BEGIN

SELECT @screenhierarchy=@screenhierarchy + ' / ' + pt.description
    FROM peril_type pt 
    WHERE pt.peril_type_id=@peril_type_id

INSERT INTO Peril_Type_Usage (
    peril_group_id ,
    peril_type_id ,
    allocate_percent,
	UserId,
    uniqueid ,
	screenhierarchy )
VALUES (
    @peril_group_id,
    @peril_type_id,
    @allocate_percent,
	@userid,
	@uniqueid ,
	@screenhierarchy)
END

GO

