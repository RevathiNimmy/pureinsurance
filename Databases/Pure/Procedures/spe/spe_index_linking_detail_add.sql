SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_index_linking_detail_add'
GO

CREATE PROCEDURE spe_index_linking_detail_add
    @index_linking_detail_id int OUTPUT ,
    @index_linking_id int ,
    @effective_date datetime ,
    @is_deleted tinyint ,
    @percentage numeric(7,4),
	@UserId INT = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL
AS
BEGIN

IF @index_linking_detail_id = 0
                SELECT @index_linking_detail_id = NULL

IF @index_linking_detail_id IS NULL
                SELECT @index_linking_detail_id = MAX(index_linking_detail_id) + 1
    FROM index_linking_detail

IF @index_linking_detail_id IS NULL
    SELECT @index_linking_detail_id = 1

INSERT INTO index_linking_detail (
    index_linking_detail_id ,
    index_linking_id ,
    effective_date ,
    is_deleted ,
    percentage,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES (
    @index_linking_detail_id,
    @index_linking_id,
    @effective_date,
    @is_deleted,
    @percentage,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)

SELECT index_linking_detail_id = @index_linking_detail_id

END
GO

