SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Peril_Type_Usage_del'
GO


CREATE PROCEDURE spu_Peril_Type_Usage_del
    @peril_group_id int,
	@userid INT = NULL,
	@uniqueid VARCHAR(50) = NULL,
	@screenhierarchy VARCHAR(100) = NULL
AS

UPDATE ptu  SET 
        userid=@UserId,
        uniqueid=@uniqueid,
		screenhierarchy=@screenhierarchy + ' / ' + pt.description
    FROM Peril_Type_Usage ptu INNER JOIN peril_type pt ON ptu.peril_type_id=pt.peril_type_id
    WHERE
	    peril_group_id = @peril_group_id

DELETE FROM Peril_Type_Usage

WHERE peril_group_id = @peril_group_id
GO






