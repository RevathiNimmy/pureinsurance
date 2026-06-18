SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_pm_get_user_sources2'
GO

-- Same as spu_pm_get_user_sources, but this one also load the given source, even if it is closed.
CREATE PROCEDURE spu_pm_get_user_sources2
    @UserID SMALLINT,
	@IncludeBranchID INT,  
 @ProductID INT = NULL
AS
BEGIN

    SELECT source_id, [description], country_id
    FROM source 
    WHERE
		(is_deleted <> 1 OR source_id = @IncludeBranchID) AND
		source_id NOT IN (SELECT source_id FROM pmuser_source WHERE [user_id] = @UserID)
		AND (source_id IN (SELECT source_id from Product_Source WHERE product_id=@ProductID) OR @ProductID IS NULL) 
END
GO
